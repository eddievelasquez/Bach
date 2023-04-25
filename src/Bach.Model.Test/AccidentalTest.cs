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

using System;
using Xunit;

namespace Bach.Model.Test;

public sealed class AccidentalTest
{
#region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    Assert.Equal( Accidental.Flat, Accidental.DoubleFlat + 1 );
    Assert.Equal( Accidental.Natural, Accidental.Flat + 1 );
    Assert.Equal( Accidental.Sharp, Accidental.Natural + 1 );
    Assert.Equal( Accidental.DoubleSharp, Accidental.Sharp + 1 );
  }

  [Fact]
  public void AddTest()
  {
    Assert.Equal( Accidental.Natural, Accidental.Natural.Add( 0 ) );
    Assert.Equal( Accidental.DoubleSharp, Accidental.Sharp.Add( 1 ) );
    Assert.Equal( Accidental.Flat, Accidental.DoubleFlat.Add( 1 ) );
    Assert.Equal( Accidental.Natural, Accidental.DoubleFlat.Add( 2 ) );
    Assert.Equal( Accidental.Sharp, Accidental.Natural.Add( 1 ) );
    Assert.Equal( Accidental.DoubleSharp, Accidental.Natural.Add( 2 ) );
  }

  [Fact]
  public void AddThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => Accidental.DoubleSharp.Add( 1 ) );
  }

  [Fact]
  public void CompareToContractTest()
  {
    object x = Accidental.Natural;
    object y = new Accidental();
    object z = (Accidental) 0;

    Assert.Equal( 0, ( (IComparable) x ).CompareTo( x ) ); // Reflexive
    Assert.Equal( 0, ( (IComparable) x ).CompareTo( y ) ); // Symmetric
    Assert.Equal( 0, ( (IComparable) y ).CompareTo( x ) );
    Assert.Equal( 0, ( (IComparable) y ).CompareTo( z ) ); // Transitive
    Assert.Equal( 0, ( (IComparable) x ).CompareTo( z ) );
    Assert.NotEqual( 0, ( (IComparable) x ).CompareTo( null ) ); // Never equal to null
  }

  [Fact]
  public void DecrementOperatorTest()
  {
    var accidental = Accidental.DoubleSharp;
    Assert.Equal( Accidental.Sharp, --accidental );
    Assert.Equal( Accidental.Natural, --accidental );
    Assert.Equal( Accidental.Flat, --accidental );
    Assert.Equal( Accidental.DoubleFlat, --accidental );
  }

  [Fact]
  public void EqualityFailsWithNullTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable CS8073
    Assert.False( lhs == null );
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualitySucceedsWithSameObjectTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    Assert.True( lhs == lhs );
#pragma warning restore 1718
  }

  [Fact]
  public void EqualitySucceedsWithTwoObjectsTest()
  {
    var lhs = Accidental.Natural;
    var rhs = new Accidental();
    Assert.True( lhs == rhs );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = Accidental.Natural;
    object y = new Accidental();
    object z = (Accidental) 0;

    // ReSharper disable once EqualExpressionComparison
    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void EqualsFailsWithDifferentTypeTest()
  {
    object actual = Accidental.Natural;
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = Accidental.Natural;
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = Accidental.Natural;
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = Accidental.Natural;
    var expected = new Accidental();
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void IncrementOperatorTest()
  {
    var accidental = Accidental.DoubleFlat;
    Assert.Equal( Accidental.Flat, ++accidental );
    Assert.Equal( Accidental.Natural, ++accidental );
    Assert.Equal( Accidental.Sharp, ++accidental );
    Assert.Equal( Accidental.DoubleSharp, ++accidental );
  }

  [Fact]
  public void InequalityFailsWithSameObjectTest()
  {
    var lhs = Accidental.Natural;
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    Assert.False( lhs != lhs );
#pragma warning restore 1718
  }

  [Fact]
  public void InequalitySucceedsWithTwoObjectsTest()
  {
    var lhs = Accidental.Natural;
    var rhs = Accidental.Sharp;
    Assert.True( lhs != rhs );
  }

  [Fact]
  public void ParseTest()
  {
    Assert.Equal( Accidental.DoubleFlat, Accidental.Parse( "bb" ) );
    Assert.Equal( Accidental.Flat, Accidental.Parse( "b" ) );
    Assert.Equal( Accidental.Natural, Accidental.Parse( "" ) );
    Assert.Equal( Accidental.Sharp, Accidental.Parse( "#" ) );
    Assert.Equal( Accidental.DoubleSharp, Accidental.Parse( "##" ) );
  }

  [Fact]
  public void ParseThrowsWithInvalidAccidentalSymbolTest()
  {
    Assert.Throws<FormatException>( () => { Accidental.Parse( "&" ); } );
  }

  [Fact]
  public void RelationalOperatorsTest()
  {
    Assert.True( Accidental.DoubleFlat < Accidental.Flat );
    Assert.True( Accidental.DoubleFlat <= Accidental.Flat );
    Assert.True( Accidental.Flat < Accidental.Natural );
    Assert.True( Accidental.Flat <= Accidental.Natural );
    Assert.True( Accidental.Natural < Accidental.Sharp );
    Assert.True( Accidental.Natural <= Accidental.Sharp );
    Assert.True( Accidental.Sharp < Accidental.DoubleSharp );
    Assert.True( Accidental.Sharp <= Accidental.DoubleSharp );

    Assert.True( Accidental.DoubleSharp > Accidental.Sharp );
    Assert.True( Accidental.DoubleSharp >= Accidental.Sharp );
    Assert.True( Accidental.Sharp > Accidental.Natural );
    Assert.True( Accidental.Sharp >= Accidental.Natural );
    Assert.True( Accidental.Natural > Accidental.Flat );
    Assert.True( Accidental.Natural >= Accidental.Flat );
    Assert.True( Accidental.Flat > Accidental.DoubleFlat );
    Assert.True( Accidental.Flat >= Accidental.DoubleFlat );
  }

  [Fact]
  public void SubtractionOperatorTest()
  {
    Assert.Equal( Accidental.DoubleSharp, Accidental.DoubleSharp - 0 );
    Assert.Equal( Accidental.Sharp, Accidental.DoubleSharp - 1 );
    Assert.Equal( Accidental.Natural, Accidental.Sharp - 1 );
    Assert.Equal( Accidental.Flat, Accidental.Natural - 1 );
    Assert.Equal( Accidental.DoubleFlat, Accidental.Flat - 1 );
  }

  [Fact]
  public void SubtractTest()
  {
    Assert.Equal( Accidental.Sharp, Accidental.DoubleSharp.Subtract( 1 ) );
    Assert.Equal( Accidental.Natural, Accidental.Sharp.Subtract( 1 ) );
    Assert.Equal( Accidental.Flat, Accidental.Natural.Subtract( 1 ) );
    Assert.Equal( Accidental.DoubleFlat, Accidental.Flat.Subtract( 1 ) );
    Assert.Equal( Accidental.Natural, Accidental.DoubleSharp.Subtract( 2 ) );
    Assert.Equal( Accidental.Flat, Accidental.DoubleSharp.Subtract( 3 ) );
    Assert.Equal( Accidental.DoubleFlat, Accidental.DoubleSharp.Subtract( 4 ) );
  }

  [Fact]
  public void SubtractThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => Accidental.DoubleFlat.Subtract( 1 ) );
  }

  [Fact]
  public void ToStringTest()
  {
    Assert.Equal( "DoubleFlat", Accidental.DoubleFlat.ToString() );
    Assert.Equal( "Flat", Accidental.Flat.ToString() );
    Assert.Equal( "Natural", Accidental.Natural.ToString() );
    Assert.Equal( "Sharp", Accidental.Sharp.ToString() );
    Assert.Equal( "DoubleSharp", Accidental.DoubleSharp.ToString() );
  }

  [Fact]
  public void ToSymbolTest()
  {
    Assert.Equal( "bb", Accidental.DoubleFlat.ToSymbol() );
    Assert.Equal( "b", Accidental.Flat.ToSymbol() );
    Assert.Equal( "", Accidental.Natural.ToSymbol() );
    Assert.Equal( "#", Accidental.Sharp.ToSymbol() );
    Assert.Equal( "##", Accidental.DoubleSharp.ToSymbol() );
  }

  [Fact]
  public void TryParseTest()
  {
    Assert.True( Accidental.TryParse( null, out var accidental ) );
    Assert.Equal( Accidental.Natural, accidental );
    Assert.True( Accidental.TryParse( "", out accidental ) );
    Assert.Equal( Accidental.Natural, accidental );
    Assert.True( Accidental.TryParse( "♮", out accidental ) );
    Assert.Equal( Accidental.Natural, accidental );
    Assert.True( Accidental.TryParse( "bb", out accidental ) );
    Assert.Equal( Accidental.DoubleFlat, accidental );
    Assert.True( Accidental.TryParse( "b", out accidental ) );
    Assert.Equal( Accidental.Flat, accidental );
    Assert.True( Accidental.TryParse( null, out accidental ) );
    Assert.Equal( Accidental.Natural, accidental );
    Assert.True( Accidental.TryParse( "", out accidental ) );
    Assert.Equal( Accidental.Natural, accidental );
    Assert.True( Accidental.TryParse( "#", out accidental ) );
    Assert.Equal( Accidental.Sharp, accidental );
    Assert.True( Accidental.TryParse( "##", out accidental ) );
    Assert.Equal( Accidental.DoubleSharp, accidental );
    Assert.False( Accidental.TryParse( "b#", out accidental ) );
    Assert.False( Accidental.TryParse( "#b", out accidental ) );
    Assert.False( Accidental.TryParse( "bbb", out accidental ) );
    Assert.False( Accidental.TryParse( "###", out accidental ) );
    Assert.False( Accidental.TryParse( "$", out accidental ) );
  }

  [Fact]
  public void TypeSafeCompareToContractTest()
  {
    var x = Accidental.Natural;
    var y = new Accidental();
    var z = (Accidental) 0;

    Assert.Equal( 0, x.CompareTo( x ) ); // Reflexive
    Assert.Equal( 0, x.CompareTo( y ) ); // Symmetric
    Assert.Equal( 0, y.CompareTo( x ) );
    Assert.Equal( 0, y.CompareTo( z ) ); // Transitive
    Assert.Equal( 0, x.CompareTo( z ) );
    Assert.NotEqual( 0, x.CompareTo( null ) ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = Accidental.Natural;
    var y = new Accidental();
    var z = (Accidental) 0;

    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var actual = Accidental.Natural;

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = Accidental.Natural;
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
