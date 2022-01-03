namespace Macabresoft.GuitarTuner.UI.Desktop;

using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Macabresoft.AvaloniaEx;
using Macabresoft.Core;
using Unity;

public class MainWindow : BaseDialog {
    public MainWindow() : this(Resolver.Resolve<MainWindowViewModel>()) {
    }

    [InjectionConstructor]
    public MainWindow(MainWindowViewModel viewModel) {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }

    private void Exit_Click(object sender, RoutedEventArgs e) {
        this.Close();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }

    private void ViewSource_Click(object sender, RoutedEventArgs e) {
        WebHelper.OpenInBrowser("https://github.com/Macabresoft/guitar-tuner");
    }
}