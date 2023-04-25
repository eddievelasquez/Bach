// Module Name: NoteNameTest.cs
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

public sealed class NoteNameTest
{
#region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    Assert.Equal( NoteName.C, NoteName.C + 0 );
    Assert.Equal( NoteName.D, NoteName.C + 1 );
    Assert.Equal( NoteName.E, NoteName.C + 2 );
    Assert.Equal( NoteName.B, NoteName.C + 6 );
    Assert.Equal( NoteName.C, NoteName.C + 7 );
    Assert.Equal( NoteName.D, NoteName.C + 8 );
    Assert.Equal( NoteName.C, NoteName.B + 1 );
  }

  [Fact]
  public void AddTest()
  {
    Assert.Equal( NoteName.C, NoteName.C.Add( 0 ) );
    Assert.Equal( NoteName.D, NoteName.C.Add( 1 ) );
    Assert.Equal( NoteName.E, NoteName.C.Add( 2 ) );
    Assert.Equal( NoteName.B, NoteName.C.Add( 6 ) );
    Assert.Equal( NoteName.C, NoteName.C.Add( 7 ) );
    Assert.Equal( NoteName.D, NoteName.C.Add( 8 ) );
  }

  [Fact]
  public void DecrementOperatorTest()
  {
    var noteName = NoteName.C;
    Assert.Equal( NoteName.B, --noteName );
    Assert.Equal( NoteName.A, --noteName );
    Assert.Equal( NoteName.G, --noteName );
    Assert.Equal( NoteName.F, --noteName );
    Assert.Equal( NoteName.E, --noteName );
    Assert.Equal( NoteName.D, --noteName );
    Assert.Equal( NoteName.C, --noteName );
    Assert.Equal( NoteName.B, --noteName );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = NoteName.C;
    object y = new NoteName();
    object z = (NoteName) 0;

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
    object actual = NoteName.C;
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = NoteName.C;
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = NoteName.C;
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = NoteName.C;
    var expected = new NoteName();
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void IncrementOperatorTest()
  {
    var noteName = NoteName.C;
    Assert.Equal( NoteName.D, ++noteName );
    Assert.Equal( NoteName.E, ++noteName );
    Assert.Equal( NoteName.F, ++noteName );
    Assert.Equal( NoteName.G, ++noteName );
    Assert.Equal( NoteName.A, ++noteName );
    Assert.Equal( NoteName.B, ++noteName );
    Assert.Equal( NoteName.C, ++noteName );
  }

  [Fact]
  public void IntegerSubtractionOperatorTest()
  {
    Assert.Equal( NoteName.B, NoteName.B - 0 );
    Assert.Equal( NoteName.A, NoteName.B - 1 );
    Assert.Equal( NoteName.G, NoteName.B - 2 );
    Assert.Equal( NoteName.C, NoteName.B - 6 );
    Assert.Equal( NoteName.B, NoteName.B - 7 );
    Assert.Equal( NoteName.A, NoteName.B - 8 );
    Assert.Equal( NoteName.C, NoteName.D - 1 );
    Assert.Equal( NoteName.B, NoteName.C - 1 );
  }

  [Fact]
  public void NoteNameSubtractionOperatorTest()
  {
    Assert.Equal( 0, NoteName.B - NoteName.B );
    Assert.Equal( 1, NoteName.B - NoteName.A );
    Assert.Equal( 5, NoteName.B - NoteName.D );
    Assert.Equal( 2, NoteName.B - NoteName.G );
    Assert.Equal( 6, NoteName.B - NoteName.C );
    Assert.Equal( 1, NoteName.D - NoteName.C );
    Assert.Equal( 2, NoteName.D - NoteName.B );
    Assert.Equal( 1, NoteName.C - NoteName.B );
  }

  [Fact]
  public void ParseEmptyThrowsTest()
  {
    Assert.Throws<ArgumentException>( () => NoteName.Parse( "" ) );
  }

  [Fact]
  public void ParseInvalidThrowsTest()
  {
    Assert.Throws<FormatException>( () => NoteName.Parse( "Z" ) );
  }

  [Fact]
  public void ParseLowercaseTest()
  {
    Assert.Equal( NoteName.C, NoteName.Parse( "c" ) );
    Assert.Equal( NoteName.D, NoteName.Parse( "d" ) );
    Assert.Equal( NoteName.E, NoteName.Parse( "e" ) );
    Assert.Equal( NoteName.F, NoteName.Parse( "f" ) );
    Assert.Equal( NoteName.G, NoteName.Parse( "g" ) );
    Assert.Equal( NoteName.A, NoteName.Parse( "a" ) );
    Assert.Equal( NoteName.B, NoteName.Parse( "b" ) );
  }

  [Fact]
  public void ParseNullThrowsTest()
  {
    Assert.Throws<ArgumentNullException>( () => NoteName.Parse( null ) );
  }

  [Fact]
  public void ParseUppercaseTest()
  {
    Assert.Equal( NoteName.C, NoteName.Parse( "C" ) );
    Assert.Equal( NoteName.D, NoteName.Parse( "D" ) );
    Assert.Equal( NoteName.E, NoteName.Parse( "E" ) );
    Assert.Equal( NoteName.F, NoteName.Parse( "F" ) );
    Assert.Equal( NoteName.G, NoteName.Parse( "G" ) );
    Assert.Equal( NoteName.A, NoteName.Parse( "A" ) );
    Assert.Equal( NoteName.B, NoteName.Parse( "B" ) );
  }

  [Fact]
  public void SubtractIntegerTest()
  {
    Assert.Equal( NoteName.B, NoteName.B.Subtract( 0 ) );
    Assert.Equal( NoteName.A, NoteName.B.Subtract( 1 ) );
    Assert.Equal( NoteName.G, NoteName.B.Subtract( 2 ) );
    Assert.Equal( NoteName.C, NoteName.B.Subtract( 6 ) );
    Assert.Equal( NoteName.B, NoteName.B.Subtract( 7 ) );
    Assert.Equal( NoteName.A, NoteName.B.Subtract( 8 ) );
  }

  [Fact]
  public void SubtractNoteNameTest()
  {
    Assert.Equal( 0, NoteName.B.Subtract( NoteName.B ) );
    Assert.Equal( 1, NoteName.B.Subtract( NoteName.A ) );
    Assert.Equal( 2, NoteName.B.Subtract( NoteName.G ) );
    Assert.Equal( 6, NoteName.B.Subtract( NoteName.C ) );
  }

  [Fact]
  public void TryParseEmptyFailsTest()
  {
    Assert.False( NoteName.TryParse( "", out var _ ) );
  }

  [Fact]
  public void TryParseNullFailsTest()
  {
    Assert.False( NoteName.TryParse( null, out var _ ) );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = NoteName.C;
    var y = new NoteName();
    var z = (NoteName) 0;

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
    var actual = NoteName.C;

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = NoteName.C;
    Assert.False( actual.Equals( null ) );
  }

#endregion

#pragma warning disable 1718
  [Fact]
  public void RelationalOperatorsTest()
  {
    Assert.True( NoteName.B > NoteName.C );
    Assert.True( NoteName.B >= NoteName.C );
    Assert.True( NoteName.C < NoteName.B );
    Assert.True( NoteName.C <= NoteName.B );

    // ReSharper disable once EqualExpressionComparison
    Assert.True( NoteName.C == NoteName.C );
    Assert.True( NoteName.C != NoteName.B );
  }
}
