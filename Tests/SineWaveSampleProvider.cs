namespace Macabresoft.GuitarTuner.Tests;

using System;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;

public sealed class SineWaveSampleProvider : ISampleProvider {
    public event EventHandler<SamplesAvailableEventArgs> SamplesAvailable;

    public SineWaveSampleProvider(float frequency, int sampleRate, int bufferSize) {
        this.Frequency = frequency;
        this.SampleRate = sampleRate;
        this.BufferSize = bufferSize;
    }

    public int BufferSize { get; }
    public int SampleRate { get; }

    public float Frequency { get; set; }

    public void Dispose() {
    }

    public float[] GetSampleBuffer() {
        var samples = new float[this.BufferSize];
        for (var i = 0; i < samples.Length; i++) {
            samples[i] = MathF.Sin(i * this.Frequency * MathF.PI * 2 / this.SampleRate);
        }

        return samples;
    }

    public void ProvideEmptySamples(int roundsOfSamples) {
        var samples = new float[this.BufferSize];

        for (var i = 0; i < roundsOfSamples; i++) {
            this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
        }
    }

    public void Start() {
        var samples = this.GetSampleBuffer();
        this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
    }

    public void Stop() {
    }
}