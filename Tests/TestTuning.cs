namespace Macabresoft.GuitarTuner.Tests;

using Macabresoft.GuitarTuner.Library;

internal class TestTuning : GenericTuning {
    public TestTuning() : base(Note.GetRange(2, NamedNotes.D, 4, NamedNotes.F)) {
    }

    public override string DisplayName => "Test Tuning";
}