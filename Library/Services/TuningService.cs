namespace Macabresoft.GuitarTuner.Library;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Macabresoft.Core;

/// <summary>
/// Interface for a tuning service.
/// </summary>
public interface ITuningService : INotifyPropertyChanged {
    /// <summary>
    /// Gets the available tunings.
    /// </summary>
    IReadOnlyCollection<ITuning> AvailableTunings { get; }

    /// <summary>
    /// Gets the tuning notes.
    /// </summary>
    IReadOnlyCollection<Note> TuningNotes { get; }

    /// <summary>
    /// Gets or sets the selected tuning.
    /// </summary>
    ITuning SelectedTuning { get; set; }
}

/// <summary>
/// Service which exposes <see cref="ITuning" />.
/// </summary>
public sealed class TuningService : PropertyChangedNotifier, ITuningService {
    private readonly ITuning[] _availableTunings = {
        new StandardGuitarTuning(),
        new DropDGuitarTuning()
    };

    private readonly ObservableCollectionExtended<Note> _tuningNotes = new();
    private ITuning _selectedTuning;

    /// <summary>
    /// Initializes a new instance of the <see cref="TuningService" /> class.
    /// </summary>
    public TuningService() {
        this._selectedTuning = this._availableTunings.First();
        this.ResetTuningNotes();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ITuning> AvailableTunings => this._availableTunings;

    public IReadOnlyCollection<Note> TuningNotes => this._tuningNotes;

    /// <inheritdoc />
    public ITuning SelectedTuning {
        get => this._selectedTuning;
        set {
            if (this.Set(ref this._selectedTuning, value)) {
                this.ResetTuningNotes();
            }
        }
    }

    private void ResetTuningNotes() {
        var tuningNotes = this.SelectedTuning.Notes.ToList();

        if (tuningNotes.Count % 2 == 0) {
            tuningNotes.Insert(tuningNotes.Count / 2, Note.Auto);
        }
        else {
            tuningNotes.Add(Note.Auto);
        }

        this._tuningNotes.Reset(tuningNotes);
    }
}