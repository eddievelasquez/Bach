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

namespace Bach.Model.Test;

public sealed class NoteNameTest
{
  #region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    ( NoteName.C + 0 ).Should()
                      .Be( NoteName.C );
    ( NoteName.C + 1 ).Should()
                      .Be( NoteName.D );
    ( NoteName.C + 2 ).Should()
                      .Be( NoteName.E );
    ( NoteName.C + 6 ).Should()
                      .Be( NoteName.B );
    ( NoteName.C + 7 ).Should()
                      .Be( NoteName.C );
    ( NoteName.C + 8 ).Should()
                      .Be( NoteName.D );
    ( NoteName.B + 1 ).Should()
                      .Be( NoteName.C );
  }

  [Fact]
  public void AddTest()
  {
    NoteName.C.Add( 0 )
            .Should()
            .Be( NoteName.C );
    NoteName.C.Add( 1 )
            .Should()
            .Be( NoteName.D );
    NoteName.C.Add( 2 )
            .Should()
            .Be( NoteName.E );
    NoteName.C.Add( 6 )
            .Should()
            .Be( NoteName.B );
    NoteName.C.Add( 7 )
            .Should()
            .Be( NoteName.C );
    NoteName.C.Add( 8 )
            .Should()
            .Be( NoteName.D );
  }

  [Fact]
  public void DecrementOperatorTest()
  {
    var noteName = NoteName.C;
    ( --noteName ).Should()
                  .Be( NoteName.B );
    ( --noteName ).Should()
                  .Be( NoteName.A );
    ( --noteName ).Should()
                  .Be( NoteName.G );
    ( --noteName ).Should()
                  .Be( NoteName.F );
    ( --noteName ).Should()
                  .Be( NoteName.E );
    ( --noteName ).Should()
                  .Be( NoteName.D );
    ( --noteName ).Should()
                  .Be( NoteName.C );
    ( --noteName ).Should()
                  .Be( NoteName.B );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = NoteName.C;
    object y = new NoteName();
    object z = (NoteName) 0;

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
    object actual = NoteName.C;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = NoteName.C;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = NoteName.C;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = NoteName.C;
    var expected = new NoteName();
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
    var noteName = NoteName.C;
    ( ++noteName ).Should()
                  .Be( NoteName.D );
    ( ++noteName ).Should()
                  .Be( NoteName.E );
    ( ++noteName ).Should()
                  .Be( NoteName.F );
    ( ++noteName ).Should()
                  .Be( NoteName.G );
    ( ++noteName ).Should()
                  .Be( NoteName.A );
    ( ++noteName ).Should()
                  .Be( NoteName.B );
    ( ++noteName ).Should()
                  .Be( NoteName.C );
  }

  [Fact]
  public void IntegerSubtractionOperatorTest()
  {
    ( NoteName.B - 0 ).Should()
                      .Be( NoteName.B );
    ( NoteName.B - 1 ).Should()
                      .Be( NoteName.A );
    ( NoteName.B - 2 ).Should()
                      .Be( NoteName.G );
    ( NoteName.B - 6 ).Should()
                      .Be( NoteName.C );
    ( NoteName.B - 7 ).Should()
                      .Be( NoteName.B );
    ( NoteName.B - 8 ).Should()
                      .Be( NoteName.A );
    ( NoteName.D - 1 ).Should()
                      .Be( NoteName.C );
    ( NoteName.C - 1 ).Should()
                      .Be( NoteName.B );
  }

  [Fact]
  public void NoteNameSubtractionOperatorTest()
  {
    ( NoteName.B - NoteName.B ).Should()
                               .Be( 0 );
    ( NoteName.B - NoteName.A ).Should()
                               .Be( 1 );
    ( NoteName.B - NoteName.D ).Should()
                               .Be( 5 );
    ( NoteName.B - NoteName.G ).Should()
                               .Be( 2 );
    ( NoteName.B - NoteName.C ).Should()
                               .Be( 6 );
    ( NoteName.D - NoteName.C ).Should()
                               .Be( 1 );
    ( NoteName.D - NoteName.B ).Should()
                               .Be( 2 );
    ( NoteName.C - NoteName.B ).Should()
                               .Be( 1 );
  }

  [Fact]
  public void ParseEmptyThrowsTest()
  {
    var act = () => NoteName.Parse( "" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void ParseInvalidThrowsTest()
  {
    var act = () => NoteName.Parse( "Z" );
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void ParseLowercaseTest()
  {
    NoteName.Parse( "c" )
            .Should()
            .Be( NoteName.C );
    NoteName.Parse( "d" )
            .Should()
            .Be( NoteName.D );
    NoteName.Parse( "e" )
            .Should()
            .Be( NoteName.E );
    NoteName.Parse( "f" )
            .Should()
            .Be( NoteName.F );
    NoteName.Parse( "g" )
            .Should()
            .Be( NoteName.G );
    NoteName.Parse( "a" )
            .Should()
            .Be( NoteName.A );
    NoteName.Parse( "b" )
            .Should()
            .Be( NoteName.B );
  }

  [Fact]
  public void ParseNullThrowsTest()
  {
    var act = () => NoteName.Parse( null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ParseUppercaseTest()
  {
    NoteName.Parse( "C" )
            .Should()
            .Be( NoteName.C );
    NoteName.Parse( "D" )
            .Should()
            .Be( NoteName.D );
    NoteName.Parse( "E" )
            .Should()
            .Be( NoteName.E );
    NoteName.Parse( "F" )
            .Should()
            .Be( NoteName.F );
    NoteName.Parse( "G" )
            .Should()
            .Be( NoteName.G );
    NoteName.Parse( "A" )
            .Should()
            .Be( NoteName.A );
    NoteName.Parse( "B" )
            .Should()
            .Be( NoteName.B );
  }

  [Fact]
  public void SubtractIntegerTest()
  {
    NoteName.B.Subtract( 0 )
            .Should()
            .Be( NoteName.B );
    NoteName.B.Subtract( 1 )
            .Should()
            .Be( NoteName.A );
    NoteName.B.Subtract( 2 )
            .Should()
            .Be( NoteName.G );
    NoteName.B.Subtract( 6 )
            .Should()
            .Be( NoteName.C );
    NoteName.B.Subtract( 7 )
            .Should()
            .Be( NoteName.B );
    NoteName.B.Subtract( 8 )
            .Should()
            .Be( NoteName.A );
  }

  [Fact]
  public void SubtractNoteNameTest()
  {
    NoteName.B.Subtract( NoteName.B )
            .Should()
            .Be( 0 );
    NoteName.B.Subtract( NoteName.A )
            .Should()
            .Be( 1 );
    NoteName.B.Subtract( NoteName.G )
            .Should()
            .Be( 2 );
    NoteName.B.Subtract( NoteName.C )
            .Should()
            .Be( 6 );
  }

  [Fact]
  public void TryParseEmptyFailsTest()
  {
    NoteName.TryParse( "", out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TryParseNullFailsTest()
  {
    NoteName.TryParse( null!, out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = NoteName.C;
    var y = new NoteName();
    var z = (NoteName) 0;

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
    var actual = NoteName.C;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = NoteName.C;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

#pragma warning disable 1718
  [Fact]
  public void RelationalOperatorsTest()
  {
    ( NoteName.B > NoteName.C ).Should()
                               .BeTrue();
    ( NoteName.B >= NoteName.C ).Should()
                                .BeTrue();
    ( NoteName.C < NoteName.B ).Should()
                               .BeTrue();
    ( NoteName.C <= NoteName.B ).Should()
                                .BeTrue();

    // ReSharper disable once EqualExpressionComparison
    ( NoteName.C == NoteName.C ).Should()
                                .BeTrue();
    ( NoteName.C != NoteName.B ).Should()
                                .BeTrue();
  }
}
