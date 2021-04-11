using Avalonia;

namespace Macabresoft.Zvukosti.Desktop.Views {

    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using Avalonia.Platform;
    using Macabresoft.Core;

    public class MainWindow : Window {

        public MainWindow() {
            var platform = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            if (platform != OperatingSystemType.OSX) {
                this.SizeToContent = SizeToContent.WidthAndHeight;
            }
            
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);

            // This can't be set in the window's XAML because of an issue with
            // AvaloniaUI not sizing content correctly unless the rest of the
            // window is loaded first.
            var platform = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            if (platform == OperatingSystemType.OSX) {
                this.SizeToContent = SizeToContent.Height;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private void ViewSource_Click(object sender, RoutedEventArgs e) {
            WebHelper.OpenInBrowser("https://github.com/Macabresoft/zvukosti-tuner");
        }
    }
}