namespace Macabresoft.GuitarTuner.Desktop;

using System.ComponentModel;
using System.Linq;
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
    public MainWindowViewModel() : this(
        Resolver.Resolve<IAudioDeviceService>(),
        Resolver.Resolve<ISampleService>(),
        Resolver.Resolve<ITuningService>()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
    /// </summary>
    /// <param name="audioDeviceService">The audio device service.</param>
    /// <param name="sampleService">The sample service.</param>
    /// <param name="tuningService">The tuning service.</param>
    [InjectionConstructor]
    public MainWindowViewModel(IAudioDeviceService audioDeviceService, ISampleService sampleService, ITuningService tuningService) {
        this.AudioDeviceService = audioDeviceService;
        this.SampleService = sampleService;
        this.TuningService = tuningService;
        this.TuningService.PropertyChanged += this.TuningService_PropertyChanged;

        this.SelectAudioDeviceCommand = ReactiveCommand.Create<AudioDevice>(this.SelectAudioDevice);
        this.SelectTuneToNoteCommand = ReactiveCommand.Create<Note>(this.SelectTuneToNote);
        this.SelectTuningCommand = ReactiveCommand.Create<ITuning>(this.SelectTuning);
    }

    /// <summary>
    /// Gets the audio device service.
    /// </summary>
    public IAudioDeviceService AudioDeviceService { get; }

    /// <summary>
    /// Gets the sample service.
    /// </summary>
    public ISampleService SampleService { get; }

    /// <summary>
    /// Gets a command to select the audio device.
    /// </summary>
    public ICommand SelectAudioDeviceCommand { get; }

    /// <summary>
    /// Gets the selected audio device.
    /// </summary>
    public string? SelectedAudioDevice => null;

    /// <summary>
    /// Gets a command to select the note to tune to.
    /// </summary>
    public ICommand SelectTuneToNoteCommand { get; }

    /// <summary>
    /// Gets a command to select the tuning.
    /// </summary>
    public ICommand SelectTuningCommand { get; }

    /// <summary>
    /// Gets the tuning service.
    /// </summary>
    public ITuningService TuningService { get; }

    private void SelectAudioDevice(AudioDevice audioDevice) {
        this.AudioDeviceService.SelectDevice(audioDevice);
    }

    private void SelectTuneToNote(Note note) {
        this.SampleService.TuneToNote = note;
    }

    private void SelectTuning(ITuning tuning) {
        this.TuningService.SelectedTuning = tuning;
    }

    private void TuningService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(ITuningService.SelectedTuning)) {
            if (!this.TuningService.SelectedTuning.Notes.Any(x => x.Equals(this.SampleService.TuneToNote))) {
                this.SampleService.TuneToNote = Note.Auto;
            }
        }
    }
}