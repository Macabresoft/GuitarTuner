﻿namespace Macabresoft.GuitarTuner.Desktop;

using Avalonia;
using Avalonia.ReactiveUI;

internal class Program {
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static void Main(string[] args) {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }
}