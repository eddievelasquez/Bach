// Module Name: PitchClassTest.cs
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

public sealed class PitchClassTest
{
  #region Public Methods

  [Theory]
  [MemberData( nameof( AddIntervalFlats ) )]
  public void Add_ShouldReturnExpectedPitchClass_WhenAddingSemitonesWithFlats( PitchClass pitchClass, int interval, PitchClass expectedFlat )
  {
    pitchClass.Add( interval )
              .Should()
              .Be( expectedFlat );
  }

  [Theory]
  [MemberData( nameof( AddSemitonesSharps ) )]
  public void Add_ShouldReturnExpectedPitchClass_WhenAddingSemitonesWithSharps( PitchClass pitchClass, int interval, PitchClass expectedSharp )
  {
    pitchClass.Add( interval )
              .Should()
              .Be( expectedSharp );
  }

  [Theory]
  [MemberData( nameof( AddIntervals ) )]
  public void AdditionOperator_ShouldReturnExpectedPitchClass_WhenAddingInterval(
    PitchClass pitchClass, Interval interval, PitchClass expected )
  {
    ( pitchClass + interval ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( AddSemitonesSharps ) )]
  public void AdditionOperator_ShouldReturnExpectedPitchClass_WhenAddingSemitones( PitchClass left, int right, PitchClass expected )
  {
    ( left + right ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( ArithmeticDecrementTestData ) )]
  public void DecrementOperator_ShouldReturnExpectedPitchClass_WhenDecrementing( PitchClass initial, PitchClass afterPostDecrement, PitchClass afterPreDecrement )
  {
    var pitchClass = initial;
    ( pitchClass-- ).Should().Be( afterPostDecrement );

    pitchClass.Should().Be( afterPreDecrement );
    ( --pitchClass ).Should().Be( afterPreDecrement.Subtract( 1 ) );
  }

  [Theory]
  [MemberData( nameof( EqualsTestData ) )]
  public void Equals_ShouldReturnExpectedValue_WhenComparingPitchClasses(
    PitchClass pitchClass, object? other, bool expected )
  {
    pitchClass.Equals( other ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( EnharmonicData ) )]
  public void GetEnharmonic_ShouldReturnExpectedPitchClass_WhenGivenEnharmonicNoteName(
    PitchClass pitchClass, string enharmonic )
  {
    var enharmonicPitchClass = PitchClass.Parse( enharmonic );
    pitchClass.GetEnharmonic( enharmonicPitchClass.NoteName )
              .Should()
              .Be( enharmonicPitchClass );
  }

  [Theory]
  [MemberData( nameof( NotEnharmonicTestData ) )]
  public void GetEnharmonic_ShouldReturnNull_WhenNoEnharmonicExists( PitchClass pitchClass, NoteName startInclusive, NoteName lastExclusive )
  {
    while( startInclusive != lastExclusive )
    {
      pitchClass.GetEnharmonic( startInclusive )
                .Should()
                .BeNull();

      ++startInclusive;
    }
  }

  [Theory]
  [MemberData( nameof( ArithmeticIncrementTestData ) )]
  public void IncrementOperator_ShouldReturnExpectedPitchClass_WhenIncrementing( PitchClass initial, PitchClass afterPostIncrement, PitchClass afterPreIncrement )
  {
    var pitchClass = initial;
    ( pitchClass++ ).Should().Be( afterPostIncrement );

    pitchClass.Should().Be( afterPreIncrement );
    ( ++pitchClass ).Should().Be( afterPreIncrement.Add( 1 ) );
  }

  [Theory]
  [MemberData( nameof( InvalidPitchClassStrings ) )]
  public void Parse_ShouldThrowException_WhenGivenInvalidString(
    string value, Type exceptionType )
  {
    var act = () => PitchClass.Parse( value );
    act.Should().Throw<Exception>().Where( e => e.GetType() == exceptionType );
  }

  [Theory]
  [MemberData( nameof( ValidPitchClassStrings ) )]
  public void Parse_ShouldReturnExpectedPitchClass_WhenGivenValidString(
    string value, NoteName noteName, Accidental accidental )
  {
    var expected = PitchClass.Create( noteName, accidental );
    PitchClass.Parse( value ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( NoteNamesAndAccidentals ) )]
  public void PredefinedNotes_ShouldReturnExpectedNoteNameAndAccidental(
    PitchClass pitchClass, NoteName noteName, Accidental accidental )
  {
    pitchClass.NoteName.Should()
              .Be( noteName );

    pitchClass.Accidental.Should()
              .Be( accidental );
  }

  [Theory]
  [MemberData( nameof( RelationalOperatorsTestData ) )]
  public void RelationalOperators_ShouldReturnTrue_WhenComparingPitchClasses(
    bool actual )
  {
    actual.Should().BeTrue();
  }

  [Theory]
  [MemberData( nameof( SubtractIntervalTestData ) )]
  public void Subtract_ShouldReturnExpectedPitchClass_WhenSubtractingInterval(
    PitchClass pitchClass, Interval interval, PitchClass expected )
  {
    ( pitchClass - interval ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( SubtractTestData ) )]
  public void Subtract_ShouldReturnExpectedPitchClass_WhenSubtractingSemitones(
    PitchClass pitchClass, int interval, PitchClass expectedSharp, PitchClass? expectedFlat )
  {
    pitchClass.Subtract( interval )
              .Should()
              .Be( expectedSharp );

    pitchClass.Subtract( interval )
              .Should()
              .Be( expectedFlat ?? expectedSharp );
  }

  [Theory]
  [MemberData( nameof( SubtractPitchClassTestData ) )]
  public void SubtractOperator_ShouldReturnExpectedInterval_WhenSubtractingPitchClasses(
    PitchClass left, PitchClass right, Interval expected )
  {
    ( left - right ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( ArithmeticSubtractionTestData ) )]
  public void SubtractionOperator_ShouldReturnExpectedPitchClass_WhenSubtractingSemitones( PitchClass left, int right, PitchClass expected )
  {
    ( left - right ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( CompareToTestData ) )]
  public void CompareTo_ShouldReturnExpectedSign_WhenComparingPitchClasses(
    PitchClass left, PitchClass right, int expectedSign )
  {
    Math.Sign( left.CompareTo( right ) ).Should().Be( expectedSign );
  }

  [Theory]
  [MemberData( nameof( ConstructorTestData ) )]
  public void Create_ShouldReturnExpectedPitchClass_WhenGivenNoteNameAndAccidental(
    NoteName noteName, Accidental accidental )
  {
    var note = PitchClass.Create( noteName, accidental );

    note.NoteName.Should()
        .Be( noteName );

    note.Accidental.Should()
        .Be( accidental );
  }

  [Theory]
  [MemberData( nameof( ToStringTestData ) )]
  public void ToString_ShouldReturnExpectedString(
    NoteName noteName,
    Accidental accidental,
    string expected )
  {
    var pitchClass = PitchClass.Create( noteName, accidental );

    pitchClass.ToString()
              .Should()
              .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ToStringWithFormatTestData ) )]
  public void ToString_ShouldReturnExpectedString_WhenFormatProvided(
    NoteName noteName,
    Accidental accidental,
    string? format,
    string expected )
  {
    var pitchClass = PitchClass.Create( noteName, accidental );

    pitchClass.ToString( format! )
              .Should()
              .Be( expected );
  }

  [Theory]
  [MemberData( nameof( TryParseInvalidStringsTestData ) )]
  public void TryParse_ShouldReturnFalse_WhenGivenInvalidString( string value )
  {
    PitchClass.TryParse( value, out _ ).Should().BeFalse();
  }

  [Theory]
  [MemberData( nameof( TryParseValidStringsTestData ) )]
  public void TryParse_ShouldReturnExpectedPitchClass_WhenGivenValidString(
    string value, PitchClass expected )
  {
    PitchClass.TryParse( value, out var actual )
              .Should()
              .BeTrue();

    actual.Should()
          .Be( expected );
  }

  #endregion

  #region TheoryData

  public static TheoryData<PitchClass, int, PitchClass, PitchClass?> AddTestData =>
    new()
    {
        { PitchClass.C, 1, PitchClass.CSharp, PitchClass.DFlat },
        { PitchClass.C, 2, PitchClass.D, null },
        { PitchClass.C, 3, PitchClass.DSharp, PitchClass.EFlat },
        { PitchClass.C, 4, PitchClass.E, null },
        { PitchClass.C, 5, PitchClass.F, null },
        { PitchClass.C, 6, PitchClass.FSharp, PitchClass.GFlat },
        { PitchClass.C, 7, PitchClass.G, null },
        { PitchClass.C, 8, PitchClass.GSharp, PitchClass.AFlat },
        { PitchClass.C, 9, PitchClass.A, null },
        { PitchClass.C, 10, PitchClass.ASharp, PitchClass.BFlat },
        { PitchClass.C, 11, PitchClass.B, null },
        { PitchClass.C, 12, PitchClass.C, null }
    };

  public static TheoryData<PitchClass, PitchClass, int> CompareToTestData =>
    new()
    {
        { PitchClass.C, PitchClass.C, 0 },
        { PitchClass.C, PitchClass.D, -1 },
        { PitchClass.D, PitchClass.C, 1 },
        { PitchClass.C, PitchClass.B, -1 },
        { PitchClass.B, PitchClass.C, 1 }
    };

  public static TheoryData<NoteName, Accidental> ConstructorTestData =>
    new()
    {
        { NoteName.C, Accidental.DoubleFlat },
        { NoteName.C, Accidental.Flat },
        { NoteName.C, Accidental.Natural },
        { NoteName.C, Accidental.Sharp },
        { NoteName.C, Accidental.DoubleSharp },
        { NoteName.D, Accidental.DoubleFlat },
        { NoteName.D, Accidental.Flat },
        { NoteName.D, Accidental.Natural },
        { NoteName.D, Accidental.Sharp },
        { NoteName.D, Accidental.DoubleSharp },
        { NoteName.E, Accidental.DoubleFlat },
        { NoteName.E, Accidental.Flat },
        { NoteName.E, Accidental.Natural },
        { NoteName.E, Accidental.Sharp },
        { NoteName.E, Accidental.DoubleSharp },
        { NoteName.F, Accidental.DoubleFlat },
        { NoteName.F, Accidental.Flat },
        { NoteName.F, Accidental.Natural },
        { NoteName.F, Accidental.Sharp },
        { NoteName.F, Accidental.DoubleSharp },
        { NoteName.G, Accidental.DoubleFlat },
        { NoteName.G, Accidental.Flat },
        { NoteName.G, Accidental.Natural },
        { NoteName.G, Accidental.Sharp },
        { NoteName.G, Accidental.DoubleSharp },
        { NoteName.A, Accidental.DoubleFlat },
        { NoteName.A, Accidental.Flat },
        { NoteName.A, Accidental.Natural },
        { NoteName.A, Accidental.Sharp },
        { NoteName.A, Accidental.DoubleSharp },
        { NoteName.B, Accidental.DoubleFlat },
        { NoteName.B, Accidental.Flat },
        { NoteName.B, Accidental.Natural },
        { NoteName.B, Accidental.Sharp },
        { NoteName.B, Accidental.DoubleSharp }
    };

  public static TheoryData<PitchClass, object?, bool> EqualsTestData =>
    new()
    {
        { PitchClass.C, PitchClass.Create(NoteName.C), true },
        { PitchClass.C, null, false }
    };

  public static TheoryData<PitchClass, string> EnharmonicData =>
    new()
    {
        { PitchClass.C, "Dbb" },
        { PitchClass.C, "B#" },
        { PitchClass.CSharp, "Db" },
        { PitchClass.CSharp, "B##" },
        { PitchClass.D, "Ebb" },
        { PitchClass.D, "C##" },
        { PitchClass.DSharp, "Fbb" },
        { PitchClass.DSharp, "Eb" },
        { PitchClass.E, "Fb" },
        { PitchClass.E, "D##" },
        { PitchClass.F, "Gbb" },
        { PitchClass.F, "E#" },
        { PitchClass.FSharp, "Gb" },
        { PitchClass.FSharp, "E##" },
        { PitchClass.G, "Abb" },
        { PitchClass.G, "F##" },
        { PitchClass.GSharp, "Ab" },
        { PitchClass.A, "Bbb" },
        { PitchClass.A, "G##" },
        { PitchClass.ASharp, "Cbb" },
        { PitchClass.ASharp, "Bb" },
        { PitchClass.B, "Cb" },
        { PitchClass.B, "A##" }
    };

  public static TheoryData<PitchClass, NoteName, NoteName> NotEnharmonicTestData =>
    new()
    {
        { PitchClass.C, NoteName.E, NoteName.G },
        { PitchClass.CSharp, NoteName.E, NoteName.G },
        { PitchClass.D, NoteName.F, NoteName.C },
        { PitchClass.DSharp, NoteName.G, NoteName.D },
        { PitchClass.E, NoteName.G, NoteName.D },
        { PitchClass.F, NoteName.A, NoteName.E },
        { PitchClass.FSharp, NoteName.A, NoteName.E },
        { PitchClass.G, NoteName.B, NoteName.F },
        { PitchClass.GSharp, NoteName.B, NoteName.G },
        { PitchClass.A, NoteName.C, NoteName.G },
        { PitchClass.ASharp, NoteName.D, NoteName.A },
        { PitchClass.B, NoteName.D, NoteName.A }
    };

  public static TheoryData<bool> RelationalOperatorsTestData =>
  [
    PitchClass.C == PitchClass.Create( NoteName.B, Accidental.Sharp ), PitchClass.C != PitchClass.B,
    PitchClass.C < PitchClass.B, PitchClass.C <= PitchClass.B, PitchClass.D > PitchClass.C, PitchClass.D >= PitchClass.C
  ];

  public static TheoryData<PitchClass, PitchClass, PitchClass?> NextTestData =>
    new()
    {
        { PitchClass.C, PitchClass.CSharp, PitchClass.DFlat },
        { PitchClass.CSharp, PitchClass.D, null },
        { PitchClass.DFlat, PitchClass.D, null },
        { PitchClass.D, PitchClass.DSharp, PitchClass.EFlat },
        { PitchClass.DSharp, PitchClass.E, null },
        { PitchClass.EFlat, PitchClass.E, null },
        { PitchClass.E, PitchClass.F, null },
        { PitchClass.F, PitchClass.FSharp, PitchClass.GFlat },
        { PitchClass.FSharp, PitchClass.G, null },
        { PitchClass.GFlat, PitchClass.G, null },
        { PitchClass.G, PitchClass.GSharp, PitchClass.AFlat },
        { PitchClass.GSharp, PitchClass.A, null },
        { PitchClass.AFlat, PitchClass.A, null },
        { PitchClass.A, PitchClass.ASharp, PitchClass.BFlat },
        { PitchClass.ASharp, PitchClass.B, null },
        { PitchClass.BFlat, PitchClass.B, null },
        { PitchClass.B, PitchClass.C, null },
        { PitchClass.Create(NoteName.C, Accidental.DoubleSharp), PitchClass.DSharp, PitchClass.EFlat },
        { PitchClass.Create(NoteName.E, Accidental.DoubleSharp), PitchClass.G, null },
        { PitchClass.Create(NoteName.B, Accidental.DoubleSharp), PitchClass.D, null }
    };

  public static TheoryData<PitchClass, Interval, PitchClass> AddIntervals =>
    new()
    {
        { PitchClass.C, Interval.MajorThird, PitchClass.E },
        { PitchClass.CSharp, Interval.MinorThird, PitchClass.E },
        { PitchClass.D, Interval.MinorThird, PitchClass.F },
        { PitchClass.D, Interval.Fourth, PitchClass.G },
        { PitchClass.E, Interval.Fourth, PitchClass.A },
        { PitchClass.EFlat, Interval.Fourth, PitchClass.AFlat },
        { PitchClass.EFlat, Interval.AugmentedThird, PitchClass.GSharp },
        { PitchClass.F, Interval.MajorSixth, PitchClass.D },
        { PitchClass.G, Interval.Fifth, PitchClass.D },
        { PitchClass.F, Interval.Fifth, PitchClass.C },
        { PitchClass.A, Interval.Fifth, PitchClass.E },
        { PitchClass.AFlat, Interval.Fifth, PitchClass.EFlat },
        { PitchClass.GSharp, Interval.DiminishedSixth, PitchClass.EFlat },
        { PitchClass.FSharp, Interval.AugmentedFourth, PitchClass.C },
        { PitchClass.GFlat, Interval.DiminishedFifth, PitchClass.C },
        { PitchClass.C, Interval.AugmentedSecond, PitchClass.DSharp },
        { PitchClass.C, Interval.DiminishedFifth, PitchClass.FSharp },
        { PitchClass.C, Interval.AugmentedFourth, PitchClass.GFlat },
        { PitchClass.DSharp, Interval.DiminishedSeventh, PitchClass.C },
        { PitchClass.DSharp, Interval.DiminishedThird, PitchClass.F },
        { PitchClass.Parse("D##"), Interval.DiminishedFourth, PitchClass.GSharp }
    };

  public static TheoryData<PitchClass, Interval, PitchClass> SubtractIntervalTestData =>
    new()
    {
        { PitchClass.F, Interval.AugmentedFourth, PitchClass.Parse("Cb") },
        { PitchClass.E, Interval.MajorThird, PitchClass.C },
        { PitchClass.E, Interval.MinorThird, PitchClass.CSharp },
        { PitchClass.F, Interval.MinorThird, PitchClass.D },
        { PitchClass.G, Interval.Fourth, PitchClass.D },
        { PitchClass.A, Interval.Fourth, PitchClass.E },
        { PitchClass.AFlat, Interval.Fourth, PitchClass.EFlat },
        { PitchClass.GSharp, Interval.AugmentedThird, PitchClass.EFlat },
        { PitchClass.D, Interval.MajorSixth, PitchClass.F },
        { PitchClass.D, Interval.Fifth, PitchClass.G },
        { PitchClass.C, Interval.Fifth, PitchClass.F },
        { PitchClass.E, Interval.Fifth, PitchClass.A },
        { PitchClass.EFlat, Interval.Fifth, PitchClass.AFlat },
        { PitchClass.EFlat, Interval.DiminishedSixth, PitchClass.GSharp },
        { PitchClass.C, Interval.AugmentedFourth, PitchClass.FSharp },
        { PitchClass.C, Interval.DiminishedFifth, PitchClass.GFlat },
        { PitchClass.DSharp, Interval.AugmentedSecond, PitchClass.C },
        { PitchClass.FSharp, Interval.DiminishedFifth, PitchClass.C },
        { PitchClass.GFlat, Interval.AugmentedFourth, PitchClass.C },
        { PitchClass.C, Interval.DiminishedSeventh, PitchClass.DSharp },
        { PitchClass.F, Interval.DiminishedThird, PitchClass.DSharp },
        { PitchClass.GSharp, Interval.DiminishedFourth, PitchClass.Parse("D##") }
    };

  public static TheoryData<PitchClass, PitchClass, Interval> SubtractPitchClassTestData =>
    new()
    {
        { PitchClass.C, PitchClass.E, Interval.MajorThird },
        { PitchClass.CSharp, PitchClass.E, Interval.MinorThird },
        { PitchClass.D, PitchClass.F, Interval.MinorThird },
        { PitchClass.D, PitchClass.G, Interval.Fourth },
        { PitchClass.E, PitchClass.A, Interval.Fourth },
        { PitchClass.EFlat, PitchClass.AFlat, Interval.Fourth },
        { PitchClass.EFlat, PitchClass.GSharp, Interval.AugmentedThird },
        { PitchClass.F, PitchClass.D, Interval.MajorSixth },
        { PitchClass.G, PitchClass.D, Interval.Fifth },
        { PitchClass.F, PitchClass.C, Interval.Fifth },
        { PitchClass.A, PitchClass.E, Interval.Fifth },
        { PitchClass.AFlat, PitchClass.EFlat, Interval.Fifth },
        { PitchClass.GSharp, PitchClass.EFlat, Interval.DiminishedSixth },
        { PitchClass.C, PitchClass.FSharp, Interval.AugmentedFourth },
        { PitchClass.C, PitchClass.GFlat, Interval.DiminishedFifth },
        { PitchClass.C, PitchClass.DSharp, Interval.AugmentedSecond },
        { PitchClass.FSharp, PitchClass.C, Interval.DiminishedFifth },
        { PitchClass.GFlat, PitchClass.C, Interval.AugmentedFourth },
        { PitchClass.DSharp, PitchClass.C, Interval.DiminishedSeventh },
        { PitchClass.C, PitchClass.Create(NoteName.E, Accidental.DoubleFlat), Interval.DiminishedThird },
        { PitchClass.Create(NoteName.D, Accidental.DoubleSharp), PitchClass.GSharp, Interval.DiminishedFourth }
    };

  public static TheoryData<string, Type> InvalidPitchClassStrings =>
    new()
   {
       { null!, typeof(ArgumentNullException) },
       { "", typeof(ArgumentException) },
       { "J", typeof(FormatException) },
       { "C$", typeof(FormatException) }
    };

  public static TheoryData<string, NoteName, Accidental> ValidPitchClassStrings =>
    new()
    {
        { "Cbb", NoteName.C, Accidental.DoubleFlat },
        { "CB", NoteName.C, Accidental.Flat },
        { "C", NoteName.C, Accidental.Natural },
        { "c#", NoteName.C, Accidental.Sharp },
        { "c##", NoteName.C, Accidental.DoubleSharp }
    };

  public static TheoryData<PitchClass, NoteName, Accidental> NoteNamesAndAccidentals =>
    new()
    {
        { PitchClass.C, NoteName.C, Accidental.Natural },
        { PitchClass.CSharp, NoteName.C, Accidental.Sharp },
        { PitchClass.DFlat, NoteName.D, Accidental.Flat },
        { PitchClass.D, NoteName.D, Accidental.Natural },
        { PitchClass.DSharp, NoteName.D, Accidental.Sharp },
        { PitchClass.EFlat, NoteName.E, Accidental.Flat },
        { PitchClass.E, NoteName.E, Accidental.Natural },
        { PitchClass.F, NoteName.F, Accidental.Natural },
        { PitchClass.FSharp, NoteName.F, Accidental.Sharp },
        { PitchClass.GFlat, NoteName.G, Accidental.Flat },
        { PitchClass.G, NoteName.G, Accidental.Natural },
        { PitchClass.GSharp, NoteName.G, Accidental.Sharp },
        { PitchClass.AFlat, NoteName.A, Accidental.Flat },
        { PitchClass.A, NoteName.A, Accidental.Natural },
        { PitchClass.ASharp, NoteName.A, Accidental.Sharp },
        { PitchClass.BFlat, NoteName.B, Accidental.Flat },
        { PitchClass.B, NoteName.B, Accidental.Natural }
    };

  public static TheoryData<PitchClass, int, PitchClass, PitchClass?> SubtractTestData =>
    new()
    {
        { PitchClass.B, 1, PitchClass.BFlat, PitchClass.ASharp },
        { PitchClass.B, 2, PitchClass.A, null },
        { PitchClass.B, 3, PitchClass.GSharp, PitchClass.AFlat },
        { PitchClass.B, 4, PitchClass.G, null },
        { PitchClass.B, 5, PitchClass.FSharp, PitchClass.GFlat },
        { PitchClass.B, 6, PitchClass.F, null },
        { PitchClass.B, 7, PitchClass.E, null },
        { PitchClass.B, 8, PitchClass.DSharp, PitchClass.EFlat },
        { PitchClass.B, 9, PitchClass.D, null },
        { PitchClass.B, 10, PitchClass.CSharp, PitchClass.DFlat },
        { PitchClass.B, 11, PitchClass.C, null },
        { PitchClass.B, 12, PitchClass.B, null }
    };

  public static TheoryData<NoteName, Accidental, string> ToStringTestData =>
    new()
    {
      { NoteName.C, Accidental.DoubleFlat, "Cbb" },
      { NoteName.C, Accidental.Flat, "Cb" },
      { NoteName.C, Accidental.Natural, "C" },
      { NoteName.C, Accidental.Sharp, "C#" },
      { NoteName.C, Accidental.DoubleSharp, "C##" }
    };

  public static TheoryData<NoteName, Accidental, string?, string> ToStringWithFormatTestData =>
    new()
    {
      { NoteName.C, Accidental.DoubleFlat, null, "Cbb" },
      { NoteName.C, Accidental.DoubleFlat, "", "Cbb" },
      { NoteName.C, Accidental.DoubleFlat, "NS", "Cbb" },
      { NoteName.C, Accidental.DoubleFlat, "NX", "C𝄫" },
      { NoteName.C, Accidental.Flat, null, "Cb" },
      { NoteName.C, Accidental.Flat, "", "Cb" },
      { NoteName.C, Accidental.Flat, "NS", "Cb" },
      { NoteName.C, Accidental.Flat, "NX", "C♭" },
      { NoteName.C, Accidental.Natural, null, "C" },
      { NoteName.C, Accidental.Natural, "", "C" },
      { NoteName.C, Accidental.Natural, "NS", "C" },
      { NoteName.C, Accidental.Natural, "NX", "C" },
      { NoteName.C, Accidental.Sharp, null, "C#" },
      { NoteName.C, Accidental.Sharp, "", "C#" },
      { NoteName.C, Accidental.Sharp, "NS", "C#" },
      { NoteName.C, Accidental.Sharp, "NX", "C♯" },
      { NoteName.C, Accidental.DoubleSharp, null, "C##" },
      { NoteName.C, Accidental.DoubleSharp, "", "C##" },
      { NoteName.C, Accidental.DoubleSharp, "NS", "C##" },
      { NoteName.C, Accidental.DoubleSharp, "NX", "C𝄪" }
    };

  public static TheoryData<string> TryParseInvalidStringsTestData => [(string) null!, "", "J", "C$"];

  public static TheoryData<string, PitchClass> TryParseValidStringsTestData =>
    new()
    {
        { "C", PitchClass.C },
        { "C#", PitchClass.CSharp },
        { "C##", PitchClass.D },
        { "Cb", PitchClass.B },
        { "Cbb", PitchClass.BFlat },
        { "B#", PitchClass.C },
        { "B##", PitchClass.CSharp },
        { "Bb", PitchClass.BFlat },
        { "Bbb", PitchClass.A }
    };

  public static TheoryData<PitchClass, int, PitchClass> AddSemitonesSharps =>
    new()
    {
      { PitchClass.C, 1, PitchClass.CSharp },
      { PitchClass.C, 2, PitchClass.D },
      { PitchClass.C, 3, PitchClass.DSharp },
      { PitchClass.C, 4, PitchClass.E },
      { PitchClass.C, 5, PitchClass.F },
      { PitchClass.C, 6, PitchClass.FSharp },
      { PitchClass.C, 7, PitchClass.G },
      { PitchClass.C, 8, PitchClass.GSharp },
      { PitchClass.C, 9, PitchClass.A },
      { PitchClass.C, 10, PitchClass.ASharp },
      { PitchClass.C, 11, PitchClass.B },
      { PitchClass.C, 12, PitchClass.C }
    };

  public static TheoryData<PitchClass, int, PitchClass> AddIntervalFlats =>
    new()
    {
      { PitchClass.C, 1, PitchClass.DFlat },
      { PitchClass.C, 2, PitchClass.D },
      { PitchClass.C, 3, PitchClass.EFlat },
      { PitchClass.C, 4, PitchClass.E },
      { PitchClass.C, 5, PitchClass.F },
      { PitchClass.C, 6, PitchClass.GFlat },
      { PitchClass.C, 7, PitchClass.G },
      { PitchClass.C, 8, PitchClass.AFlat },
      { PitchClass.C, 9, PitchClass.A },
      { PitchClass.C, 10, PitchClass.BFlat },
      { PitchClass.C, 11, PitchClass.B },
      { PitchClass.C, 12, PitchClass.C }
    };

  public static TheoryData<PitchClass, int, PitchClass> ArithmeticSubtractionTestData => new()
  {
    { PitchClass.C, 1, PitchClass.B },
    { PitchClass.C, 12, PitchClass.C }
  };

  public static TheoryData<PitchClass, PitchClass, PitchClass> ArithmeticIncrementTestData => new()
  {
    { PitchClass.B, PitchClass.B, PitchClass.C },
    { PitchClass.C, PitchClass.C, PitchClass.CSharp }
  };

  public static TheoryData<PitchClass, PitchClass, PitchClass> ArithmeticDecrementTestData => new()
  {
    { PitchClass.C, PitchClass.C, PitchClass.B },
    { PitchClass.B, PitchClass.B, PitchClass.BFlat }
  };

  #endregion
}
