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

using System;
using Bach.Model.Instruments;
using Xunit;

namespace Bach.Model.Test.Instruments;

public sealed class FingeringTest
{
#region Public Methods

  [Fact]
  public void CreateTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    Assert.Equal( 6, actual.StringNumber );
    Assert.Equal( 5, actual.Position );
    Assert.Equal( Pitch.Parse( "A2" ), actual.Pitch );
  }

  [Fact]
  public void CreateThrowsWithOutOfRangePositionNumberTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    Assert.Throws<ArgumentOutOfRangeException>( () => Fingering.Create( instrument, 6, -1 ) );

    Assert.Throws<ArgumentOutOfRangeException>( () => Fingering.Create( instrument, 6, 23 ) );
  }

  [Fact]
  public void CreateThrowsWithOutOfRangeStringNumberTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    Assert.Throws<ArgumentOutOfRangeException>( () => Fingering.Create( instrument, 0, 5 ) );
    Assert.Throws<ArgumentOutOfRangeException>( () => Fingering.Create( instrument, 7, 5 ) );
  }

  [Fact]
  public void EqualsContractTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object x = Fingering.Create( instrument, 6, 5 );
    object y = Fingering.Create( instrument, 6, 5 );
    object z = Fingering.Create( instrument, 6, 5 );

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
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object actual = Fingering.Create( instrument, 6, 5 );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    object actual = Fingering.Create( instrument, 6, 5 );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    var expected = Fingering.Create( instrument, 6, 5 );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void ToStringTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    Assert.Equal( "65", Fingering.Create( instrument, 6, 5 ).ToString() );
    Assert.Equal( "612", Fingering.Create( instrument, 6, 12 ).ToString() );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var x = Fingering.Create( instrument, 6, 5 );
    var y = Fingering.Create( instrument, 6, 5 );
    var z = Fingering.Create( instrument, 6, 5 );

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
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var instrument = StringedInstrument.Create( "guitar", 22 );
    var actual = Fingering.Create( instrument, 6, 5 );
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
