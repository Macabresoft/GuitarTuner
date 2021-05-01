namespace Macabresoft.GuitarTuner.Library {
    using System;
    using Macabresoft.Core;
    using Macabresoft.GuitarTuner.Library.Input;

    /// <summary>
    /// Monitors the frequency of a <see cref="ISampleProvider" /> device.
    /// </summary>
    public sealed class FrequencyMonitor : PropertyChangedNotifier, IDisposable {
        /// <summary>
        /// The highest frequency this monitor can detect. May need to be increased or customizable
        /// if the tuner begins supporting custom tunings.
        /// </summary>
        public const float HighestFrequency = 600f;

        /// <summary>
        /// The hold time for a note in seconds. For instance, if a user hits the note E and
        /// then provides no sound for 2 seconds, the frequency will continue to report E
        /// until those 2 seconds are up.
        /// </summary>
        public const float HoldTime = 2f;

        /// <summary>
        /// The lowest frequency this monitor can detect.
        /// </summary>
        public const float LowestFrequency = 30f;

        private readonly int _highPeriod;
        private readonly object _lock = new();
        private readonly int _lowPeriod;
        private readonly RollingMeanFloat _rollingAverageFrequency = new(5);
        private float _frequency;
        private bool _isDisposed;
        private ISampleProvider _sampleProvider;
        private float _timeElapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyMonitor" /> class.
        /// </summary>
        public FrequencyMonitor(ISampleProvider sampleProvider) {
            this._sampleProvider = sampleProvider;
            this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
            this._lowPeriod = (int)Math.Floor(this._sampleProvider.SampleRate / HighestFrequency);
            this._highPeriod = (int)Math.Ceiling(this._sampleProvider.SampleRate / LowestFrequency);
        }

        /// <summary>
        /// Gets the frequency in Hz.
        /// </summary>
        /// <value>The frequency in Hz.</value>
        public float Frequency {
            get => this._frequency;
            private set => this.Set(ref this._frequency, value);
        }

        /// <inheritdoc />
        public void Dispose() {
            if (!this._isDisposed) {
                this._sampleProvider.SamplesAvailable -= this.SampleProvider_SamplesAvailable;
                this._isDisposed = true;
            }
        }

        /// <summary>
        /// Sets the sample provider.
        /// </summary>
        /// <param name="sampleProvider">The sample provider.</param>
        public void SetSampleProvider(ISampleProvider sampleProvider) {
            lock (this._lock) {
                if (this._sampleProvider != sampleProvider) {
                    this._sampleProvider.SamplesAvailable -= this.SampleProvider_SamplesAvailable;
                    this._sampleProvider = sampleProvider;
                    this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
                    this._rollingAverageFrequency.Clear();
                    this.Frequency = 0f;
                }
            }
        }

        private void ClearFrequency() {
            this._rollingAverageFrequency.Clear();
            this._timeElapsed = 0f;
            this.Frequency = 0f;
        }

        private BufferInformation GetBufferInformation(float[] samples) {
            if (samples.Length < this._highPeriod) {
                throw new InvalidOperationException("The sample rate isn't large enough for the buffer length.");
            }

            var greatestMagnitude = float.NegativeInfinity;
            var chosenPeriod = -1;

            for (var period = this._lowPeriod; period < this._highPeriod; period++) {
                var sum = 0f;
                for (var i = 0; i < samples.Length - period; i++) {
                    sum += samples[i] * samples[i + period];
                }

                var newMagnitude = sum / samples.Length;
                if (newMagnitude > greatestMagnitude) {
                    chosenPeriod = period;
                    greatestMagnitude = newMagnitude;
                }
            }

            var frequency = (float)this._sampleProvider.SampleRate / chosenPeriod;
            return frequency is < LowestFrequency or > HighestFrequency ? BufferInformation.Unknown : new BufferInformation(frequency, greatestMagnitude);
        }

        private void HoldForReset(int sampleCount) {
            if (this.Frequency != 0f && sampleCount > 0) {
                if (this._sampleProvider.SampleRate > 0) {
                    this._timeElapsed += sampleCount / (float)this._sampleProvider.SampleRate;
                    this._rollingAverageFrequency.Remove();

                    if (this._timeElapsed >= HoldTime) {
                        this.ClearFrequency();
                    }
                }
                else {
                    this.ClearFrequency();
                }
            }
        }

        private void SampleProvider_SamplesAvailable(object? sender, SamplesAvailableEventArgs e) {
            lock (this._lock) {
                if (sender == this._sampleProvider) {
                    if (e.Samples.Length > 0 && e.Samples[^2] != 0f) {
                        var bufferInformation = this.GetBufferInformation(e.Samples);

                        if (bufferInformation.Frequency == 0f && bufferInformation.Magnitude == 0f) {
                            this.HoldForReset(e.Samples.Length);
                        }
                        else {
                            this._rollingAverageFrequency.Add(bufferInformation.Frequency);
                            this.Frequency = this._rollingAverageFrequency.MeanValue;
                            this.Frequency = bufferInformation.Frequency;
                        }
                    }
                    else {
                        this.HoldForReset(e.Samples.Length);
                    }
                }
            }
        }
    }
}