namespace Macabresoft.Zvukosti.Tuner.Controls {
    // This file was largely adapted from NAudio's sample project.

    using NAudio.Dsp;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SpectrumAnalyser.xaml
    /// </summary>
    public partial class SpectrumAnalyser : UserControl {
        private const int BinsPerPoint = 2;
        private int _bins = 512;
        private int _updateCount;
        private double _xScale = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumAnalyser"/> class.
        /// </summary>
        public SpectrumAnalyser() {
            this.InitializeComponent();
            this.CalculateXScale();
            this.SizeChanged += this.SpectrumAnalyser_SizeChanged;
        }

        public void Update(Complex[] fftResults) {
            // no need to repaint too many frames per second
            if (this._updateCount++ % 2 == 0) {
                return;
            }

            if (fftResults.Length / 2 != _bins) {
                this._bins = fftResults.Length / 2;
                this.CalculateXScale();
            }

            for (var n = 0; n < fftResults.Length / 2; n += BinsPerPoint) {
                // averaging out bins
                var yPos = 0d;
                for (var b = 0; b < BinsPerPoint; b++) {
                    yPos += GetYPosLog(fftResults[n + b]);
                }

                this.AddResult(n / BinsPerPoint, yPos / BinsPerPoint);
            }
        }

        private void AddResult(int index, double power) {
            Point p = new Point(CalculateXPos(index), power);
            if (index >= this._polyLine.Points.Count) {
                this._polyLine.Points.Add(p);
            }
            else {
                this._polyLine.Points[index] = p;
            }
        }

        private double CalculateXPos(int bin) {
            if (bin == 0) return 0;
            return bin * this._xScale;
        }

        private void CalculateXScale() {
            this._xScale = this.ActualWidth / (this._bins / BinsPerPoint);
        }

        private double GetYPosLog(Complex c) {
            // not entirely sure whether the multiplier should be 10 or 20 in this case. going with
            // 10 from here http://stackoverflow.com/a/10636698/7532
            var intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            var minDB = -90;

            if (intensityDB < minDB) {
                intensityDB = minDB;
            }

            var percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            return percent * this.ActualHeight;
        }

        private void SpectrumAnalyser_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.CalculateXScale();
        }
    }
}