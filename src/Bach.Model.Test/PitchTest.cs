// Module Name: ${File.FileName}
// Project:     ${File.ProjectName}
// Copyright (c) 2012, ${CurrentDate.Year}  Eddie Velasquez.
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

public sealed class PitchTest
{
  #region Public Methods

  [Fact]
  public void CompareTo_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    {
      var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      a.CompareTo( a ).Should().Be( 0 );

      var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      a.CompareTo( b ).Should().Be( 0 );
      b.CompareTo( a ).Should().Be( 0 );

      var c = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      b.CompareTo( c ).Should().Be( 0 );
      a.CompareTo( c ).Should().Be( 0 );
    }

    {
      var a = Pitch.Create( NoteName.C, Accidental.Natural, 1 );
      var b = Pitch.Create( NoteName.D, Accidental.Natural, 1 );

      ( -b.CompareTo( a ) ).Should()
                           .Be( a.CompareTo( b ) );

      var c = Pitch.Create( NoteName.E, Accidental.Natural, 1 );
      a.CompareTo( b ).Should().BeLessThan( 0 );
      b.CompareTo( c ).Should().BeLessThan( 0 );
      a.CompareTo( c ).Should().BeLessThan( 0 );
    }
  }

  [Theory]
  [MemberData( nameof( CompareToTestData ) )]
  public void CompareTo_ShouldReturnExpectedValue_WhenComparingPitches(
    Pitch left,
    Pitch right,
    int expectedSign )
  {
    Math.Sign( left.CompareTo( right ) ).Should().Be( expectedSign );
  }

  [Fact]
  public void ComparisonOperators_ShouldReturnExpectedValue_WhenComparingPitches()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( b > a ).Should()
           .BeTrue();
    ( b >= a ).Should()
            .BeTrue();
    ( b < a ).Should()
           .BeFalse();
    ( b <= a ).Should()
            .BeFalse();
  }

  [Theory]
  [MemberData( nameof( OutOfRangePitches ) )]
  public void Create_ShouldThrowArgumentOutOfRangeException_WhenPitchesAreOutOfRange(
      NoteName noteName,
      Accidental accidental,
      int octave )
  {
    var pitchClass = PitchClass.Create( noteName, accidental );
    var act = () => Pitch.Create( pitchClass, octave );
    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( CreateTestData ) )]
  public void Create_ShouldReturnExpectedValue_WhenUsingToneAndAccidental(
      NoteName noteName,
      Accidental accidental,
      int octave )
  {
    var target = Pitch.Create( noteName, accidental, octave );

    target.PitchClass.NoteName.Should().Be( noteName );
    target.PitchClass.Accidental.Should().Be( accidental );
    target.Octave.Should().Be( octave );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    object y = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    object z = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

    // ReSharper disable once EqualExpressionComparison
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
  public void Equals_ShouldReturnFalse_WhenComparingObjectOfDifferentType()
  {
    object actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Theory]
  [MemberData( nameof( FrequencyTestData ) )]
  public void Frequency_ShouldReturnExpectedValue( string pitchString, double expectedFrequency )
  {
    Pitch.Parse( pitchString ).Frequency.Should().BeApproximately( expectedFrequency, 0.01 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var expected = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void Max_ShouldReturnExpectedValue_WhenComparingPitches()
  {
    // Arrange
    var pitchA4 = Pitch.Parse( "A4" );
    var pitchB4 = Pitch.Parse( "B4" );

    // Act & Assert
    Pitch.Max( pitchA4, pitchB4 )
         .Should()
         .Be( pitchB4 );

    Pitch.Max( pitchB4, pitchA4 )
         .Should()
         .Be( pitchB4 );
  }

  [Theory]
  [MemberData( nameof( MidiTestData ) )]
  public void Midi_ShouldReturnExpectedValue_WhenCalculatingMidiValue( string pitchString, int expectedMidi )
  {
    Pitch.Parse( pitchString )
         .Midi.Should()
         .Be( expectedMidi );
  }

  [Fact]
  public void Midi_ShouldThrowArgumentOutOfRangeException_WhenMidiValueIsInvalid()
  {
    var act = () => Pitch.CreateFromMidi( 11 );
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Min_ShouldReturnExpectedValue_WhenComparingPitches()
  {
    // Arrange
    var pitchA4 = Pitch.Parse( "A4" );
    var pitchB4 = Pitch.Parse( "B4" );

    // Act & Assert
    Pitch.Min( pitchA4, pitchB4 )
         .Should()
         .Be( pitchA4 );

    Pitch.Min( pitchB4, pitchA4 )
         .Should()
         .Be( pitchA4 );
  }

  [Fact]
  public void AdditionOperator_ShouldReturnExpectedValue_WhenAddingPitchAndInt()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( c2 + 1 ).Should()
            .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 2 ) );
    ( c2 + -1 ).Should()
             .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( c2 + 2 ).Should()
            .Be( Pitch.Create( NoteName.D, Accidental.Natural, 2 ) );
    ( c2 + -2 ).Should()
             .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void DecrementOperator_ShouldReturnExpectedValue_WhenDecrementingPitch()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( --c2 ).Should()
          .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( --c2 ).Should()
          .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void EqualityOperator_ShouldReturnExpectedValue_WhenComparingPitches()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var c = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( a == b ).Should()
            .BeTrue();
    ( a == c ).Should()
            .BeFalse();
    ( b == c ).Should()
            .BeFalse();
  }

  [Fact]
  public void IncrementOperator_ShouldReturnExpectedValue_WhenIncrementingPitch()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( ++c2 ).Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 2 ) );
    ( ++c2 ).Should()
          .Be( Pitch.Create( NoteName.D, Accidental.Natural, 2 ) );
  }

  [Fact]
  public void InequalityOperator_ShouldReturnExpectedValue_WhenComparingPitches()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var c = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( a != c ).Should()
            .BeTrue();
    ( b != c ).Should()
            .BeTrue();
    ( a != b ).Should()
            .BeFalse();
  }

  [Fact]
  public void SubtractionOperator_ShouldReturnExpectedValue_WhenSubtractingPitchAndInt()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( c2 - 1 ).Should()
            .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( c2 - 2 ).Should()
            .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void SubtractionOperator_ShouldReturnExpectedValue_WhenSubtractingTwoPitches()
  {
    var cDoubleFlat2 = Pitch.Create( NoteName.C, Accidental.DoubleFlat, 2 );
    var cFlat2 = Pitch.Create( NoteName.C, Accidental.Flat, 2 );
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );
    var cSharp2 = Pitch.Create( NoteName.C, Accidental.Sharp, 2 );
    var cDoubleSharp2 = Pitch.Create( NoteName.C, Accidental.DoubleSharp, 2 );

    // Test interval with same pitches in the same octave with different accidentals
    ( cDoubleFlat2 - cDoubleFlat2 ).Should()
                                 .Be( 0 );
    ( cDoubleFlat2 - cFlat2 ).Should()
                           .Be( -1 );
    ( cDoubleFlat2 - c2 ).Should()
                       .Be( -2 );
    ( cDoubleFlat2 - cSharp2 ).Should()
                            .Be( -3 );
    ( cDoubleFlat2 - cDoubleSharp2 ).Should()
                                  .Be( -4 );
    ( cFlat2 - cDoubleFlat2 ).Should()
                           .Be( 1 );
    ( c2 - cDoubleFlat2 ).Should()
                       .Be( 2 );
    ( cSharp2 - cDoubleFlat2 ).Should()
                            .Be( 3 );
    ( cDoubleSharp2 - cDoubleFlat2 ).Should()
                                  .Be( 4 );

    var c3 = Pitch.Create( NoteName.C, Accidental.Natural, 3 );
    ( c2 - c3 ).Should()
             .Be( -12 );
    ( c3 - c2 ).Should()
             .Be( 12 );
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    var act1 = () => Pitch.Parse( "C$4" );
    act1.Should()
        .Throw<FormatException>();
    var act2 = () => { Pitch.Parse( "A9" ); };
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( AdditionOperatorTestData ) )]
  public void AdditionOperator_ShouldReturnExpectedValue_WhenAddingPitchAndInterval(
    string pitchString,
    Interval interval,
    string expectedPitchString )
  {
    ( Pitch.Parse( pitchString ) + interval ).Should().Be( Pitch.Parse( expectedPitchString ) );
  }

  [Theory]
  [MemberData( nameof( ToStringTestData ) )]
  public void ToString_ShouldReturnExpectedValue_WhenConvertingPitchToString(
      NoteName noteName, Accidental accidental, int octave, string expected )
  {
    var target = Pitch.Create( noteName, accidental, octave );
    target.ToString()
          .Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidPitchTestData ) )]
  public void TryParse_ShouldReturnTrue_WhenParsingValidPitch( string input, NoteName expectedNoteName, Accidental expectedAccidental, int expectedOctave )
  {
    Pitch.TryParse( input, out var actual )
         .Should()
         .BeTrue();

    actual.Should()
          .Be( Pitch.Create( expectedNoteName, expectedAccidental, expectedOctave ) );
  }

  [Theory]
  [MemberData( nameof( InvalidPitchTestData ) )]
  public void TryParse_ShouldReturnFalse_WhenParsingInvalidPitch( string? input )
  {
    Pitch.TryParse( input!, out var actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var y = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var z = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

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
  public void Equals_ShouldReturnFalse_WhenComparingDifferentTypes()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull_ObjectVariant()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<Pitch, Pitch, int> CompareToTestData =>
    new()
    {
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Natural, 1), 0 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Sharp, 1), -1 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Flat, 1), 1 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Natural, 2), -1 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Flat, 2), -1 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Sharp, 2), -1 },
      { Pitch.Create(NoteName.A, Accidental.Natural, 1), Pitch.Create(NoteName.C, Accidental.Natural, 1), 1 },
      { Pitch.Create(NoteName.C, Accidental.Natural, 1), Pitch.Create(NoteName.A, Accidental.Natural, 1), -1 },
    };

  public static TheoryData<NoteName, Accidental, int> OutOfRangePitches => new()
  {
    { NoteName.C, Accidental.Flat, Pitch.MinOctave },
    { NoteName.C, Accidental.DoubleFlat, Pitch.MinOctave },
    { NoteName.B, Accidental.Sharp, Pitch.MaxOctave },
    { NoteName.B, Accidental.DoubleSharp, Pitch.MaxOctave }
  };

  public static TheoryData<NoteName, Accidental, int> CreateTestData => new()
  {
    { NoteName.C, Accidental.Natural, Pitch.MinOctave },
    { NoteName.A, Accidental.Natural, 1 },
    { NoteName.G, Accidental.Natural, Pitch.MaxOctave }
  };

  public static TheoryData<string, double> FrequencyTestData => new()
{
    { "A0", 27.5 },
    { "C1", 32.7 },
    { "F2", 87.31 },
    { "A4", 440.0 },
    { "C5", 523.25 },
    { "F4", 349.23 },
    { "A5", 880.0 },
    { "C8", 4186.01 },
    { "G9", 12543.85 }
};

  public static TheoryData<string, int> MidiTestData => new()
  {
    { "C0", 12 },
    { "C1", 24 },
    { "C2", 36 },
    { "C3", 48 },
    { "C4", 60 },
    { "C5", 72 },
    { "C6", 84 },
    { "C7", 96 },
    { "C8", 108 },
    { "C9", 120 },
    { "G9", 127 }
  };

  public static TheoryData<string, Interval, string> AdditionOperatorTestData =>
    new()
    {
      { "C4", Interval.MajorThird, "E4" },
      { "C#4", Interval.MinorThird, "E4" },
      { "D4", Interval.MinorThird, "F4" },
      { "D4", Interval.Fourth, "G4" },
      { "E4", Interval.Fourth, "A4" },
      { "Eb4", Interval.Fourth, "Ab4" },
      { "Eb4", Interval.AugmentedThird, "G#4" },
      { "F4", Interval.MajorSixth, "D5" },
      { "G4", Interval.Fifth, "D5" },
      { "F4", Interval.Fifth, "C5" },
      { "A4", Interval.Fifth, "E5" },
      { "Ab4", Interval.Fifth, "Eb5" },
      { "G#4", Interval.DiminishedSixth, "Eb5" },
      { "F#4", Interval.AugmentedFourth, "C5" },
      { "Gb4", Interval.DiminishedFifth, "C5" },
      { "C4", Interval.AugmentedSecond, "D#4" },
      { "C4", Interval.DiminishedFifth, "F#4" },
      { "C4", Interval.AugmentedFourth, "Gb4" },
      { "D#4", Interval.DiminishedSeventh, "C5" },
      { "D#4", Interval.DiminishedThird, "F4" },
      { "D##4", Interval.DiminishedFourth, "G#4" }
    };

  public static TheoryData<NoteName, Accidental, int, string> ToStringTestData => new()
  {
    { NoteName.A, Accidental.DoubleFlat, 1, "Abb1" },
    { NoteName.A, Accidental.Flat, 1, "Ab1" },
    { NoteName.A, Accidental.Natural, 1, "A1" },
    { NoteName.A, Accidental.Sharp, 1, "A#1" },
    { NoteName.A, Accidental.DoubleSharp, 1, "A##1" }
  };

  public static TheoryData<string, NoteName, Accidental, int> ValidPitchTestData => new()
  {
    { "C4", NoteName.C, Accidental.Natural, 4 },
    { "C#4", NoteName.C, Accidental.Sharp, 4 },
    { "C##4", NoteName.C, Accidental.DoubleSharp, 4 },
    { "Cb4", NoteName.C, Accidental.Flat, 4 },
    { "Cbb4", NoteName.C, Accidental.DoubleFlat, 4 },
    { "C2", NoteName.C, Accidental.Natural, 2 },
    { "C#2", NoteName.C, Accidental.Sharp, 2 },
    { "C##2", NoteName.C, Accidental.DoubleSharp, 2 },
    { "Cb2", NoteName.C, Accidental.Flat, 2 },
    { "Cbb2", NoteName.C, Accidental.DoubleFlat, 2 },
    { "60", NoteName.C, Accidental.Natural, 4 }
  };

  public static TheoryData<string?> InvalidPitchTestData => new()
  {
    "H",
    "C!",
    "C#-1",
    "C#10",
    "C#b2",
    "Cb#2",
    (string?)null,
    "",
    "256",
    "-1",
    "1X"
  };

  #endregion
}
