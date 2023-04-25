// Module Name: PitchCollectionTest.cs
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

public sealed class PitchCollectionTest
{
#region Public Methods

  [Fact]
  public void EqualsContractTest()
  {
    object x = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    object y = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    object z = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );

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
    object actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    var expected = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void ParseTest()
  {
    var expected = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.Equal( expected, PitchCollection.Parse( "C4,C5" ) ); // Using pitches
    Assert.Equal( expected, PitchCollection.Parse( "60,72" ) ); // Using midi
    Assert.Throws<ArgumentNullException>( () => PitchCollection.Parse( null ) );
    Assert.Throws<ArgumentException>( () => PitchCollection.Parse( "" ) );
    Assert.Throws<FormatException>( () => PitchCollection.Parse( "C$4,Z5" ) );
  }

  [Fact]
  public void ToStringTest()
  {
    var actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.Equal( "C4,C5", actual.ToString() );
  }

  [Fact]
  public void TryParseTest()
  {
    Assert.True( PitchCollection.TryParse( "C4,E4", out var collection ) );
    Assert.Equal( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "E4" ) }, collection );
    Assert.False( PitchCollection.TryParse( null, out collection ) );
    Assert.Null( collection );
    Assert.False( PitchCollection.TryParse( "", out collection ) );
    Assert.Null( collection );
    Assert.False( PitchCollection.TryParse( "C$4,Z5", out collection ) );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    var y = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    var z = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );

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
    var actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new PitchCollection( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "C5" ) } );
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
