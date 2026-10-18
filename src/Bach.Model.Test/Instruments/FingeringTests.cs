// Module Name: FingeringTest.cs
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

namespace Bach.Model.Instruments.Test;

public sealed class FingeringTests
{
  #region Public Methods

  [Fact]
  public void CreateTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var actual = result;

    actual.StringNumber.Should()
          .Be( 6 );

    actual.Position.Should()
          .Be( 5 );

    actual.Pitch.Should()
          .Be( Pitch.Parse( "A2" ) );
  }

  [Fact]
  public void CreateThrowsWithOutOfRangePositionNumberTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var act1 = () =>
    {
      var result = new Fingering( instrument, 6, -1 );
      return result;
    };

    act1.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act2 = () =>
    {
      var result = new Fingering( instrument, 6, 23 );
      return result;
    };

    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CreateThrowsWithOutOfRangeStringNumberTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var act1 = () =>
    {
      var result = new Fingering( instrument, 0, 5 );
      return result;
    };

    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () =>
    {
      var result = new Fingering( instrument, 7, 5 );
      return result;
    };

    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    object x = result;
    var result1 = new Fingering( instrument, 6, 5 );
    object y = result1;
    var result2 = new Fingering( instrument, 6, 5 );
    object z = result2;

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
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    object actual = result;

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    object actual = result;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var actual = result;

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var actual = result;
    var result1 = new Fingering( instrument, 6, 5 );
    var expected = result1;

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void MutedShouldReturnMutedFingering()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = instrument.CreateMutedFingering( 6 );

    result.StringNumber.Should()
          .Be( 6 );

    result.Position.Should()
          .Be( -1 );

    result.ToString()
          .Should()
          .Be( "6x" );
  }

  [Fact]
  public void ToStringTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var result = new Fingering( instrument, 6, 5 );

    result
             .ToString()
             .Should()
             .Be( "65" );

    var result1 = new Fingering( instrument, 6, 12 );

    result1
             .ToString()
             .Should()
             .Be( "612" );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var x = result;
    var result1 = new Fingering( instrument, 6, 5 );
    var y = result1;
    var result2 = new Fingering( instrument, 6, 5 );
    var z = result2;

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
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var actual = result;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );
    var result = new Fingering( instrument, 6, 5 );
    var actual = result;

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
