namespace Macabresoft.GuitarTuner.Desktop.Common;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Macabresoft.GuitarTuner.Library;

public class NeedlePositionConverter : IMultiValueConverter {
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) {
        var note = values.OfType<Note>().FirstOrDefault();
        var distanceFromBase = values.OfType<float>().FirstOrDefault();
        var canvasBounds = values.OfType<Rect>().FirstOrDefault();
        var position = -100d;
        if (note != null && note != Note.Empty && distanceFromBase < double.PositiveInfinity && canvasBounds != Rect.Empty) {
            if (distanceFromBase > note.DistanceFromBase + 1) {
                position = canvasBounds.Width;
            }
            else if (distanceFromBase < note.DistanceFromBase - 1) {
                position = 0d;
            }
            else {
                var difference = Math.Abs(note.DistanceFromBase + 1 - distanceFromBase);
                if (difference < 2d) {
                    position = (2d - difference) * 0.5d * canvasBounds.Width;
                }
            }
        }

        return position;
    }
}