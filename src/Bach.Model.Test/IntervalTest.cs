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

public sealed class IntervalTest
{
  #region Public Methods

  [Fact]
  public void EqualityOperator_ShouldReturnFalse_WhenComparedWithNull()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

#pragma warning disable CS8073
    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
    ( lhs == null ).Should()
                   .BeFalse();
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualityOperator_ShouldReturnTrue_WhenComparingWithSameObject()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

#pragma warning disable 1718
    // ReSharper disable once EqualExpressionComparison
    ( lhs == lhs ).Should()
                  .BeTrue();
#pragma warning restore 1718
  }

  [Fact]
  public void Equality_ShouldReturnTrue_WhenComparingEquivalentObjects()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    ( lhs == rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
  public void Equals_ShouldReturnFalse_WhenComparingObjectsOfDifferentTypes()
  {
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFall_WhenComparingWithNull()
  {
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var expected = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalCombinations ) )]
  public void GetSemitoneCount_ShouldThrowArgumentException_WithInvalidIntervalQuantityAndQualityCombination(
    IntervalQuantity quantity, IntervalQuality quality )
  {
    var act = () => Interval.GetSemitoneCount( quantity, quality );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void InequalityOperator_ShouldReturnFalse_WhenComparingWithSameObject()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

#pragma warning disable 1718
    // ReSharper disable once EqualExpressionComparison
    ( lhs != lhs ).Should()
                  .BeFalse();
#pragma warning restore 1718
  }

  [Fact]
  public void InequalityOperator_ShouldReturnTrue_WhenComparingDifferentIntervals()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented );
    ( lhs != rhs ).Should()
                  .BeTrue();
  }

  [Theory]
  [MemberData( nameof( InversionData ) )]
  public void Inversion_ShouldReturnCorrectInterval( Interval interval, Interval expectedInversion )
  {
    interval.Inversion.Should().Be( expectedInversion );
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalCombinations ) )]
  public void IsValid_ShouldReturnFalse_WhenInvalidIntervalCombinationOccurs( IntervalQuantity quantity, IntervalQuality quality )
  {
    Interval.IsValid( quantity, quality )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void RelationalOperators_ShouldSatisfyOrdering()
  {
    ( Interval.Unison == Interval.Parse( "P1" ) ).Should()
                                                 .BeTrue();
    ( Interval.Unison != Interval.Fourth ).Should()
                                          .BeTrue();
    ( Interval.Unison < Interval.Fourth ).Should()
                                         .BeTrue();
    ( Interval.Unison <= Interval.Fourth ).Should()
                                          .BeTrue();
    ( Interval.Fourth > Interval.Unison ).Should()
                                         .BeTrue();
    ( Interval.Fourth >= Interval.Unison ).Should()
                                          .BeTrue();
    ( Interval.MinorThird < Interval.MajorThird ).Should()
                                                 .BeTrue();
  }

  [Theory]
  [MemberData( nameof( ValidIntervalStrings ) )]
  public void Parse_ShouldReturnCorrectInterval_WhenValidStringIsProvided( string intervalString, Interval expected )
  {
    Interval.Parse( intervalString )
            .Should()
            .Be( expected );
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenInvalidStringIsProvided()
  {
    var act = () => Interval.Parse( "X2" );
    act.Should()
       .Throw<FormatException>();
  }

  [Theory]
  [MemberData( nameof( SemitoneCountData ) )]
  public void SemitoneCount_ShouldReturnCorrectCount( Interval interval, int expectedSemitoneCount )
  {
    interval.SemitoneCount.Should().Be( expectedSemitoneCount );
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalStrings ) )]
  public void TryParse_ShouldReturnFalse_WhenInvalidStringIsProvided( string? input )
  {
    Interval.TryParse( input, out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TryParse_ShouldSkipLeadingWhitespace_WhenValidStringIsProvided()
  {
    Interval.TryParse( "  P5", out var actual )
            .Should()
            .BeTrue();

    actual.Should()
          .Be( Interval.Fifth );
  }

  [Theory]
  [MemberData( nameof( ValidIntervalStrings ) )]
  public void TryParse_ShouldReturnCorrectInterval_WhenValidStringIsProvided( string intervalString, Interval expected )
  {
    Interval.TryParse( intervalString, out var actual )
            .Should()
            .BeTrue();

    actual.Should()
          .Be( expected );
  }

  [Fact]
  public void StronglyTypedEquals_ShouldSatisfyEquivalenceRelation()
  {
    var x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
    x.Equals( (Interval?) null! )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToObjectOfDifferentType()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<IntervalQuantity, IntervalQuality> InvalidIntervalCombinations =>
    new()
    {
      { IntervalQuantity.Unison, IntervalQuality.Diminished },
      { IntervalQuantity.Unison, IntervalQuality.Minor },
      { IntervalQuantity.Unison, IntervalQuality.Major },
      { IntervalQuantity.Second, IntervalQuality.Perfect },
      { IntervalQuantity.Third, IntervalQuality.Perfect },
      { IntervalQuantity.Fourth, IntervalQuality.Minor },
      { IntervalQuantity.Fourth, IntervalQuality.Major },
      { IntervalQuantity.Fifth, IntervalQuality.Minor },
      { IntervalQuantity.Fifth, IntervalQuality.Major },
      { IntervalQuantity.Sixth, IntervalQuality.Perfect },
      { IntervalQuantity.Seventh, IntervalQuality.Perfect },
      { IntervalQuantity.Octave, IntervalQuality.Minor },
      { IntervalQuantity.Octave, IntervalQuality.Major },
      { IntervalQuantity.Ninth, IntervalQuality.Perfect },
      { IntervalQuantity.Tenth, IntervalQuality.Perfect },
      { IntervalQuantity.Eleventh, IntervalQuality.Minor },
      { IntervalQuantity.Eleventh, IntervalQuality.Major },
      { IntervalQuantity.Twelfth, IntervalQuality.Minor },
      { IntervalQuantity.Twelfth, IntervalQuality.Major },
      { IntervalQuantity.Thirteenth, IntervalQuality.Perfect },
      { IntervalQuantity.Fourteenth, IntervalQuality.Perfect }
    };

  public static TheoryData<Interval, Interval> InversionData => new()
  {
    { Interval.Unison, Interval.Octave },
    { Interval.MinorSecond, Interval.MajorSeventh },
    { Interval.MajorSecond, Interval.MinorSeventh },
    { Interval.MinorThird, Interval.MajorSixth },
    { Interval.MajorThird, Interval.MinorSixth },
    { Interval.Fourth, Interval.Fifth },
    { Interval.AugmentedFourth, Interval.DiminishedFifth },
    { Interval.Fifth, Interval.Fourth },
    { Interval.AugmentedFifth, Interval.DiminishedFourth },
    { Interval.MinorSixth, Interval.MajorThird },
    { Interval.MajorSixth, Interval.MinorThird },
    { Interval.MinorSeventh, Interval.MajorSecond },
    { Interval.MajorSeventh, Interval.MinorSecond },
    { Interval.Octave, Interval.Unison }
  };

  public static TheoryData<string, Interval> ValidIntervalStrings => new()
  {
    { "P1", Interval.Unison },
    { "R", Interval.Unison },
    { "1", Interval.Unison },
    { "A1", Interval.AugmentedFirst },
    { "d2", Interval.DiminishedSecond },
    { "m2", Interval.MinorSecond },
    { "M2", Interval.MajorSecond },
    { "A2", Interval.AugmentedSecond },
    { "d3", Interval.DiminishedThird },
    { "m3", Interval.MinorThird },
    { "M3", Interval.MajorThird },
    { "A3", Interval.AugmentedThird },
    { "d4", Interval.DiminishedFourth },
    { "P4", Interval.Fourth },
    { "A4", Interval.AugmentedFourth },
    { "d5", Interval.DiminishedFifth },
    { "P5", Interval.Fifth },
    { "A5", Interval.AugmentedFifth },
    { "d6", Interval.DiminishedSixth },
    { "m6", Interval.MinorSixth },
    { "M6", Interval.MajorSixth },
    { "A6", Interval.AugmentedSixth },
    { "d7", Interval.DiminishedSeventh },
    { "m7", Interval.MinorSeventh },
    { "M7", Interval.MajorSeventh },
    { "A7", Interval.AugmentedSeventh },
    { "d8", Interval.DiminishedOctave },
    { "P8", Interval.Octave }
  };

  public static TheoryData<Interval, int> SemitoneCountData => new()
  {
    { Interval.Unison, 0 },
    { Interval.AugmentedFirst, 1 },
    { Interval.DiminishedSecond, 0 },
    { Interval.MinorSecond, 1 },
    { Interval.MajorSecond, 2 },
    { Interval.AugmentedSecond, 3 },
    { Interval.DiminishedThird, 2 },
    { Interval.MinorThird, 3 },
    { Interval.MajorThird, 4 },
    { Interval.AugmentedThird, 5 },
    { Interval.DiminishedFourth, 4 },
    { Interval.Fourth, 5 },
    { Interval.AugmentedFourth, 6 },
    { Interval.DiminishedFifth, 6 },
    { Interval.Fifth, 7 },
    { Interval.AugmentedFifth, 8 },
    { Interval.DiminishedSixth, 7 },
    { Interval.MinorSixth, 8 },
    { Interval.MajorSixth, 9 },
    { Interval.AugmentedSixth, 10 },
    { Interval.DiminishedSeventh, 9 },
    { Interval.MinorSeventh, 10 },
    { Interval.MajorSeventh, 11 },
    { Interval.AugmentedSeventh, 12 },
    { Interval.DiminishedOctave, 11 },
    { Interval.Octave, 12 }
  };

  public static TheoryData<string?> InvalidIntervalStrings => [(string?) null, "", "   ", "M1", "P2", "L2", "Px"];

  #endregion
}
