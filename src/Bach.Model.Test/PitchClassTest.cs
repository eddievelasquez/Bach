// Module Name: PitchClassTest.cs
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

public sealed class PitchClassTest
{
  #region Properties

  public static TheoryData<PitchClass, int, PitchClass> TransposeTestData =>
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
      { PitchClass.C, 12, PitchClass.C },
      { PitchClass.C, -1, PitchClass.B },
      { PitchClass.C, -2, PitchClass.BFlat },
      { PitchClass.C, -3, PitchClass.A },
      { PitchClass.C, -4, PitchClass.GSharp },
      { PitchClass.C, -5, PitchClass.G },
      { PitchClass.C, -6, PitchClass.FSharp },
      { PitchClass.C, -7, PitchClass.F },
      { PitchClass.C, -8, PitchClass.E },
      { PitchClass.C, -9, PitchClass.DSharp },
      { PitchClass.C, -10, PitchClass.D },
      { PitchClass.C, -11, PitchClass.CSharp },
      { PitchClass.C, -12, PitchClass.C }
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
      { PitchClass.C, PitchClass.Create( NoteName.C ), true },
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
      { PitchClass.Create( NoteName.C, Accidental.DoubleSharp ), PitchClass.DSharp, PitchClass.EFlat },
      { PitchClass.Create( NoteName.E, Accidental.DoubleSharp ), PitchClass.G, null },
      { PitchClass.Create( NoteName.B, Accidental.DoubleSharp ), PitchClass.D, null }
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
      { PitchClass.Parse( "D##" ), Interval.DiminishedFourth, PitchClass.GSharp }
    };

  public static TheoryData<PitchClass, Interval, PitchClass> SubtractIntervalTestData =>
    new()
    {
      { PitchClass.F, Interval.AugmentedFourth, PitchClass.Parse( "Cb" ) },
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
      { PitchClass.GSharp, Interval.DiminishedFourth, PitchClass.Parse( "D##" ) }
    };

  public static TheoryData<PitchClass, PitchClass, Interval> GetIntervalToPitchClassTestData =>
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
      { PitchClass.C, PitchClass.Create( NoteName.E, Accidental.DoubleFlat ), Interval.DiminishedThird },
      { PitchClass.Create( NoteName.D, Accidental.DoubleSharp ), PitchClass.GSharp, Interval.DiminishedFourth }
    };

  public static TheoryData<string, Type> InvalidPitchClassStrings =>
    new()
    {
      { null!, typeof( ArgumentNullException ) },
      { "", typeof( ArgumentException ) },
      { "J", typeof( FormatException ) },
      { "C$", typeof( FormatException ) }
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

  public static TheoryData<PitchClass, int, PitchClass> AdditionOperatorTestData => new()
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

  #region Public Methods

  [Fact]
  public void AFlat_ShouldReturnPitchClassWithNoteNameAAndFlatAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.AFlat;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.A );

    result.Accidental.Should()
          .Be( Accidental.Flat );

    ( (int) result ).Should()
                    .Be( 24 );

    result.ToString()
          .Should()
          .Be( "Ab" );
  }

  [Fact]
  public void ASharp_ShouldReturnPitchClassWithNoteNameAAndSharpAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.ASharp;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.A );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    ( (int) result ).Should()
                    .Be( 31 );

    result.ToString()
          .Should()
          .Be( "A#" );
  }

  [Fact]
  public void A_ShouldReturnPitchClassWithNoteNameAAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.A;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.A );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 27 );

    result.ToString()
          .Should()
          .Be( "A" );
  }

  [Theory]
  [MemberData( nameof( AddIntervals ) )]
  public void AdditionOperator_ShouldReturnExpectedPitchClass_WhenAddingInterval(
    PitchClass pitchClass,
    Interval interval,
    PitchClass expected )
  {
    ( pitchClass + interval ).Should()
                             .Be( expected );
  }

  [Theory]
  [MemberData( nameof( AdditionOperatorTestData ) )]
  public void AdditionOperator_ShouldReturnExpectedPitchClass_WhenAddingSemitones(
    PitchClass left,
    int right,
    PitchClass expected )
  {
    ( left + right ).Should()
                    .Be( expected );
  }

  [Fact]
  public void BFlat_ShouldReturnPitchClassWithNoteNameBAndFlatAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.BFlat;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.B );

    result.Accidental.Should()
          .Be( Accidental.Flat );

    ( (int) result ).Should()
                    .Be( 30 );

    result.ToString()
          .Should()
          .Be( "Bb" );
  }

  [Fact]
  public void B_ShouldReturnPitchClassWithNoteNameBAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.B;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.B );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 33 );

    result.ToString()
          .Should()
          .Be( "B" );
  }

  [Fact]
  public void CSharp_ShouldReturnPitchClassWithNoteNameCAndSharpAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.CSharp;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.C );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    ( (int) result ).Should()
                    .Be( 4 );

    result.ToString()
          .Should()
          .Be( "C#" );
  }

  [Fact]
  public void C_ShouldReturnPitchClassWithNoteNameCAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.C;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.C );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 1 );

    result.ToString()
          .Should()
          .Be( "C" );
  }

  [Theory]
  [MemberData( nameof( CompareToTestData ) )]
  public void CompareTo_ShouldReturnExpectedSign_WhenComparingPitchClasses(
    PitchClass left,
    PitchClass right,
    int expectedSign )
  {
    Math.Sign( left.CompareTo( right ) )
        .Should()
        .Be( expectedSign );
  }

  [Fact]
  public void CompareTo_ShouldReturnNegative_WhenThisIsLower()
  {
    // Arrange
    var lower = PitchClass.C;
    var higher = PitchClass.D;

    // Act
    var result = lower.CompareTo( higher );

    // Assert
    result.Should()
          .BeLessThan( 0 );
  }

  [Theory]
  [MemberData( nameof( ConstructorTestData ) )]
  public void Create_ShouldReturnExpectedPitchClass_WhenGivenNoteNameAndAccidental(
    NoteName noteName,
    Accidental accidental )
  {
    var note = PitchClass.Create( noteName, accidental );

    note.NoteName.Should()
        .Be( noteName );

    note.Accidental.Should()
        .Be( accidental );
  }

  [Fact]
  public void Create_ShouldReturnNaturalPitchClass_WhenGivenNoteName()
  {
    // Arrange & Act
    var result = PitchClass.Create( NoteName.F );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.F );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    result.Should()
          .Be( PitchClass.F );
  }

  [Fact]
  public void DFlat_ShouldReturnPitchClassWithNoteNameDAndFlatAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.DFlat;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.D );

    result.Accidental.Should()
          .Be( Accidental.Flat );

    ( (int) result ).Should()
                    .Be( 3 );

    result.ToString()
          .Should()
          .Be( "Db" );
  }

  [Fact]
  public void DSharp_ShouldReturnPitchClassWithNoteNameDAndSharpAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.DSharp;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.D );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    ( (int) result ).Should()
                    .Be( 11 );

    result.ToString()
          .Should()
          .Be( "D#" );
  }

  [Fact]
  public void D_ShouldReturnPitchClassWithNoteNameDAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.D;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.D );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 7 );

    result.ToString()
          .Should()
          .Be( "D" );
  }

  [Theory]
  [MemberData( nameof( ArithmeticDecrementTestData ) )]
  public void DecrementOperator_ShouldReturnExpectedPitchClass_WhenDecrementing(
    PitchClass initial,
    PitchClass afterPostDecrement,
    PitchClass afterPreDecrement )
  {
    var pitchClass = initial;

    ( pitchClass-- ).Should()
                    .Be( afterPostDecrement );

    pitchClass.Should()
              .Be( afterPreDecrement );

    ( --pitchClass ).Should()
                    .Be( afterPreDecrement.Transpose( -1 ) );
  }

  [Fact]
  public void EFlat_ShouldReturnPitchClassWithNoteNameEAndFlatAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.EFlat;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.E );

    result.Accidental.Should()
          .Be( Accidental.Flat );

    ( (int) result ).Should()
                    .Be( 10 );

    result.ToString()
          .Should()
          .Be( "Eb" );
  }

  [Fact]
  public void E_ShouldReturnPitchClassWithNoteNameEAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.E;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.E );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 13 );

    result.ToString()
          .Should()
          .Be( "E" );
  }

  [Theory]
  [MemberData( nameof( EqualsTestData ) )]
  public void Equals_ShouldReturnExpectedValue_WhenComparingPitchClasses(
    PitchClass pitchClass,
    object? other,
    bool expected )
  {
    pitchClass.Equals( other )
              .Should()
              .Be( expected );
  }

  [Fact]
  public void FSharp_ShouldReturnPitchClassWithNoteNameFAndSharpAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.FSharp;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.F );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    ( (int) result ).Should()
                    .Be( 19 );

    result.ToString()
          .Should()
          .Be( "F#" );
  }

  [Fact]
  public void F_ShouldReturnPitchClassWithNoteNameFAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.F;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.F );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 16 );

    result.ToString()
          .Should()
          .Be( "F" );
  }

  [Fact]
  public void GFlat_ShouldReturnPitchClassWithNoteNameGAndFlatAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.GFlat;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.G );

    result.Accidental.Should()
          .Be( Accidental.Flat );

    ( (int) result ).Should()
                    .Be( 18 );

    result.ToString()
          .Should()
          .Be( "Gb" );
  }

  [Fact]
  public void GSharp_ShouldReturnPitchClassWithNoteNameGAndSharpAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.GSharp;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.G );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    ( (int) result ).Should()
                    .Be( 25 );

    result.ToString()
          .Should()
          .Be( "G#" );
  }

  [Fact]
  public void G_ShouldReturnPitchClassWithNoteNameGAndNaturalAccidental_WhenAccessed()
  {
    // Arrange & Act
    var result = PitchClass.G;

    // Assert
    result.NoteName.Should()
          .Be( NoteName.G );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    ( (int) result ).Should()
                    .Be( 22 );

    result.ToString()
          .Should()
          .Be( "G" );
  }

  [Theory]
  [MemberData( nameof( EnharmonicData ) )]
  public void GetEnharmonic_ShouldReturnExpectedPitchClass_WhenGivenEnharmonicNoteName(
    PitchClass pitchClass,
    string enharmonic )
  {
    var enharmonicPitchClass = PitchClass.Parse( enharmonic );

    pitchClass.GetEnharmonic( enharmonicPitchClass.NoteName )
              .Should()
              .Be( enharmonicPitchClass );
  }

  [Theory]
  [MemberData( nameof( NotEnharmonicTestData ) )]
  public void GetEnharmonic_ShouldReturnNull_WhenNoEnharmonicExists(
    PitchClass pitchClass,
    NoteName startInclusive,
    NoteName lastExclusive )
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
  [MemberData( nameof( GetIntervalToPitchClassTestData ) )]
  public void GetIntervalTo_ShouldReturnExpectedInterval_WhenGettingIntervalToPitchClasses(
    PitchClass left,
    PitchClass right,
    Interval expected )
  {
    left.GetIntervalTo( right )
        .Should()
        .Be( expected );
  }

  [Fact]
  public void GetIntervalTo_ShouldReturnMajorSecond_WhenFromCToD()
  {
    // Arrange
    var from = PitchClass.C;
    var to = PitchClass.D;

    // Act
    var interval = from.GetIntervalTo( to );

    // Assert
    interval.Quantity.Should()
            .Be( IntervalQuantity.Second );

    interval.SemitoneCount.Should()
            .Be( Interval.MajorSecond.SemitoneCount );

    interval.Should()
            .Be( Interval.MajorSecond );
  }

  [Fact]
  public void IPitchPitchClass_PitchClassProperty_ShouldReturnSelf_WhenAccessed()
  {
    // Arrange
    var original = PitchClass.B;
    var asInterface = (IPitch<PitchClass>) original;

    // Act
    var pitchClass = asInterface.PitchClass;

    // Assert
    pitchClass.Should()
              .Be( original );
  }

  [Theory]
  [MemberData( nameof( ArithmeticIncrementTestData ) )]
  public void IncrementOperator_ShouldReturnExpectedPitchClass_WhenIncrementing(
    PitchClass initial,
    PitchClass afterPostIncrement,
    PitchClass afterPreIncrement )
  {
    var pitchClass = initial;

    ( pitchClass++ ).Should()
                    .Be( afterPostIncrement );

    pitchClass.Should()
              .Be( afterPreIncrement );

    ( ++pitchClass ).Should()
                    .Be( afterPreIncrement.Transpose( 1 ) );
  }

  [Theory]
  [MemberData( nameof( ValidPitchClassStrings ) )]
  public void Parse_ShouldReturnExpectedPitchClass_WhenGivenValidString(
    string value,
    NoteName noteName,
    Accidental accidental )
  {
    var expected = PitchClass.Create( noteName, accidental );

    PitchClass.Parse( value )
              .Should()
              .Be( expected );
  }

  [Theory]
  [MemberData( nameof( InvalidPitchClassStrings ) )]
  public void Parse_ShouldThrowException_WhenGivenInvalidString(
    string value,
    Type exceptionType )
  {
    var act = () => PitchClass.Parse( value );

    act.Should()
       .Throw<Exception>()
       .Where( e => e.GetType() == exceptionType );
  }

  [Fact]
  public void PitchClass_ShouldImplementIPitchClassContract()
  {
    IPitch<PitchClass> pitchClass = PitchClass.C;

    pitchClass.NoteName.Should()
              .Be( NoteName.C );

    pitchClass.Accidental.Should()
              .Be( Accidental.Natural );

    pitchClass.Transpose( 1 )
              .Should()
              .Be( PitchClass.CSharp );

    pitchClass.Transpose( -1 )
              .Should()
              .Be( PitchClass.B );
  }

  [Theory]
  [MemberData( nameof( NoteNamesAndAccidentals ) )]
  public void PredefinedNotes_ShouldReturnExpectedNoteNameAndAccidental(
    PitchClass pitchClass,
    NoteName noteName,
    Accidental accidental )
  {
    pitchClass.NoteName.Should()
              .Be( noteName );

    pitchClass.Accidental.Should()
              .Be( accidental );
  }

  [Fact]
  public void RelationalOperators_ShouldReturnTrue_WhenComparingPitchClasses()
  {
    ( PitchClass.C == PitchClass.Create( NoteName.B, Accidental.Sharp ) ).Should()
                                                                         .BeTrue();

    ( PitchClass.C != PitchClass.B ).Should()
                                    .BeTrue();

    ( PitchClass.C < PitchClass.B ).Should()
                                   .BeTrue();

    ( PitchClass.C <= PitchClass.B ).Should()
                                    .BeTrue();

    ( PitchClass.D > PitchClass.C ).Should()
                                   .BeTrue();

    ( PitchClass.D >= PitchClass.C ).Should()
                                    .BeTrue();
  }

  [Theory]
  [MemberData( nameof( SubtractIntervalTestData ) )]
  public void Subtract_ShouldReturnExpectedPitchClass_WhenSubtractingInterval(
    PitchClass pitchClass,
    Interval interval,
    PitchClass expected )
  {
    ( pitchClass - interval ).Should()
                             .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ArithmeticSubtractionTestData ) )]
  public void SubtractionOperator_ShouldReturnExpectedPitchClass_WhenSubtractingSemitones(
    PitchClass left,
    int right,
    PitchClass expected )
  {
    ( left - right ).Should()
                    .Be( expected );
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

  [Fact]
  public void Transpose_Int_ShouldReturnPreviousNatural_WhenNegativeOneSemitone()
  {
    // Arrange
    var original = PitchClass.C;

    // Act
    var result = original.Transpose( -1 );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.B );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    result.ToString()
          .Should()
          .Be( "B" );
  }

  [Fact]
  public void Transpose_Int_ShouldReturnSharp_WhenOneSemitoneUp()
  {
    // Arrange
    var original = PitchClass.C;

    // Act
    var result = original.Transpose( 1 );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.C );

    result.Accidental.Should()
          .Be( Accidental.Sharp );

    result.ToString()
          .Should()
          .Be( "C#" );
  }

  [Fact]
  public void Transpose_Int_ShouldWrapAround_WhenTwelveSemitones()
  {
    // Arrange
    var original = PitchClass.C;

    // Act
    var result = original.Transpose( 12 );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.C );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    result.ToString()
          .Should()
          .Be( "C" );
  }

  [Fact]
  public void Transpose_Interval_ShouldReturnB_WhenDescendingMinorSecondFromC()
  {
    // Arrange
    var original = PitchClass.C;
    var interval = new Interval( IntervalQuantity.Second, IntervalQuality.Minor, true );

    // Act
    var result = original.Transpose( interval );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.B );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    result.ToString()
          .Should()
          .Be( "B" );
  }

  [Fact]
  public void Transpose_Interval_ShouldReturnD_WhenMajorSecondFromC()
  {
    // Arrange
    var original = PitchClass.C;
    var interval = Interval.MajorSecond;

    // Act
    var result = original.Transpose( interval );

    // Assert
    result.NoteName.Should()
          .Be( NoteName.D );

    result.Accidental.Should()
          .Be( Accidental.Natural );

    result.ToString()
          .Should()
          .Be( "D" );
  }

  [Theory]
  [MemberData( nameof( TransposeTestData ) )]
  public void Transpose_ShouldReturnExpectedPitchClass_WhenGivenSemitoneCount(
    PitchClass pitchClass,
    int semitoneCount,
    PitchClass expected )
  {
    pitchClass.Transpose( semitoneCount )
              .Should()
              .Be( expected );
  }

  [Theory]
  [MemberData( nameof( TryParseValidStringsTestData ) )]
  public void TryParse_ShouldReturnExpectedPitchClass_WhenGivenValidString(
    string value,
    PitchClass expected )
  {
    PitchClass.TryParse( value, out var actual )
              .Should()
              .BeTrue();

    actual.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( TryParseInvalidStringsTestData ) )]
  public void TryParse_ShouldReturnFalse_WhenGivenInvalidString(
    string value )
  {
    PitchClass.TryParse( value, out _ )
              .Should()
              .BeFalse();
  }

  #endregion
}
