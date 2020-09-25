namespace Macabresoft.Tuner.Tests {

    using Macabresoft.Tuner.Library;
    using NUnit.Framework;
    using System;

    public class FrequencyMonitorTests {
        private const int SampleRate = 44100;
        private SineWaveSampleProvider _sampleProvider;

        [Test]
        [TestCase(200f)]
        [TestCase(300f)]
        [TestCase(150f)]
        [TestCase(200f)]
        [TestCase(120f)]
        public void FrequencyMonitor_Update_SineWaveTest(float frequency) {
            // Arrange
            this._sampleProvider.Frequency = frequency;
            var frequencyMonitor = new FrequencyMonitor(this._sampleProvider);

            // Act
            this._sampleProvider.Start();

            // Assert
            Assert.AreEqual(frequency, frequencyMonitor.Frequency, Math.Log(frequency));
        }

        [SetUp]
        public void Setup() {
            this._sampleProvider = new SineWaveSampleProvider(0f, SampleRate);
        }
    }
}