namespace Zvukosti.Desktop.Views {

    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using System.Diagnostics;

    public class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void ViewSource_Click(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo("cmd", "/c start https://github.com/Macabresoft/zvukosti-tuner-desktop") {
                    CreateNoWindow = true
                });
        }
    }
}