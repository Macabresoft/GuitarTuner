namespace Macabresoft.GuitarTuner.Tests.Tuning;

using FluentAssertions;
using FluentAssertions.Execution;
using Macabresoft.GuitarTuner.Library;
using Macabresoft.GuitarTuner.Library.Tuning;
using NUnit.Framework;

[TestFixture]
public class StandardGuitarTuningTests {
    [Test]
    [Category("Unit Tests")]
    [TestCase(330d, NamedNotes.E, 4)]
    [TestCase(320d, NamedNotes.E, 4)]
    [TestCase(340d, NamedNotes.E, 4)]
    [TestCase(247d, NamedNotes.B, 3)]
    [TestCase(240d, NamedNotes.B, 3)]
    [TestCase(260d, NamedNotes.B, 3)]
    [TestCase(196d, NamedNotes.G, 3)]
    [TestCase(190d, NamedNotes.G, 3)]
    [TestCase(200d, NamedNotes.G, 3)]
    [TestCase(146d, NamedNotes.D, 3)]
    [TestCase(143d, NamedNotes.D, 3)]
    [TestCase(149d, NamedNotes.D, 3)]
    [TestCase(110d, NamedNotes.A, 2)]
    [TestCase(107d, NamedNotes.A, 2)]
    [TestCase(113d, NamedNotes.A, 2)]
    [TestCase(82d, NamedNotes.E, 2)]
    [TestCase(87d, NamedNotes.E, 2)]
    [TestCase(80d, NamedNotes.E, 2)]
    public void GetNearestNote_ShouldGetCorrectNote(double frequency, NamedNotes expectedNote, byte expectedOctave) {
        var tuning = new StandardGuitarTuning();
        using (new AssertionScope()) {
            var nearestNote = tuning.GetNearestNote(frequency, out _);
            nearestNote.NamedNote.Should().Be(expectedNote);
            nearestNote.Octave.Should().Be(expectedOctave);
        }
    }
}