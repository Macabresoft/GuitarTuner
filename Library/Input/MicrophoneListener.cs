namespace Macabresoft.Zvukosti.Library.Input {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Macabresoft.Core;
    using OpenToolkit.Audio.OpenAL;

    /// <summary>
    /// A <see cref="ISampleProvider" /> that listens in on a recording device and provides samples
    /// from it.
    /// </summary>
    public class MicrophoneListener : ISampleProvider, IDisposable {
        private readonly ALCaptureDevice _captureDevice;
        private readonly int _halfBufferSize;
        private bool _isDisposed;
        private bool _isEnabled;
        private Task? _listenTask;

        /// <inheritdoc />
        public event EventHandler<SamplesAvailableEventArgs>? SamplesAvailable;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrophoneListener" /> class.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="format">The format.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public MicrophoneListener(string deviceName, int sampleRate, ALFormat format, int bufferSize) {
            if (sampleRate < 8000) {
                throw new ArgumentOutOfRangeException(nameof(sampleRate));
            }

            if (bufferSize <= 0) {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            this._captureDevice = ALC.CaptureOpenDevice(deviceName, sampleRate, format, bufferSize);
            this.BufferSize = bufferSize;
            this.Format = format;
            this.SampleRate = sampleRate;
            this._halfBufferSize = this.BufferSize / 2;
        }

        /// <inheritdoc />
        public int BufferSize { get; }

        /// <inheritdoc />
        public ALFormat Format { get; }

        /// <inheritdoc />
        public int SampleRate { get; }

        /// <inheritdoc />
        public void Dispose() {
            if (!this._isDisposed) {
                this.Stop();
                this.SamplesAvailable = null;
                this._isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Start() {
            this._isEnabled = true;
            this._listenTask = this.Listen();
        }

        /// <inheritdoc />
        public void Stop() {
            this._isEnabled = false;
            this._listenTask?.Wait();
        }

        private Task Listen() {
            return Task.Run(() => {
                while (this._isEnabled && !this._isDisposed) {
                    var index = 0;
                    var buffer = new short[this.BufferSize];
                    ALC.CaptureStart(this._captureDevice);

                    while (index < buffer.Length) {
                        var samplesAvailable = ALC.GetAvailableSamples(this._captureDevice);
                        if (samplesAvailable > this._halfBufferSize) {
                            var samplesToRead = Math.Min(samplesAvailable, buffer.Length - index);
                            ALC.CaptureSamples(this._captureDevice, ref buffer[index], samplesToRead);
                            index += samplesToRead;
                        }

                        Thread.Yield();
                    }

                    var samples = new float[this.BufferSize];

                    for (var i = 0; i < buffer.Length; i++) {
                        samples[i] = buffer[i] / (float)short.MaxValue;
                    }

                    this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, index));
                }

                ALC.CaptureStop(this._captureDevice);
            });
        }
    }
}