namespace Macabresoft.Zvukosti.Library {

    /// <summary>
    /// Information about a buffer of samples that has been processed by the <see
    /// cref="FrequencyMonitor" />.
    /// </summary>
    public struct BufferInformation {

        /// <summary>
        /// An instance of <see cref="BufferInformation" /> returned when the frequency could not be determined.
        /// </summary>
        public static BufferInformation Unknown = new BufferInformation();

        /// <summary>
        /// The fundamental frequency of the buffer.
        /// </summary>
        public float Frequency;

        /// <summary>
        /// The magnitude of the frequency within the buffer.
        /// </summary>
        public float Magnitude;

        public BufferInformation(float frequency, float magnitude) {
            this.Frequency = frequency;
            this.Magnitude = magnitude;
        }
    }
}