namespace Macabresoft.GuitarTuner.UI.Desktop;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

public class TunerReadoutView : UserControl {
    public TunerReadoutView() {
        this.DataContext = Resolver.Resolve<TunerReadoutViewModel>();
        this.InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}