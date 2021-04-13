namespace Macabresoft.Zvukosti.Tests {

    using Macabresoft.Core;
    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Input;
    using OpenToolkit.Audio.OpenAL;
    using System;

    public sealed class SineWaveSampleProvider : ISampleProvider {

        public SineWaveSampleProvider(float frequency, int sampleRate) {
            this.Frequency = frequency;
            this.SampleRate = sampleRate;
            this.BufferSize = (int)Math.Ceiling(this.SampleRate / FrequencyMonitor.LowestFrequency) * 15;
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
            var samples = new float[this.BufferSize];
            for (var i = 0; i < samples.Length; i++) {
                samples[i] = MathF.Sin(i * this.Frequency * MathF.PI * 2 / this.SampleRate);
            }

            this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, samples.Length));
        }

        public void Stop() {
            throw new NotSupportedException();
        }
    }
}