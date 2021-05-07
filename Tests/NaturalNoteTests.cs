namespace Macabresoft.GuitarTuner.Tests {
    using System;
    using System.Linq;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Macabresoft.GuitarTuner.Library;
    using NUnit.Framework;

    [TestFixture]
    public class NaturalNoteTests {
        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsDifferentOctave() {
            var range = Note.GetRange(0, NamedNotes.C, 4, NamedNotes.B).ToList();

            using (new AssertionScope()) {
                var naturalNotes = Enum.GetValues<NamedNotes>().Where(x => x.IsNatural()).ToList();
                range.Count.Should().Be(naturalNotes.Count * 5);
                foreach (var note in naturalNotes) {
                    range.Count(x => x.NamedNote == note).Should().Be(5);
                }
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsDifferentOctave_AndNotesAreOffset() {
            var range = Note.GetRange(0, NamedNotes.CSharp, 4, NamedNotes.BFlat).ToList();

            using (new AssertionScope()) {
                var naturalNotes = Enum.GetValues<NamedNotes>().Where(x => x.IsNatural()).ToList();
                range.Count.Should().Be(naturalNotes.Count * 5 - 2);
                foreach (var note in naturalNotes.Where(x => x != NamedNotes.C && x != NamedNotes.B)) {
                    range.Count(x => x.NamedNote == note).Should().Be(5);
                }

                range.Count(x => x.NamedNote == NamedNotes.C).Should().Be(4);
                range.Count(x => x.NamedNote == NamedNotes.B).Should().Be(4);
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetCorrectNotes_WhenRangeIsSameOctave() {
            var range = Note.GetRange(1, NamedNotes.C, 1, NamedNotes.B).ToList();

            using (new AssertionScope()) {
                var naturalNotes = Enum.GetValues<NamedNotes>().Where(x => x.IsNatural()).ToList();
                range.Count.Should().Be(naturalNotes.Count);
                foreach (var note in naturalNotes) {
                    range.Any(x => x.NamedNote == note).Should().BeTrue();
                }
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetNoNotes_WhenLowOctaveIsHigherThanHighOctave() {
            var range = Note.GetRange(2, NamedNotes.C, 1, NamedNotes.B).ToList();
            using (new AssertionScope()) {
                range.Count.Should().Be(0);
            }
        }

        [Test]
        [Category("Unit Tests")]
        public void GetRange_ShouldGetNoNotes_WhenSameOctaveAndLowerNoteIsHigherThanHighNote() {
            var range = Note.GetRange(2, NamedNotes.A, 2, NamedNotes.C).ToList();
            using (new AssertionScope()) {
                range.Count.Should().Be(0);
            }
        }
    }
}