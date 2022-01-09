namespace Macabresoft.GuitarTuner.Library;

/// <summary>
/// Provides a tuning for bass guitars.
/// </summary>
public sealed class StandardBassTuning : GenericTuning {
    private static readonly Note[] TuningNotes = {
        new(NamedNotes.E, 1),
        new(NamedNotes.A, 1),
        new(NamedNotes.D, 2),
        new(NamedNotes.G, 2)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardGuitarTuning" /> class.
    /// </summary>
    public StandardBassTuning() : base(
        TuningNotes,
        FrequencyCalculator.GetFrequency(NamedNotes.C, 1),
        FrequencyCalculator.GetFrequency(NamedNotes.B, 2)) {
    }

    /// <inheritdoc />
    public override string DisplayName => "Standard Bass";
}