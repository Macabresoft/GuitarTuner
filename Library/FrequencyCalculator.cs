namespace Macabresoft.GuitarTuner.Library;

using System;

/// <summary>
/// Calculates notes given a <see cref="NamedNotes" /> and an octave.
/// </summary>
public static class FrequencyCalculator {
    internal const double BaseFrequency = 440d;
    internal const NamedNotes BaseNote = NamedNotes.A;
    internal const byte BaseOctave = 4;
    internal const byte NumberOfNotes = 12;
    private static readonly double FrequencyConstant = Math.Pow(2d, 1d / NumberOfNotes);
    private static readonly double FrequencyConstantLog = Math.Log(FrequencyConstant);

    /// <summary>
    /// Gets the distance in semitones from the base note, which is A4 at 440Hz in this instance.
    /// </summary>
    /// <param name="namedNote">The note.</param>
    /// <param name="octave">The octave.</param>
    /// <returns>The distance in semitones from A4 at 440Hz.</returns>
    public static int GetDistanceFromBase(NamedNotes namedNote, byte octave) {
        var octavesAway = octave - BaseOctave;
        var noteDifference = (int)namedNote - (int)BaseNote;
        return octavesAway * NumberOfNotes + noteDifference;
    }

    /// <summary>
    /// Gets the distance in semitones from the base note, which is A4 at 440Hz in this instance.
    /// This method returns a float, because a frequency may lie between two semitones.
    /// </summary>
    /// <param name="frequency">The frequency.</param>
    /// <returns>The distance in semitones from A4 at 440Hz.</returns>
    public static double GetDistanceFromBase(double frequency) {
        // This is the inverse of the formula to get a frequency.
        return frequency > 0d ? Math.Log(frequency / BaseFrequency) / FrequencyConstantLog : 0d;
    }

    /// <summary>
    /// Gets the frequency given a note and its octave.
    /// </summary>
    /// <param name="namedNote">The note.</param>
    /// <param name="octave">The octave.</param>
    /// <returns>The frequency.</returns>
    public static double GetFrequency(NamedNotes namedNote, byte octave) {
        var distance = GetDistanceFromBase(namedNote, octave);
        return GetFrequency(distance);
    }

    /// <summary>
    /// Gets the frequency given the distance in semitones from A4 at 440Hz.
    /// </summary>
    /// <param name="distanceFromBase">The distance in semitones from A4 at 440Hz.</param>
    /// <returns>The frequency.</returns>
    public static double GetFrequency(int distanceFromBase) {
        return BaseFrequency * Math.Pow(FrequencyConstant, distanceFromBase);
    }
}