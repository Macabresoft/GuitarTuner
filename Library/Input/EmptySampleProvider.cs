namespace Macabresoft.GuitarTuner.Library;

using System;

/// <summary>
/// An empty sample provider.
/// </summary>
public class EmptySampleProvider : ISampleProvider {
    /// <inheritdoc />
    public event EventHandler<SamplesAvailableEventArgs>? SamplesAvailable;

    /// <inheritdoc />
    public int BufferSize => 0;

    /// <inheritdoc />
    public int SampleRate => 0;

    /// <inheritdoc />
    public void Dispose() {
    }

    /// <inheritdoc />
    public void Start() {
    }

    /// <inheritdoc />
    public void Stop() {
    }
}