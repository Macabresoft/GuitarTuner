namespace Macabresoft.GuitarTuner.Library.Tuning;

/// <summary>
/// Provides a tuning for standard guitars.
/// </summary>
/// <seealso cref="Macabresoft.GuitarTuner.Library.Tuning.GenericTuning" />
public sealed class StandardGuitarTuning : GenericTuning {
    private static readonly Note[] StandardNotes = {
        new(NamedNotes.E, 2),
        new(NamedNotes.A, 3),
        new(NamedNotes.D, 3),
        new(NamedNotes.G, 3),
        new(NamedNotes.B, 4),
        new(NamedNotes.E, 4)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardGuitarTuning" /> class.
    /// </summary>
    public StandardGuitarTuning() : base(
        StandardNotes,
        FrequencyCalculator.GetFrequency(NamedNotes.D, 2),
        FrequencyCalculator.GetFrequency(NamedNotes.F, 4)) {
    }

    /// <inheritdoc />
    public override string DisplayName => "Standard Guitar";
}