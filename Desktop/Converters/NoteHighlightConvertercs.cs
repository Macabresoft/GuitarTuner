namespace Macabresoft.GuitarTuner.Desktop;

public sealed class NoteHighlightConverter : BaseHighlightConverter {
    protected override float AcceptableDifference => 0.1f;
    protected override float DistanceOffset => 0f;
}