namespace Macabresoft.GuitarTuner.UI.Desktop;

using System;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;
using ReactiveUI;

/// <summary>
/// Interface for a service which handles sampling operations.
/// </summary>
public interface ISampleService {
    /// <summary>
    /// Gets the distance the current frequency is from the base note of the tuning.
    /// </summary>
    float DistanceFromBase { get; }

    /// <summary>
    /// Gets the current nearest note to the current frequency.
    /// </summary>
    Note Note { get; }

    /// <summary>
    /// Gets the peak volume.
    /// </summary>
    float PeakVolume { get; }

    /// <summary>
    /// Gets or sets the note for which to tune towards.
    /// </summary>
    /// <remarks>
    /// When this is Note.Empty, automatic note detection will be enabled (much like your standard guitar tuner).
    /// </remarks>
    Note TuneToNote { get; set; }

    /// <summary>
    /// Gets the sample provider.
    /// </summary>
    ISampleProvider SampleProvider { get; set; }
}

/// <summary>
/// Services which handles sampling operations.
/// </summary>
public class SampleService : ReactiveObject, ISampleService {
    /// <summary>
    /// The hold time for a note in seconds. For instance, if a user hits the note E and
    /// then provides no sound for 3 seconds, the frequency will continue to report E
    /// until those 3 seconds are up.
    /// </summary>
    private const float HoldTime = 3f;

    private readonly ISampleAnalyzer _sampleAnalyzer;
    private readonly object _sampleProviderLock = new();
    private float _distanceFromBase;
    private float _frequency;
    private Note _note = Note.Empty;
    private float _peakVolume;
    private ISampleProvider _sampleProvider;
    private float _timeElapsed;
    private Note _tuneToNote = Note.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleService" /> class.
    /// </summary>
    /// <param name="sampleAnalyzer">The sample analyzer.</param>
    /// <param name="sampleProvider">The sample provider.</param>
    public SampleService(ISampleAnalyzer sampleAnalyzer, ISampleProvider sampleProvider) {
        this._sampleAnalyzer = sampleAnalyzer;
        this._sampleProvider = sampleProvider;

        this.StartSampleProvider();
    }

    /// <inheritdoc />
    public float DistanceFromBase {
        get => this._distanceFromBase;
        private set => this.RaiseAndSetIfChanged(ref this._distanceFromBase, value);
    }

    /// <summary>
    /// Gets the frequency.
    /// </summary>
    public float Frequency {
        get => this._frequency;
        private set {
            if (Math.Abs(this._frequency - value) > 0.01f) {
                this.RaiseAndSetIfChanged(ref this._frequency, value);
                this.ResetNote();
            }

            this.RaisePropertyChanged(nameof(this.DistanceFromBase));
        }
    }

    /// <inheritdoc />
    public Note Note {
        get => this._note;
        private set => this.RaiseAndSetIfChanged(ref this._note, value);
    }

    /// <inheritdoc />
    public float PeakVolume {
        get => this._peakVolume;
        private set => this.RaiseAndSetIfChanged(ref this._peakVolume, value);
    }

    /// <inheritdoc />
    public ISampleProvider SampleProvider {
        get => this._sampleProvider;
        set {
            this.StopSampleProvider();
            this.RaiseAndSetIfChanged(ref this._sampleProvider, value);
            this.StartSampleProvider();
        }
    }

    /// <inheritdoc />
    public Note TuneToNote {
        get => this._tuneToNote;
        set {
            this.RaiseAndSetIfChanged(ref this._tuneToNote, value);
            this.ResetNote();
        }
    }

    private void ClearFrequency() {
        this._timeElapsed = 0f;
        this.Frequency = 0f;
        this.PeakVolume = 0f;
    }

    private void HoldForReset(int sampleCount) {
        if (this.Frequency != 0f && sampleCount > 0) {
            if (this._sampleProvider.SampleRate > 0) {
                this._timeElapsed += sampleCount / (float)this._sampleProvider.SampleRate;

                if (this._timeElapsed >= HoldTime) {
                    this.ClearFrequency();
                }
            }
            else {
                this.ClearFrequency();
            }
        }
    }

    private void ResetNote() {
        var note = this._sampleAnalyzer.Tuning.GetNearestNote(this.Frequency, out var distanceFromBase);
        this.Note = this.TuneToNote == Note.Empty ? note : this.TuneToNote;
        this.DistanceFromBase = (float)distanceFromBase;
    }

    private void SampleProvider_SamplesAvailable(object? sender, SamplesAvailableEventArgs e) {
        lock (this._sampleProviderLock) {
            if (sender == this._sampleProvider) {
                if (e.Samples.Length >= 2 && e.Samples[^2] != 0f) {
                    var bufferInformation = this._sampleAnalyzer.GetBufferInformation(e.Samples);
                    this.PeakVolume = bufferInformation.PeakVolume;
                    if (bufferInformation.Frequency == 0f || bufferInformation.PeakVolume < 0.25f) {
                        this.HoldForReset(e.Samples.Length);
                    }
                    else {
                        this.Frequency = bufferInformation.Frequency;
                    }
                }
                else {
                    this.PeakVolume = 0f;
                    this.HoldForReset(e.Samples.Length);
                }
            }
        }
    }

    private void StartSampleProvider() {
        this._sampleAnalyzer.SampleRate = this._sampleProvider.SampleRate;
        this._sampleProvider.SamplesAvailable += this.SampleProvider_SamplesAvailable;
        this._sampleProvider.Start();
    }

    private void StopSampleProvider() {
        this._sampleProvider.SamplesAvailable -= this.SampleProvider_SamplesAvailable;
        this._sampleProvider.Stop();
    }
}