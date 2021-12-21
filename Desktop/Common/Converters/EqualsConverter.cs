namespace Macabresoft.GuitarTuner.Desktop.Common;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

public class EqualsConverter : IMultiValueConverter {
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture) {
        return values.Any() && values.All(x => x != AvaloniaProperty.UnsetValue && x == values.First());
    }
}