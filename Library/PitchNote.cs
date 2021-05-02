namespace Macabresoft.GuitarTuner.Library {
    using System.Collections.Generic;

    /// <summary>
    /// An instance of a note that contains information important to its pitch, including its octave and frequency.
    /// </summary>
    public struct PitchNote {
        /// <summary>
        /// Initializes a new instance of the <see cref="PitchNote" /> struct.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="octave">The octave.</param>
        internal PitchNote(Notes note, byte octave) {
            this.Note = note;
            this.Octave = octave;
            this.Name = this.Note.ToDisplayName(this.Octave);
            this.Frequency = FrequencyCalculator.GetFrequency(note, octave);
        }

        /// <summary>
        /// The frequency.
        /// </summary>
        public double Frequency;
        
        /// <summary>
        /// The name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The note.
        /// </summary>
        public Notes Note;
        
        /// <summary>
        /// The octave.
        /// </summary>
        public byte Octave;

        /// <summary>
        /// Gets a range of <see cref="PitchNote" />.
        /// </summary>
        /// <param name="lowestNoteOctave">The octave for the lowest frequency note in the range.</param>
        /// <param name="lowestNote">The lowest note.</param>
        /// <param name="highestNoteOctave">The octave for the highest frequency note in the range.</param>
        /// <param name="highestNote">The highest note.</param>
        /// <returns>A range of <see cref="PitchNote" />.</returns>
        public static IEnumerable<PitchNote> GetRange(byte lowestNoteOctave, Notes lowestNote, byte highestNoteOctave, Notes highestNote) {
            var result = new List<PitchNote>();
            if (lowestNoteOctave <= highestNoteOctave) {
                if (lowestNoteOctave != highestNoteOctave) {
                    AddNotesForRange(result, lowestNoteOctave, lowestNote, Notes.B);

                    for (var octave = lowestNoteOctave + 1; octave < highestNoteOctave; octave++) {
                        AddNotesForRange(result, (byte)octave, Notes.C, Notes.B);
                    }

                    AddNotesForRange(result, highestNoteOctave, Notes.C, highestNote);
                }
                else if ((byte)lowestNote < (byte)highestNote) {
                    AddNotesForRange(result, lowestNoteOctave, lowestNote, highestNote);
                }
            }

            return result;
        }

        private static void AddNotesForRange(IList<PitchNote> notes, byte octave, Notes lowestNote, Notes highestNote) {
            for (var note = (byte)lowestNote; note <= (byte)highestNote; note++) {
                var pitchNote = new PitchNote((Notes)note, octave);
                notes.Add(pitchNote);
            }
        }
    }
}