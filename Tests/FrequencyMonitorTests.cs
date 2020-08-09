namespace Macabresoft.Zvukosti.Tests {

    using Macabresoft.Zvukosti.Library;
    using Macabresoft.Zvukosti.Library.Tuning;
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    public class FrequencyMonitorTests {
        private const int Channels = 2;
        private const int SampleRate = 8000;
        private readonly byte[] _buffer = new byte[FrequencyMonitor.BufferSizeByte];
        private SignalGenerator _signalGenerator;
        private IWaveIn _waveIn;
        private IWaveProvider _waveProvider;

        [Test]
        [TestCase(200f, 50f, 300f, 200f)]
        [TestCase(300f, 100f, 400f, 300f)]
        [TestCase(150f, 100f, 500f, 150f)]
        [TestCase(200f, 300f, 400f, float.NegativeInfinity)]
        [TestCase(120f, 100f, 500f, 120f)]
        public void FrequencyMonitor_Update_SineWaveTest(float actualFrequency, float minimumFrequency, float maximumFrequency, float expectedFrequency) {
            // Arrange
            this._signalGenerator.Frequency = actualFrequency;
            var tuning = Substitute.For<ITuning>();
            tuning.DisplayName.Returns($"Freq: {actualFrequency} Hz, Min: {minimumFrequency} Hz, Max: {maximumFrequency} Hz");
            tuning.MinimumFrequency.Returns(minimumFrequency);
            tuning.MaxinimumFrequency.Returns(maximumFrequency);
            var frequencyMonitor = new FrequencyMonitor(this._waveIn, tuning);
            this._waveProvider.Read(this._buffer, 0, FrequencyMonitor.BufferSizeByte);

            // Act
            this._waveIn.DataAvailable += Raise.EventWith(this._waveIn, new WaveInEventArgs(this._buffer, FrequencyMonitor.BufferSizeByte));
            frequencyMonitor.Update();

            // Assert
            TestContext.Out.WriteLine($"Expected: {expectedFrequency} Hz");
            TestContext.Out.WriteLine($"Actual: { frequencyMonitor.Frequency} Hz");
            TestContext.Out.WriteLine($"Min: {minimumFrequency} Hz");
            TestContext.Out.WriteLine($"Max: {maximumFrequency} Hz");
            Assert.AreEqual(expectedFrequency, frequencyMonitor.Frequency, Math.Log(actualFrequency, 4f));
        }

        [SetUp]
        public void Setup() {
            Array.Clear(this._buffer, 0, FrequencyMonitor.BufferSizeByte);
            this._signalGenerator = new SignalGenerator(SampleRate, Channels) {
                Gain = 1f,
                Type = SignalGeneratorType.Sin
            };

            this._waveProvider = this._signalGenerator.ToWaveProvider16();
            this._waveIn = Substitute.For<IWaveIn>();
            this._waveIn.WaveFormat = new WaveFormat(SampleRate, Channels);
        }
    }
}