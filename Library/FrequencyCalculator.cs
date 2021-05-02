namespace Macabresoft.GuitarTuner.Library {
    using System;

    /// <summary>
    /// Calculates notes given a <see cref="Notes" /> and an octave.
    /// </summary>
    public static class FrequencyCalculator {
        internal const double BaseFrequency = 440d;
        internal const Notes BaseNote = Notes.A;
        internal const byte BaseOctave = 4;
        internal const byte NumberOfNotes = 12;

        /// <summary>
        /// Gets the frequency given a note and its octave.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="octave">The octave.</param>
        /// <returns>The frequency.</returns>
        public static double GetFrequency(Notes note, byte octave) {
            var distance = GetDistanceFromBase(note, octave);
            return BaseFrequency * Math.Pow(2, distance / (double)NumberOfNotes);
        }

        internal static int GetDistanceFromBase(Notes note, byte octave) {
            var octavesAway = octave - BaseOctave;
            var noteDifference = (int)note - (int)BaseNote;
            return octavesAway * 12 + noteDifference;
        }
    }
}