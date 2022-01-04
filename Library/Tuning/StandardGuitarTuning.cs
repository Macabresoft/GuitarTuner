namespace Macabresoft.GuitarTuner.Library;

/// <summary>
/// Provides a tuning for standard guitars.
/// </summary>
public sealed class StandardGuitarTuning : GenericTuning {
    private static readonly Note[] TuningNotes = {
        new(NamedNotes.E, 2),
        new(NamedNotes.A, 2),
        new(NamedNotes.D, 3),
        new(NamedNotes.G, 3),
        new(NamedNotes.B, 3),
        new(NamedNotes.E, 4)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardGuitarTuning" /> class.
    /// </summary>
    public StandardGuitarTuning() : base(
        TuningNotes,
        FrequencyCalculator.GetFrequency(NamedNotes.C, 2),
        FrequencyCalculator.GetFrequency(NamedNotes.G, 4)) {
    }

    /// <inheritdoc />
    public override string DisplayName => "Standard Guitar Tuning";
}