namespace Macabresoft.GuitarTuner.Desktop;

using System.Collections.Generic;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;
using OpenTK.Audio.OpenAL;

/// <summary>
/// Provides audio devices.
/// </summary>
public sealed class AudioDeviceService : IAudioDeviceService {
    private readonly ObservableCollectionExtended<string?> _availableDevices = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioDeviceService" /> class.
    /// </summary>
    public AudioDeviceService() {
        this.Refresh();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string?> AvailableDevices => this._availableDevices;

    /// <inheritdoc />
    public void Refresh() {
        this._availableDevices.Clear();
        this._availableDevices.Add(null);
        this._availableDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier));
    }
}