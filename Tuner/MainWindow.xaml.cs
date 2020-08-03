namespace Macabresoft.Zvukosti.Tuner {

    using Macabresoft.Zvukosti.Library;
    using System.Windows;
    using Unity;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
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
                if (value != null) {
                    value.FFTCalculated += this.Value_FFTCalculated;
                }
            }
        }

        private void Value_FFTCalculated(object sender, FFTEventArgs e) {
            this._spectrumAnalyser.Update(e.Result, e.SampleRate);
        }
    }
}