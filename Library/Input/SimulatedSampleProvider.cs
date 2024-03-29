﻿namespace Macabresoft.GuitarTuner.Library;

using System;
using System.Threading;
using System.Threading.Tasks;
using Macabresoft.Core;

/// <summary>
/// A simulated sample provider.
/// </summary>
public class SimulatedSampleProvider : PropertyChangedNotifier, ISampleProvider {
    /// <summary>
    /// Gets the minimum frequency.
    /// </summary>
    public const double MinimumFrequency = 20f;

    /// <summary>
    /// Gets the maximum frequency.
    /// </summary>
    public const double MaximumFrequency = 400f;
    
    private readonly Random _random = new();
    private double _frequency = 75f;
    private bool _isEnabled;
    private Task? _sampleTask;
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
    public double Frequency {
        get => this._frequency;
        set => this.Set(ref this._frequency, Math.Min(MaximumFrequency, Math.Max(value, MinimumFrequency)));
    }

    /// <summary>
    /// Gets or sets the volume. Should be between 0 and 1.
    /// </summary>
    public float Volume {
        get => this._volume;
        set => this.Set(ref this._volume, Math.Min(1f, Math.Max(value, 0f)));
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

    private void ResendSamples(double frequency, float volume) {
        var samples = new float[this.BufferSize];
        frequency = frequency + 0.5f - this._random.Next(-100, 100) / 200f;
        for (var i = 0; i < samples.Length; i++) {
            samples[i] = volume * (float)Math.Sin(i * frequency * Math.PI * 2 / this.SampleRate);
        }

        this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
    }

    private Task RunSimulation() {
        return Task.Run(() => {
            while (this._isEnabled) {
                this.ResendSamples();
                Thread.Sleep(100);
            }
        });
    }

    /// <inheritdoc />
    public void Dispose() {
        this._sampleTask?.Dispose();
        GC.SuppressFinalize(this);
    }
}