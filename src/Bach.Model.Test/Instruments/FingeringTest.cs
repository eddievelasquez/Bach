// Module Name: FingeringTest.cs
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

namespace Bach.Model.Test.Instruments;

using Model.Instruments;

public sealed class FingeringTest
{
  #region Public Methods

  [Fact]
  public void CreateTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
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
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var act1 = () => Fingering.Create( instrument, 6, -1 );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act2 = () => Fingering.Create( instrument, 6, 23 );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CreateThrowsWithOutOfRangeStringNumberTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var act1 = () => Fingering.Create( instrument, 0, 5 );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () => Fingering.Create( instrument, 7, 5 );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object x = Fingering.Create( instrument, 6, 5 );
    object y = Fingering.Create( instrument, 6, 5 );
    object z = Fingering.Create( instrument, 6, 5 );

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
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object actual = Fingering.Create( instrument, 6, 5 );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object actual = Fingering.Create( instrument, 6, 5 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    var expected = Fingering.Create( instrument, 6, 5 );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void ToStringTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    Fingering.Create( instrument, 6, 5 )
             .ToString()
             .Should()
             .Be( "65" );
    Fingering.Create( instrument, 6, 12 )
             .ToString()
             .Should()
             .Be( "612" );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var x = Fingering.Create( instrument, 6, 5 );
    var y = Fingering.Create( instrument, 6, 5 );
    var z = Fingering.Create( instrument, 6, 5 );

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
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
