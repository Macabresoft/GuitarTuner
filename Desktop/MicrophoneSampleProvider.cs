namespace Macabresoft.GuitarTuner.Desktop;

using System;
using System.Threading;
using System.Threading.Tasks;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;
using OpenTK.Audio.OpenAL;

/// <summary>
/// A <see cref="ISampleProvider" /> that listens in on a recording device and provides samples
/// from it.
/// </summary>
public class MicrophoneSampleProvider : ISampleProvider, IDisposable {
    private readonly string? _deviceName;
    private readonly int _halfBufferSize;
    private ALCaptureDevice _captureDevice;
    private bool _isDisposed;
    private bool _isEnabled;
    private Task? _listenTask;

    /// <inheritdoc />
    public event EventHandler<SamplesAvailableEventArgs>? SamplesAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrophoneSampleProvider" /> class.
    /// </summary>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <param name="deviceName">The device name.</param>
    public MicrophoneSampleProvider(int bufferSize, string? deviceName) {
        if (bufferSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(bufferSize));
        }

        this.BufferSize = bufferSize;
        this._halfBufferSize = this.BufferSize / 2;
        this._deviceName = deviceName;
    }

    /// <inheritdoc />
    public int BufferSize { get; }

    /// <inheritdoc />
    public int SampleRate => SampleRates.Default;

    /// <inheritdoc />
    public void Dispose() {
        if (!this._isDisposed) {
            this.Stop();
            this.SamplesAvailable = null;
            ALC.CaptureStop(this._captureDevice);
            ALC.CaptureCloseDevice(this._captureDevice);
            this._isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Start() {
        this._isEnabled = true;

        this._captureDevice = ALC.CaptureOpenDevice(
            this._deviceName,
            this.SampleRate,
            ALFormat.Mono16,
            this.BufferSize);

        this._listenTask = this.Listen();
    }

    /// <inheritdoc />
    public void Stop() {
        this._isEnabled = false;
        this._listenTask?.Wait();
        this._listenTask = null;
        ALC.CaptureStop(this._captureDevice);
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
                    samples[i] = (float)(buffer[i] / (double)short.MaxValue);
                }

                this.SamplesAvailable.SafeInvoke(this, new SamplesAvailableEventArgs(samples, index));
            }

            ALC.CaptureStop(this._captureDevice);
        });
    }
}