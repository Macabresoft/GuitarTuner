namespace Macabresoft.GuitarTuner.UI.Desktop;

using Macabresoft.AvaloniaEx;
using Macabresoft.GuitarTuner.Library.Input;
using Macabresoft.GuitarTuner.Library.Tuning;
using Unity;

public class MainWindowViewModel : BaseDialogViewModel {
    public MainWindowViewModel() : this(Resolver.Resolve<ISampleProvider>(), Resolver.Resolve<ITuning>()) {
    }

    [InjectionConstructor]
    public MainWindowViewModel(ISampleProvider sampleProvider, ITuning tuning) {
        this.SampleProvider = sampleProvider;
    }

    public ISampleProvider SampleProvider { get; }
}