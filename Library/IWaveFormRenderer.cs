namespace Macabresoft.Zvukosti.Library {

    /// <summary>
    /// Handles the rendering for audio to wave form.
    /// </summary>
    public interface IWaveFormRenderer {

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="minValue">The minimum value.</param>
        void AddValue(float maxValue, float minValue);
    }
}