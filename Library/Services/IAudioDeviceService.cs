namespace Macabresoft.GuitarTuner.Library;

using System.Collections.Generic;

/// <summary>
/// An interface which provides audio devices.
/// </summary>
public interface IAudioDeviceService {
    /// <summary>
    /// Gets the available devices.
    /// </summary>
    IReadOnlyCollection<string?> AvailableDevices { get; }

    /// <summary>
    /// Refreshes the available devices.
    /// </summary>
    void Refresh();
}