namespace Macabresoft.Zvukosti.Tests {

    using Macabresoft.Zvukosti.Library;
    using NUnit.Framework;
    using System;
    using FluentAssertions;
    using FluentAssertions.Execution;

    public class FrequencyMonitorTests {
        private const int SampleRate = 44100;
        private SineWaveSampleProvider _sampleProvider;

        [Test]
        [TestCase(1000f)]
        [TestCase(500f)]
        [TestCase(300f)]
        [TestCase(200f)]
        [TestCase(150f)]
        [TestCase(120f)]
        [TestCase(80f)] // TODO: figure out why this test is failing
        public void SineWaveSample_Should_ProcessToSameFrequency(float frequency) {
            this._sampleProvider.Frequency = frequency;
            var frequencyMonitor = new FrequencyMonitor(this._sampleProvider);

            this._sampleProvider.Start();

            using (new AssertionScope()) {
                frequencyMonitor.Frequency.Should().BeApproximately(frequency, (float)Math.Log(frequency));
            }
        }

        [Test]
        public void EmptySample_Should_HoldPreviousValue_UntilHoldTimeReached() {
            this._sampleProvider.Frequency = 300f;
            var frequencyMonitor = new FrequencyMonitor(this._sampleProvider);
            var numberOfRunsUntilZero = (int)Math.Ceiling(FrequencyMonitor.HoldTime * this._sampleProvider.SampleRate / this._sampleProvider.BufferSize);
            
            for (var i = 0; i < numberOfRunsUntilZero; i++) {
                this._sampleProvider.Start();
            }

            using (new AssertionScope()) {
                frequencyMonitor.Frequency.Should().BeApproximately(this._sampleProvider.Frequency, (float)Math.Log(this._sampleProvider.Frequency));
                this._sampleProvider.ProvideEmptySamples(numberOfRunsUntilZero - 1);
                frequencyMonitor.Frequency.Should().BeApproximately(this._sampleProvider.Frequency, (float)Math.Log(this._sampleProvider.Frequency));
                this._sampleProvider.ProvideEmptySamples(1);
                frequencyMonitor.Frequency.Should().Be(0f);
            }
        }
        
        [SetUp]
        public void Setup() {
            this._sampleProvider = new SineWaveSampleProvider(0f, SampleRate);
        }
    }
}