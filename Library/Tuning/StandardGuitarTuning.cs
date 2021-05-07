namespace Macabresoft.GuitarTuner.Library.Tuning {

    /// <summary>
    /// Provides a tuning for standard guitars.
    /// </summary>
    /// <seealso cref="Macabresoft.GuitarTuner.Library.Tuning.GenericTuning"/>
    public sealed class StandardGuitarTuning : GenericTuning {

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGuitarTuning"/> class.
        /// </summary>
        public StandardGuitarTuning() : base(Note.GetRange(2, Library.NamedNotes.D, 4, Library.NamedNotes.E)) {
            
        }

        /// <inheritdoc/>
        public override string DisplayName => "Standard Guitar";
    }
}