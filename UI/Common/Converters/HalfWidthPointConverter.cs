namespace Macabresoft.GuitarTuner.UI.Common;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

public sealed class HalfWidthPointConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is Point(var x, var y) ? new Point(x * 0.5d, y) : AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is Point(var x, var y) ? new Point(x * 2d, y) : AvaloniaProperty.UnsetValue;
    }
}