using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Macabresoft.Zvukosti.Windows.Controls {

    public partial class SkullControl : UserControl {
        private const int PixelHeight = 7;
        private const int PixelWidth = 8;

        private static readonly IReadOnlyCollection<Point> _leftEyeDefaults = new List<Point>() {
            new Point(2, 2),
            new Point(3, 2),
            new Point(3, 4),
            new Point(2, 4),
            new Point(2, 2)
        };

        private static readonly IReadOnlyCollection<Point> _outlineDefaults = new List<Point>() {
            new Point(1, 1),
            new Point(PixelWidth - 1, 1),
            new Point(PixelWidth - 1, PixelHeight - 2),
            new Point(PixelWidth - 2, PixelHeight - 2),
            new Point(PixelWidth - 2, PixelHeight - 1),
            new Point(4, PixelHeight - 1),
            new Point(4, PixelHeight - 2),
            new Point(3, PixelHeight - 2),
            new Point(3, PixelHeight - 1),
            new Point(2, PixelHeight - 1),
            new Point(2, PixelHeight - 2),
            new Point(1, PixelHeight - 2),
            new Point(1, 1)
        };

        private static readonly IReadOnlyCollection<Point> _rightEyeDefaults = new List<Point>() {
            new Point(5, 2),
            new Point(6, 2),
            new Point(6, 4),
            new Point(5, 4),
            new Point(5, 2)
        };

        private static readonly Brush _transparentGreen = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
        private readonly List<Line> _horizontalGridLines = new List<Line>();
        private readonly List<Line> _verticalGridLines = new List<Line>();

        public SkullControl() {
            this.InitializeComponent();
            this.Loaded += this.SkullControl_Loaded;
            this.Unloaded += this.SkullControl_Unloaded;
            this.SizeChanged += this.SkullControl_SizeChanged;
        }

        private Line CreateLine() {
            var line = new Line();
            this._canvas.Children.Add(line);
            line.Stroke = SkullControl._transparentGreen;
            line.StrokeThickness = 1;
            return line;
        }

        private void ResetCanvas() {
            var scale = ActualWidth / PixelWidth;
            this.Height = scale * PixelHeight;
            this._outline.Points.Clear();
            this._leftEye.Points.Clear();
            this._rightEye.Points.Clear();

            foreach (var point in SkullControl._outlineDefaults) {
                this._outline.Points.Add(new Point(point.X * scale, point.Y * scale));
            }

            foreach (var point in SkullControl._leftEyeDefaults) {
                this._leftEye.Points.Add(new Point(point.X * scale, point.Y * scale));
            }

            foreach (var point in SkullControl._rightEyeDefaults) {
                this._rightEye.Points.Add(new Point(point.X * scale, point.Y * scale));
            }

            while (this._verticalGridLines.Count < PixelWidth - 1) {
                this._verticalGridLines.Add(this.CreateLine());
            }

            while (this._horizontalGridLines.Count < PixelHeight - 1) {
                this._horizontalGridLines.Add(this.CreateLine());
            }

            var height = PixelHeight * scale;
            for (var x = 1; x < PixelWidth; x++) {
                var line = this._verticalGridLines.ElementAt(x - 1);
                line.X1 = x * scale;
                line.X2 = line.X1;
                line.Y1 = 0f;
                line.Y2 = height;
            }

            var width = PixelWidth * scale;
            for (var y = 1; y < PixelHeight; y++) {
                var line = this._horizontalGridLines.ElementAt(y - 1);
                line.X1 = 0f;
                line.X2 = width;
                line.Y1 = y * scale;
                line.Y2 = line.Y1;
            }
        }

        private void SkullControl_Loaded(object sender, RoutedEventArgs e) {
            this.ResetCanvas();
            this.Loaded -= this.SkullControl_Loaded;
        }

        private void SkullControl_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.ResetCanvas();
        }

        private void SkullControl_Unloaded(object sender, RoutedEventArgs e) {
            this.SizeChanged -= this.SkullControl_SizeChanged;
            this.Unloaded -= this.SkullControl_Unloaded;
        }
    }
}