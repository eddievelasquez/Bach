// Module Name: PitchClassCollectionTest.cs
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

public sealed class PitchClassCollectionTest
{
#region Public Methods

  [Fact]
  public void EqualsContractTest()
  {
    object x = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    object y = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    object z = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

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
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var expected = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void ParseTest()
  {
    Assert.Equal( new[] { PitchClass.C, PitchClass.DFlat }, PitchClassCollection.Parse( "C,Db" ) );
    Assert.Throws<ArgumentNullException>( () => PitchClassCollection.Parse( null ) );
    Assert.Throws<ArgumentException>( () => PitchClassCollection.Parse( "" ) );
    Assert.Throws<FormatException>( () => PitchClassCollection.Parse( "C$" ) );
  }

  [Fact]
  public void TryParseTest()
  {
    Assert.True( PitchClassCollection.TryParse( "C,Db", out var actual ) );
    Assert.Equal( new[] { PitchClass.C, PitchClass.DFlat }, actual );
    Assert.False( PitchClassCollection.TryParse( null, out actual ) );
    Assert.False( PitchClassCollection.TryParse( "", out actual ) );
    Assert.False( PitchClassCollection.TryParse( "C$", out actual ) );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var y = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var z = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

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
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
