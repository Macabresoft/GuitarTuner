namespace Macabresoft.Zvukosti.Library {

    using NAudio.Dsp;
    using NAudio.Wave;
    using System;

    /// <summary>
    /// Event arguments for a completed FFT.
    /// </summary>
    /// <seealso cref="System.EventArgs"/>
    public class FFTEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="FFTEventArgs"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="sampleRate">The sample rate.</param>
        public FFTEventArgs(Complex[] result, int sampleRate) {
            this.Result = result;
            this.SampleRate = sampleRate;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public Complex[] Result { get; }

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        /// <value>The sample rate.</value>
        public int SampleRate { get; }
    }

    /// <summary>
    /// Aggregates samples and provides fast fourier transform values.
    /// </summary>
    public class SampleAggregator : ISampleProvider {
        private readonly int _channels;
        private readonly FFTEventArgs _fftArgs;
        private readonly Complex[] _fftBuffer;
        private readonly int _fftLength;
        private readonly int _m;

        private int _fftPos;
        private ISampleProvider _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleAggregator"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fftLength">Length of the FFT.</param>
        /// <exception cref="ArgumentException">FFT Length must be a power of two</exception>
        public SampleAggregator(ISampleProvider source, int fftLength = 1024) {
            if (!IsPowerOfTwo(fftLength)) {
                throw new ArgumentException("FFT Length must be a power of two");
            }

            this._source = source;
            this._channels = 1;
            this._m = (int)Math.Log(fftLength, 2.0);
            this._fftLength = fftLength;
            this._fftBuffer = new Complex[fftLength];
            this._fftArgs = new FFTEventArgs(_fftBuffer, source.WaveFormat.SampleRate);
        }

        /// <summary>
        /// Occurs after the FFT is calculated.
        /// </summary>
        public event EventHandler<FFTEventArgs> FFTCalculated;

        /// <inheritdoc/>
        public WaveFormat WaveFormat => this._source?.WaveFormat;

        /// <inheritdoc/>
        public int Read(float[] buffer, int offset, int count) {
            var samplesRead = this._source.Read(buffer, offset, count);

            for (int n = 0; n < samplesRead; n += _channels) {
                Add(buffer[n + offset]);
            }

            return count;
        }

        private static bool IsPowerOfTwo(int x) {
            return (x & (x - 1)) == 0;
        }

        private void Add(float value) {
            if (FFTCalculated != null) {
                _fftBuffer[_fftPos].X = (float)(value * FastFourierTransform.HammingWindow(_fftPos, _fftLength));
                _fftBuffer[_fftPos].Y = 0;
                _fftPos++;
                if (_fftPos >= _fftBuffer.Length) {
                    _fftPos = 0;
                    // 1024 = 2^10
                    FastFourierTransform.FFT(true, _m, _fftBuffer);
                    FFTCalculated(this, _fftArgs);
                }
            }
        }
    }
}