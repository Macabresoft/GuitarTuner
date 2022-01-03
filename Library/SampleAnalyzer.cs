namespace Macabresoft.GuitarTuner.Library;

using System;
using System.Collections.Generic;

/// <summary>
/// Interface for an audio buffer analyzer.
/// </summary>
public interface ISampleAnalyzer {
    /// <summary>
    /// Gets or sets the sample rate.
    /// </summary>
    int SampleRate { get; set; }

    /// <summary>
    /// Analyzes the provided audio samples and returns valuable information in the form of <see cref="BufferInformation" />.
    /// </summary>
    /// <param name="samples"></param>
    /// <returns>The frequency, magnitude, and nearest note in the form of <see cref="BufferInformation" />.</returns>
    BufferInformation GetBufferInformation(IReadOnlyList<float> samples);
}

/// <summary>
/// Analyzers audio buffers down to their respective frequency, magnitude, and nearest note.
/// </summary>
public sealed class SampleAnalyzer : ISampleAnalyzer {
    private readonly ITuningService _tuningService;
    private int _sampleRate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleAnalyzer" /> class.
    /// </summary>
    public SampleAnalyzer(ITuningService tuningService) {
        this._sampleRate = SampleRates.Default;
        this._tuningService = tuningService;
    }

    /// <inheritdoc />
    public int SampleRate {
        get => this._sampleRate;
        set {
            if (value <= 0) {
                value = 1;
            }

            this._sampleRate = value;
        }
    }

    /// <inheritdoc />
    public BufferInformation GetBufferInformation(IReadOnlyList<float> samples) {
        var tuning = this._tuningService.SelectedTuning;
        var lowPeriod = (int)Math.Floor(this.SampleRate / tuning.MaximumFrequency);
        var highPeriod = (int)Math.Ceiling(this.SampleRate / tuning.MinimumFrequency);

        if (samples.Count < highPeriod) {
            throw new InvalidOperationException("The sample rate isn't large enough for the buffer length.");
        }

        var greatestMagnitude = float.NegativeInfinity;
        var chosenPeriod = -1;
        var peakVolume = 0f;

        for (var period = lowPeriod; period < highPeriod; period++) {
            var sum = 0f;
            for (var i = 0; i < samples.Count - period; i++) {
                sum += samples[i] * samples[i + period];
                peakVolume = Math.Max(peakVolume, Math.Abs(samples[i]));
            }

            var newMagnitude = sum / samples.Count;
            if (newMagnitude > greatestMagnitude) {
                chosenPeriod = period;
                greatestMagnitude = newMagnitude;
            }
        }

        var frequency = (double)this.SampleRate / chosenPeriod;
        return frequency < tuning.MinimumFrequency || frequency > tuning.MaximumFrequency ? BufferInformation.Unknown : new BufferInformation((float)frequency, peakVolume);
    }
}