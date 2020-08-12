namespace Macabresoft.Zvukosti.Windows {

    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Tuning;
    using NAudio.MediaFoundation;
    using NAudio.Wave;
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// The main view model.
    /// </summary>
    public sealed class MainViewModel : NotifyPropertyChanged {
        private const int SampleRate = 8000;
        private readonly FrequencyMonitor _frequencyMonitor;
        private readonly DispatcherTimer _timer;
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

            this._frequencyMonitor = new FrequencyMonitor(this._waveIn, this.SelectedTuning);
            MediaFoundationApi.Startup();
            this._waveIn.StartRecording();

            this._timer = new DispatcherTimer() {
                Interval = new TimeSpan(0, 0, 0, 0, 100)
            };

            this._timer.Tick += this.Timer_Tick;
            this._timer.Start();
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

        private void Timer_Tick(object sender, EventArgs e) {
            this._timer.IsEnabled = false;
            this._frequencyMonitor.Update();
            if (this._frequencyMonitor.Frequency > 0f) {
                this.Frequency = this._frequencyMonitor.Frequency;
            }

            this._timer.IsEnabled = true;
        }
    }
}