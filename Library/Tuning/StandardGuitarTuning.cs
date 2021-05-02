namespace Macabresoft.GuitarTuner.Library.Tuning {

    /// <summary>
    /// Provides a tuning for standard guitars.
    /// </summary>
    /// <seealso cref="Macabresoft.GuitarTuner.Library.Tuning.GenericTuning"/>
    public sealed class StandardGuitarTuning : GenericTuning {

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGuitarTuning"/> class.
        /// </summary>
        public StandardGuitarTuning() : base(PitchNote.GetRange(2, Library.Notes.D, 4, Library.Notes.E)) {
            
        }

        /// <inheritdoc/>
        public override string DisplayName => "Standard Guitar";
    }
}