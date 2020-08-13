namespace Macabresoft.Zvukosti.Windows.Controls {

    using Macabresoft.Zvukosti.Library.Tuning;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public partial class PitchDisplayControl : UserControl {

        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register(
            nameof(Frequency),
            typeof(float),
            typeof(PitchDisplayControl),
            new PropertyMetadata(0f, new PropertyChangedCallback(OnFrequencyChanged)));

        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
            nameof(Note),
            typeof(Note),
            typeof(PitchDisplayControl),
            new PropertyMetadata(Note.Empty, new PropertyChangedCallback(OnNoteChanged)));

        private float _flatScale;
        private float _halfWidth;
        private float _sharpScale;

        public PitchDisplayControl() {
            this.InitializeComponent();
            this.Loaded += this.PitchDisplayControl_Loaded;
            this.SizeChanged += this.PitchDisplayControl_SizeChanged;
            this.Unloaded += this.PitchDisplayControl_Unloaded;
        }

        public float Frequency {
            get { return (float)this.GetValue(FrequencyProperty); }
            set { this.SetValue(FrequencyProperty, value); }
        }

        public Note Note {
            get { return (Note)this.GetValue(NoteProperty); }
            set { this.SetValue(NoteProperty, value); }
        }

        private static void OnFrequencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is PitchDisplayControl control) {
                control.MoveNeedle();
            }
        }

        private static void OnNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is PitchDisplayControl control) {
                control.ResetCanvas();
            }
        }

        private void MoveNeedle() {
            if (this.Frequency == this.Note.Frequency) {
                this.SetNeedlePosition(this._halfWidth);
            }
            else if (this.Frequency < this.Note.Frequency) {
                this.SetNeedlePosition((float)Math.Max(0f, (this.Frequency - this.Note.StepDownFrequency) * this._flatScale));
            }
            else {
                this.SetNeedlePosition((float)Math.Min(this.ActualWidth, this._halfWidth + ((this.Frequency - this.Note.Frequency) * this._sharpScale)));
            }
        }

        private void PitchDisplayControl_Loaded(object sender, RoutedEventArgs e) {
            this.ResetCanvas();
            this.Loaded -= this.PitchDisplayControl_Loaded;
        }

        private void PitchDisplayControl_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.ResetCanvas();
        }

        private void PitchDisplayControl_Unloaded(object sender, RoutedEventArgs e) {
            this.SizeChanged -= this.PitchDisplayControl_SizeChanged;
            this.Unloaded -= this.PitchDisplayControl_Unloaded;
        }

        private void ResetCanvas() {
            if (this.Note != Note.Empty) {
                this._needle.Visibility = Visibility.Visible;
                this._halfWidth = (float)this.ActualWidth * 0.5f;
                var flatDifference = this.Note.Frequency - this.Note.StepDownFrequency;
                this._flatScale = flatDifference > 0f ? this._halfWidth / (flatDifference) : 0f;
                var sharpDifference = this.Note.StepUpFrequency - this.Note.Frequency;
                this._sharpScale = sharpDifference > 0f ? this._halfWidth / (sharpDifference) : 0f;
                this.MoveNeedle();
            }
            else {
                this._needle.Visibility = Visibility.Hidden;
            }
        }

        private void SetNeedlePosition(float x) {
            this._needle.X1 = x;
            this._needle.X2 = x;
        }
    }
}