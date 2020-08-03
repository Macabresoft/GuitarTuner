/*
 *  This file was largely adapted from NAudio's sample project.
 *  https://github.com/naudio/NAudio
 */

namespace Macabresoft.Zvukosti.Tuner.Controls {

    using NAudio.Dsp;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SpectrumAnalyzer.xaml
    /// </summary>
    public partial class SpectrumAnalyzer : UserControl {

        /// <summary>
        /// Dependency property for <see cref="Frequency"/>.
        /// </summary>
        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register(
            nameof(Frequency),
            typeof(float),
            typeof(SpectrumAnalyzer),
            new PropertyMetadata());

        private const int BinsPerPoint = 1;
        private int _bins = 512;
        private int _updateCount;
        private double _xScale = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumAnalyzer"/> class.
        /// </summary>
        public SpectrumAnalyzer() {
            this.InitializeComponent();
            this.CalculateXScale();
            this.SizeChanged += this.SpectrumAnalyser_SizeChanged;
        }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public float Frequency {
            get { return (float)this.GetValue(FrequencyProperty); }
            set { this.SetValue(FrequencyProperty, value); }
        }

        /// <summary>
        /// Updates with the specified FFT results.
        /// </summary>
        /// <param name="fftResults">The FFT results.</param>
        public void Update(Complex[] fftResults, int sampleRate) {
            // no need to repaint too many frames per second
            if (this._updateCount++ % 4 != 0 && fftResults.Length > 0) {
                if (fftResults.Length / 2 != _bins) {
                    this._bins = fftResults.Length / 2;
                    this.CalculateXScale();
                }

                var maximumIndex = 0;
                for (var n = 0; n < fftResults.Length / 2; n += 1) {
                    var fftResult = fftResults[n];

                    // not entirely sure whether the multiplier should be 10 or 20 in this case.
                    // going with 10 from here http://stackoverflow.com/a/10636698/7532
                    var intensityDB = 10 * Math.Log10(Math.Sqrt(fftResult.X * fftResult.X + fftResult.Y * fftResult.Y));

                    var minDB = -40; // Maybe make the minimum decibels configurable?
                    if (intensityDB < minDB) {
                        intensityDB = minDB;
                    }

                    var yPos = GetYPosLog(intensityDB, minDB);
                    this.AddResult(n / BinsPerPoint, yPos / BinsPerPoint);

                    if (intensityDB > minDB && fftResult.X > fftResults[maximumIndex].X) {
                        maximumIndex = n;
                    }
                }

                this.Frequency = maximumIndex * sampleRate / (float)fftResults.Length;
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
            // Maybe do this? Figure out if its backwards: bin == 0 ? this.ActualWidth :
            return bin == 0 ? 0 : bin * this._xScale;
        }

        private void CalculateXScale() {
            this._xScale = this.ActualWidth / (this._bins / BinsPerPoint);
        }

        private double GetYPosLog(double intensityDB, double minDB) {
            var percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            return Math.Clamp(percent, 0.05d, 0.95d) * this.ActualHeight;
        }

        private void SpectrumAnalyser_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.CalculateXScale();
        }
    }
}