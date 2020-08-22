using Avalonia.Threading;
using Macabresoft.Zvukosti.Library;
using Macabresoft.Zvukosti.Library.Tuning;
using NAudio.Wave;

namespace Zvukosti.Desktop.ViewModels {

    public class MainWindowViewModel : ViewModelBase {
        private const int SampleRate = 44100;
        private readonly FrequencyMonitor _frequencyMonitor;
        private readonly WaveIn _waveIn;
        private float _frequency;
        private Note _note;

        public MainWindowViewModel() {
            ////var device = WaveIn.GetCapabilities(0);
            ////this._waveIn = new WaveIn {
            ////    WaveFormat = new WaveFormat(SampleRate, device.Channels),
            ////    DeviceNumber = 0,
            ////    BufferMilliseconds = 100
            ////};

            ////this._frequencyMonitor = new FrequencyMonitor(this._waveIn);
            ////MediaFoundationApi.Startup();
            ////this._waveIn.StartRecording();
            ////this._frequencyMonitor.PropertyChanged += this.FrequencyMonitor_PropertyChanged;
        }

        public float Frequency {
            get {
                return this._frequency;
            }

            private set {
                if (this.Set(ref this._frequency, value)) {
                    this.Note = this.SelectedTuning.GetNearestNote(this.Frequency);
                }
            }
        }

        public Note Note {
            get {
                return this._note;
            }

            private set {
                this.Set(ref this._note, value);
            }
        }

        public ITuning SelectedTuning { get; } = new StandardGuitarTuning();

        private void FrequencyMonitor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(FrequencyMonitor.Frequency)) {
                Dispatcher.UIThread.Post(() => this.Frequency = this._frequencyMonitor.Frequency);
            }
        }
    }
}