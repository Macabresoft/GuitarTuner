namespace Macabresoft.Zvukosti.Library {

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Averages frequencies over time.
    /// </summary>
    public sealed class RollingAverageFrequency {
        private readonly List<float> _frequencies = new List<float>();
        private readonly byte _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingAverageFrequency" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <exception cref="ArgumentOutOfRangeException">size</exception>
        public RollingAverageFrequency(byte size) {
            if (size == 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            this._size = size;
        }

        /// <summary>
        /// Gets the average frequency.
        /// </summary>
        /// <value>The average frequency.</value>
        public float AverageFrequency { get; private set; }

        /// <summary>
        /// Adds the specified frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        public void Add(float frequency) {
            this._frequencies.Add(frequency);
            if (this._frequencies.Count > this._size) {
                this._frequencies.RemoveAt(0);
            }

            this.CalculateAverageFrequency();
        }

        /// <summary>
        /// Removes the first item in the rolling average.
        /// </summary>
        public void Remove() {
            if (this._frequencies.Any()) {
                this._frequencies.RemoveAt(0);
                this.CalculateAverageFrequency();
            }
        }

        private void CalculateAverageFrequency() {
            if (this._frequencies.Any()) {
                this.AverageFrequency = this._frequencies.Sum() / this._frequencies.Count;
            }
            else {
                this.AverageFrequency = 0f;
            }
        }
    }
}