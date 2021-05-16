namespace Macabresoft.GuitarTuner.Desktop.Converters {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Avalonia;
    using Avalonia.Data.Converters;
    using Avalonia.Media;

    public abstract class BaseHighlightConverter : IMultiValueConverter {
        protected virtual float AcceptableDifference => 0.5f;
        protected abstract float DistanceOffset { get; }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture) {
            var defaultBrush = values[2] as Brush;
            if (values.Count == 4 &&
                values[0] is int noteDistanceFromBase &&
                values[1] is float distanceFromBase &&
                defaultBrush != null &&
                values[3] is Brush highLightBrush) {
                var difference = Math.Abs(distanceFromBase - noteDistanceFromBase - this.DistanceOffset);
                return difference < this.AcceptableDifference ? highLightBrush : defaultBrush;
            }

            return defaultBrush ?? AvaloniaProperty.UnsetValue;
        }
    }
}