namespace Macabresoft.GuitarTuner.Tests {
    using System;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Macabresoft.GuitarTuner.Library;
    using NUnit.Framework;

    [TestFixture]
    public class FrequencyCalculatorTests {
        [Test]
        [Category("Unit Tests")]
        public void GetDistanceFromBase_ShouldGetCorrectDistance_WhenOctaveIsBase() {
            using (new AssertionScope()) {
                foreach (var note in Enum.GetValues<Notes>()) {
                    var expected = (int)note - (int)FrequencyCalculator.BaseNote;
                    FrequencyCalculator.GetDistanceFromBase(note, FrequencyCalculator.BaseOctave).Should().Be(expected);
                }
            }
        }

        [Test]
        [Category("Unit Tests")]
        [TestCase(FrequencyCalculator.BaseNote, FrequencyCalculator.BaseOctave, FrequencyCalculator.BaseFrequency)]
        [TestCase(Notes.C, 0, 16.35d)]
        [TestCase(Notes.B, 8, 7902.13d)]
        [TestCase(Notes.FSharp, 6, 1479.98d)]
        [TestCase(Notes.GFlat5, 6, 1479.98d)]
        [TestCase(Notes.E, 4, 329.63d)]
        [TestCase(Notes.B, 3, 246.94d)]
        [TestCase(Notes.G, 3, 196d)]
        [TestCase(Notes.D, 3, 146.83d)]
        [TestCase(Notes.A, 2, 110d)]
        [TestCase(Notes.E, 2, 82.41d)]
        [TestCase(Notes.G, 2, 97.999d)]
        [TestCase(Notes.D, 2, 73.416d)]
        [TestCase(Notes.A, 1, 55d)]
        [TestCase(Notes.E, 1, 41.204d)]
        [TestCase(Notes.B, 0, 30.868d)]
        public void GetFrequency_ShouldGetFrequency(Notes note, byte octave, double expectedFrequency) {
            using (new AssertionScope()) {
                FrequencyCalculator.GetFrequency(note, octave).Should().BeApproximately(expectedFrequency, 0.01d);
            }
        }
    }
}