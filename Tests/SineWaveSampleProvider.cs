namespace Macabresoft.GuitarTuner.Tests {

    using Macabresoft.Core;
    using Macabresoft.GuitarTuner.Library;
    using Macabresoft.GuitarTuner.Library.Input;
    using OpenToolkit.Audio.OpenAL;
    using System;
    using System.Collections.Generic;

    public sealed class SineWaveSampleProvider : ISampleProvider {

        public SineWaveSampleProvider(float frequency, int sampleRate, int bufferSize) {
            this.Frequency = frequency;
            this.SampleRate = sampleRate;
            this.BufferSize = bufferSize;
        }

        public event EventHandler<SamplesAvailableEventArgs> SamplesAvailable;

        public int BufferSize { get; }

        public ALFormat Format {
            get {
                return ALFormat.Mono16;
            }
        }

        public float Frequency { get; set; }
        public int SampleRate { get; }

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

        public float[] GetSampleBuffer() {
            var samples = new float[this.BufferSize];
            for (var i = 0; i < samples.Length; i++) {
                samples[i] = MathF.Sin(i * this.Frequency * MathF.PI * 2 / this.SampleRate);
            }

            return samples;
        }

        public void Stop() {
            throw new NotSupportedException();
        }
    }
}