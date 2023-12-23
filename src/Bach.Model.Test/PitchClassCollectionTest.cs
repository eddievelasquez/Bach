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
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var expected = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void ParseTest()
  {
    PitchClassCollection.Parse( "C,Db" )
                        .Should()
                        .BeEquivalentTo( new[] { PitchClass.C, PitchClass.DFlat } );
    var act1 = () => PitchClassCollection.Parse( null! );
    act1.Should()
        .Throw<ArgumentNullException>();
    var act2 = () => PitchClassCollection.Parse( "" );
    act2.Should()
        .Throw<ArgumentException>();
    var act3 = () => PitchClassCollection.Parse( "C$" );
    act3.Should()
        .Throw<FormatException>();
  }

  [Fact]
  public void TryParseTest()
  {
    PitchClassCollection.TryParse( "C,Db", out var actual )
                        .Should()
                        .BeTrue();
    actual.Should()
          .BeEquivalentTo( new[] { PitchClass.C, PitchClass.DFlat } );
    PitchClassCollection.TryParse( null!, out _ )
                        .Should()
                        .BeFalse();
    PitchClassCollection.TryParse( "", out _ )
                        .Should()
                        .BeFalse();
    PitchClassCollection.TryParse( "C$", out _ )
                        .Should()
                        .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var y = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    var z = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

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
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
