namespace Macabresoft.GuitarTuner.Desktop.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Windows.Input;
    using Macabresoft.Core;
    using Macabresoft.GuitarTuner.Desktop.Input;
    using Macabresoft.GuitarTuner.Library;
    using Macabresoft.GuitarTuner.Library.Input;
    using Macabresoft.GuitarTuner.Library.Tuning;
    using OpenTK.Audio.OpenAL;
    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase {
        /// <summary>
        /// The hold time for a note in seconds. For instance, if a user hits the note E and
        /// then provides no sound for 3 seconds, the frequency will continue to report E
        /// until those 3 seconds are up.
        /// </summary>
        private const float HoldTime = 3f;

        private const int SampleRate = 44100;

        private readonly ObservableCollectionExtended<string> _availableDevices = new();
        private readonly ISampleAnalyzer _sampleAnalyzer;
        private readonly object _sampleProviderLock = new();
        private readonly ReactiveCommand<string, Unit> _selectDeviceCommand;
        private float _frequency;
        private float _magnitude;
        private Note _note = Note.Empty;
        private ISampleProvider _sampleProvider;
        private string _selectedDevice;
        private float _timeElapsed;

        public MainWindowViewModel() {
            this._availableDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier).ToList());

            // TODO: save the previously selected available device and load that here if possible.
            this._selectedDevice = this._availableDevices.Any() ? this._availableDevices.First() : string.Empty;
            this._selectDeviceCommand = ReactiveCommand.Create<string, Unit>(this.SelectDevice);
            this._sampleProvider = this.CreateListener();
            this._sampleAnalyzer = new SampleAnalyzer(this._sampleProvider.SampleRate, this.SelectedTuning);
            this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
            this._sampleProvider.Start();
        }

        public IReadOnlyCollection<string> AvailableDevices => this._availableDevices;

        public ICommand SelectDeviceCommand => this._selectDeviceCommand;

        public ITuning SelectedTuning { get; } = new StandardGuitarTuning();

        public float Frequency {
            get => this._frequency;

            private set {
                if (this.Set(ref this._frequency, value)) {
                    this.Note = this.SelectedTuning.GetNearestNote(this.Frequency);
                }
            }
        }

        public float Magnitude {
            get => this._magnitude;
            private set => this.Set(ref this._magnitude, value);
        }

        public Note Note {
            get => this._note;
            private set => this.Set(ref this._note, value);
        }

        public string SelectedDevice {
            get => this._selectedDevice;
            set => this.Set(ref this._selectedDevice, value);
        }

        private void ClearFrequency() {
            this._timeElapsed = 0f;
            this.Frequency = 0f;
            this.Magnitude = 0f;
        }

        private MicrophoneListener CreateListener() {
            return new(
                this.SelectedDevice,
                ALFormat.Mono16,
                (int)Math.Ceiling(SampleRate / this.SelectedTuning.MinimumFrequency) * 2);
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
                        this.Magnitude = bufferInformation.Magnitude;
                        if (bufferInformation.Frequency == 0f || bufferInformation.Magnitude < 0.4f) {
                            this.HoldForReset(e.Samples.Length);
                        }
                        else {
                            this.Frequency = bufferInformation.Frequency;
                        }
                    }
                    else {
                        this.Magnitude = 0f;
                        this.HoldForReset(e.Samples.Length);
                    }
                }
            }
        }

        private Unit SelectDevice(string deviceName) {
            if (deviceName != this.SelectedDevice) {
                this.SelectedDevice = deviceName;
                this._sampleProvider.Stop();
                this._sampleProvider.SamplesAvailable -= this.SampleProvider_SamplesAvailable;
                this._sampleProvider = this.CreateListener();
                this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
                this._sampleProvider.Start();
            }

            return Unit.Default;
        }
    }
}