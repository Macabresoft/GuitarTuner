namespace Macabresoft.GuitarTuner.Library;

using System.Collections.Generic;
using System.Linq;
using Macabresoft.Core;

/// <summary>
/// Interface for a tuning service.
/// </summary>
public interface ITuningService {
    /// <summary>
    /// Gets the available tunings.
    /// </summary>
    IReadOnlyCollection<ITuning> AvailableTunings { get; }

    /// <summary>
    /// Gets or sets the selected tuning.
    /// </summary>
    ITuning SelectedTuning { get; set; }
}

/// <summary>
/// Service which exposes <see cref="ITuning" />.
/// </summary>
public class TuningService : PropertyChangedNotifier, ITuningService {
    private readonly ITuning[] _availableTunings = {
        new StandardGuitarTuning()
    };

    private ITuning _selectedTuning;

    /// <summary>
    /// Initializes a new instance of the <see cref="TuningService" /> class.
    /// </summary>
    public TuningService() {
        this._selectedTuning = this._availableTunings.First();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ITuning> AvailableTunings => this._availableTunings;

    /// <inheritdoc />
    public ITuning SelectedTuning {
        get => this._selectedTuning;
        set => this.Set(ref this._selectedTuning, value);
    }
}