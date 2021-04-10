namespace Macabresoft.Zvukosti.Desktop.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Avalonia.Threading;
    using Macabresoft.Core;
    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Input;
    using Macabresoft.Zvukosti.Library.Tuning;
    using OpenToolkit.Audio.OpenAL;

    public class MainWindowViewModel : ViewModelBase {
        private const int SampleRate = 44100;
        private readonly ObservableCollectionExtended<string> _availableDevices = new();
        private readonly FrequencyMonitor _frequencyMonitor;
        private float _frequency;
        private Note _note;
        private string _selectedDevice;

        public MainWindowViewModel() {
            this._availableDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier).ToList());

            // TODO: save the previously selected available device and load that here if possible.
            this.SelectedDevice = this._availableDevices.FirstOrDefault();

            var sampleProvider = new MicrophoneListener(this.SelectedDevice, SampleRate, ALFormat.Mono16, (int)Math.Ceiling(SampleRate / FrequencyMonitor.LowestFrequency) * 8);
            this._frequencyMonitor = new FrequencyMonitor(sampleProvider);
            this._frequencyMonitor.PropertyChanged += this.FrequencyMonitor_PropertyChanged;
            sampleProvider.Start();
        }

        public IReadOnlyCollection<string> AvailableDevices => this._availableDevices;

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

        public string SelectedDevice {
            get => this._selectedDevice;
            set => this.Set(ref this._selectedDevice, value);
        }

        private void FrequencyMonitor_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(FrequencyMonitor.Frequency)) {
                Dispatcher.UIThread.Post(() => this.Frequency = this._frequencyMonitor.Frequency);
            }
        }
    }
}