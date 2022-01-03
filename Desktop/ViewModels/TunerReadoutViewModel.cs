namespace Macabresoft.GuitarTuner.UI.Desktop;

using System.Windows.Input;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Tuning;
using ReactiveUI;
using Unity;

/// <summary>
/// A view model for tuner readouts.
/// </summary>
public class TunerReadoutViewModel : ReactiveObject {
    /// <summary>
    /// Initializes a new instance of the <see cref="TunerReadoutViewModel" /> class.
    /// </summary>
    public TunerReadoutViewModel() : this(Resolver.Resolve<ISampleService>(), Resolver.Resolve<ITuning>()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TunerReadoutViewModel" /> class.
    /// </summary>
    /// <param name="sampleService">The sample service.</param>
    /// <param name="tuning">The tuning.</param>
    [InjectionConstructor]
    public TunerReadoutViewModel(ISampleService sampleService, ITuning tuning) {
        this.SampleService = sampleService;
        this.SelectedTuning = tuning;
        this.SelectTuneToNoteCommand = ReactiveCommand.Create<Note>(this.SelectTuneToNote);
    }

    /// <summary>
    /// Gets the sample service.
    /// </summary>
    public ISampleService SampleService { get; }

    /// <summary>
    /// Gets the selected tuning.
    /// </summary>
    public ITuning SelectedTuning { get; }

    /// <summary>
    /// Gets a command to select the note to tune to.
    /// </summary>
    public ICommand SelectTuneToNoteCommand { get; }

    private void SelectTuneToNote(Note note) {
        this.SampleService.TuneToNote = note;
    }
}