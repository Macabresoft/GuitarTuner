namespace Macabresoft.GuitarTuner.Desktop.Common;

using System.Globalization;
using Avalonia.Data.Converters;

/// <summary>
/// Converts a <see cref="float"/> to a string percentage representation.
/// </summary>
public class ToPercentageConverter : IValueConverter {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var percentage = "0%";
        if (value is float floatValue) {
            percentage = $"{floatValue * 100f:0.00}%";
        }

        return percentage;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}