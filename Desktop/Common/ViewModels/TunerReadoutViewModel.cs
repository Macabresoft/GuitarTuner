namespace Macabresoft.GuitarTuner.Desktop.Common;

using System.Windows.Input;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;
using Macabresoft.GuitarTuner.Library.Tuning;
using ReactiveUI;
using Unity;

/// <summary>
/// A view model for tuner readouts.
/// </summary>
public class TunerReadoutViewModel : ReactiveObject {
    /// <summary>
    /// The hold time for a note in seconds. For instance, if a user hits the note E and
    /// then provides no sound for 3 seconds, the frequency will continue to report E
    /// until those 3 seconds are up.
    /// </summary>
    private const float HoldTime = 3f;

    private readonly ISampleAnalyzer _sampleAnalyzer;
    private readonly ISampleProvider _sampleProvider;
    private readonly object _sampleProviderLock = new();
    private float _distanceFromBase;
    private float _frequency;
    private Note _note = Note.Empty;
    private float _peakVolume;
    private float _timeElapsed;
    private Note? _tuneToNote;

    /// <summary>
    /// Initializes a new instance of the <see cref="TunerReadoutViewModel" /> class.
    /// </summary>
    public TunerReadoutViewModel() : this(Resolver.Resolve<ISampleProvider>(), Resolver.Resolve<ITuning>()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TunerReadoutViewModel" /> class.
    /// </summary>
    /// <param name="sampleProvider">The sample provider.</param>
    /// <param name="tuning">The tuning.</param>
    [InjectionConstructor]
    public TunerReadoutViewModel(ISampleProvider sampleProvider, ITuning tuning) {
        this.SelectedTuning = tuning;
        this._sampleProvider = sampleProvider;
        this._sampleAnalyzer = new SampleAnalyzer(this._sampleProvider.SampleRate, this.SelectedTuning);
        this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
        this._sampleProvider.Start();

        this.SelectTuneToNoteCommand = ReactiveCommand.Create<Note?>(this.SelectTuneToNote);
    }

    /// <summary>
    /// Gets the selected tuning.
    /// </summary>
    public ITuning SelectedTuning { get; }

    /// <summary>
    /// Gets a command to select the <see cref="TuneToNote" />.
    /// </summary>
    public ICommand SelectTuneToNoteCommand { get; }

    /// <summary>
    /// Gets the distance the current frequency is from the base note of the tuning.
    /// </summary>
    public float DistanceFromBase {
        get => this._distanceFromBase;
        private set => this.RaiseAndSetIfChanged(ref this._distanceFromBase, value);
    }

    /// <summary>
    /// Gets the frequency.
    /// </summary>
    public float Frequency {
        get => this._frequency;
        private set {
            if (Math.Abs(this._frequency - value) > 0.01f) {
                this.RaiseAndSetIfChanged(ref this._frequency, value);
                this.ResetNote();
            }
        }
    }

    /// <summary>
    /// Gets the current nearest note to the current frequency.
    /// </summary>
    public Note Note {
        get => this._note;
        private set => this.RaiseAndSetIfChanged(ref this._note, value);
    }

    /// <summary>
    /// Gets the peak volume.
    /// </summary>
    public float PeakVolume {
        get => this._peakVolume;
        private set => this.RaiseAndSetIfChanged(ref this._peakVolume, value);
    }

    /// <summary>
    /// Gets or sets the note for which to tune towards.
    /// </summary>
    /// <remarks>
    /// When this is null, automatic note detection will be enabled (much like your standard guitar tuner).
    /// </remarks>
    public Note? TuneToNote {
        get => this._tuneToNote;
        private set {
            this.RaiseAndSetIfChanged(ref this._tuneToNote, value);
            this.ResetNote();
        }
    }

    private void ClearFrequency() {
        this._timeElapsed = 0f;
        this.Frequency = 0f;
        this.PeakVolume = 0f;
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

    private void ResetNote() {
        var note = this.SelectedTuning.GetNearestNote(this.Frequency, out var distanceFromBase);
        this.Note = this.TuneToNote ?? note;
        this.DistanceFromBase = (float)distanceFromBase;
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

    private void SelectTuneToNote(Note? note) {
        this.TuneToNote = note;
    }
}