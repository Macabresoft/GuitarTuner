namespace Macabresoft.Zvukosti.Tests {

    using Macabresoft.Zvukosti.Library;
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    public class FrequencyMonitorTests {
        private const int Channels = 2;
        private const int SampleRate = 44100;
        private byte[] _buffer;
        private SignalGenerator _signalGenerator;
        private IWaveIn _waveIn;
        private IWaveProvider _waveProvider;

        [Test]
        [TestCase(200f)]
        [TestCase(300f)]
        [TestCase(150f)]
        [TestCase(200f)]
        [TestCase(120f)]
        public void FrequencyMonitor_Update_SineWaveTest(float frequency) {
            // Arrange
            this._signalGenerator.Frequency = frequency;
            var frequencyMonitor = new FrequencyMonitor(this._waveIn);

            // Act
            this._waveProvider.Read(this._buffer, 0, this._buffer.Length);
            this._waveIn.DataAvailable += Raise.EventWith(this._waveIn, new WaveInEventArgs(this._buffer, this._buffer.Length));
            this._waveProvider.Read(this._buffer, 0, this._buffer.Length);
            this._waveIn.DataAvailable += Raise.EventWith(this._waveIn, new WaveInEventArgs(this._buffer, this._buffer.Length));
            this._waveProvider.Read(this._buffer, 0, this._buffer.Length);
            this._waveIn.DataAvailable += Raise.EventWith(this._waveIn, new WaveInEventArgs(this._buffer, this._buffer.Length));

            // Assert
            Assert.AreEqual(frequency, frequencyMonitor.Frequency, Math.Log(frequency));
        }

        [SetUp]
        public void Setup() {
            this._buffer = new byte[((int)Math.Ceiling(SampleRate / FrequencyMonitor.LowestFrequency) * 2) * 4];

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