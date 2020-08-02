namespace Macabresoft.Zvukosti.Library {

    using NAudio.MediaFoundation;
    using NAudio.Wave;
    using System;
    using System.Linq;

    public sealed class MainViewModel : NotifyPropertyChanged {
        private readonly SampleAggregator _sampleAggregator;
        private readonly IWaveIn _waveIn;

        public MainViewModel(IWaveIn waveIn) {
            this._waveIn = waveIn;
            this._waveIn.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._waveIn.WaveFormat.SampleRate, 2);
            this._waveIn.DataAvailable += this.WaveIn_DataAvailable;
            MediaFoundationApi.Startup();
            this._sampleAggregator = new SampleAggregator(this._waveIn.WaveFormat);
            this._sampleAggregator.NotificationCount = waveIn.WaveFormat.SampleRate / 100;
            this._sampleAggregator.PerformFFT = true;
            this._sampleAggregator.FFTCalculated += this.SampleProvider_FFTCalculated;
            this._sampleAggregator.MaximumCalculated += this.SampleProvider_MaximumCalculated;
            this._waveIn.StartRecording();
        }

        private void ReadAudio(WaveBuffer buffer, int samples) {
            Console.WriteLine($"Recorded '{samples}' samples, with a minimum of '{buffer.FloatBuffer.Min()}' and a maximum of {buffer.FloatBuffer.Max()}");
        }

        private void SampleProvider_FFTCalculated(object sender, FFTEventArgs e) {
            Console.WriteLine(e.Result.Length);
        }

        private void SampleProvider_MaximumCalculated(object sender, MaxSampleEventArgs e) {
            Console.WriteLine($"{e.MinSample}  -> {e.MaxSample}");
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
            var waveBuffer = new WaveBuffer(e.Buffer);
            var samples = this._sampleAggregator.Read(waveBuffer, 0, waveBuffer.FloatBuffer.Length);
            this.ReadAudio(waveBuffer, samples);
        }
    }
}