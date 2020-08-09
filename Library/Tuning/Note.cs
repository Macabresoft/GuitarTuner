namespace Macabresoft.Zvukosti.Library.Tuning {

    using System;

    /// <summary>
    /// Represents a note inside of a particular <see cref="ITuning"/>.
    /// </summary>
    public struct Note {

        /// <summary>
        /// The A2 note.
        /// </summary>
        public static Note A2 = new Note(110f, 97.99f, 123.47f, "A2");

        /// <summary>
        /// The B3 note.
        /// </summary>
        public static Note B3 = new Note(246.94f, 233.08f, 261.63f, "B3");

        /// <summary>
        /// The D3 note.
        /// </summary>
        public static Note D3 = new Note(146.83f, 138.59f, 155.56f, "D3");

        /// <summary>
        /// The E2 note.
        /// </summary>
        public static Note E2 = new Note(82.407f, 77.782f, 87.307f, "E2");

        /// <summary>
        /// The E4 note.
        /// </summary>
        public static Note E4 = new Note(329.63f, 311.13f, 349.23f, "E4");

        /// <summary>
        /// An empty note.
        /// </summary>
        public static Note Empty = new Note();

        /// <summary>
        /// The G3 note.
        /// </summary>
        public static Note G3 = new Note(196f, 185f, 207.65f, "G3");

        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// The frequency.
        /// </summary>
        public float Frequency;

        /// <summary>
        /// The frequency a single step down from this note.
        /// </summary>
        public float StepDownFrequency;

        /// <summary>
        /// The frequency a single step up from this note.
        /// </summary>
        public float StepUpFrequency;

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> struct.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="displayName">The display name.</param>
        public Note(float frequency, float stepDownFrequency, float stepUpFrequency, string displayName) {
            if (stepDownFrequency > frequency) {
                throw new ArgumentOutOfRangeException(nameof(stepDownFrequency), "Note's step down frequency must be less than its actual frequency.");
            }
            else if (stepUpFrequency < frequency) {
                throw new ArgumentOutOfRangeException(nameof(stepDownFrequency), "Note's step up frequency must be greater than its actual frequency.");
            }

            this.Frequency = frequency;
            this.StepDownFrequency = stepDownFrequency;
            this.StepUpFrequency = stepUpFrequency;
            this.DisplayName = displayName;
        }
    }
}