﻿namespace Macabresoft.GuitarTuner.Desktop;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Macabresoft.GuitarTuner.Library;

public class TunerReadout : UserControl {
    public static readonly StyledProperty<ISampleService> SampleServiceProperty =
        AvaloniaProperty.Register<TunerReadout, ISampleService>(nameof(SampleService));

    public TunerReadout() {
        this.InitializeComponent();
    }

    public ISampleService SampleService {
        get => this.GetValue(SampleServiceProperty);
        set => this.SetValue(SampleServiceProperty, value);
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}