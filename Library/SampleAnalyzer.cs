namespace Macabresoft.GuitarTuner.Library;

using System;
using System.Collections.Generic;
using Macabresoft.GuitarTuner.Library.Tuning;

/// <summary>
/// Interface for an audio buffer analyzer.
/// </summary>
public interface ISampleAnalyzer {
    int SampleRate { get; set; }

    /// <summary>
    /// Gets or sets the tuning by which to measure the audio buffer.
    /// </summary>
    ITuning Tuning { get; set; }

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
    private int _highPeriod;
    private int _lowPeriod;
    private int _sampleRate;
    private ITuning _tuning;

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleAnalyzer" /> class.
    /// </summary>
    public SampleAnalyzer(ITuning tuning) {
        this._sampleRate = SampleRates.Default;
        this._tuning = tuning;
        this.ResetPeriods();
    }

    /// <inheritdoc />
    public int SampleRate {
        get => this._sampleRate;
        set {
            if (value <= 0) {
                value = 1;
            }

            if (value != this._sampleRate) {
                this._sampleRate = value;
                this.ResetPeriods();
            }
        }
    }

    /// <inheritdoc />
    public ITuning Tuning {
        get => this._tuning;
        set {
            if (value != this._tuning) {
                this._tuning = value;
                this.ResetPeriods();
            }
        }
    }

    /// <inheritdoc />
    public BufferInformation GetBufferInformation(IReadOnlyList<float> samples) {
        if (samples.Count < this._highPeriod) {
            throw new InvalidOperationException("The sample rate isn't large enough for the buffer length.");
        }

        var greatestMagnitude = float.NegativeInfinity;
        var chosenPeriod = -1;
        var peakVolume = 0f;

        for (var period = this._lowPeriod; period < this._highPeriod; period++) {
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
        return frequency < this.Tuning.MinimumFrequency || frequency > this.Tuning.MaximumFrequency ? BufferInformation.Unknown : new BufferInformation((float)frequency, peakVolume);
    }

    private void ResetPeriods() {
        this._lowPeriod = (int)Math.Floor(this.SampleRate / this.Tuning.MaximumFrequency);
        this._highPeriod = (int)Math.Ceiling(this.SampleRate / this.Tuning.MinimumFrequency);
    }
}