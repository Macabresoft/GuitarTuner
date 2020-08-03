namespace Macabresoft.Zvukosti.Library {

    using NAudio.MediaFoundation;
    using NAudio.Wave;
    using System;

    /// <summary>
    /// The main view model.
    /// </summary>
    public sealed class MainViewModel : NotifyPropertyChanged {
        private readonly BufferedWaveProvider _bufferedWaveProvider;
        private readonly SampleAggregator _sampleAggregator;
        private readonly IWaveIn _waveIn;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="waveIn">The wave in.</param>
        public MainViewModel(IWaveIn waveIn) {
            this._waveIn = waveIn;

            var device = WaveIn.GetCapabilities(0);
            this._waveIn.WaveFormat = new WaveFormat(8000, device.Channels);
            this._waveIn.DataAvailable += this.WaveIn_DataAvailable;
            this._bufferedWaveProvider = new BufferedWaveProvider(this._waveIn.WaveFormat);
            this._bufferedWaveProvider.DiscardOnBufferOverflow = true;
            this._bufferedWaveProvider.BufferLength = 1024;
            MediaFoundationApi.Startup();
            this._sampleAggregator = new SampleAggregator(this._bufferedWaveProvider.ToSampleProvider().ToMono());
            this._sampleAggregator.FFTCalculated += this.SampleProvider_FFTCalculated;
            this._waveIn.StartRecording();
        }

        /// <summary>
        /// Occurs after the FFT is calculated.
        /// </summary>
        public event EventHandler<FFTEventArgs> FFTCalculated;

        private void SampleProvider_FFTCalculated(object sender, FFTEventArgs e) {
            this.FFTCalculated?.Invoke(sender, e);
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) {
            var waveBuffer = new WaveBuffer(e.Buffer);
            this._bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            this._sampleAggregator.Read(waveBuffer.FloatBuffer, 0, e.BytesRecorded / 4);
        }
    }
}