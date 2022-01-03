namespace Macabresoft.GuitarTuner.UI.Desktop;

using System.Windows.Input;
using Macabresoft.AvaloniaEx;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;
using Macabresoft.GuitarTuner.Library.Tuning;
using ReactiveUI;
using Unity;

public class MainWindowViewModel : BaseDialogViewModel {
    public MainWindowViewModel() : this(Resolver.Resolve<ISampleProvider>(), Resolver.Resolve<ISampleService>(), Resolver.Resolve<ITuning>()) {
    }

    [InjectionConstructor]
    public MainWindowViewModel(ISampleProvider sampleProvider, ISampleService sampleService, ITuning tuning) {
        this.SampleProvider = sampleProvider;
        this.SampleService = sampleService;
        this.SelectedTuning = tuning;
        
        this.SelectTuneToNoteCommand = ReactiveCommand.Create<Note>(this.SelectTuneToNote);
    }

    // TODO: remove this
    public ISampleProvider SampleProvider { get; }
    
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
    }}