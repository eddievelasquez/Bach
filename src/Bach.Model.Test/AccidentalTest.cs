// Module Name: AccidentalTest.cs
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

public sealed class AccidentalTest
{
  #region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    ( Accidental.DoubleFlat + 1 ).Should()
                                 .Be( Accidental.Flat );
    ( Accidental.Flat + 1 ).Should()
                           .Be( Accidental.Natural );
    ( Accidental.Natural + 1 ).Should()
                              .Be( Accidental.Sharp );
    ( Accidental.Sharp + 1 ).Should()
                            .Be( Accidental.DoubleSharp );
  }

  [Fact]
  public void AddTest()
  {
    Accidental.Natural.Add( 0 )
              .Should()
              .Be( Accidental.Natural );
    Accidental.Sharp.Add( 1 )
              .Should()
              .Be( Accidental.DoubleSharp );
    Accidental.DoubleFlat.Add( 1 )
              .Should()
              .Be( Accidental.Flat );
    Accidental.DoubleFlat.Add( 2 )
              .Should()
              .Be( Accidental.Natural );
    Accidental.Natural.Add( 1 )
              .Should()
              .Be( Accidental.Sharp );
    Accidental.Natural.Add( 2 )
              .Should()
              .Be( Accidental.DoubleSharp );
  }

  [Fact]
  public void AddThrowsWhenOutOfRangeTest()
  {
    var act = () => Accidental.DoubleSharp.Add( 1 );
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CompareToContractTest()
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

  [Fact]
  public void DecrementOperatorTest()
  {
    var accidental = Accidental.DoubleSharp;
    ( --accidental ).Should()
                    .Be( Accidental.Sharp );
    ( --accidental ).Should()
                    .Be( Accidental.Natural );
    ( --accidental ).Should()
                    .Be( Accidental.Flat );
    ( --accidental ).Should()
                    .Be( Accidental.DoubleFlat );
  }

  [Fact]
  public void EqualityFailsWithNullTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable CS8073
    ( lhs == null! ).Should()
                    .BeFalse();
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualitySucceedsWithSameObjectTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs == lhs ).Should()
                  .BeTrue();
#pragma warning restore 1718
  }

  [Fact]
  public void EqualitySucceedsWithTwoObjectsTest()
  {
    var lhs = Accidental.Natural;
    var rhs = new Accidental();
    ( lhs == rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void EqualsContractTest()
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
  public void EqualsFailsWithDifferentTypeTest()
  {
    object actual = Accidental.Natural;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = Accidental.Natural;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = Accidental.Natural;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
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

  [Fact]
  public void IncrementOperatorTest()
  {
    var accidental = Accidental.DoubleFlat;
    ( ++accidental ).Should()
                    .Be( Accidental.Flat );
    ( ++accidental ).Should()
                    .Be( Accidental.Natural );
    ( ++accidental ).Should()
                    .Be( Accidental.Sharp );
    ( ++accidental ).Should()
                    .Be( Accidental.DoubleSharp );
  }

  [Fact]
  public void InequalityFailsWithSameObjectTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs != lhs ).Should()
                  .BeFalse();
#pragma warning restore 1718
  }

  [Fact]
  public void InequalitySucceedsWithTwoObjectsTest()
  {
    var lhs = Accidental.Natural;
    var rhs = Accidental.Sharp;
    ( lhs != rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void ParseTest()
  {
    Accidental.Parse( "bb" )
              .Should()
              .Be( Accidental.DoubleFlat );
    Accidental.Parse( "b" )
              .Should()
              .Be( Accidental.Flat );
    Accidental.Parse( "" )
              .Should()
              .Be( Accidental.Natural );
    Accidental.Parse( "#" )
              .Should()
              .Be( Accidental.Sharp );
    Accidental.Parse( "##" )
              .Should()
              .Be( Accidental.DoubleSharp );
  }

  [Fact]
  public void ParseThrowsWithInvalidAccidentalSymbolTest()
  {
    var act = () => Accidental.Parse( "&" );
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void RelationalOperatorsTest()
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

  [Fact]
  public void SubtractionOperatorTest()
  {
    ( Accidental.DoubleSharp - 0 ).Should()
                                  .Be( Accidental.DoubleSharp );
    ( Accidental.DoubleSharp - 1 ).Should()
                                  .Be( Accidental.Sharp );
    ( Accidental.Sharp - 1 ).Should()
                            .Be( Accidental.Natural );
    ( Accidental.Natural - 1 ).Should()
                              .Be( Accidental.Flat );
    ( Accidental.Flat - 1 ).Should()
                           .Be( Accidental.DoubleFlat );
  }

  [Fact]
  public void SubtractTest()
  {
    Accidental.DoubleSharp.Subtract( 1 )
              .Should()
              .Be( Accidental.Sharp );
    Accidental.Sharp.Subtract( 1 )
              .Should()
              .Be( Accidental.Natural );
    Accidental.Natural.Subtract( 1 )
              .Should()
              .Be( Accidental.Flat );
    Accidental.Flat.Subtract( 1 )
              .Should()
              .Be( Accidental.DoubleFlat );
    Accidental.DoubleSharp.Subtract( 2 )
              .Should()
              .Be( Accidental.Natural );
    Accidental.DoubleSharp.Subtract( 3 )
              .Should()
              .Be( Accidental.Flat );
    Accidental.DoubleSharp.Subtract( 4 )
              .Should()
              .Be( Accidental.DoubleFlat );
  }

  [Fact]
  public void SubtractThrowsWhenOutOfRangeTest()
  {
    var act = () => Accidental.DoubleFlat.Subtract( 1 );
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void ToStringTest()
  {
    Accidental.DoubleFlat.ToString()
              .Should()
              .Be( "DoubleFlat" );
    Accidental.Flat.ToString()
              .Should()
              .Be( "Flat" );
    Accidental.Natural.ToString()
              .Should()
              .Be( "Natural" );
    Accidental.Sharp.ToString()
              .Should()
              .Be( "Sharp" );
    Accidental.DoubleSharp.ToString()
              .Should()
              .Be( "DoubleSharp" );
  }

  [Fact]
  public void ToSymbolTest()
  {
    Accidental.DoubleFlat.ToSymbol()
              .Should()
              .Be( "bb" );
    Accidental.Flat.ToSymbol()
              .Should()
              .Be( "b" );
    Accidental.Natural.ToSymbol()
              .Should()
              .Be( "" );
    Accidental.Sharp.ToSymbol()
              .Should()
              .Be( "#" );
    Accidental.DoubleSharp.ToSymbol()
              .Should()
              .Be( "##" );
  }

  [Fact]
  public void TryParseTest()
  {
    Accidental.TryParse( null!, out var accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Natural );
    Accidental.TryParse( "", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Natural );
    Accidental.TryParse( "♮", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Natural );
    Accidental.TryParse( "bb", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.DoubleFlat );
    Accidental.TryParse( "b", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Flat );
    Accidental.TryParse( null!, out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Natural );
    Accidental.TryParse( "", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Natural );
    Accidental.TryParse( "#", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.Sharp );
    Accidental.TryParse( "##", out accidental )
              .Should()
              .BeTrue();
    accidental.Should()
              .Be( Accidental.DoubleSharp );
    Accidental.TryParse( "b#", out _ )
              .Should()
              .BeFalse();
    Accidental.TryParse( "#b", out _ )
              .Should()
              .BeFalse();
    Accidental.TryParse( "bbb", out _ )
              .Should()
              .BeFalse();
    Accidental.TryParse( "###", out _ )
              .Should()
              .BeFalse();
    Accidental.TryParse( "$", out _ )
              .Should()
              .BeFalse();
  }

  [Fact]
  public void TypeSafeCompareToContractTest()
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
  public void TypeSafeEqualsContractTest()
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
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var actual = Accidental.Natural;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = Accidental.Natural;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
