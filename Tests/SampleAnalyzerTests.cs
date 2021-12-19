namespace Macabresoft.GuitarTuner.Tests {
    using System;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Macabresoft.GuitarTuner.Library;
    using Macabresoft.GuitarTuner.Library.Tuning;
    using NUnit.Framework;

    [TestFixture]
    public class SampleAnalyzerTests {
        [SetUp]
        public void Setup() {
            this._tuning = new TestTuning();
            this._sampleProvider = new SineWaveSampleProvider(
                0f,
                SampleRate,
                (int)Math.Ceiling(SampleRate / this._tuning.MinimumFrequency) * 2);
        }

        [Test]
        [TestCase(110f)]
        [TestCase(97.99f)]
        [TestCase(123.47f)]
        [TestCase(246.94f)]
        [TestCase(233.08f)]
        [TestCase(261.63f)]
        [TestCase(146.83f)]
        [TestCase(138.59f)]
        [TestCase(155.56f)]
        [TestCase(82.407f)]
        [TestCase(77.782f)]
        [TestCase(87.307f)]
        [TestCase(329.63f)]
        [TestCase(311.13f)]
        [TestCase(196f)]
        [TestCase(185f)]
        [TestCase(207.65f)]
        [TestCase(300f)]
        [TestCase(200f)]
        [TestCase(150f)]
        [TestCase(120f)]
        [TestCase(80f)]
        public void GetBufferInformation_Should_ReturnCorrectBufferInformation(float frequency) {
            this._sampleProvider.Frequency = frequency;
            var sampleAnalyzer = new SampleAnalyzer(this._sampleProvider.SampleRate, new StandardGuitarTuning());

            var samples = this._sampleProvider.GetSampleBuffer();
            var bufferInformation = sampleAnalyzer.GetBufferInformation(samples);

            using (new AssertionScope()) {
                bufferInformation.Frequency.Should().BeApproximately(frequency, (float)Math.Log(frequency));
            }
        }

        private const int SampleRate = 44100;
        private ITuning _tuning;
        private SineWaveSampleProvider _sampleProvider;
    }
}