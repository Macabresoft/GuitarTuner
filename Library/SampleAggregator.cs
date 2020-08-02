using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace Macabresoft.Zvukosti.Library {

    public class FFTEventArgs : EventArgs {

        public FFTEventArgs(Complex[] result) {
            Result = result;
        }

        public Complex[] Result { get; private set; }
    }

    public class MaxSampleEventArgs : EventArgs {

        public MaxSampleEventArgs(float minValue, float maxValue) {
            MaxSample = maxValue;
            MinSample = minValue;
        }

        public float MaxSample { get; private set; }

        public float MinSample { get; private set; }
    }

    public class SampleAggregator : ISampleProvider {
        private readonly int _channels;

        private readonly FFTEventArgs _fftArgs;

        private readonly Complex[] _fftBuffer;

        private readonly int _fftLength;

        private readonly int _m;

        private int _count;

        private int _fftPos;

        private float _maxValue;

        private float _minValue;

        public SampleAggregator(WaveFormat waveFormat, int fftLength = 1024) {
            _channels = waveFormat.Channels;
            WaveFormat = waveFormat;

            if (!IsPowerOfTwo(fftLength)) {
                throw new ArgumentException("FFT Length must be a power of two");
            }
            _m = (int)Math.Log(fftLength, 2.0);
            this._fftLength = fftLength;
            _fftBuffer = new Complex[fftLength];
            _fftArgs = new FFTEventArgs(_fftBuffer);
        }

        // FFT
        public event EventHandler<FFTEventArgs> FFTCalculated;

        // volume
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public int NotificationCount { get; set; }

        public bool PerformFFT { get; set; }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count) {
            for (int n = 0; n < count; n += _channels) {
                Add(buffer[n + offset]);
            }

            return count;
        }

        public void Reset() {
            _count = 0;
            _maxValue = _minValue = 0;
        }

        private static bool IsPowerOfTwo(int x) {
            return (x & (x - 1)) == 0;
        }

        private void Add(float value) {
            if (PerformFFT && FFTCalculated != null) {
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

            _maxValue = Math.Max(_maxValue, value);
            _minValue = Math.Min(_minValue, value);
            _count++;
            if (_count >= NotificationCount && NotificationCount > 0) {
                MaximumCalculated?.Invoke(this, new MaxSampleEventArgs(_minValue, _maxValue));
                Reset();
            }
        }
    }
}