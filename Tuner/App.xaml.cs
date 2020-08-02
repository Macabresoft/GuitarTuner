namespace Macabresoft.Zvukosti.Tuner {

    using NAudio.Wave;
    using System.Windows;
    using Unity;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private readonly IUnityContainer _container = new UnityContainer();
        private MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            this._container.RegisterType<IWaveIn, WaveIn>();
            this._mainWindow = this._container.Resolve<MainWindow>();
            this._mainWindow.Show();
        }
    }
}