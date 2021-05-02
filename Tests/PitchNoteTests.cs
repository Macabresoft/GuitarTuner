namespace Macabresoft.GuitarTuner.Tests {
    using System;
    using System.Linq;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Macabresoft.GuitarTuner.Library;
    using NUnit.Framework;

    [TestFixture]
    public class PitchNoteTests {
        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsDifferentOctave() {
            var range = PitchNote.GetRange(0, Notes.C, 4, Notes.B).ToList();

            using (new AssertionScope()) {
                range.Count.Should().Be(FrequencyCalculator.NumberOfNotes * 5);
                foreach (var note in Enum.GetValues<Notes>()) {
                    range.Count(x => x.Note == note).Should().Be(5);
                }
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsDifferentOctave_AndNotesAreOffset() {
            var range = PitchNote.GetRange(0, Notes.CSharp, 4, Notes.BFlat).ToList();

            using (new AssertionScope()) {
                range.Count.Should().Be(FrequencyCalculator.NumberOfNotes * 5 - 2);
                foreach (var note in Enum.GetValues<Notes>().Where(x => x != Notes.C && x != Notes.B)) {
                    range.Count(x => x.Note == note).Should().Be(5);
                }

                range.Count(x => x.Note == Notes.C).Should().Be(4);
                range.Count(x => x.Note == Notes.B).Should().Be(4);
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsSameOctave() {
            var range = PitchNote.GetRange(1, Notes.C, 1, Notes.B).ToList();

            using (new AssertionScope()) {
                range.Count.Should().Be(FrequencyCalculator.NumberOfNotes);
                foreach (var note in Enum.GetValues<Notes>()) {
                    range.Any(x => x.Note == note).Should().BeTrue();
                }
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetNoNotes_WhenLowOctaveIsHigherThanHighOctave() {
            var range = PitchNote.GetRange(2, Notes.C, 1, Notes.B).ToList();
            using (new AssertionScope()) {
                range.Count.Should().Be(0);
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetNoNotes_WhenSameOctaveAndLowerNoteIsHigherThanHighNote() {
            var range = PitchNote.GetRange(2, Notes.A, 2, Notes.C).ToList();
            using (new AssertionScope()) {
                range.Count.Should().Be(0);
            }
        }
    }
}