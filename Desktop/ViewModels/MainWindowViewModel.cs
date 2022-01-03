namespace Macabresoft.GuitarTuner.UI.Desktop;

using Macabresoft.AvaloniaEx;
using Macabresoft.GuitarTuner.Library.Input;
using Unity;

public class MainWindowViewModel : BaseDialogViewModel {
    public MainWindowViewModel() : this(Resolver.Resolve<ISampleProvider>()) {
    }

    [InjectionConstructor]
    public MainWindowViewModel(ISampleProvider sampleProvider) {
        this.SampleProvider = sampleProvider;
    }

    public ISampleProvider SampleProvider { get; }
}