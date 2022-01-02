namespace Macabresoft.GuitarTuner.UI.Desktop;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

public sealed class HalfWidthPointConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is Point point ? new Point(point.X * 0.5d, point.Y) : AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is Point(var x, var y) ? new Point(x * 2d, y) : AvaloniaProperty.UnsetValue;
    }
}