namespace Macabresoft.GuitarTuner.Library {
    using System.Collections.Generic;

    /// <summary>
    /// An instance of a note that contains information important to its pitch, including its octave and frequency.
    /// </summary>
    public sealed class Note {
        public static readonly Note Empty = new();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="namedNote">The note name.</param>
        /// <param name="octave">The octave.</param>
        public Note(NamedNotes namedNote, byte octave) {
            this.NamedNote = namedNote;
            this.Octave = octave;
            this.Name = this.NamedNote.ToDisplayName(this.Octave);

            this.DistanceFromBase = FrequencyCalculator.GetDistanceFromBase(namedNote, octave);
            this.Frequency = FrequencyCalculator.GetFrequency(this.DistanceFromBase);
            this.StepDownFrequency = FrequencyCalculator.GetFrequency(this.DistanceFromBase - 1);
            this.StepUpFrequency = FrequencyCalculator.GetFrequency(this.DistanceFromBase + 1);
        }

        private Note() {
            this.Name = string.Empty;
        }
        
        /// <summary>
        /// Gets the distance in semitones from the base note of A4 at 440Hz.
        /// </summary>
        public int DistanceFromBase { get; }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        public double Frequency { get; }
        
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the note.
        /// </summary>
        public NamedNotes NamedNote { get; }
        
        /// <summary>
        /// Gets the octave.
        /// </summary>
        public byte Octave { get; }

        /// <summary>
        /// GEts the frequency of a note one semitone lower.
        /// </summary>
        public double StepDownFrequency { get; }

        /// <summary>
        /// Gets the frequency of a note one semitone higher.
        /// </summary>
        public double StepUpFrequency { get; }

        /// <summary>
        /// Gets a range of <see cref="Note" />.
        /// </summary>
        /// <param name="lowestNoteOctave">The octave for the lowest frequency note in the range.</param>
        /// <param name="lowestNamedNote">The lowest note.</param>
        /// <param name="highestNoteOctave">The octave for the highest frequency note in the range.</param>
        /// <param name="highestNamedNote">The highest note.</param>
        /// <returns>A range of <see cref="Note" />.</returns>
        public static IEnumerable<Note> GetRange(byte lowestNoteOctave, NamedNotes lowestNamedNote, byte highestNoteOctave, NamedNotes highestNamedNote) {
            var result = new List<Note>();
            if (lowestNoteOctave <= highestNoteOctave) {
                if (lowestNoteOctave != highestNoteOctave) {
                    AddNotesForRange(result, lowestNoteOctave, lowestNamedNote, NamedNotes.B);

                    for (var octave = lowestNoteOctave + 1; octave < highestNoteOctave; octave++) {
                        AddNotesForRange(result, (byte)octave, NamedNotes.C, NamedNotes.B);
                    }

                    AddNotesForRange(result, highestNoteOctave, NamedNotes.C, highestNamedNote);
                }
                else if ((byte)lowestNamedNote < (byte)highestNamedNote) {
                    AddNotesForRange(result, lowestNoteOctave, lowestNamedNote, highestNamedNote);
                }
            }

            return result;
        }

        private static void AddNotesForRange(IList<Note> notes, byte octave, NamedNotes lowestNamedNote, NamedNotes highestNamedNote) {
            for (var noteByte = (byte)lowestNamedNote; noteByte <= (byte)highestNamedNote; noteByte++) {
                var note = (NamedNotes)noteByte;
                if (note.IsNatural()) {
                    var pitchNote = new Note((NamedNotes)noteByte, octave);
                    notes.Add(pitchNote);
                }
            }
        }
    }
}