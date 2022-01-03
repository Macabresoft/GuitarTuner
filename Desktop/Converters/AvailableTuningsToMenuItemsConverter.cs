namespace Macabresoft.GuitarTuner.Desktop;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Macabresoft.GuitarTuner.Library;
using ReactiveUI;

/// <summary>
/// Converts a list of tunings on the <see cref="ITuningService" /> to menu items for selection.
/// </summary>
public sealed class AvailableTuningsToMenuItemsConverter : IValueConverter {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        var result = new List<MenuItem>();

        if (value is ITuningService tuningService) {
            var selectCommand = ReactiveCommand.Create<ITuning>(x => tuningService.SelectedTuning = x);

            result.AddRange(tuningService.AvailableTunings.Select(
                tuning => new MenuItem {
                    Header = tuning.DisplayName,
                    Command = selectCommand,
                    CommandParameter = tuning
                }));
        }

        return result;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}