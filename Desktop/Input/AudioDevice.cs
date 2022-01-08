namespace Macabresoft.GuitarTuner.Desktop;

using System;

/// <summary>
/// Defines audio device types.
/// </summary>
public enum AudioDeviceType {
    Input,
    Output,
    Miscellaneous,
    Separator
}

/// <summary>
/// Represents an audio device.
/// </summary>
public sealed class AudioDevice {
    /// <summary>
    /// The default input device name.
    /// </summary>
    public const string DefaultInputName = "Default Input";

    /// <summary>
    /// The name of the simulated sample provider.
    /// </summary>
    public const string SimulatedName = "Simulated (Sin Wave)";

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioDevice" /> class.
    /// </summary>
    public AudioDevice() : this(AudioDeviceType.Miscellaneous, string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioDevice" /> class.
    /// </summary>
    /// <param name="deviceType">The audio device type.</param>
    /// <param name="name">The name of the audio device.</param>
    public AudioDevice(AudioDeviceType deviceType, string name) {
        this.DeviceType = deviceType;
        this.Name = name;
    }

    /// <summary>
    /// The type of audio device.
    /// </summary>
    public AudioDeviceType DeviceType { get; }

    /// <summary>
    /// The name of the audio device.
    /// </summary>
    public string Name { get; }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        return obj is AudioDevice device && this.Equals(device);
    }

    /// <inheritdoc />
    public override int GetHashCode() {
        return HashCode.Combine((int)this.DeviceType, this.Name);
    }

    /// <summary>
    /// The equals operator.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A value indicating whether the arguments are equal.</returns>
    public static bool operator ==(AudioDevice left, AudioDevice right) {
        return left.Equals(right);
    }

    /// <summary>
    /// The not equals operator.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A value indicating whether the arguments are not equal.</returns>
    public static bool operator !=(AudioDevice left, AudioDevice right) {
        return !(left == right);
    }

    private bool Equals(AudioDevice other) {
        return this.DeviceType == other.DeviceType && this.Name == other.Name;
    }
}