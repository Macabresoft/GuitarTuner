namespace Macabresoft.GuitarTuner.UI.Desktop;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Macabresoft.GuitarTuner.Library;

/// <summary>
/// A converter for note names.
/// </summary>
public class NoteNameConverter : IValueConverter {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var result = AvaloniaProperty.UnsetValue;

        if (value is Note note) {
            result = note == Note.Empty ? "Auto" : note.Name;
        }

        return result;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}