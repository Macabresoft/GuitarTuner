namespace Macabresoft.GuitarTuner.Library.Tuning {

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A generic tuning that class that gives a default implementation of <see cref="GetNearestNote(float)"/>.
    /// </summary>
    public abstract class GenericTuning : ITuning {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTuning"/> class.
        /// </summary>
        /// <param name="notes">The notes.</param>
        protected GenericTuning(IEnumerable<NaturalNote> notes) {
            this.Notes = notes.OrderBy(x => x.Frequency).ToList();

            this.MaximumFrequency = this.Notes.Select(x => x.Frequency).Max();
            this.MinimumFrequency = this.Notes.Select(x => x.Frequency).Min();
        }

        /// <inheritdoc/>
        public abstract string DisplayName { get; }

        /// <inheritdoc/>
        public double MaximumFrequency { get; }

        /// <inheritdoc/>
        public double MinimumFrequency { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<NaturalNote> Notes { get; }

        /// <inheritdoc/>
        public virtual NaturalNote GetNearestNote(float frequency) {
            return Notes.FirstOrDefault(x => (frequency > x.StepDownFrequency && frequency <= x.Frequency) || (frequency < x.StepUpFrequency && frequency >= x.Frequency));
        }
    }
}