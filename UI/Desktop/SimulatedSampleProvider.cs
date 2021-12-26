namespace Macabresoft.GuitarTuner.UI.Desktop;

using System;
using System.Threading;
using System.Threading.Tasks;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Input;using ReactiveUI;

public class SimulatedSampleProvider : ReactiveObject, ISampleProvider {
    private bool _isEnabled;
    private Task? _sampleTask;
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
        set => this.RaiseAndSetIfChanged(ref this._frequency, Math.Clamp(value, 75f, 400f));
    }

    /// <summary>
    /// Gets or sets the volume. Should be between 0 and 1.
    /// </summary>
    public float Volume {
        get => this._volume;
        set => this.RaiseAndSetIfChanged(ref this._volume, Math.Clamp(value, 0f, 1f));
    }

    /// <inheritdoc />
    public void Start() {
        this.Stop();
        this._isEnabled = true;
        this._sampleTask = this.RunSimulation();
    }

    /// <inheritdoc />
    public void Stop() {
        this._isEnabled = false;
        this._sampleTask?.Wait();
        this._sampleTask = null;
        this.ResendSamples(0f, 0f);
    }

    private void ResendSamples() {
        this.ResendSamples(this.Frequency, this.Volume);
    }

    private Task RunSimulation() {
        return Task.Run(() => {
            while (this._isEnabled) {
                this.ResendSamples();
                Thread.Sleep(100);
            }
        });
    }

    private void ResendSamples(float frequency, float volume) {
        var samples = new float[this.BufferSize];
        for (var i = 0; i < samples.Length; i++) {
            samples[i] = volume * MathF.Sin(i * frequency * MathF.PI * 2 / this.SampleRate);
        }

        this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
    }
}