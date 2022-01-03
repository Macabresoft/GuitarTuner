namespace Macabresoft.GuitarTuner.Desktop;

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;
using OpenTK.Audio.OpenAL;
using Unity;

/// <inheritdoc />
public class App : Application {
    private const string SimulationArg = "--simulate";

    /// <summary>
    /// Gets a value indicating whether or not this is a simulated environment.
    /// </summary>
    public static bool IsSimulated { get; private set; }

    /// <inheritdoc />
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted() {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            Resolver.Container.RegisterType<ISampleService, SampleService>();
            Resolver.Container.RegisterType<ISampleAnalyzer, SampleAnalyzer>();
            Resolver.Container.RegisterType<ITuningService, TuningService>();

            var tuning = Resolver.Resolve<ITuningService>().SelectedTuning;
            var bufferSize = (int)Math.Ceiling(SampleRates.Default / tuning.MinimumFrequency) * 2;
            ISampleProvider sampleProvider;
            if (desktop.Args.Any(x => string.Equals(x, SimulationArg, StringComparison.OrdinalIgnoreCase))) {
                IsSimulated = true;
                sampleProvider = new SimulatedSampleProvider(bufferSize);
            }
            else {
                sampleProvider = new MicrophoneSampleProvider(ALFormat.Mono16, bufferSize);
            }

            Resolver.Container.RegisterInstance(sampleProvider);
            desktop.MainWindow = Resolver.Resolve<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}