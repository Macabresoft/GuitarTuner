namespace Macabresoft.Tuner.Desktop.Controls {

    using Avalonia;
    using Avalonia.Collections;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using System.Collections.Generic;
    using System.Linq;

    public class SkullControl : UserControl {
        private const int PixelHeight = 7;
        private const int PixelWidth = 8;

        private static readonly IReadOnlyCollection<Point> _leftEyeDefaults = new List<Point>() {
            new Point(2, 2),
            new Point(3, 2),
            new Point(3, 4),
            new Point(2, 4)
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
            new Point(1, PixelHeight - 2)
        };

        private static readonly IReadOnlyCollection<Point> _rightEyeDefaults = new List<Point>() {
            new Point(5, 2),
            new Point(6, 2),
            new Point(6, 4),
            new Point(5, 4)
        };

        private readonly List<Line> _horizontalGridLines = new List<Line>();
        private readonly List<Line> _verticalGridLines = new List<Line>();

        private Canvas _canvas;
        private Polygon _leftEye;
        private Polygon _outline;
        private Polygon _rightEye;

        public SkullControl() {
            this.PropertyChanged += this.SkullControl_PropertyChanged;
            this.AttachedToVisualTree += this.SkullControl_AttachedToVisualTree;
            this.DetachedFromVisualTree += this.SkullControl_DetachedFromVisualTree;
            this.InitializeComponent();
        }

        private Line CreateLine() {
            var line = new Line();
            this._canvas.Children.Add(line);
            line.StrokeThickness = 1;
            return line;
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
            this._canvas = this.FindControl<Canvas>(nameof(this._canvas));
            this._outline = this.FindControl<Polygon>(nameof(this._outline));
            this._outline.Points = new AvaloniaList<Point>();
            this._leftEye = this.FindControl<Polygon>(nameof(this._leftEye));
            this._leftEye.Points = new AvaloniaList<Point>();
            this._rightEye = this.FindControl<Polygon>(nameof(this._rightEye));
            this._rightEye.Points = new AvaloniaList<Point>();
        }

        private void ResetCanvas() {
            if (this.Width > 0) {
                var scale = this.Width / PixelWidth;
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

                var stroke = this.FindResource("TransparentAccentBrush") as Brush;

                var height = PixelHeight * scale;
                for (var x = 1; x < PixelWidth; x++) {
                    var line = this._verticalGridLines.ElementAt(x - 1);
                    line.StartPoint = new Point(x * scale, 0f);
                    line.EndPoint = new Point(line.StartPoint.X, height);
                    line.Stroke = stroke;
                }

                var width = PixelWidth * scale;
                for (var y = 1; y < PixelHeight; y++) {
                    var line = this._horizontalGridLines.ElementAt(y - 1);
                    line.StartPoint = new Point(0f, y * scale);
                    line.EndPoint = new Point(width, line.StartPoint.Y);
                    line.Stroke = stroke;
                }
            }
        }

        private void SkullControl_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e) {
            this.ResetCanvas();
            this.AttachedToVisualTree -= this.SkullControl_AttachedToVisualTree;
        }

        private void SkullControl_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e) {
            this.PropertyChanged -= this.SkullControl_PropertyChanged;
            this.DetachedFromVisualTree -= this.SkullControl_DetachedFromVisualTree;
        }

        private void SkullControl_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
            if (e.Property.Name == nameof(this.Width)) {
                this.ResetCanvas();
            }
        }
    }
}