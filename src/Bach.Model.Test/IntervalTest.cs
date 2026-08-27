// Module Name: IntervalTest.cs
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

public sealed class IntervalTest
{
  #region Properties

  public static TheoryData<IntervalQuantity, IntervalQuality> InvalidIntervalCombinations =>
    new()
    {
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

  public static TheoryData<string, string> InversionData => new()
  {
    { "m2", "M7" },
    { "M2", "m7" },
    { "m3", "M6" },
    { "M3", "m6" },
    { "P4", "P5" },
    { "A4", "d5" },
    { "P5", "P4" },
    { "A5", "d4" },
    { "m6", "M3" },
    { "M6", "m3" },
    { "m7", "M2" },
    { "M7", "m2" },
    { "m9", "M7" },
    { "M9", "m7" },
    { "m10", "M6" },
    { "M10", "m6" },
    { "P11", "P5" },
    { "d11", "A5" },
    { "A11", "d5" },
    { "P12", "P4" },
    { "d12", "A4" },
    { "A12", "d4" },
    { "m13", "M3" },
    { "M13", "m3" },
    { "m14", "M2" },
    { "M14", "m2" }
  };

  public static TheoryData<Interval, Interval> ExtendedInversionData => new()
  {
    {
      new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 2 ),
      new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 2, true )
    },
    {
      new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 3 ),
      new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 3, true )
    },
    {
      new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 2 ),
      new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 2, true )
    },
    {
      new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 3 ),
      new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 3, true )
    },
    {
      new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 2 ),
      new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 2, true )
    },
    {
      new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 3 ),
      new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 3, true )
    },
    {
      new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 2 ),
      new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 2, true )
    },
    {
      new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 3 ),
      new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 3, true )
    }
  };

  public static TheoryData<Interval, Interval> InversionEdgeCaseData => new()
  {
    { Interval.Unison, -Interval.Octave },
    {
      new Interval( IntervalQuantity.Unison, IntervalQuality.Diminished, 3 ),
      new Interval( IntervalQuantity.Octave, IntervalQuality.Augmented, 3 )
    },
    {
      new Interval( IntervalQuantity.Octave, IntervalQuality.Augmented, 3 ),
      new Interval( IntervalQuantity.Unison, IntervalQuality.Diminished, 3 )
    },
    {
      new Interval( IntervalQuantity.Second, IntervalQuality.Diminished ),
      new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented, descending: true )
    },
    {
      new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, 2 ),
      new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented, 2 )
    },
    {
      new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, 3 ),
      new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented, 3 )
    }
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

  public static TheoryData<string, Interval, int> ExtendedAlterationIntervalStrings => new()
  {
    { "A1", new Interval( IntervalQuantity.Unison, IntervalQuality.Augmented ), 1 },
    { "AA1", new Interval( IntervalQuantity.Unison, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA1", new Interval( IntervalQuantity.Unison, IntervalQuality.Augmented, 3 ), 3 },
    { "d2",  new Interval( IntervalQuantity.Second, IntervalQuality.Diminished ), 1 },
    { "dd2", new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd2", new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, 3 ), 3 },
    { "A2", new Interval( IntervalQuantity.Second, IntervalQuality.Augmented ), 1 },
    { "AA2", new Interval( IntervalQuantity.Second, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA2", new Interval( IntervalQuantity.Second, IntervalQuality.Augmented, 3 ), 3 },
    { "d3", new Interval( IntervalQuantity.Third, IntervalQuality.Diminished ), 1 },
    { "dd3", new Interval( IntervalQuantity.Third, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd3", new Interval( IntervalQuantity.Third, IntervalQuality.Diminished, 3 ), 3 },
    { "A3", new Interval( IntervalQuantity.Third, IntervalQuality.Augmented ), 1 },
    { "AA3", new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA3", new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 3 ), 3 },
    { "d4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Diminished ), 1 },
    { "dd4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Diminished, 3 ), 3 },
    { "A4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented ), 1 },
    { "AA4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA4", new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, 3 ), 3 },
    { "d5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished ), 1 },
    { "dd5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, 3 ), 3 },
    { "A5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented ), 1 },
    { "AA5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA5", new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented, 3 ), 3 },
    { "d6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished ), 1 },
    { "dd6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Diminished, 3 ), 3 },
    { "A6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Augmented ), 1 },
    { "AA6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA6", new Interval( IntervalQuantity.Sixth, IntervalQuality.Augmented, 3 ), 3 },
    { "d7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Diminished ), 1 },
    { "dd7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Diminished, 3 ), 3 },
    { "A7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented ), 1 },
    { "AA7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented, 2 ), 2 },
    { "AAA7", new Interval( IntervalQuantity.Seventh, IntervalQuality.Augmented, 3 ), 3 },
    { "d8", new Interval( IntervalQuantity.Octave, IntervalQuality.Diminished ), 1 },
    { "dd8", new Interval( IntervalQuantity.Octave, IntervalQuality.Diminished, 2 ), 2 },
    { "ddd8", new Interval( IntervalQuantity.Octave, IntervalQuality.Diminished, 3 ), 3 }
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

  public static TheoryData<Interval, string, string> ToStringWithFormatData =>
    new()
    {
      { Interval.Unison, "sq", "1" },
      { Interval.MajorSecond, "Sq", "M2" },
      { Interval.MajorThird, "SQ", "MThird" },
      { Interval.Fifth, "sq", "5" },
      { Interval.Fifth, "Sq", "P5" },
      { Interval.MinorSeventh, "Sq", "m7" },
      { Interval.MinorSeventh, "sq", "m7" },
      { Interval.Octave, "q", "8" },
      { -Interval.MajorThird, "Sq", "M3" },
      { -Interval.MajorThird, "q", "3" },
      { -Interval.MajorThird, "Q", "Third" },
      { -Interval.Unison, "Sq", "P1" },
      { -Interval.Unison, "q", "1" },
      { -Interval.Unison, "Q", "Unison" }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality, int, int> PerfectBasedAugmentedIntervals =>
    new()
    {
      { IntervalQuantity.Unison, IntervalQuality.Augmented, 1, 1 },
      { IntervalQuantity.Unison, IntervalQuality.Augmented, 2, 2 },
      { IntervalQuantity.Unison, IntervalQuality.Augmented, 3, 3 },
      { IntervalQuantity.Fourth, IntervalQuality.Augmented, 1, 6 },
      { IntervalQuantity.Fourth, IntervalQuality.Augmented, 2, 7 },
      { IntervalQuantity.Fourth, IntervalQuality.Augmented, 3, 8 },
      { IntervalQuantity.Fifth, IntervalQuality.Augmented, 1, 8 },
      { IntervalQuantity.Fifth, IntervalQuality.Augmented, 2, 9 },
      { IntervalQuantity.Fifth, IntervalQuality.Augmented, 3, 10 },
      { IntervalQuantity.Octave, IntervalQuality.Augmented, 1, 13 },
      { IntervalQuantity.Eleventh, IntervalQuality.Augmented, 1, 18 },
      { IntervalQuantity.Twelfth, IntervalQuality.Augmented, 1, 20 }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality, int, int> PerfectBasedDiminishedIntervals =>
    new()
    {
      { IntervalQuantity.Unison, IntervalQuality.Diminished, 1, -1 },
      { IntervalQuantity.Unison, IntervalQuality.Diminished, 2, -2 },
      { IntervalQuantity.Unison, IntervalQuality.Diminished, 3, -3 },
      { IntervalQuantity.Fourth, IntervalQuality.Diminished, 1, 4 },
      { IntervalQuantity.Fourth, IntervalQuality.Diminished, 2, 3 },
      { IntervalQuantity.Fourth, IntervalQuality.Diminished, 3, 2 },
      { IntervalQuantity.Fifth, IntervalQuality.Diminished, 1, 6 },
      { IntervalQuantity.Fifth, IntervalQuality.Diminished, 2, 5 },
      { IntervalQuantity.Fifth, IntervalQuality.Diminished, 3, 4 },
      { IntervalQuantity.Octave, IntervalQuality.Diminished, 1, 11 },
      { IntervalQuantity.Eleventh, IntervalQuality.Diminished, 1, 16 },
      { IntervalQuantity.Twelfth, IntervalQuality.Diminished, 1, 18 }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality, int, int> MajorBasedAugmentedIntervals =>
    new()
    {
      { IntervalQuantity.Second, IntervalQuality.Augmented, 1, 3 },
      { IntervalQuantity.Second, IntervalQuality.Augmented, 2, 4 },
      { IntervalQuantity.Second, IntervalQuality.Augmented, 3, 5 },
      { IntervalQuantity.Third, IntervalQuality.Augmented, 1, 5 },
      { IntervalQuantity.Sixth, IntervalQuality.Augmented, 1, 10 },
      { IntervalQuantity.Seventh, IntervalQuality.Augmented, 1, 12 },
      { IntervalQuantity.Ninth, IntervalQuality.Augmented, 1, 15 },
      { IntervalQuantity.Tenth, IntervalQuality.Augmented, 1, 17 },
      { IntervalQuantity.Thirteenth, IntervalQuality.Augmented, 1, 22 },
      { IntervalQuantity.Fourteenth, IntervalQuality.Augmented, 1, 24 }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality, int, int> MajorBasedDiminishedIntervals =>
    new()
    {
      { IntervalQuantity.Second, IntervalQuality.Diminished, 1, 0 },
      { IntervalQuantity.Second, IntervalQuality.Diminished, 2, -1 },
      { IntervalQuantity.Second, IntervalQuality.Diminished, 3, -2 },
      { IntervalQuantity.Third, IntervalQuality.Diminished, 1, 2 },
      { IntervalQuantity.Third, IntervalQuality.Diminished, 2, 1 },
      { IntervalQuantity.Sixth, IntervalQuality.Diminished, 1, 7 },
      { IntervalQuantity.Seventh, IntervalQuality.Diminished, 1, 9 },
      { IntervalQuantity.Ninth, IntervalQuality.Diminished, 1, 12 },
      { IntervalQuantity.Tenth, IntervalQuality.Diminished, 1, 14 },
      { IntervalQuantity.Thirteenth, IntervalQuality.Diminished, 1, 19 },
      { IntervalQuantity.Fourteenth, IntervalQuality.Diminished, 1, 21 }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality> InvalidPerfectBasedCombinations =>
    new()
    {
      { IntervalQuantity.Unison, IntervalQuality.Major },
      { IntervalQuantity.Unison, IntervalQuality.Minor },
      { IntervalQuantity.Fourth, IntervalQuality.Major },
      { IntervalQuantity.Fourth, IntervalQuality.Minor },
      { IntervalQuantity.Fifth, IntervalQuality.Major },
      { IntervalQuantity.Fifth, IntervalQuality.Minor },
      { IntervalQuantity.Octave, IntervalQuality.Major },
      { IntervalQuantity.Octave, IntervalQuality.Minor },
      { IntervalQuantity.Eleventh, IntervalQuality.Major },
      { IntervalQuantity.Eleventh, IntervalQuality.Minor },
      { IntervalQuantity.Twelfth, IntervalQuality.Major },
      { IntervalQuantity.Twelfth, IntervalQuality.Minor }
    };

  public static TheoryData<IntervalQuantity, IntervalQuality> InvalidMajorBasedCombinations =>
    new()
    {
      { IntervalQuantity.Second, IntervalQuality.Perfect },
      { IntervalQuantity.Third, IntervalQuality.Perfect },
      { IntervalQuantity.Sixth, IntervalQuality.Perfect },
      { IntervalQuantity.Seventh, IntervalQuality.Perfect },
      { IntervalQuantity.Ninth, IntervalQuality.Perfect },
      { IntervalQuantity.Tenth, IntervalQuality.Perfect },
      { IntervalQuantity.Thirteenth, IntervalQuality.Perfect },
      { IntervalQuantity.Fourteenth, IntervalQuality.Perfect }
    };

  #endregion

  #region Public Methods

  [Fact]
  public void Constructor_ShouldCreateAscendingInterval_WhenDescendingIsFalse()
  {
    // Arrange & Act
    var interval = new Interval( IntervalQuantity.Fourth, IntervalQuality.Diminished, 2 );

    // Assert
    interval.Quantity.Should()
            .Be( IntervalQuantity.Fourth );

    interval.Quality.Should()
            .Be( IntervalQuality.Diminished );

    interval.AlterationDegree.Should()
            .Be( 2 );

    interval.SemitoneCount.Should()
            .Be( 3 );

    interval.IsAscending.Should()
            .BeTrue();
  }

  [Theory]
  [MemberData( nameof( MajorBasedAugmentedIntervals ) )]
  public void Constructor_ShouldCreateAugmentedInterval_WhenMajorBasedQuantity(
    IntervalQuantity quantity,
    IntervalQuality quality,
    int alterationDegree,
    int expectedSemitones )
  {
    // Arrange & Act
    var interval = new Interval( quantity, quality, alterationDegree );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.AlterationDegree.Should()
            .Be( alterationDegree );

    interval.SemitoneCount.Should()
            .Be( expectedSemitones );

    interval.IsAscending.Should()
            .BeTrue();
  }

  [Theory]
  [MemberData( nameof( PerfectBasedAugmentedIntervals ) )]
  public void Constructor_ShouldCreateAugmentedInterval_WhenPerfectBasedQuantity(
    IntervalQuantity quantity,
    IntervalQuality quality,
    int alterationDegree,
    int expectedSemitones )
  {
    // Arrange & Act
    var interval = new Interval( quantity, quality, alterationDegree );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.AlterationDegree.Should()
            .Be( alterationDegree );

    interval.SemitoneCount.Should()
            .Be( expectedSemitones );

    interval.IsAscending.Should()
            .BeTrue();
  }

  [Fact]
  public void Constructor_ShouldCreateAugmentedUnison_WhenPerfectBasedIntervalWithAlterationDegree1()
  {
    // Arrange & Act
    var interval = new Interval( IntervalQuantity.Unison, IntervalQuality.Augmented );

    // Assert
    interval.Quantity.Should()
            .Be( IntervalQuantity.Unison );

    interval.Quality.Should()
            .Be( IntervalQuality.Augmented );

    interval.AlterationDegree.Should()
            .Be( 1 );

    interval.SemitoneCount.Should()
            .Be( 1 );

    interval.IsAscending.Should()
            .BeTrue();
  }

  [Fact]
  public void Constructor_ShouldCreateDescendingDiminishedUnison_WhenDescendingIsTrue()
  {
    // Arrange & Act
    var interval = new Interval( IntervalQuantity.Unison, IntervalQuality.Diminished, 1, true );

    // Assert
    interval.SemitoneCount.Should()
            .Be( 1 );

    interval.IsDescending.Should()
            .BeFalse();
  }

  [Fact]
  public void Constructor_ShouldCreateDescendingInterval_WhenDescendingIsTrue()
  {
    // Arrange & Act
    var interval = new Interval( IntervalQuantity.Third, IntervalQuality.Augmented, 1, true );

    // Assert
    interval.Quantity.Should()
            .Be( IntervalQuantity.Third );

    interval.Quality.Should()
            .Be( IntervalQuality.Augmented );

    interval.AlterationDegree.Should()
            .Be( 1 );

    interval.SemitoneCount.Should()
            .Be( -5 );

    interval.IsDescending.Should()
            .BeTrue();
  }

  [Theory]
  [MemberData( nameof( MajorBasedDiminishedIntervals ) )]
  public void Constructor_ShouldCreateDiminishedInterval_WhenMajorBasedQuantity(
    IntervalQuantity quantity,
    IntervalQuality quality,
    int alterationDegree,
    int expectedSemitones )
  {
    // Arrange & Act
    var interval = new Interval( quantity, quality, alterationDegree );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.AlterationDegree.Should()
            .Be( alterationDegree );

    interval.SemitoneCount.Should()
            .Be( expectedSemitones );

    interval.IsAscending.Should()
            .Be( expectedSemitones >= 0 );
  }

  [Theory]
  [MemberData( nameof( PerfectBasedDiminishedIntervals ) )]
  public void Constructor_ShouldCreateDiminishedInterval_WhenPerfectBasedQuantity(
    IntervalQuantity quantity,
    IntervalQuality quality,
    int alterationDegree,
    int expectedSemitones )
  {
    // Arrange & Act
    var interval = new Interval( quantity, quality, alterationDegree );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.AlterationDegree.Should()
            .Be( alterationDegree );

    interval.SemitoneCount.Should()
            .Be( expectedSemitones );

    interval.IsAscending.Should()
            .Be( expectedSemitones >= 0 );
  }

  [Theory]
  [MemberData( nameof( InvalidMajorBasedCombinations ) )]
  public void Constructor_ShouldThrowArgumentException_WhenInvalidQualityForMajorBasedInterval(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    // Arrange & Act
    var act = () => new Interval( quantity, quality );

    // Assert
    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( $"{quality} is not valid for a major-based interval ({quantity})" );
  }

  [Theory]
  [MemberData( nameof( InvalidPerfectBasedCombinations ) )]
  public void Constructor_ShouldThrowArgumentException_WhenInvalidQualityForPerfectBasedInterval(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    // Arrange & Act
    var act = () => new Interval( quantity, quality );

    // Assert
    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( $"{quality} is not valid for a perfect-based interval ({quantity})" );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenQuantityIsUndefined()
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Undefined, IntervalQuality.Perfect );

    // Assert
    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( "Undefined is not a valid interval quantity*" );
  }

  [Theory]
  [InlineData( 4 )]
  [InlineData( 5 )]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAlterationDegreeGreaterThan3ForAugmented(
    int alterationDegree )
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Fourth, IntervalQuality.Augmented, alterationDegree );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>()
       .WithParameterName( "alterationDegree" );
  }

  [Theory]
  [InlineData( 4 )]
  [InlineData( 5 )]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAlterationDegreeGreaterThan3ForDiminished(
    int alterationDegree )
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, alterationDegree );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>()
       .WithParameterName( "alterationDegree" );
  }

  [Theory]
  [InlineData( 0 )]
  [InlineData( -1 )]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAlterationDegreeLessThan1ForAugmented(
    int alterationDegree )
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Unison, IntervalQuality.Augmented, alterationDegree );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>()
       .WithParameterName( "alterationDegree" );
  }

  [Theory]
  [InlineData( 0 )]
  [InlineData( -1 )]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAlterationDegreeLessThan1ForDiminished(
    int alterationDegree )
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Fifth, IntervalQuality.Diminished, alterationDegree );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>()
       .WithParameterName( "alterationDegree" );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenMajorBasedDiminishedDisplacementBelowMinimum()
  {
    // Arrange & Act
    var act = () => new Interval( IntervalQuantity.Second, IntervalQuality.Diminished, 4 );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>()
       .WithParameterName( "alterationDegree" );
  }

  [Fact]
  public void DefaultInterval_ShouldBeAscending()
  {
    default( Interval ).IsAscending.Should()
                       .BeTrue();

    default( Interval ).IsDescending.Should()
                       .BeFalse();
  }

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
  public void Equals_ShouldReturnFall_WhenComparingWithNull()
  {
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

    actual.Equals( null )
          .Should()
          .BeFalse();
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

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

    actual.Equals( actual )
          .Should()
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
  [InlineData( "1", IntervalQuantity.Unison, "" )]
  [InlineData( "12", IntervalQuantity.Twelfth, "" )]
  [InlineData( "10A", IntervalQuantity.Tenth, "A" )]
  [InlineData( "123", IntervalQuantity.Twelfth, "3" )]
  public void IntervalQuantityTryParse_ShouldReadOnlyOneOrTwoDigitsAndIgnoreTail(
    string input,
    IntervalQuantity expected,
    string expectedTail )
  {
    IntervalQuantity.TryParse( input.AsSpan(), null, out var actual, out var tail )
                    .Should()
                    .BeTrue();

    actual.Should()
          .Be( expected );

    tail.ToString()
        .Should()
        .Be( expectedTail );
  }

  [Theory]
  [MemberData( nameof( ExtendedInversionData ) )]
  public void Inversion_ShouldPreserveAlterationDegree_WhenIntervalHasExtendedAlteration(
    Interval interval,
    Interval expectedInversion )
  {
    interval.Inversion.Should()
            .Be( expectedInversion );

    ( -interval ).Inversion.Should()
                 .Be( -expectedInversion );
  }

  [Theory]
  [MemberData( nameof( InversionData ) )]
  public void Inversion_ShouldReturnCorrectInterval(
    string intervalString,
    string expectedInversionString )
  {
    var interval = Interval.Parse( intervalString );

    var expectedInversion = Interval.Parse( expectedInversionString )
                                    .FlipDirection();

    interval.Inversion.Should()
            .Be( expectedInversion );

    if( interval != Interval.Unison )
    {
      ( -interval ).Inversion.Should()
                   .Be( -expectedInversion );
    }
  }

  [Theory]
  [MemberData( nameof( InversionEdgeCaseData ) )]
  public void Inversion_ShouldReturnOriginalInterval_WhenInvertedTwiceAtOrBelowUnison(
    Interval interval,
    Interval expectedInversion )
  {
    interval.Inversion.Should()
            .Be( expectedInversion );

    expectedInversion.Inversion.Should()
                     .Be( interval );
  }

  [Fact]
  public void IsAscending_ShouldReturnCorrectValue()
  {
    Interval.MajorThird.IsAscending.Should()
            .BeTrue();

    ( -Interval.MajorThird ).IsAscending.Should()
                            .BeFalse();
  }

  [Fact]
  public void IsDescending_ShouldReturnCorrectValue()
  {
    Interval.MajorThird.IsDescending.Should()
            .BeFalse();

    ( -Interval.MajorThird ).IsDescending.Should()
                            .BeTrue();
  }

  [Theory]
  [MemberData( nameof( ValidIntervalStrings ) )]
  public void Parse_ShouldReturnCorrectInterval_WhenValidStringIsProvided(
    string intervalString,
    Interval expected )
  {
    Interval.Parse( intervalString )
            .Should()
            .Be( expected );
  }

  [Theory]
  [MemberData(nameof( ExtendedAlterationIntervalStrings ))]
  public void Parse_ShouldReturnCorrectInterval_WhenExtendedAlterationStringIsProvided(
    string intervalString,
    Interval expected,
    int expectedAlterationDegree )
  {
    var actual = Interval.Parse( intervalString );
    actual.Should()
          .Be( expected );

    actual.AlterationDegree.Should()
          .Be( expectedAlterationDegree );
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenInvalidStringIsProvided()
  {
    var act = () => Interval.Parse( "X2" );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void PublicConstructor_ShouldBeDescending_WhenNonUnisonAndDescendingTrue()
  {
    // Arrange
    var quantity = IntervalQuantity.Second;
    var quality = IntervalQuality.Major;

    // Act
    var interval = new Interval( quantity, quality, descending: true );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.IsAscending.Should()
            .BeFalse();
  }

  [Fact]
  public void PublicConstructor_ShouldSetQuantityQualityAndBeAscending_WhenNotDescending()
  {
    // Arrange
    var quantity = IntervalQuantity.Second;
    var quality = IntervalQuality.Major;

    // Act
    var interval = new Interval( quantity, quality );

    // Assert
    interval.Quantity.Should()
            .Be( quantity );

    interval.Quality.Should()
            .Be( quality );

    interval.IsAscending.Should()
            .BeTrue();
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalCombinations ) )]
  public void PublicConstructor_ShouldThrowArgumentException_ForInvalidCombination(
    IntervalQuantity q,
    IntervalQuality qual )
  {
    // Act
    Action act = () => _ = new Interval( q, qual );

    // Assert
    act.Should()
       .Throw<ArgumentException>();
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

    ( -Interval.Unison != Interval.Unison ).Should()
                                           .BeFalse();

    ( -Interval.MajorThird < -Interval.MinorThird ).Should()
                                                   .BeTrue();

    ( -Interval.MajorThird < Interval.Unison ).Should()
                                              .BeTrue();

    ( -Interval.DiminishedSecond < Interval.Unison ).Should()
                                                    .BeFalse();

    // -d2 and d2 have zero semitones so they are equal
    ( -Interval.DiminishedSecond < Interval.DiminishedSecond ).Should()
                                                              .BeFalse();

    // 1 and d2 have zero semitones so they are equal
    ( Interval.Unison < Interval.DiminishedSecond ).Should()
                                                   .BeFalse();
  }

  [Theory]
  [MemberData( nameof( SemitoneCountData ) )]
  public void SemitoneCount_ShouldReturnCorrectCount(
    Interval interval,
    int expectedSemitoneCount )
  {
    interval.SemitoneCount.Should()
            .Be( expectedSemitoneCount );
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

    x.Equals( null! )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void ToString_ShouldReturnDefaultFormat_WhenFormatIsEmpty()
  {
    Interval.MajorThird.ToString( "" )
            .Should()
            .Be( "3" );
  }

  [Fact]
  public void ToString_ShouldReturnDefaultFormat_WhenFormatIsNull()
  {
    Interval.MajorThird.ToString( null! )
            .Should()
            .Be( "3" );
  }

  [Theory]
  [MemberData( nameof( ToStringWithFormatData ) )]
  public void ToString_ShouldReturnFormattedString_WhenFormatIsProvided(
    Interval interval,
    string format,
    string expected )
  {
    interval.ToString( format )
            .Should()
            .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidIntervalStrings ) )]
  public void TryParse_ShouldReturnCorrectInterval_WhenValidStringIsProvided(
    string intervalString,
    Interval expected )
  {
    Interval.TryParse( intervalString, out var actual )
            .Should()
            .BeTrue();

    actual.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalStrings ) )]
  public void TryParse_ShouldReturnFalse_WhenInvalidStringIsProvided(
    string? input )
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

  #endregion
}
