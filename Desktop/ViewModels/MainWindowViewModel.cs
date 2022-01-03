namespace Macabresoft.GuitarTuner.Desktop;

using System.Windows.Input;
using Macabresoft.AvaloniaEx;
using Macabresoft.GuitarTuner.Library;
using ReactiveUI;
using Unity;

/// <summary>
/// A view model for the main window.
/// </summary>
public class MainWindowViewModel : BaseDialogViewModel {
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
    /// </summary>
    public MainWindowViewModel() : this(Resolver.Resolve<ISampleProvider>(), Resolver.Resolve<ISampleService>(), Resolver.Resolve<ITuningService>()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
    /// </summary>
    /// <param name="sampleProvider">The sample provider.</param>
    /// <param name="sampleService"></param>
    /// <param name="tuningService">The tuning service.</param>
    [InjectionConstructor]
    public MainWindowViewModel(ISampleProvider sampleProvider, ISampleService sampleService, ITuningService tuningService) {
        this.SampleProvider = sampleProvider;
        this.SampleService = sampleService;
        this.TuningService = tuningService;

        this.SelectTuneToNoteCommand = ReactiveCommand.Create<Note>(this.SelectTuneToNote);
    }

    // TODO: remove this
    public ISampleProvider SampleProvider { get; }

    /// <summary>
    /// Gets the sample service.
    /// </summary>
    public ISampleService SampleService { get; }

    /// <summary>
    /// Gets a command to select the note to tune to.
    /// </summary>
    public ICommand SelectTuneToNoteCommand { get; }

    /// <summary>
    /// Gets the tuning service.
    /// </summary>
    public ITuningService TuningService { get; }

    private void SelectTuneToNote(Note note) {
        this.SampleService.TuneToNote = note;
    }
}