namespace Macabresoft.GuitarTuner.Library {
    using System.Collections.Generic;

    /// <summary>
    /// The 12 notes in an octave, with C as the base because for some reason that's the convention.
    /// </summary>
    public enum NamedNotes : byte {
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
    /// Extensions methods for <see cref="NamedNotes" />.
    /// </summary>
    public static class NotesExtensions {
        private static readonly HashSet<NamedNotes> NaturalNotes = new() { NamedNotes.A, NamedNotes.B, NamedNotes.C, NamedNotes.D, NamedNotes.E, NamedNotes.F, NamedNotes.G };

        /// <summary>
        /// Gets a value indicating whether not the note is natural.
        /// </summary>
        /// <param name="namedNote"></param>
        /// <returns>A value indicating whether not the note is natural.</returns>
        public static bool IsNatural(this NamedNotes namedNote) {
            return NaturalNotes.Contains(namedNote);
        }

        /// <summary>
        /// Converts a note into a display name.
        /// </summary>
        /// <param name="namedNote">The note.</param>
        /// <returns>The display name.</returns>
        public static string ToDisplayName(this NamedNotes namedNote) {
            var noteName = namedNote.ToString();

            if (noteName.Length > 1) {
                noteName = noteName.Replace("Sharp", "#");
                noteName = noteName.Replace("Flat", "♭");
            }

            return noteName;
        }

        /// <summary>
        /// Converts a note and its octave into a display name.
        /// </summary>
        /// <param name="namedNote">The note.</param>
        /// <param name="octave">The octave,</param>
        /// <returns>The display name.</returns>
        public static string ToDisplayName(this NamedNotes namedNote, byte octave) {
            return $"{namedNote.ToDisplayName()}{octave}";
        }
    }
}