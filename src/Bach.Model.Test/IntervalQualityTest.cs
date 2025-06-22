// Module Name: IntervalQualityTest.cs
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

public sealed class IntervalQualityTest
{
  #region Public Methods

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void AdditionOperator_ShouldSucceed(
    IntervalQuality quality,
    int increment,
    IntervalQuality expectedQuality )
  {
    ( quality + increment ).Should()
                           .Be( expectedQuality );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void AdditionOperator_ShouldThrowArgumentOutOfRange(
    IntervalQuality quality,
    int increment )
  {
    var act = () => quality + increment;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void Add_ShouldSucceed(
    IntervalQuality quality,
    int increment,
    IntervalQuality expectedQuality )
  {
    quality.Add( increment )
           .Should()
           .Be( expectedQuality );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void Add_ShouldThrowArgumentOutOfRange(
    IntervalQuality quality,
    int increment )
  {
    var act = () => quality.Add( increment );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CompareTo_ShouldSatisfyEquivalenceRelation()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

    ( (IComparable) x ).CompareTo( x )
                       .Should()
                       .Be( 0 ); // Reflexive

    ( (IComparable) x ).CompareTo( y )
                       .Should()
                       .Be( 0 ); // Symmetric

    ( (IComparable) y ).CompareTo( x )
                       .Should()
                       .Be( 0 );

    ( (IComparable) y ).CompareTo( z )
                       .Should()
                       .Be( 0 ); // Transitive

    ( (IComparable) x ).CompareTo( z )
                       .Should()
                       .Be( 0 );

    ( (IComparable) x ).CompareTo( null )
                       .Should()
                       .NotBe( 0 ); // Never equal to null
  }

  [Theory]
  [MemberData( nameof( ValidDecrementData ) )]
  public void DecrementOperator_ShouldSucceed(
    IntervalQuality quality,
    IntervalQuality expectedQuality )
  {
    ( --quality ).Should()
                 .Be( expectedQuality );
  }

  [Fact]
  public void DecrementOperator_ShouldThrowArgumentOutOfRange()
  {
    var quality = IntervalQuality.Diminished;
    var act = () => --quality;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualityOperator_ShouldReturnFalse_WhenComparingToNull()
  {
    var lhs = IntervalQuality.Diminished;
#pragma warning disable CS8073
    ( lhs == null! ).Should()
                    .BeFalse();
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualityOperator_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var lhs = IntervalQuality.Diminished;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs == lhs ).Should()
                  .BeTrue();
#pragma warning restore 1718
  }

  [Fact]
  public void EqualityOperator_ShouldReturnTrue_WhenComparingEquivalentObjects()
  {
    var lhs = IntervalQuality.Diminished;
    var rhs = new IntervalQuality();

    ( lhs == rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

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
  public void Equals_ShouldReturnFalse_WhenComparingObjectsOfDifferentType()
  {
    object actual = IntervalQuality.Diminished;

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = IntervalQuality.Diminished;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = IntervalQuality.Diminished;

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
  {
    var actual = IntervalQuality.Diminished;
    var expected = (IntervalQuality) 0;

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Theory]
  [MemberData( nameof( ValidIncrementData ) )]
  public void IncrementOperator_ShouldSucceed(
    IntervalQuality quality,
    IntervalQuality expectedQuality )
  {
    ( ++quality ).Should()
                 .Be( expectedQuality );
  }

  [Fact]
  public void IncrementOperator_ShouldThrowArgumentOutOfRange()
  {
    var quality = IntervalQuality.Augmented;
    var act = () => ++quality;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void RelationalOperators_ShouldSatisfyOrdering()
  {
    ( IntervalQuality.Diminished < IntervalQuality.Minor ).Should()
                                                          .BeTrue();

    ( IntervalQuality.Diminished <= IntervalQuality.Minor ).Should()
                                                           .BeTrue();

    ( IntervalQuality.Minor < IntervalQuality.Perfect ).Should()
                                                       .BeTrue();

    ( IntervalQuality.Minor <= IntervalQuality.Perfect ).Should()
                                                        .BeTrue();

    ( IntervalQuality.Perfect < IntervalQuality.Major ).Should()
                                                       .BeTrue();

    ( IntervalQuality.Perfect <= IntervalQuality.Major ).Should()
                                                        .BeTrue();

    ( IntervalQuality.Major < IntervalQuality.Augmented ).Should()
                                                         .BeTrue();

    ( IntervalQuality.Major <= IntervalQuality.Augmented ).Should()
                                                          .BeTrue();

    ( IntervalQuality.Augmented > IntervalQuality.Major ).Should()
                                                         .BeTrue();

    ( IntervalQuality.Augmented >= IntervalQuality.Major ).Should()
                                                          .BeTrue();

    ( IntervalQuality.Major > IntervalQuality.Perfect ).Should()
                                                       .BeTrue();

    ( IntervalQuality.Major >= IntervalQuality.Perfect ).Should()
                                                        .BeTrue();

    ( IntervalQuality.Perfect > IntervalQuality.Minor ).Should()
                                                       .BeTrue();

    ( IntervalQuality.Perfect >= IntervalQuality.Minor ).Should()
                                                        .BeTrue();

    ( IntervalQuality.Minor > IntervalQuality.Diminished ).Should()
                                                          .BeTrue();

    ( IntervalQuality.Minor >= IntervalQuality.Diminished ).Should()
                                                           .BeTrue();
  }

  [Theory]
  [MemberData( nameof( ValidQualityNames ) )]
  public void ToString_ShouldReturnName( IntervalQuality quality, string expectedName )
  {
    quality.ToString()
           .Should()
           .Be( expectedName );
  }

  [Theory]
  [MemberData( nameof( ValidQualityStrings ) )]
  public void Parse_ShouldSucceed_WhenValueIsValid( string input, IntervalQuality expected )
  {
    IntervalQuality.Parse( input )
                   .Should()
                   .Be( expected );
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    var act = () => IntervalQuality.Parse( "X" );

    act.Should()
       .Throw<FormatException>();
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void SubtractionOperator_ShouldSucceed(
    IntervalQuality quality,
    int decrement,
    IntervalQuality expectedQuality )
  {
    ( quality - decrement ).Should()
                         .Be( expectedQuality );
  }

  [Fact]
  public void SubtractionOperator_ShouldThrowArgumentOutOfRange()
  {
    var act = () => IntervalQuality.Diminished - 1;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void Subtract_ShouldSucceed(
    IntervalQuality quality,
    int decrement,
    IntervalQuality expectedQuality )
  {
    quality.Subtract( decrement )
           .Should()
           .Be( expectedQuality );
  }

  [Theory]
  [MemberData( nameof( InvalidSubtractionData ) )]
  public void Subtract_ShouldThrowArgumentOutOfRange(
    IntervalQuality quality,
    int decrement )
  {
    var act = () => quality.Subtract( decrement );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( LongNameData ) )]
  public void LongName_ShouldReturnName(
    IntervalQuality quality,
    string expectedName )
  {
    quality.LongName.Should()
           .Be( expectedName );
  }

  [Theory]
  [MemberData( nameof( ShortNameData ) )]
  public void ShortName_ShouldReturnName(
    IntervalQuality quality,
    string expectedShortName )
  {
    quality.ShortName.Should()
           .Be( expectedShortName );
  }

  [Fact]
  public void StronglyTypedCompareTo_ShouldSatisfyEquivalenceRelation()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

    x.CompareTo( x )
     .Should()
     .Be( 0 ); // Reflexive

    x.CompareTo( y )
     .Should()
     .Be( 0 ); // Symmetric

    y.CompareTo( x )
     .Should()
     .Be( 0 );

    y.CompareTo( z )
     .Should()
     .Be( 0 ); // Transitive

    x.CompareTo( z )
     .Should()
     .Be( 0 );

    x.CompareTo( null )
     .Should()
     .NotBe( 0 ); // Never equal to null
  }

  [Fact]
  public void StronglyTypedEquals_ShouldSatisfyEquivalenceRelation()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

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
  public void StronglyTypedEquals_ReturnsFalse_WhenComparingDifferentIntervalQualities()
  {
    var actual = IntervalQuality.Diminished;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void StronglyTypedEquals_ReturnsFalse_WhenComparingToNull()
  {
    var actual = IntervalQuality.Diminished;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<IntervalQuality, int, IntervalQuality> ValidAdditionData { get; } = new()
  {
    { IntervalQuality.Diminished, 0, IntervalQuality.Diminished },
    { IntervalQuality.Diminished, 1, IntervalQuality.Minor },
    { IntervalQuality.Diminished, 2, IntervalQuality.Perfect },
    { IntervalQuality.Diminished, 3, IntervalQuality.Major },
    { IntervalQuality.Diminished, 4, IntervalQuality.Augmented },
    { IntervalQuality.Minor, 1, IntervalQuality.Perfect },
    { IntervalQuality.Minor, 2, IntervalQuality.Major },
    { IntervalQuality.Minor, 3, IntervalQuality.Augmented },
    { IntervalQuality.Perfect, 1, IntervalQuality.Major },
    { IntervalQuality.Perfect, 2, IntervalQuality.Augmented },
    { IntervalQuality.Major, 1, IntervalQuality.Augmented }
  };

  public static TheoryData<IntervalQuality, int> InvalidAdditionData { get; } = new()
  {
    { IntervalQuality.Diminished, 5 },
    { IntervalQuality.Minor, 4 },
    { IntervalQuality.Perfect, 3 },
    { IntervalQuality.Major, 2 },
    { IntervalQuality.Augmented, 1 },
    { IntervalQuality.Diminished, -1 },
    { IntervalQuality.Minor, -2 },
    { IntervalQuality.Perfect, -3 },
    { IntervalQuality.Major, -4 },
    { IntervalQuality.Augmented, -5 },
  };

  public static TheoryData<IntervalQuality, IntervalQuality> ValidIncrementData { get; } = new()
  {
    { IntervalQuality.Diminished, IntervalQuality.Minor },
    { IntervalQuality.Minor, IntervalQuality.Perfect },
    { IntervalQuality.Perfect, IntervalQuality.Major },
    { IntervalQuality.Major, IntervalQuality.Augmented }
  };

  public static TheoryData<IntervalQuality, IntervalQuality> ValidDecrementData { get; } = new()
  {
    { IntervalQuality.Augmented, IntervalQuality.Major },
    { IntervalQuality.Major, IntervalQuality.Perfect },
    { IntervalQuality.Perfect, IntervalQuality.Minor },
    { IntervalQuality.Minor, IntervalQuality.Diminished }
  };

  public static TheoryData<IntervalQuality, int, IntervalQuality> ValidSubtractionData { get; } = new()
  {
    { IntervalQuality.Augmented, 0, IntervalQuality.Augmented },
    { IntervalQuality.Augmented, 1, IntervalQuality.Major },
    { IntervalQuality.Augmented, 2, IntervalQuality.Perfect },
    { IntervalQuality.Augmented, 3, IntervalQuality.Minor },
    { IntervalQuality.Augmented, 4, IntervalQuality.Diminished },
    { IntervalQuality.Major, 1, IntervalQuality.Perfect },
    { IntervalQuality.Major, 2, IntervalQuality.Minor },
    { IntervalQuality.Major, 3, IntervalQuality.Diminished },
    { IntervalQuality.Perfect, 1, IntervalQuality.Minor },
    { IntervalQuality.Perfect, 2, IntervalQuality.Diminished },
    { IntervalQuality.Minor, 1, IntervalQuality.Diminished }
  };

  public static TheoryData<IntervalQuality, int> InvalidSubtractionData { get; } = new()
  {
    { IntervalQuality.Augmented, 5 },
    { IntervalQuality.Major, 4 },
    { IntervalQuality.Perfect, 3 },
    { IntervalQuality.Minor, 2 },
    { IntervalQuality.Diminished, 1 },
  };

  public static TheoryData<IntervalQuality, string> LongNameData { get; } = new()
  {
    { IntervalQuality.Diminished, "Diminished" },
    { IntervalQuality.Minor, "Minor" },
    { IntervalQuality.Perfect, "Perfect" },
    { IntervalQuality.Major, "Major" },
    { IntervalQuality.Augmented, "Augmented" }
  };

  public static TheoryData<IntervalQuality, string> ShortNameData { get; } = new()
  {
    { IntervalQuality.Diminished, "dim" },
    { IntervalQuality.Minor, "min" },
    { IntervalQuality.Perfect, "Perf" },
    { IntervalQuality.Major, "Maj" },
    { IntervalQuality.Augmented, "Aug" }
  };

  public static TheoryData<IntervalQuality, string> ValidQualityNames { get; } = new()
  {
    { IntervalQuality.Diminished, "Diminished" },
    { IntervalQuality.Minor, "Minor" },
    { IntervalQuality.Perfect, "Perfect" },
    { IntervalQuality.Major, "Major" },
    { IntervalQuality.Augmented, "Augmented" }
  };

  public static TheoryData<string, IntervalQuality> ValidQualityStrings { get; } = new()
  {
    { "d", IntervalQuality.Diminished },
    { "m", IntervalQuality.Minor },
    { "P", IntervalQuality.Perfect },
    { "M", IntervalQuality.Major },
    { "A", IntervalQuality.Augmented }
  };

  #endregion
}
