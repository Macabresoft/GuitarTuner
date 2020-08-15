namespace Macabresoft.Zvukosti.Windows {

    using Macabresoft.Core;
    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Tuning;
    using NAudio.MediaFoundation;
    using NAudio.Wave;
    using System.Windows.Threading;

    /// <summary>
    /// The main view model.
    /// </summary>
    public sealed class MainViewModel : PropertyChangedNotifier {
        private const int SampleRate = 44100;
        private readonly FrequencyMonitor _frequencyMonitor;
        private readonly WaveIn _waveIn;
        private float _frequency;
        private Note _note;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel() {
            var device = WaveIn.GetCapabilities(0);
            this._waveIn = new WaveIn {
                WaveFormat = new WaveFormat(SampleRate, device.Channels),
                DeviceNumber = 0,
                BufferMilliseconds = 100
            };

            this._frequencyMonitor = new FrequencyMonitor(this._waveIn);
            MediaFoundationApi.Startup();
            this._waveIn.StartRecording();
            this._frequencyMonitor.PropertyChanged += this.FrequencyMonitor_PropertyChanged;
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
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

        /// <summary>
        /// Gets the note.
        /// </summary>
        /// <value>The note.</value>
        public Note Note {
            get {
                return this._note;
            }

            private set {
                this.Set(ref this._note, value);
            }
        }

        /// <summary>
        /// Gets the selected tuning.
        /// </summary>
        /// <value>The selected tuning.</value>
        public ITuning SelectedTuning { get; } = new StandardGuitarTuning();

        private void FrequencyMonitor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(FrequencyMonitor.Frequency)) {
                Dispatcher.CurrentDispatcher.BeginInvoke(() => this.Frequency = this._frequencyMonitor.Frequency);
            }
        }
    }
}