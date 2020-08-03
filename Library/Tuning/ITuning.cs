using System.Collections.Generic;

namespace Macabresoft.Zvukosti.Library.Tuning {

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
        /// Gets the notes.
        /// </summary>
        /// <value>The notes.</value>
        IReadOnlyCollection<Note> Notes { get; }

        /// <summary>
        /// Gets the nearest note to the provided frequency in this tuning.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>The nearest note.</returns>
        Note GetNearestNote(float frequency);
    }
}