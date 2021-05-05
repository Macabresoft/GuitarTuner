﻿namespace Macabresoft.GuitarTuner.Library {
    using System.Collections.Generic;

    /// <summary>
    /// An instance of a note that contains information important to its pitch, including its octave and frequency.
    /// </summary>
    public sealed class NaturalNote {
        public static readonly NaturalNote Empty = new();
        
        private NaturalNote(Notes note, byte octave) {
            this.Note = note;
            this.Octave = octave;
            this.Name = this.Note.ToDisplayName(this.Octave);
            this.Frequency = FrequencyCalculator.GetFrequency(note, octave);
            this.StepDownFrequency = FrequencyCalculator.GetStepDownFrequency(note, octave);
            this.StepUpFrequency = FrequencyCalculator.GetStepUpFrequency(note, octave);
        }

        private NaturalNote() {
            this.Name = string.Empty;
        }

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
        public Notes Note { get; }
        
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
        /// Gets a range of <see cref="NaturalNote" />.
        /// </summary>
        /// <param name="lowestNoteOctave">The octave for the lowest frequency note in the range.</param>
        /// <param name="lowestNote">The lowest note.</param>
        /// <param name="highestNoteOctave">The octave for the highest frequency note in the range.</param>
        /// <param name="highestNote">The highest note.</param>
        /// <returns>A range of <see cref="NaturalNote" />.</returns>
        public static IEnumerable<NaturalNote> GetRange(byte lowestNoteOctave, Notes lowestNote, byte highestNoteOctave, Notes highestNote) {
            var result = new List<NaturalNote>();
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

        private static void AddNotesForRange(IList<NaturalNote> notes, byte octave, Notes lowestNote, Notes highestNote) {
            for (var noteByte = (byte)lowestNote; noteByte <= (byte)highestNote; noteByte++) {
                var note = (Notes)noteByte;
                if (note.IsNatural()) {
                    var pitchNote = new NaturalNote((Notes)noteByte, octave);
                    notes.Add(pitchNote);
                }
            }
        }
    }
}