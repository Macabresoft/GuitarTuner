namespace Macabresoft.GuitarTuner.Library {
    using System.Collections.Generic;

    /// <summary>
    /// The 12 notes in an octave, with C as the base because for some reason that's the convention.
    /// </summary>
    public enum Notes : byte {
        C = 0,
        CSharp = 1,
        DFlat = CSharp,
        D = 2,
        DSharp = 3,
        EFlat = DSharp,
        E = 4,
        F = 5,
        FSharp = 6,
        GFlat5 = FSharp,
        G = 7,
        GSharp = 8,
        AFlat = GSharp,
        A = 9,
        ASharp = 10,
        BFlat = ASharp,
        B = 11
    }

    /// <summary>
    /// Extensions methods for <see cref="Notes" />.
    /// </summary>
    public static class NotesExtensions {
        private static readonly HashSet<Notes> NaturalNotes = new() { Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.F, Notes.G };

        /// <summary>
        /// Gets a value indicating whether not the note is natural.
        /// </summary>
        /// <param name="note"></param>
        /// <returns>A value indicating whether not the note is natural.</returns>
        public static bool IsNatural(this Notes note) {
            return NaturalNotes.Contains(note);
        }

        /// <summary>
        /// Converts a note into a display name.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>The display name.</returns>
        public static string ToDisplayName(this Notes note) {
            var noteName = note.ToString();

            if (noteName.Length > 1) {
                noteName = noteName.Replace("Sharp", "#");
                noteName = noteName.Replace("Flat", "♭");
            }

            return noteName;
        }

        /// <summary>
        /// Converts a note and its octave into a display name.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="octave">The octave,</param>
        /// <returns>The display name.</returns>
        public static string ToDisplayName(this Notes note, byte octave) {
            return $"{note.ToDisplayName()}{octave}";
        }
    }
}