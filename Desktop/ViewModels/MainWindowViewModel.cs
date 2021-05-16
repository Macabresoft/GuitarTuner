namespace Macabresoft.GuitarTuner.Desktop.ViewModels {
    using System;
    using Macabresoft.GuitarTuner.Desktop.Input;
    using Macabresoft.GuitarTuner.Library;
    using Macabresoft.GuitarTuner.Library.Input;
    using Macabresoft.GuitarTuner.Library.Tuning;
    using OpenTK.Audio.OpenAL;

    public class MainWindowViewModel : ViewModelBase {
        /// <summary>
        /// The hold time for a note in seconds. For instance, if a user hits the note E and
        /// then provides no sound for 3 seconds, the frequency will continue to report E
        /// until those 3 seconds are up.
        /// </summary>
        private const float HoldTime = 3f;

        private const int SampleRate = 44100;

        private readonly ISampleAnalyzer _sampleAnalyzer;
        private readonly ISampleProvider _sampleProvider;
        private readonly object _sampleProviderLock = new();
        private float _frequency;
        private Note _note = Note.Empty;
        private float _peakVolume;
        private float _timeElapsed;

        public MainWindowViewModel() {
            this._sampleProvider = this.CreateListener();
            this._sampleAnalyzer = new SampleAnalyzer(this._sampleProvider.SampleRate, this.SelectedTuning);
            this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
            this._sampleProvider.Start();
        }

        public ITuning SelectedTuning { get; } = new StandardGuitarTuning();

        public float Frequency {
            get => this._frequency;

            private set {
                if (this.Set(ref this._frequency, value)) {
                    this.Note = this.SelectedTuning.GetNearestNote(this.Frequency);
                }
            }
        }

        public Note Note {
            get => this._note;
            private set => this.Set(ref this._note, value);
        }

        public float PeakVolume {
            get => this._peakVolume;
            private set => this.Set(ref this._peakVolume, value);
        }

        private void ClearFrequency() {
            this._timeElapsed = 0f;
            this.Frequency = 0f;
            this.PeakVolume = 0f;
        }

        private MicrophoneListener CreateListener() {
            return new(ALFormat.Mono16, (int)Math.Ceiling(SampleRate / this.SelectedTuning.MinimumFrequency) * 2);
        }

        private void HoldForReset(int sampleCount) {
            if (this.Frequency != 0f && sampleCount > 0) {
                if (this._sampleProvider.SampleRate > 0) {
                    this._timeElapsed += sampleCount / (float)this._sampleProvider.SampleRate;

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
            lock (this._sampleProviderLock) {
                if (sender == this._sampleProvider) {
                    if (e.Samples.Length > 0 && e.Samples[^2] != 0f) {
                        var bufferInformation = this._sampleAnalyzer.GetBufferInformation(e.Samples);
                        this.PeakVolume = bufferInformation.PeakVolume;
                        if (bufferInformation.Frequency == 0f || bufferInformation.PeakVolume < 0.25f) {
                            this.HoldForReset(e.Samples.Length);
                        }
                        else {
                            this.Frequency = bufferInformation.Frequency;
                        }
                    }
                    else {
                        this.PeakVolume = 0f;
                        this.HoldForReset(e.Samples.Length);
                    }
                }
            }
        }
    }
}