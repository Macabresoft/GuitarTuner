namespace Macabresoft.Zvukosti.Desktop.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive;
    using System.Windows.Input;
    using Avalonia.Threading;
    using Macabresoft.Core;
    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Input;
    using Macabresoft.Zvukosti.Library.Tuning;
    using OpenToolkit.Audio.OpenAL;
    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase {
        private const int SampleRate = 44100;
        private readonly ObservableCollectionExtended<string> _availableDevices = new();
        private readonly FrequencyMonitor _frequencyMonitor;
        private readonly ReactiveCommand<string, Unit> _selectDeviceCommand;
        private MicrophoneListener _listener;
        private float _frequency;
        private Note _note;
        private string _selectedDevice;

        public MainWindowViewModel() {
            this._availableDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier).ToList());

            // TODO: save the previously selected available device and load that here if possible.
            this.SelectedDevice = this._availableDevices.FirstOrDefault();
            this._selectDeviceCommand = ReactiveCommand.Create<string, Unit>(this.SelectDevice);

            this._listener = this.CreateListener();
            this._frequencyMonitor = new FrequencyMonitor(this._listener);
            this._frequencyMonitor.PropertyChanged += this.FrequencyMonitor_PropertyChanged;
            this._listener.Start();
        }

        private MicrophoneListener CreateListener() {
            return new MicrophoneListener(this.SelectedDevice, SampleRate, ALFormat.Mono16, (int)Math.Ceiling(SampleRate / FrequencyMonitor.LowestFrequency) * 15);
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

        private Unit SelectDevice(string deviceName) {
            if (deviceName != this.SelectedDevice) {
                this.SelectedDevice = deviceName;
                this._listener.Stop();
                this._listener = this.CreateListener();
                this._frequencyMonitor.SetSampleProvider(this._listener);
                this._listener.Start();
            }

            return Unit.Default;
        }
    }
}