// Module Name: ChordOfTTests.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2026  Eddie Velasquez.
// 
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test;

public sealed class ChordOfTTests
{
  #region Public Methods

  [Fact]
  public void Bass_ShouldReturnFirstElement_WhenInversionIsNonZero()
  {
    // Arrange
    var chord = Chord.Create( PitchClass.C, "Major" )
                     .GetInversion( 1 );

    // Act
    var bass = chord.Bass;

    // Assert
    bass.Should()
        .Be( PitchClass.E );
  }

  [Fact]
  public void Constructor_ShouldInitializeBaseProperties_WhenCalledViaConcreteChord()
  {
    // Arrange
    var formula = Registry.ChordFormulas["Major"];

    // Act
    var chord = Chord.Create( PitchClass.C, formula, 1 );

    // Assert
    chord.Root.Should()
         .Be( PitchClass.C );

    chord.Formula.Should()
         .Be( formula );

    chord.Inversion.Should()
         .Be( 1 );

    chord.Bass.Should()
         .Be( PitchClass.E );

    chord.Name.Should()
         .Be( "C/E" );
  }

  [Fact]
  public void Equals_GenericEquals_ShouldReturnFalse_WhenOtherIsNull()
  {
    // Arrange
    var chord = Chord.Create( PitchClass.C, "Major" );

    // Act
    var result = chord.Equals( null );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_GenericEquals_ShouldReturnTrue_WhenSameReference()
  {
    // Arrange
    var chord = Chord.Create( PitchClass.C, "Major" );

    // Act
    var result = chord.Equals( chord );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ObjectEquals_ShouldReturnFalse_WhenDifferentType()
  {
    // Arrange
    object obj = int.MinValue;
    var chord = Chord.Create( PitchClass.C, "Major" );

    // Act
    var result = chord.Equals( obj );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void IsExtended_ShouldReturnTrue_WhenFormulaHasIntervalBeyondOctave()
  {
    // Arrange
    var custom = new ChordFormula(
      "X",
      "X",
      "X",
      Interval.Unison,
      new Interval( IntervalQuantity.Ninth, IntervalQuality.Major )
    );

    // Act
    var chord = Chord.Create( PitchClass.C, custom );

    // Assert
    chord.IsExtended.Should()
         .BeTrue();
  }

  #endregion
}
