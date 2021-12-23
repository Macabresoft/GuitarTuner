namespace Macabresoft.GuitarTuner.Desktop.UI;

using System;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;using ReactiveUI;

public class SimulatedSampleProvider : ReactiveObject, ISampleProvider {
    private float _frequency = 75f;
    private float _volume;

    /// <inheritdoc />
    public event EventHandler<SamplesAvailableEventArgs>? SamplesAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulatedSampleProvider" /> class.
    /// </summary>
    /// <param name="bufferSize">Size of the buffer.</param>
    public SimulatedSampleProvider(int bufferSize) {
        this.BufferSize = bufferSize;
    }

    /// <inheritdoc />
    public int BufferSize { get; }

    /// <inheritdoc />
    public int SampleRate => SampleRates.Default;

    /// <summary>
    /// Gets or sets the frequency.
    /// </summary>
    public float Frequency {
        get => this._frequency;
        set {
            this.RaiseAndSetIfChanged(ref this._frequency, Math.Clamp(value, 75f, 400f));
            this.ResendSamples();
        }
    }

    /// <summary>
    /// Gets or sets the volume. Should be between 0 and 1.
    /// </summary>
    public float Volume {
        get => this._volume;
        set {
            this.RaiseAndSetIfChanged(ref this._volume, Math.Clamp(value, 0f, 1f));
            this.ResendSamples();
        }
    }

    /// <inheritdoc />
    public void Start() {
        this.ResendSamples();
    }

    /// <inheritdoc />
    public void Stop() {
        this.ResendSamples(0f, 0f);
    }

    private void ResendSamples() {
        this.ResendSamples(this.Frequency, this.Volume);
    }

    private void ResendSamples(float frequency, float volume) {
        var samples = new float[this.BufferSize];
        for (var i = 0; i < samples.Length; i++) {
            samples[i] = volume * MathF.Sin(i * frequency * MathF.PI * 2 / this.SampleRate);
        }

        this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
    }
}