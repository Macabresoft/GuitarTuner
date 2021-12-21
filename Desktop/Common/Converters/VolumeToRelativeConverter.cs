namespace Macabresoft.GuitarTuner.Desktop.Common;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

public class VolumeToRelativeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is float magnitude) {
            return new RelativePoint(0d, 1d - magnitude, RelativeUnit.Relative);
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is RelativePoint relativePoint) {
            return relativePoint.Point.Y;
        }

        return AvaloniaProperty.UnsetValue;
    }
}