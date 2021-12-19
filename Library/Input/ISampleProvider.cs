namespace Macabresoft.GuitarTuner.Library.Input;

using System;

/// <summary>
/// Provides audio at regular intervals.
/// </summary>
public interface ISampleProvider {
    /// <summary>
    /// Occurs when samples are available, which is generally when the buffer is filled to
    /// <see
    ///     cref="BufferSize" />
    /// .
    /// </summary>
    event EventHandler<SamplesAvailableEventArgs>? SamplesAvailable;

    /// <summary>
    /// Gets the size of the buffer.
    /// </summary>
    /// <value>The size of the buffer.</value>
    int BufferSize { get; }

    /// <summary>
    /// Gets the sample rate in Hertz.
    /// </summary>
    /// <value>The sample rate in Hertz.</value>
    int SampleRate { get; }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops this instance.
    /// </summary>
    void Stop();
}