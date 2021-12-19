namespace Macabresoft.GuitarTuner.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Macabresoft.GuitarTuner.Library;
using NUnit.Framework;

[TestFixture]
public class FrequencyCalculatorTests {
    [Test]
    [Category("Unit Tests")]
    public void GetDistanceFromBase_ShouldGetBase_ForKnownFrequencies() {
        var noteNames = Enum.GetValues<NamedNotes>();
        var notes = new List<Note>();

        for (byte octave = 0; octave < 10; octave++) {
            notes.AddRange(noteNames.Select(noteName => new Note(noteName, octave)));
        }

        using (new AssertionScope()) {
            foreach (var note in notes) {
                var calculatedDistance = FrequencyCalculator.GetDistanceFromBase(note.Frequency);
                calculatedDistance.Should().BeApproximately(note.DistanceFromBase, 0.001d);
            }
        }
    }

    [Test]
    [Category("Unit Tests")]
    public void GetDistanceFromBase_ShouldGetCorrectDistance_WhenOctaveIsBase() {
        using (new AssertionScope()) {
            foreach (var note in Enum.GetValues<NamedNotes>()) {
                var expected = (int)note - (int)FrequencyCalculator.BaseNote;
                FrequencyCalculator.GetDistanceFromBase(note, FrequencyCalculator.BaseOctave).Should().Be(expected);
            }
        }
    }

    [Test]
    [Category("Unit Tests")]
    [TestCase(FrequencyCalculator.BaseNote, FrequencyCalculator.BaseOctave, FrequencyCalculator.BaseFrequency)]
    [TestCase(NamedNotes.C, 0, 16.35d)]
    [TestCase(NamedNotes.B, 8, 7902.13d)]
    [TestCase(NamedNotes.FSharp, 6, 1479.98d)]
    [TestCase(NamedNotes.GFlat5, 6, 1479.98d)]
    [TestCase(NamedNotes.E, 4, 329.63d)]
    [TestCase(NamedNotes.B, 3, 246.94d)]
    [TestCase(NamedNotes.G, 3, 196d)]
    [TestCase(NamedNotes.D, 3, 146.83d)]
    [TestCase(NamedNotes.A, 2, 110d)]
    [TestCase(NamedNotes.E, 2, 82.41d)]
    [TestCase(NamedNotes.G, 2, 97.999d)]
    [TestCase(NamedNotes.D, 2, 73.416d)]
    [TestCase(NamedNotes.A, 1, 55d)]
    [TestCase(NamedNotes.E, 1, 41.204d)]
    [TestCase(NamedNotes.B, 0, 30.868d)]
    public void GetFrequency_ShouldGetFrequency(NamedNotes namedNote, byte octave, double expectedFrequency) {
        using (new AssertionScope()) {
            FrequencyCalculator.GetFrequency(namedNote, octave).Should().BeApproximately(expectedFrequency, 0.01d);
        }
    }
}