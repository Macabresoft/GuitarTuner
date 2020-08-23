namespace Zvukosti.Desktop.ViewModels {

    using Avalonia.Threading;
    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Input;
    using Macabresoft.Zvukosti.Library.Tuning;
    using OpenToolkit.Audio.OpenAL;
    using System;

    public class MainWindowViewModel : ViewModelBase {
        private const int SampleRate = 44100;
        private readonly FrequencyMonitor _frequencyMonitor;
        private readonly ISampleProvider _sampleProvider;
        private float _frequency;
        private Note _note;

        public MainWindowViewModel() {
            var lowPeriod = (int)Math.Floor(SampleRate / FrequencyMonitor.HighestFrequency);
            var highPeriod = (int)Math.Ceiling(SampleRate / FrequencyMonitor.LowestFrequency);
            this._sampleProvider = new MicrophoneListener(null, SampleRate, ALFormat.Mono16, highPeriod * 2);
            this._frequencyMonitor = new FrequencyMonitor(this._sampleProvider, highPeriod, lowPeriod);
            this._frequencyMonitor.PropertyChanged += this.FrequencyMonitor_PropertyChanged;

            this._sampleProvider.Start();
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