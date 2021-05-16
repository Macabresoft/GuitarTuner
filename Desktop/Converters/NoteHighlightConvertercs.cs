namespace Macabresoft.GuitarTuner.Desktop.Converters {
    public sealed class NoteHighlightConverter : BaseHighlightConverter {
        protected override float DistanceOffset => 0f;
        protected override float AcceptableDifference => 0.1f;
    }
}