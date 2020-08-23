namespace Macabresoft.Zvukosti.Desktop.Controls {

    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Markup.Xaml;
    using Macabresoft.Zvukosti.Library.Tuning;
    using System;

    public class PitchDisplayControl : UserControl {

        public static readonly DirectProperty<PitchDisplayControl, float> FrequencyProperty = AvaloniaProperty.RegisterDirect<PitchDisplayControl, float>(
            nameof(Frequency),
            x => x.Frequency,
            (x, v) => x.Frequency = v);

        public static readonly DirectProperty<PitchDisplayControl, Note> NoteProperty = AvaloniaProperty.RegisterDirect<PitchDisplayControl, Note>(
            nameof(Note),
            x => x.Note,
            (x, v) => x.Note = v);

        private float _flatScale;
        private float _frequency;
        private float _halfWidth;
        private Line _needle;
        private Note _note;
        private float _sharpScale;

        public PitchDisplayControl() {
            this.AttachedToVisualTree += this.PitchDisplayControl_AttachedToVisualTree;
            this.PropertyChanged += this.PitchDisplayControl_PropertyChanged;
            this.DetachedFromVisualTree += this.PitchDisplayControl_DetachedFromVisualTree;
            this.InitializeComponent();
        }

        public float Frequency {
            get {
                return this._frequency;
            }

            set {
                if (this.SetAndRaise(FrequencyProperty, ref this._frequency, value)) {
                    this.MoveNeedle();
                }
            }
        }

        public Note Note {
            get {
                return this._note;
            }
            set {
                if (this.SetAndRaise(NoteProperty, ref this._note, value)) {
                    this.ResetCanvas();
                }
            }
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
            this._needle = this.FindControl<Line>(nameof(this._needle));
        }

        private void MoveNeedle() {
            if (this.Width > 0f) {
                if (this.Frequency == this.Note.Frequency) {
                    this.SetNeedlePosition(this._halfWidth);
                }
                else if (this.Frequency < this.Note.Frequency) {
                    this.SetNeedlePosition((float)Math.Max(0f, (this.Frequency - this.Note.StepDownFrequency) * this._flatScale));
                }
                else {
                    this.SetNeedlePosition((float)Math.Min(this.Width, this._halfWidth + ((this.Frequency - this.Note.Frequency) * this._sharpScale)));
                }
            }
        }

        private void PitchDisplayControl_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e) {
            this.ResetCanvas();
            this.AttachedToVisualTree -= this.PitchDisplayControl_AttachedToVisualTree;
        }

        private void PitchDisplayControl_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e) {
            this.PropertyChanged -= this.PitchDisplayControl_PropertyChanged;
            this.DetachedFromVisualTree -= this.PitchDisplayControl_DetachedFromVisualTree;
        }

        private void PitchDisplayControl_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
            if (e.Property.Name == nameof(this.Width)) {
                this.ResetCanvas();
            }
        }

        private void ResetCanvas() {
            if (this.Note != Note.Empty && this.Width > 0f) {
                this._needle.IsVisible = true;
                this._halfWidth = (float)this.Width * 0.5f;
                var flatDifference = this.Note.Frequency - this.Note.StepDownFrequency;
                this._flatScale = flatDifference > 0f ? this._halfWidth / (flatDifference) : 0f;
                var sharpDifference = this.Note.StepUpFrequency - this.Note.Frequency;
                this._sharpScale = sharpDifference > 0f ? this._halfWidth / (sharpDifference) : 0f;
                this.MoveNeedle();
            }
            else {
                this._needle.IsVisible = false;
            }
        }

        private void SetNeedlePosition(float x) {
            this._needle.StartPoint = new Point(x - this._halfWidth, 0d);
            this._needle.EndPoint = new Point(x - this._halfWidth, this.Height);
        }
    }
}