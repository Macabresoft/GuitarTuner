namespace Macabresoft.GuitarTuner.Desktop;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Macabresoft.Core;
using Macabresoft.GuitarTuner.Library;
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
    private readonly ObservableCollectionExtended<AudioDevice> _availableInputDevices = new();
    private readonly ISampleService _sampleService;
    private readonly ITuningService _tuningService;
    private AudioDevice _selectedDevice = new(AudioDeviceType.Input, AudioDevice.DefaultInputName);

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioDeviceService" /> class.
    /// </summary>
    /// <param name="sampleService">The sample service.</param>
    /// <param name="tuningService">The tuning service.</param>
    public AudioDeviceService(ISampleService sampleService, ITuningService tuningService) {
        this._sampleService = sampleService;
        this._tuningService = tuningService;

        this._tuningService.PropertyChanged += this.TuningService_PropertyChanged;
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
        var defaultInputDevice = new AudioDevice(AudioDeviceType.Input, AudioDevice.DefaultInputName);
        this._availableInputDevices.Clear();
        this._availableInputDevices.Add(defaultInputDevice);
        this._availableInputDevices.AddRange(ALC.GetString(AlcGetStringList.CaptureDeviceSpecifier).Select(x => new AudioDevice(AudioDeviceType.Input, x)));
        this._availableInputDevices.Add(new AudioDevice(AudioDeviceType.Miscellaneous, AudioDevice.SimulatedName));
        this.SelectDevice(defaultInputDevice);
    }

    /// <inheritdoc />
    public void SelectDevice(AudioDevice device) {
        if (this.AvailableInputDevices.Contains(device)) {
            this.SelectedDevice = device;

            var bufferSize = (int)Math.Ceiling(SampleRates.Default / this._tuningService.SelectedTuning.MinimumFrequency) * 2;

            this._sampleService.SampleProvider = this.SelectedDevice.Name switch {
                AudioDevice.DefaultInputName => new MicrophoneSampleProvider(bufferSize, null),
                AudioDevice.SimulatedName => new SimulatedSampleProvider(bufferSize),
                _ => new MicrophoneSampleProvider(bufferSize, this.SelectedDevice.Name)
            };
        }
    }

    private void TuningService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(ITuningService.SelectedTuning)) {
            this.SelectDevice(this.SelectedDevice);
        }
    }
}