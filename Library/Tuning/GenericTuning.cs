namespace Macabresoft.Zvukosti.Library.Tuning {

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
        protected GenericTuning(params Note[] notes) {
            this.Notes = notes.Any() ? notes.OrderBy(x => x.Frequency).ToList() : throw new ArgumentOutOfRangeException(nameof(notes));

            this.MaxinimumFrequency = notes.Select(x => x.StepUpFrequency).Max();
            this.MinimumFrequency = notes.Select(x => x.StepDownFrequency).Min();
        }

        /// <inheritdoc/>
        public abstract string DisplayName { get; }

        /// <inheritdoc/>
        public float MaxinimumFrequency { get; }

        /// <inheritdoc/>
        public float MinimumFrequency { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Note> Notes { get; }

        /// <inheritdoc/>
        public virtual Note GetNearestNote(float frequency) {
            return Notes.FirstOrDefault(x => (frequency > x.StepDownFrequency && frequency <= x.Frequency) || (frequency < x.StepUpFrequency && frequency >= x.Frequency));
        }
    }
}