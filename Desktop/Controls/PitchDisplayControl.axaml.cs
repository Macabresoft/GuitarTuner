namespace Macabresoft.GuitarTuner.Desktop.Controls {
    using System;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Threading;
    using Macabresoft.GuitarTuner.Library;

    public class PitchDisplayControl : UserControl {
        public static readonly DirectProperty<PitchDisplayControl, float> FrequencyProperty = AvaloniaProperty.RegisterDirect<PitchDisplayControl, float>(
            nameof(Frequency),
            x => x.Frequency,
            (x, v) => x.Frequency = v);

        public static readonly DirectProperty<PitchDisplayControl, NaturalNote> NoteProperty = AvaloniaProperty.RegisterDirect<PitchDisplayControl, NaturalNote>(
            nameof(Note),
            x => x.Note,
            (x, v) => x.Note = v);

        private float _flatScale;
        private float _frequency;
        private float _halfWidth;
        private Line _needle;
        private NaturalNote _note = NaturalNote.Empty;
        private float _sharpScale;

        public PitchDisplayControl() {
            this.InitializeComponent();
        }

        public float Frequency {
            get => this._frequency;

            set {
                if (this.SetAndRaise(FrequencyProperty, ref this._frequency, value)) {
                    this.MoveNeedle();
                }
            }
        }


        public NaturalNote Note {
            get => this._note;
            set {
                if (this.SetAndRaise(NoteProperty, ref this._note, value)) {
                    Dispatcher.UIThread.Post(this.ResetCanvas, DispatcherPriority.Render);
                }
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);
            this.ResetCanvas();
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change) {
            base.OnPropertyChanged(change);

            if (change.Property.Name == nameof(this.Width)) {
                this.ResetCanvas();
            }
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
            this._needle = this.FindControl<Line>(nameof(this._needle));
        }

        private void MoveNeedle() {
            if (this.Width > 0f && this._halfWidth > 0f) {
                if (Math.Abs(this.Frequency - this.Note.Frequency) < 0.01f) {
                    this.SetNeedlePosition(this._halfWidth);
                }
                else if (this.Frequency < this.Note.Frequency) {
                    this.SetNeedlePosition((float)Math.Max(0f, (this.Frequency - this.Note.StepDownFrequency) * this._flatScale));
                }
                else {
                    this.SetNeedlePosition((float)Math.Min(this.Width, this._halfWidth + (this.Frequency - this.Note.Frequency) * this._sharpScale));
                }
            }
        }

        private void ResetCanvas() {
            if (this.Note.Frequency != 0f && this.Width > 0f) {
                this._needle.IsVisible = true;
                this._halfWidth = (float)this.Width * 0.5f;
                var flatDifference = this.Note.Frequency - this.Note.StepDownFrequency;
                this._flatScale = flatDifference > 0f ? this._halfWidth / (float)flatDifference : 0f;
                var sharpDifference = this.Note.StepUpFrequency - this.Note.Frequency;
                this._sharpScale = sharpDifference > 0f ? this._halfWidth / (float)sharpDifference : 0f;
                this.MoveNeedle();
            }
            else if (this._needle != null) {
                this._needle.IsVisible = false;
            }
        }

        private void SetNeedlePosition(float x) {
            /*this._needle.StartPoint = new Point(x - this._halfWidth, 0d);
            this._needle.EndPoint = new Point(x - this._halfWidth, this.Height);*/
        }
    }
}