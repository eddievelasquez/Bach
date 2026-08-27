// Module Name: ChordTest.cs
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

using System.Linq;

namespace Bach.Model.Test;

public sealed class ChordTests
{
  #region Properties

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

  #region Public Methods

  [Theory]
  [MemberData( nameof( ChordData ) )]
  public void Constructor_ShouldInitializeChordCorrectly(
    PitchClass root,
    string formulaName,
    string expectedNotes )
  {
    var chord = Chord.Create( root, formulaName );

    var actualNotes = chord.Take(
      expectedNotes.ParsePitchClasses()
                   .Count
    );

    actualNotes.Should()
               .BeEquivalentTo( expectedNotes.ParsePitchClasses() );
  }

  [Fact]
  public void Constructor_ShouldInitializeChordUsingFormula()
  {
    var formula = Registry.ChordFormulas["Minor"];
    var target = Chord.Create( PitchClass.C, formula );

    target.Root.Should()
          .Be( PitchClass.C );

    target.Formula.Should()
          .Be( Registry.ChordFormulas["Minor"] );

    target.Name.Should()
          .Be( "Cm" );

    target.Should()
          .BeEquivalentTo( "C,Eb,G".ParsePitchClasses() );

    target.ToString()
          .Should()
          .Be( target.Name );
  }

  [Fact]
  public void Constructor_ShouldInitializeChordUsingString()
  {
    var target = Chord.Create( PitchClass.C, "Minor" );

    target.Root.Should()
          .Be( PitchClass.C );

    target.Formula.Should()
          .Be( Registry.ChordFormulas["Minor"] );

    target.Name.Should()
          .Be( "Cm" );

    target.Should()
          .BeEquivalentTo( "C,Eb,G".ParsePitchClasses() );

    target.ToString()
          .Should()
          .Be( target.Name );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenFormulaNameIsEmpty()
  {
    var act = () => Chord.Create( PitchClass.C, "" );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenFormulaIsNull()
  {
    var act = () => Chord.Create( PitchClass.C, (ChordFormula) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenFormulaNameIsNull()
  {
    var act = () => Chord.Create( PitchClass.C, (string) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Enumerator_ShouldEnumerateChordCorrectly()
  {
    var cMajor = Chord.Create( PitchClass.C, "Major" );
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
  public void Equals_ShouldReturnFalse_WhenComparingDifferentInversions()
  {
    var rootPosition = Chord.Create( PitchClass.C, "Major" );
    var firstInversion = rootPosition.GetInversion( 1 );

    rootPosition.Equals( firstInversion )
                .Should()
                .BeFalse();

    firstInversion.GetHashCode()
                  .Should()
                  .NotBe( rootPosition.GetHashCode() );
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingDifferentType()
  {
    object actual = Chord.Create( PitchClass.C, "Major" );

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = Chord.Create( PitchClass.C, "Major" );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingSameObject()
  {
    var actual = Chord.Create( PitchClass.C, "Major" );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = Chord.Create( PitchClass.C, "Major" );
    object y = Chord.Create( PitchClass.C, "Major" );
    object z = Chord.Create( PitchClass.C, "Major" );

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
  public void GetHashCode_ShouldReturnSameValue_ForEquivalentObjects()
  {
    var actual = Chord.Create( PitchClass.C, "Major" );
    var expected = Chord.Create( PitchClass.C, "Major" );

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
    var cMajor = Chord.Create( PitchClass.C, "Major" );
    var firstInversion = cMajor.GetInversion( 1 );

    firstInversion.Should()
                  .NotBeNull();

    firstInversion.Name.Should()
                  .Be( "C/E" );

    firstInversion.Should()
                  .BeEquivalentTo( "E,G,C".ParsePitchClasses() );

    var secondInversion = cMajor.GetInversion( 2 );

    secondInversion.Should()
                   .NotBeNull();

    secondInversion.Name.Should()
                   .Be( "C/G" );

    secondInversion.Should()
                   .BeEquivalentTo( "G,C,E".ParsePitchClasses() );

    var act = () => cMajor.GetInversion( 3 );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void ImplementsGenericInterface_ShouldExposeSharedContract()
  {
    var chord = Chord.Create( PitchClass.C, "Major" );

    chord.Root.Should()
         .Be( PitchClass.C );

    chord.Bass.Should()
         .Be( PitchClass.C );

    chord.Inversion.Should()
         .Be( 0 );

    chord.Formula.Should()
         .Be( Registry.ChordFormulas["Major"] );

    chord.Name.Should()
         .Be( "C" );

    var inversion = chord.GetInversion( 1 );

    inversion.Should()
             .NotBeNull();

    inversion.Inversion.Should()
             .Be( 1 );

    inversion.Root.Should()
             .Be( PitchClass.C );

    inversion.Bass.Should()
             .Be( PitchClass.E );
  }

  [Theory]
  [MemberData( nameof( ExtendedChordData ) )]
  public void IsExtended_ShouldReturnExpectedResult(
    PitchClass root,
    string formulaName,
    bool isExtended )
  {
    var chord = Chord.Create( root, formulaName );

    chord.IsExtended.Should()
         .Be( isExtended );
  }

  [Fact]
  public void Parse_InvalidString_ShouldThrow()
  {
    var act = () => Chord.Parse( "Invalid" );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Parse_ValidString_ShouldSucceed()
  {
    var chord = Chord.Parse( "Cmaj7" );

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( PitchClass.C );

    chord.Formula.Symbol.Should()
         .Be( "maj7" );
  }

  [Fact]
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingDifferentType()
  {
    var actual = Chord.Create( PitchClass.C, "Major" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingToNull()
  {
    var actual = Chord.Create( PitchClass.C, "Major" );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldSatisfyEquivalenceRelation()
  {
    var x = Chord.Create( PitchClass.C, "Major" );
    var y = Chord.Create( PitchClass.C, "Major" );
    var z = Chord.Create( PitchClass.C, "Major" );

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

  [Theory]
  [InlineData( "" )]
  [InlineData( " " )]
  [InlineData( "Invalid" )]
  [InlineData( "C/invalid" )]
  [InlineData( "C/D" )] // D is not in C Major triad
  public void TryParse_InvalidChords_ShouldFail(
    string input )
  {
    var result = Chord.TryParse( input.AsSpan(), null, out var chord, out _ );

    result.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "C", "C", "", null )]
  [InlineData( "Cm", "C", "m", null )]
  [InlineData( "Cmaj7", "C", "maj7", null )]
  [InlineData( "C/E", "C", "", "E" )]
  [InlineData( "Am7/G", "A", "m7", "G" )]
  [InlineData( "C#7#9/E#", "C#", "7#9", "E#" )]
  public void TryParse_ValidChords_ShouldSucceed(
    string input,
    string expectedRoot,
    string expectedFormula,
    string? expectedBass )
  {
    var result = Chord.TryParse( input.AsSpan(), null, out var chord, out var tail );

    result.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.ToString()
         .Should()
         .Be( expectedRoot );

    chord.Formula.Symbol.Should()
         .Be( expectedFormula );

    if( expectedBass != null )
    {
      chord.Bass.ToString()
           .Should()
           .Be( expectedBass );

      chord.Inversion.Should()
           .BeGreaterThan( 0 );
    }
    else
    {
      chord.Inversion.Should()
           .Be( 0 );
    }

    tail.IsEmpty.Should()
        .BeTrue();
  }

  [Fact]
  public void TryParse_WithBassAndTail_ShouldWork()
  {
    var result = Chord.TryParse( "Cmaj7/E rest of the song".AsSpan(), null, out var chord, out var tail );

    result.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( PitchClass.C );

    chord.Formula.Symbol.Should()
         .Be( "maj7" );

    chord.Bass.Should()
         .Be( PitchClass.E );

    tail.ToString()
        .Should()
        .Be( " rest of the song" );
  }

  [Fact]
  public void TryParse_WithTail_ShouldWork()
  {
    var result = Chord.TryParse( "Cmaj7 and more".AsSpan(), null, out var chord, out var tail );

    result.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( PitchClass.C );

    chord.Formula.Symbol.Should()
         .Be( "maj7" );

    tail.ToString()
        .Should()
        .Be( " and more" );
  }

  #endregion
}
