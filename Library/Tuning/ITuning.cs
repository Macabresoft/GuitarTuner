namespace Macabresoft.GuitarTuner.Library;

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
    IReadOnlyCollection<Note> Notes { get; }

    /// <summary>
    /// Gets the nearest note to the provided frequency in this tuning.
    /// </summary>
    /// <param name="frequency">The frequency.</param>
    /// <param name="distanceFromBase">
    /// The number of semitones away the provided frequency is from the base note of A4 at
    /// 440Hz.
    /// </param>
    /// <returns>The nearest note.</returns>
    Note GetNearestNote(double frequency, out double distanceFromBase);
}