namespace Macabresoft.GuitarTuner.Library {
    /// <summary>
    /// Information about a buffer of samples that has been processed by the <see cref="SampleAnalyzer" />.
    /// </summary>
    public struct BufferInformation {
        /// <summary>
        /// An instance of <see cref="BufferInformation" /> returned when the frequency could not be determined.
        /// </summary>
        public static BufferInformation Unknown = new();

        /// <summary>
        /// The fundamental frequency of the buffer.
        /// </summary>
        public float Frequency;

        /// <summary>
        /// The magnitude of the frequency within the buffer.
        /// </summary>
        public float Magnitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferInformation" /> struct.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="magnitude">The magnitude.</param>
        public BufferInformation(float frequency, float magnitude) {
            this.Frequency = frequency;
            this.Magnitude = magnitude;
        }
    }
}