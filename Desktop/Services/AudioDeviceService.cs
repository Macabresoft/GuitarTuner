namespace Macabresoft.GuitarTuner.Desktop;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Macabresoft.Core;
using OpenTK.Audio.OpenAL;

/// <summary>
/// An interface which provides audio devices.
/// </summary>
public interface IAudioDeviceService : INotifyPropertyChanged {
    /// <summary>
    /// Gets the input devices.
    /// </summary>
    IReadOnlyCollection<AudioDevice> AvailableInputDevices { get; }

    /// <summary>
    /// Gets the selected device.
    /// </summary>
    public AudioDevice SelectedDevice { get; }

    /// <summary>
    /// Refreshes the available devices.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Selects the device if it is valid.
    /// </summary>
    /// <param name="device">The device.</param>
    void SelectDevice(AudioDevice device);
}

/// <summary>
/// Provides audio devices.
/// </summary>
public sealed class AudioDeviceService : PropertyChangedNotifier, IAudioDeviceService {
    private const string DefaultInputName = "Default Input";
    private readonly ObservableCollectionExtended<AudioDevice> _availableInputDevices = new();
    private AudioDevice _selectedDevice = new(AudioDeviceType.Input, DefaultInputName);

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioDeviceService" /> class.
    /// </summary>
    public AudioDeviceService() {
        this.Refresh();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<AudioDevice> AvailableInputDevices => this._availableInputDevices;

    /// <inheritdoc />
    public AudioDevice SelectedDevice {
        get => this._selectedDevice;
        private set => this.Set(ref this._selectedDevice, value);
    }

    /// <inheritdoc />
    public void Refresh() {
        var defaultInputDevice = new AudioDevice(AudioDeviceType.Input, DefaultInputName);
        this._availableInputDevices.Clear();
        this._availableInputDevices.Add(defaultInputDevice);
        this._availableInputDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier).Select(x => new AudioDevice(AudioDeviceType.Input, x)));
        this.SelectedDevice = defaultInputDevice;
    }

    /// <inheritdoc />
    public void SelectDevice(AudioDevice device) {
        if (this.AvailableInputDevices.Contains(device)) {
            this.SelectedDevice = device;
        }
    }
}