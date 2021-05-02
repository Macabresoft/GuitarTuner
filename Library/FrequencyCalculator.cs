namespace Macabresoft.GuitarTuner.Library {
    using System;
    using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Gets the frequency one semitone down given a note and its octave.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="octave">The octave.</param>
        /// <returns>The frequency.</returns>
        public static double GetStepDownFrequency(Notes note, byte octave) {
            return note == Notes.C ? 
                GetFrequency(Notes.B, (byte)(octave - 1)) : 
                GetFrequency(note - 1, octave);
        }
        
        /// <summary>
        /// Gets the frequency one semitone down given a note and its octave.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="octave">The octave.</param>
        /// <returns>The frequency.</returns>
        public static double GetStepUpFrequency(Notes note, byte octave) {
            return note == Notes.B ? 
                GetFrequency(Notes.C, (byte)(octave + 1)) : 
                GetFrequency(note + 1, octave);
        }

        internal static int GetDistanceFromBase(Notes note, byte octave) {
            var octavesAway = octave - BaseOctave;
            var noteDifference = (int)note - (int)BaseNote;
            return octavesAway * 12 + noteDifference;
        }
    }
}