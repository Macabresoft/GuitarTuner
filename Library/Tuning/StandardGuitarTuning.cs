namespace Macabresoft.Zvukosti.Library.Tuning {

    /// <summary>
    /// Provides a tuning for standard guitars.
    /// </summary>
    /// <seealso cref="Macabresoft.Zvukosti.Library.Tuning.GenericTuning"/>
    public sealed class StandardGuitarTuning : GenericTuning {

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGuitarTuning"/> class.
        /// </summary>
        public StandardGuitarTuning() : base(Note.E2, Note.A2, Note.D3, Note.G3, Note.B3, Note.E4) {
        }

        /// <inheritdoc/>
        public override string DisplayName => "Standard Guitar";
    }
}