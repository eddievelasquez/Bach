// Module Name: ChordTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System.Linq;

public sealed class ChordTest
{
  #region Public Methods

  [Theory]
  [MemberData( nameof( ExtendedChordData ) )]
  public void IsExtended_ShouldReturnExpectedResult(
    PitchClass root,
    string formulaName,
    bool isExtended )
  {
    var chord = new Chord( root, formulaName );

    chord.IsExtended.Should()
         .Be( isExtended );
  }

  [Theory]
  [MemberData( nameof( ChordData ) )]
  public void Constructor_ShouldInitializeChordCorrectly(PitchClass root, string formulaName, string expectedNotes)
  {
    var chord = new Chord( root, formulaName );

    var actualNotes = chord.Take(
      PitchClassCollection.Parse( expectedNotes )
                          .Count
    );

    actualNotes.Should()
               .BeEquivalentTo( PitchClassCollection.Parse( expectedNotes ) );
  }

  [Fact]
  public void Enumerator_ShouldEnumerateChordCorrectly()
  {
    var cMajor = new Chord( PitchClass.C, "Major" );
    using var enumerator = cMajor.GetEnumerator();

    enumerator.Should()
              .NotBeNull();

    enumerator.MoveNext()
              .Should()
              .BeTrue();

    enumerator.Current.Should()
              .Be( PitchClass.C );

    enumerator.MoveNext()
              .Should()
              .BeTrue();

    enumerator.Current.Should()
              .Be( PitchClass.E );

    enumerator.MoveNext()
              .Should()
              .BeTrue();

    enumerator.Current.Should()
              .Be( PitchClass.G );

    enumerator.MoveNext()
              .Should()
              .BeFalse();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = new Chord( PitchClass.C, "Major" );
    object y = new Chord( PitchClass.C, "Major" );
    object z = new Chord( PitchClass.C, "Major" );

    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingDifferentType()
  {
    object actual = new Chord( PitchClass.C, "Major" );

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = new Chord( PitchClass.C, "Major" );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingSameObject()
  {
    var actual = new Chord( PitchClass.C, "Major" );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Constructor_ShouldInitializeChordUsingFormula()
  {
    var formula = Registry.ChordFormulas["Minor"];
    var target = new Chord( PitchClass.C, formula );

    target.Root.Should()
          .Be( PitchClass.C );

    target.Formula.Should()
          .Be( Registry.ChordFormulas["Minor"] );

    target.Name.Should()
          .Be( "Cm" );

    target.PitchClasses.Should()
          .BeEquivalentTo( PitchClassCollection.Parse( "C,Eb,G" ) );

    target.ToString()
          .Should()
          .Be( target.Name );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenFormulaIsNull()
  {
    var act = () => new Chord( PitchClass.C, (ChordFormula) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetHashCode_ShouldReturnSameValue_ForEquivalentObjects()
  {
    var actual = new Chord( PitchClass.C, "Major" );
    var expected = new Chord( PitchClass.C, "Major" );

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void GetInversion_ShouldReturnExpectedResult()
  {
    var cMajor = new Chord( PitchClass.C, "Major" );
    var firstInversion = cMajor.GetInversion( 1 );

    firstInversion.Should()
                  .NotBeNull();

    firstInversion.Name.Should()
                  .Be( "C/E" );

    firstInversion.PitchClasses.Should()
                  .BeEquivalentTo( PitchClassCollection.Parse( "E,G,C" ) );

    var secondInversion = cMajor.GetInversion( 2 );

    secondInversion.Should()
                   .NotBeNull();

    secondInversion.Name.Should()
                   .Be( "C/G" );

    secondInversion.PitchClasses.Should()
                   .BeEquivalentTo( PitchClassCollection.Parse( "G,C,E" ) );

    var act = () => cMajor.GetInversion( 3 );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Constructor_ShouldInitializeChordUsingString()
  {
    var target = new Chord( PitchClass.C, "Minor" );

    target.Root.Should()
          .Be( PitchClass.C );

    target.Formula.Should()
          .Be( Registry.ChordFormulas["Minor"] );

    target.Name.Should()
          .Be( "Cm" );

    target.PitchClasses.Should()
          .BeEquivalentTo( PitchClassCollection.Parse( "C,Eb,G" ) );

    target.ToString()
          .Should()
          .Be( target.Name );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenFormulaNameIsEmpty()
  {
    var act = () => new Chord( PitchClass.C, "" );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenFormulaNameIsNull()
  {
    var act = () => new Chord( PitchClass.C, (string) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldSatisfyEquivalenceRelation()
  {
    var x = new Chord( PitchClass.C, "Major" );
    var y = new Chord( PitchClass.C, "Major" );
    var z = new Chord( PitchClass.C, "Major" );

    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingDifferentType()
  {
    var actual = new Chord( PitchClass.C, "Major" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingToNull()
  {
    var actual = new Chord( PitchClass.C, "Major" );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

  #region Implementation

  public static TheoryData<PitchClass, string, bool> ExtendedChordData { get; } = new()
  {
    { PitchClass.C, "Major", false },
    { PitchClass.C, "Major7", false },
    { PitchClass.C, "Major9", true },
    { PitchClass.C, "Major11", true },
    { PitchClass.C, "Major13", true },
    { PitchClass.C, "Minor", false },
    { PitchClass.C, "Minor7", false },
    { PitchClass.C, "Minor9", true },
    { PitchClass.C, "Minor11", true },
    { PitchClass.C, "Minor13", true },
    { PitchClass.C, "Dominant7", false },
    { PitchClass.C, "Dominant9", true },
    { PitchClass.C, "Dominant11", true },
    { PitchClass.C, "Dominant13", true },
    { PitchClass.C, "SixNine", true },
    { PitchClass.C, "AddNine", true },
    { PitchClass.C, "Diminished", false },
    { PitchClass.C, "Diminished7", false },
    { PitchClass.C, "HalfDiminished", false },
    { PitchClass.C, "Augmented", false }
  };

  public static TheoryData<PitchClass, string, string> ChordData { get; } = new()
  {
    { PitchClass.C, "Major", "C,E,G" },
    { PitchClass.C, "Major7", "C,E,G,B" },
    { PitchClass.C, "Major9", "C,E,G,B,D" },
    { PitchClass.C, "Major11", "C,E,G,B,D,F" },
    { PitchClass.C, "Major13", "C,E,G,B,D,F,A" },
    { PitchClass.C, "Minor", "C,Eb,G" },
    { PitchClass.C, "Minor7", "C,Eb,G,Bb" },
    { PitchClass.C, "Minor9", "C,Eb,G,Bb,D" },
    { PitchClass.C, "Minor11", "C,Eb,G,Bb,D,F" },
    { PitchClass.C, "Minor13", "C,Eb,G,Bb,D,F,A" },
    { PitchClass.C, "Dominant7", "C,E,G,Bb" },
    { PitchClass.C, "Dominant9", "C,E,G,Bb,D" },
    { PitchClass.C, "Dominant11", "C,E,G,Bb,D,F" },
    { PitchClass.C, "Dominant13", "C,E,G,Bb,D,F,A" },
    { PitchClass.C, "SixNine", "C,E,G,A,D" },
    { PitchClass.C, "AddNine", "C,E,G,D" },
    { PitchClass.C, "Diminished", "C,Eb,Gb" },
    { PitchClass.C, "Diminished7", "C,Eb,Gb,A" },
    { PitchClass.C, "HalfDiminished", "C,Eb,Gb,Bb" },
    { PitchClass.C, "Augmented", "C,E,G#" }
  };

  #endregion
}
