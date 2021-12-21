namespace Macabresoft.GuitarTuner.Desktop.UI;

using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;
using Macabresoft.GuitarTuner.Library.Tuning;
using OpenTK.Audio.OpenAL;
using Unity;

public class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            // TODO: tuning and listeners should be provided dynamically, probably with a factory pattern
            var tuning = new StandardGuitarTuning();
            Resolver.Container.RegisterInstance<ITuning>(tuning);
            Resolver.Container.RegisterInstance<ISampleProvider>(new MicrophoneListener(ALFormat.Mono16, (int)Math.Ceiling(SampleRates.Default / tuning.MinimumFrequency) * 2));

            desktop.MainWindow = Resolver.Resolve<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}