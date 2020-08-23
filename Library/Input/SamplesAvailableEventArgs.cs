namespace Macabresoft.Zvukosti.Library.Input {

    using System;

    /// <summary>
    /// An event for when a <see cref="ISampleProvider" /> has samples available to process.
    /// </summary>
    public sealed class SamplesAvailableEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplesAvailableEventArgs" /> class.
        /// </summary>
        /// <param name="samples">The samples.</param>
        /// <param name="samplesRead">The samples read.</param>
        public SamplesAvailableEventArgs(float[] samples, int samplesRead) {
            this.Samples = samples;
            this.SamplesRead = samplesRead;
        }

        /// <summary>
        /// Gets the samples.
        /// </summary>
        /// <value>The samples.</value>
        public float[] Samples { get; }

        /// <summary>
        /// Gets the number of samples read.
        /// </summary>
        /// <value>The number of samples read.</value>
        public int SamplesRead { get; }
    }
}