namespace Macabresoft.GuitarTuner.Desktop;

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;
using Unity;
using Unity.Lifetime;

/// <inheritdoc />
public class App : Application {
    private const string SimulationArg = "--simulate";

    /// <inheritdoc />
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted() {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            Resolver.Container.RegisterType<IAudioDeviceService, AudioDeviceService>(new SingletonLifetimeManager())
                .RegisterType<ISampleService, SampleService>(new SingletonLifetimeManager())
                .RegisterType<ISampleAnalyzer, SampleAnalyzer>(new SingletonLifetimeManager())
                .RegisterType<ITuningService, TuningService>(new SingletonLifetimeManager())
                .RegisterInstance<ISampleProvider>(new EmptySampleProvider());

            var audioDeviceService = Resolver.Resolve<IAudioDeviceService>();
            if (desktop.Args.Any(x => string.Equals(x, SimulationArg, StringComparison.OrdinalIgnoreCase))) {
                audioDeviceService.SelectDevice(audioDeviceService.AvailableInputDevices.First(
                    x => x.Name == AudioDevice.SimulatedName && x.DeviceType == AudioDeviceType.Miscellaneous));
            }
            else {
                audioDeviceService.SelectDevice(audioDeviceService.AvailableInputDevices.First(
                    x => x.Name == AudioDevice.DefaultInputName && x.DeviceType == AudioDeviceType.Input));
            }

            desktop.MainWindow = Resolver.Resolve<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}