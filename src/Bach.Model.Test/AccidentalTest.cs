// Module Name: AccidentalTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2025  Eddie Velasquez.
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

public sealed class AccidentalTest
{
  #region Public Methods

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void AdditionOperator_ShouldSucceed(
    Accidental accidental,
    int increment,
    Accidental expectedAccidental )
  {
    ( accidental + increment ).Should()
                              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void AdditionOperator_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int increment )
  {
    var act = () => accidental + increment;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void Add_ShouldSucceed(
    Accidental accidental,
    int increment,
    Accidental expectedAccidental )
  {
    accidental.Add( increment )
              .Should()
              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void Add_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int increment )
  {
    var act = () => accidental.Add( increment );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CompareTo_ShouldSatisfyEquivalenceRelation()
  {
    object x = Accidental.Natural;
    object y = new Accidental();
    object z = (Accidental) 0;

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
    Accidental accidental,
    Accidental expectedAccidental )
  {
    ( --accidental ).Should()
                    .Be( expectedAccidental );
  }

  [Fact]
  public void DecrementOperator_ShouldThrowArgumentOutOfRange()
  {
    var accidental = Accidental.DoubleFlat;
    var act = () => --accidental;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualityOperator_ShouldReturnFalse_WhenComparingToNull()
  {
    var lhs = Accidental.Natural;
#pragma warning disable CS8073
    ( lhs == null! ).Should()
                    .BeFalse();
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualityOperator_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs == lhs ).Should()
                  .BeTrue();
#pragma warning restore 1718
  }

  [Fact]
  public void EqualityOperator_ShouldReturnTrue_WhenComparingEquivalentObjects()
  {
    var lhs = Accidental.Natural;
    var rhs = new Accidental();

    ( lhs == rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = Accidental.Natural;
    object y = new Accidental();
    object z = (Accidental) 0;

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
  public void Equals_ReturnFalse_WhenComparingObjectOfDifferentType()
  {
    object actual = Accidental.Natural;

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = Accidental.Natural;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = Accidental.Natural;

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
  {
    var actual = Accidental.Natural;
    var expected = new Accidental();

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
    Accidental accidental,
    Accidental expectedAccidental )
  {
    ( ++accidental ).Should()
                    .Be( expectedAccidental );
  }

  [Fact]
  public void IncrementOperator_ShouldThrowArgumentOutOfRange()
  {
    var accidental = Accidental.DoubleSharp;
    var act = () => ++accidental;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void InequalityOperator_ShouldReturnFalse_WhenComparingTheSameObject()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs != lhs ).Should()
                  .BeFalse();
#pragma warning restore 1718
  }

  [Fact]
  public void InequalityOperator_ShouldReturnTrue_WhenComparingTwoDifferentObjects()
  {
    var lhs = Accidental.Natural;
    var rhs = Accidental.Sharp;

    ( lhs != rhs ).Should()
                  .BeTrue();
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void Parse_ShouldSucceed_WhenValueIsValid(
    string? value,
    Accidental accidental,
    string name,
    string symbol )
  {
    _ = name; // Unused parameter, but kept for clarity in the test data
    _ = symbol; // Unused parameter, but kept for clarity in the test data

    Accidental.Parse( value )
              .Should()
              .Be( accidental );
  }

  [Theory]
  [MemberData( nameof( InvalidAccidentals ) )]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(
    string? value,
    Accidental _ )
  {
    var act = () => Accidental.Parse( value );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void RelationalOperators_ShouldSatisfyOrdering()
  {
    ( Accidental.DoubleFlat < Accidental.Flat ).Should()
                                               .BeTrue();

    ( Accidental.DoubleFlat <= Accidental.Flat ).Should()
                                                .BeTrue();

    ( Accidental.Flat < Accidental.Natural ).Should()
                                            .BeTrue();

    ( Accidental.Flat <= Accidental.Natural ).Should()
                                             .BeTrue();

    ( Accidental.Natural < Accidental.Sharp ).Should()
                                             .BeTrue();

    ( Accidental.Natural <= Accidental.Sharp ).Should()
                                              .BeTrue();

    ( Accidental.Sharp < Accidental.DoubleSharp ).Should()
                                                 .BeTrue();

    ( Accidental.Sharp <= Accidental.DoubleSharp ).Should()
                                                  .BeTrue();

    ( Accidental.DoubleSharp > Accidental.Sharp ).Should()
                                                 .BeTrue();

    ( Accidental.DoubleSharp >= Accidental.Sharp ).Should()
                                                  .BeTrue();

    ( Accidental.Sharp > Accidental.Natural ).Should()
                                             .BeTrue();

    ( Accidental.Sharp >= Accidental.Natural ).Should()
                                              .BeTrue();

    ( Accidental.Natural > Accidental.Flat ).Should()
                                            .BeTrue();

    ( Accidental.Natural >= Accidental.Flat ).Should()
                                             .BeTrue();

    ( Accidental.Flat > Accidental.DoubleFlat ).Should()
                                               .BeTrue();

    ( Accidental.Flat >= Accidental.DoubleFlat ).Should()
                                                .BeTrue();
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void SubtractionOperator_ShouldSucceed(
    Accidental accidental,
    int decrement,
    Accidental expectedAccidental )
  {
    ( accidental - decrement ).Should()
                              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( InvalidSubtractionData ) )]
  public void SubtractionOperator_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int decrement )
  {
    var act = () => accidental - decrement;

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void Subtract_ShouldSucceed(
    Accidental accidental,
    int decrement,
    Accidental expectedAccidental )
  {
    accidental.Subtract( decrement )
              .Should()
              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( InvalidSubtractionData ) )]
  public void Subtract_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int decrement )
  {
    var act = () => accidental.Subtract( decrement );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void ToString_ShouldReturnName(
    string? value,
    Accidental accidental,
    string name,
    string symbol )
  {
    _ = value; // Unused parameter, but kept for clarity in the test data
    _ = symbol; // Unused parameter, but kept for clarity in the test data

    accidental.ToString()
              .Should()
              .Be( name );
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void ToSymbol_ShouldReturnSymbol(
    string? value,
    Accidental accidental,
    string name,
    string symbol )
  {
    _ = value; // Unused parameter, but kept for clarity in the test data
    _ = name; // Unused parameter, but kept for clarity in the test data

    accidental.ToSymbol()
              .Should()
              .Be( symbol );
  }

  [Theory]
  [MemberData( nameof( AllAccidentals ) )]
  public void TryParse_ShouldProcessAnyValue(
    string? input,
    Accidental expectedAccidental,
    bool expectedResult )
  {
    var result = Accidental.TryParse( input, out var accidental );

    result.Should()
          .Be( expectedResult );

    accidental.Should()
              .Be( expectedAccidental );
  }

  [Fact]
  public void StronglyTypedCompareTo_ShouldSatisfyEquivalenceRelation()
  {
    var x = Accidental.Natural;
    var y = new Accidental();
    var z = (Accidental) 0;

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
    var x = Accidental.Natural;
    var y = new Accidental();
    var z = (Accidental) 0;

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
  public void StronglyTypedEquals_RetursFalse_WhenComparingDifferentAccidentalst()
  {
    var actual = Accidental.Natural;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ReturnsFalse_WhenComparingToNull()
  {
    var actual = Accidental.Natural;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<string?, Accidental, bool> AllAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental, bool>();

      foreach( var row in ValidAccidentals )
      {
        var (value, accidental, _, _) = row.Data;
        data.Add( value, accidental, true );
      }

      foreach( var row in InvalidAccidentals )
      {
        var (value, accidental) = row.Data;
        data.Add( value, accidental, false );
      }

      return data;
    }
  }

  public static TheoryData<string?, Accidental, string, string> ValidAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental, string, string>
      {
        { null, Accidental.Natural, "Natural", "" },
        { "", Accidental.Natural, "Natural", "" },
        { "♮", Accidental.Natural, "Natural", "" },
        { "bb", Accidental.DoubleFlat, "DoubleFlat", "bb" },
        { "b", Accidental.Flat, "Flat", "b" },
        { "#", Accidental.Sharp, "Sharp", "#" },
        { "##", Accidental.DoubleSharp, "DoubleSharp", "##" },
      };

      return data;
    }
  }

  public static TheoryData<string?, Accidental> InvalidAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental>
      {
        { "b#", Accidental.Natural },
        { "#b", Accidental.Natural },
        { "bbb", Accidental.Natural },
        { "###", Accidental.Natural },
        { "$", Accidental.Natural }
      };

      return data;
    }
  }

  public static TheoryData<Accidental, int, Accidental> ValidAdditionData { get; } = new()
  {
    { Accidental.DoubleFlat, 0, Accidental.DoubleFlat },
    { Accidental.DoubleFlat, 1, Accidental.Flat },
    { Accidental.DoubleFlat, 2, Accidental.Natural },
    { Accidental.DoubleFlat, 3, Accidental.Sharp },
    { Accidental.DoubleFlat, 4, Accidental.DoubleSharp },
    { Accidental.Flat, 0, Accidental.Flat },
    { Accidental.Flat, 1, Accidental.Natural },
    { Accidental.Flat, 2, Accidental.Sharp },
    { Accidental.Flat, 3, Accidental.DoubleSharp },
    { Accidental.Natural, 0, Accidental.Natural },
    { Accidental.Natural, 1, Accidental.Sharp },
    { Accidental.Natural, 2, Accidental.DoubleSharp },
    { Accidental.Sharp, 0, Accidental.Sharp },
    { Accidental.Sharp, 1, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 0, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, -1, Accidental.Sharp },
    { Accidental.DoubleSharp, -2, Accidental.Natural },
    { Accidental.DoubleSharp, -3, Accidental.Flat },
    { Accidental.DoubleSharp, -4, Accidental.DoubleFlat },
    { Accidental.Sharp, -1, Accidental.Natural },
    { Accidental.Sharp, -2, Accidental.Flat },
    { Accidental.Sharp, -3, Accidental.DoubleFlat },
    { Accidental.Natural, -1, Accidental.Flat },
    { Accidental.Natural, -2, Accidental.DoubleFlat },
    { Accidental.Flat, -1, Accidental.DoubleFlat },
  };

  public static TheoryData<Accidental, Accidental> ValidIncrementData { get; } = new()
  {
    { Accidental.DoubleFlat, Accidental.Flat },
    { Accidental.Flat, Accidental.Natural },
    { Accidental.Natural, Accidental.Sharp },
    { Accidental.Sharp, Accidental.DoubleSharp }
  };

  public static TheoryData<Accidental, int> InvalidAdditionData { get; } = new()
  {
    { Accidental.DoubleFlat, 5 },
    { Accidental.Flat, 4 },
    { Accidental.Natural, 3 },
    { Accidental.Sharp, 2 },
    { Accidental.DoubleSharp, 1 },
    { Accidental.DoubleSharp, -5 },
    { Accidental.Sharp, -4 },
    { Accidental.Natural, -3 },
    { Accidental.Flat, -2 },
    { Accidental.DoubleFlat, -1 },
  };

  public static TheoryData<Accidental, int, Accidental> ValidSubtractionData { get; } = new()
  {
    { Accidental.DoubleFlat, 0, Accidental.DoubleFlat },
    { Accidental.DoubleFlat, -1, Accidental.Flat },
    { Accidental.DoubleFlat, -2, Accidental.Natural },
    { Accidental.DoubleFlat, -3, Accidental.Sharp },
    { Accidental.DoubleFlat, -4, Accidental.DoubleSharp },
    { Accidental.Flat, 0, Accidental.Flat },
    { Accidental.Flat, -1, Accidental.Natural },
    { Accidental.Flat, -2, Accidental.Sharp },
    { Accidental.Flat, -3, Accidental.DoubleSharp },
    { Accidental.Natural, 0, Accidental.Natural },
    { Accidental.Natural, -1, Accidental.Sharp },
    { Accidental.Natural, -2, Accidental.DoubleSharp },
    { Accidental.Sharp, 0, Accidental.Sharp },
    { Accidental.Sharp, -1, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 0, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 1, Accidental.Sharp },
    { Accidental.DoubleSharp, 2, Accidental.Natural },
    { Accidental.DoubleSharp, 3, Accidental.Flat },
    { Accidental.DoubleSharp, 4, Accidental.DoubleFlat },
    { Accidental.Sharp, 1, Accidental.Natural },
    { Accidental.Sharp, 2, Accidental.Flat },
    { Accidental.Sharp, 3, Accidental.DoubleFlat },
    { Accidental.Natural, 1, Accidental.Flat },
    { Accidental.Natural, 2, Accidental.DoubleFlat },
    { Accidental.Flat, 1, Accidental.DoubleFlat },
  };

  public static TheoryData<Accidental, Accidental> ValidDecrementData { get; } = new()
  {
    { Accidental.DoubleSharp, Accidental.Sharp },
    { Accidental.Sharp, Accidental.Natural },
    { Accidental.Natural, Accidental.Flat },
    { Accidental.Flat, Accidental.DoubleFlat },
  };

  public static TheoryData<Accidental, int> InvalidSubtractionData { get; } = new()
  {
    { Accidental.DoubleFlat, -5 },
    { Accidental.Flat, -4 },
    { Accidental.Natural, -3 },
    { Accidental.Sharp, -2 },
    { Accidental.DoubleSharp, -1 },
    { Accidental.DoubleSharp, 5 },
    { Accidental.Sharp, 4 },
    { Accidental.Natural, 3 },
    { Accidental.Flat, 2 },
    { Accidental.DoubleFlat, 1 }
  };

  #endregion
}
