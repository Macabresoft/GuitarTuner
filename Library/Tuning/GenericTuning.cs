﻿namespace Macabresoft.GuitarTuner.Library;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A generic tuning that class that gives a default implementation of <see cref="GetNearestNote(float)" />.
/// </summary>
public abstract class GenericTuning : ITuning {
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericTuning" /> class.
    /// </summary>
    /// <param name="notes">The notes.</param>
    protected GenericTuning(IEnumerable<Note> notes) {
        this.Notes = notes.OrderBy(x => x.Frequency).ToList();

        this.MaximumFrequency = this.Notes.Select(x => x.Frequency).LastOrDefault();
        this.MinimumFrequency = this.Notes.Select(x => x.Frequency).FirstOrDefault();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericTuning" /> class.
    /// </summary>
    /// <param name="notes">The notes.</param>
    /// <param name="minimumFrequency">The minimum frequency.</param>
    /// <param name="maximumFrequency">The maximum frequency.</param>
    protected GenericTuning(IEnumerable<Note> notes, double minimumFrequency, double maximumFrequency) {
        this.Notes = notes.OrderBy(x => x.Frequency).ToList();

        this.MaximumFrequency = maximumFrequency;
        this.MinimumFrequency = minimumFrequency;
    }

    /// <inheritdoc />
    public abstract string DisplayName { get; }

    /// <inheritdoc />
    public double MaximumFrequency { get; }

    /// <inheritdoc />
    public double MinimumFrequency { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<Note> Notes { get; }

    /// <inheritdoc />
    public virtual Note GetNearestNote(double frequency, out double distanceFromBase) {
        Note result;
        var distance = double.PositiveInfinity;
        if (frequency < this.MinimumFrequency || frequency > this.MaximumFrequency) {
            result = Note.Empty;
        }
        else {
            distance = FrequencyCalculator.GetDistanceFromBase(frequency);
            result = this.Notes.OrderBy(note => Math.Abs(distance - note.DistanceFromBase)).FirstOrDefault() ?? Note.Empty;
        }

        distanceFromBase = result != Note.Empty ? distance : double.PositiveInfinity;
        return result;
    }
}