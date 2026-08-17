namespace Bach.Model.Test;

using FluentAssertions;
using Xunit;

public sealed class ChordOfTTest
{
  [Fact]
  public void Constructor_ShouldInitializeBaseProperties_WhenCalledViaConcreteChord()
  {
    // Arrange
    var formula = Registry.ChordFormulas["Major"];

    // Act
    var chord = Chord.Create(PitchClass.C, formula, 1);

    // Assert
    chord.Root.Should().Be(PitchClass.C);
    chord.Formula.Should().Be(formula);
    chord.Inversion.Should().Be(1);
    chord.Bass.Should().Be(PitchClass.E);
    chord.Name.Should().Be("C/E");
  }

  [Fact]
  public void Bass_ShouldReturnFirstElement_WhenInversionIsNonZero()
  {
    // Arrange
    var chord = Chord.Create(PitchClass.C, "Major").GetInversion(1);

    // Act
    var bass = chord.Bass;

    // Assert
    bass.Should().Be(PitchClass.E);
  }

  [Fact]
  public void IsExtended_ShouldReturnTrue_WhenFormulaHasIntervalBeyondOctave()
  {
    // Arrange
    var custom = new ChordFormula("X","X","X", Interval.Unison, new Interval(IntervalQuantity.Ninth, IntervalQuality.Major));

    // Act
    var chord = Chord.Create(PitchClass.C, custom);

    // Assert
    chord.IsExtended.Should().BeTrue();
  }

  [Fact]
  public void Equals_GenericEquals_ShouldReturnFalse_WhenOtherIsNull()
  {
    // Arrange
    var chord = Chord.Create(PitchClass.C, "Major");

    // Act
    var result = chord.Equals((Chord?) null);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void Equals_GenericEquals_ShouldReturnTrue_WhenSameReference()
  {
    // Arrange
    var chord = Chord.Create(PitchClass.C, "Major");

    // Act
    var result = chord.Equals(chord);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void Equals_ObjectEquals_ShouldReturnFalse_WhenDifferentType()
  {
    // Arrange
    object obj = int.MinValue;
    var chord = Chord.Create(PitchClass.C, "Major");

    // Act
    var result = chord.Equals(obj);

    // Assert
    result.Should().BeFalse();
  }
}
