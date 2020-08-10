namespace Macabresoft.Zvukosti.Windows {

    using System.Diagnostics;
    using System.Windows;
    using Unity;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow() {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        [Dependency]
        public MainViewModel ViewModel {
            get {
                return this.DataContext as MainViewModel;
            }

            set {
                this.DataContext = value;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void ViewSource_Click(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo("cmd", "/c start https://github.com/Macabresoft/zvukosti-tuner-desktop") {
                    CreateNoWindow = true
                });
        }
    }
}