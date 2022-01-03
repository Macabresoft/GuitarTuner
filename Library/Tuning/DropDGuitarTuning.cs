namespace Macabresoft.GuitarTuner.Library;

/// <summary>
/// Provides a tuning for Drop D on guitars.
/// </summary>
public sealed class DropDGuitarTuning : GenericTuning {
    private static readonly Note[] TuningNotes = {
        new(NamedNotes.D, 2),
        new(NamedNotes.A, 2),
        new(NamedNotes.D, 3),
        new(NamedNotes.G, 3),
        new(NamedNotes.B, 3),
        new(NamedNotes.E, 4)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="DropDGuitarTuning" /> class.
    /// </summary>
    public DropDGuitarTuning() : base(
        TuningNotes,
        FrequencyCalculator.GetFrequency(NamedNotes.B, 2),
        FrequencyCalculator.GetFrequency(NamedNotes.G, 4)) {
    }

    /// <inheritdoc />
    public override string DisplayName => "Drop D (Guitar)";
}