namespace Macabresoft.GuitarTuner.Library.Tuning {

    using System.Collections.Generic;

    /// <summary>
    /// A tuning of notes. For example, standard tuning on a guitar or drop D tuning.
    /// </summary>
    public interface ITuning {

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets the maximum frequency.
        /// </summary>
        /// <value>The maximum frequency.</value>
        double MaximumFrequency { get; }

        /// <summary>
        /// Gets the minimum frequency.
        /// </summary>
        /// <value>The minimum frequency.</value>
        double MinimumFrequency { get; }

        /// <summary>
        /// Gets the notes.
        /// </summary>
        /// <value>The notes.</value>
        IReadOnlyCollection<PitchNote> Notes { get; }

        /// <summary>
        /// Gets the nearest note to the provided frequency in this tuning.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>The nearest note.</returns>
        PitchNote GetNearestNote(float frequency);
    }
}