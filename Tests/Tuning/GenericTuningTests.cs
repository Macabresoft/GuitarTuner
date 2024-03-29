﻿namespace Macabresoft.GuitarTuner.Tests.Tuning;

using FluentAssertions;
using FluentAssertions.Execution;
using Macabresoft.GuitarTuner.Library;
using NUnit.Framework;

[TestFixture]
public class GenericTuningTests {
    [Test]
    [Category("Unit Tests")]
    [TestCase(146.83d, NamedNotes.D, 3)]
    [TestCase(143, NamedNotes.D, 3)]
    [TestCase(149d, NamedNotes.D, 3)]
    [TestCase(110d, NamedNotes.A, 2)]
    [TestCase(107d, NamedNotes.A, 2)]
    [TestCase(113d, NamedNotes.A, 2)]
    [TestCase(82.41d, NamedNotes.E, 2)]
    [TestCase(84d, NamedNotes.E, 2)]
    [TestCase(87.31d, NamedNotes.F, 2)]
    [TestCase(85d, NamedNotes.F, 2)]
    [TestCase(97.999d, NamedNotes.G, 2)]
    public void GetNearestNote_ShouldGetCorrectNote(double frequency, NamedNotes expectedNote, byte expectedOctave) {
        var tuning = new TestTuning();
        using (new AssertionScope()) {
            var nearestNote = tuning.GetNearestNote(frequency, out _);
            nearestNote.NamedNote.Should().Be(expectedNote);
            nearestNote.Octave.Should().Be(expectedOctave);
        }
    }
}