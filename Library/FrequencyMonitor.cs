namespace Macabresoft.Zvukosti.Library {

    using Macabresoft.Zvukosti.Library.Tuning;
    using NAudio.Dsp;
    using NAudio.Wave;
    using System;

    /// <summary>
    /// Monitors the frequency of a <see cref="IWaveIn" /> device.
    /// </summary>
    public sealed class FrequencyMonitor : NotifyPropertyChanged, IDisposable {

        /// <summary>
        /// The highest frequency this monitor can detect. May need to be increased or customizable
        /// if the tuner begins supporting custom tunings.
        /// </summary>
        public const float HighestFrequency = 1500f;

        /// <summary>
        /// The lowest frequency this monitor can detect.
        /// </summary>
        public const float LowestFrequency = 20f;

        private readonly float[] _buffer;
        private readonly BufferedWaveProvider _bufferedWaveProvider;
        private readonly int _byteBufferSize;
        private readonly BiQuadFilter _highPassFilter;
        private readonly int _highPeriod;
        private readonly object _lock = new object();
        private readonly BiQuadFilter _lowPassFilter;
        private readonly int _lowPeriod;
        private readonly RollingAverageFrequency _rollingAverageFrequency = new RollingAverageFrequency(5);
        private readonly ISampleProvider _sampleProvider;
        private readonly ITuning _tuning;
        private readonly IWaveIn _waveIn;
        private float _frequency;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyMonitor" /> class.
        /// </summary>
        public FrequencyMonitor(IWaveIn waveIn, ITuning tuning) {
            this._waveIn = waveIn ?? throw new ArgumentNullException(nameof(waveIn));
            this._tuning = tuning ?? throw new ArgumentNullException(nameof(tuning));

            this._lowPassFilter = BiQuadFilter.LowPassFilter(this.SampleRate, LowestFrequency, 1f);
            this._highPassFilter = BiQuadFilter.HighPassFilter(this.SampleRate, HighestFrequency, 1f);
            this._lowPeriod = (int)Math.Floor(this.SampleRate / HighestFrequency);
            this._highPeriod = (int)Math.Ceiling(this.SampleRate / LowestFrequency);

            this._buffer = new float[this._highPeriod * 2];
            this._byteBufferSize = this._buffer.Length * 4;

            this._waveIn.DataAvailable += this.WaveIn_DataAvailable;
            this._bufferedWaveProvider = new BufferedWaveProvider(this._waveIn.WaveFormat) {
                BufferLength = this._byteBufferSize,
                DiscardOnBufferOverflow = true,
                ReadFully = false
            };

            if (this._waveIn.WaveFormat.Channels == 2) {
                // adjust the volume of the channels based on selected input later
                this._sampleProvider = this._bufferedWaveProvider.ToSampleProvider().ToMono(0.5f, 0.5f);
            }
            else if (this._waveIn.WaveFormat.Channels == 1) {
                this._sampleProvider = this._bufferedWaveProvider.ToSampleProvider();
            }
            else {
                throw new ArgumentOutOfRangeException("Yo, your input device has way too many channels and I didn't account for it.");
            }
        }

        /// <summary>
        /// Gets or sets the frequency in Hz.
        /// </summary>
        /// <value>The frequency in Hz.</value>
        public float Frequency {
            get {
                return this._frequency;
            }

            set {
                this.Set(ref this._frequency, value);
            }
        }

        /// <summary>
        /// Gets the sample rate this frequency monitor is currently using.
        /// </summary>
        /// <value>The sample rate this frequency monitor is currently using.</value>
        public int SampleRate {
            get {
                return this._waveIn.WaveFormat.SampleRate;
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            if (!this._isDisposed) {
                this._waveIn.DataAvailable -= this.WaveIn_DataAvailable;
                this._isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        private BufferInformation GetBufferInformation() {
            if (this._buffer.Length < this._highPeriod) {
                throw new InvalidOperationException("The sample rate isn't large enough for the buffer length.");
            }

            var greatestMagnitude = float.NegativeInfinity;
            var chosenPeriod = -1;

            for (var period = this._lowPeriod; period < this._highPeriod; period++) {
                var sum = 0f;
                for (var i = 0; i < this._buffer.Length - period; i++) {
                    sum += this._buffer[i] * this._buffer[i + period];
                }

                var newMagnitude = sum / this._buffer.Length;
                if (newMagnitude > greatestMagnitude) {
                    chosenPeriod = period;
                    greatestMagnitude = newMagnitude;
                }
            }

            var frequency = (float)this.SampleRate / chosenPeriod;
            return frequency < this._tuning.MinimumFrequency || frequency > this._tuning.MaxinimumFrequency ?
                BufferInformation.Unknown :
                new BufferInformation((float)frequency, greatestMagnitude);
        }

        private void Update() {
            lock (this._lock) {
                this._sampleProvider.Read(this._buffer, 0, this._buffer.Length);

                if (this._buffer.Length > 0 && this._buffer[^2] != 0f) {
                    for (var i = 0; i < this._buffer.Length; i++) {
                        this._buffer[i] = this._highPassFilter.Transform(this._lowPassFilter.Transform(this._buffer[i])) * (float)FastFourierTransform.HammingWindow(i, this._buffer.Length);
                    }

                    var bufferInformation = this.GetBufferInformation();
                    this._rollingAverageFrequency.Add(bufferInformation.Frequency);
                    this.Frequency = this._rollingAverageFrequency.AverageFrequency;
                    this.Frequency = bufferInformation.Frequency;
                }
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
            if (e.BytesRecorded > 0) {
                this._bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                if (this._bufferedWaveProvider.BufferedBytes >= this._byteBufferSize) {
                    this.Update();
                }
            }
        }
    }
}