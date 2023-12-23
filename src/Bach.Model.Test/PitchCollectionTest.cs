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

namespace Bach.Model.Test;

public sealed class PitchCollectionTest
{
  #region Public Methods

  [Fact]
  public void EqualsContractTest()
  {
    object x = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    object y = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    object z = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

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
    object actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    var expected = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
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
    var expected = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    PitchCollection.Parse( "C4,C5" )
                   .Should()
                   .BeEquivalentTo( expected ); // Using pitches
    PitchCollection.Parse( "60,72" )
                   .Should()
                   .BeEquivalentTo( expected ); // Using midi
    var act1 = () => PitchCollection.Parse( null! );
    act1.Should()
        .Throw<ArgumentNullException>();
    var act2 = () => PitchCollection.Parse( "" );
    act2.Should()
        .Throw<ArgumentException>();
    var act3 = () => PitchCollection.Parse( "C$4,Z5" );
    act3.Should()
        .Throw<FormatException>();
  }

  [Fact]
  public void ToStringTest()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    actual.ToString()
          .Should()
          .Be( "C4,C5" );
  }

  [Fact]
  public void TryParseTest()
  {
    PitchCollection.TryParse( "C4,E4", out var collection )
                   .Should()
                   .BeTrue();
    collection.Should()
              .BeEquivalentTo( new[] { Pitch.Parse( "C4" ), Pitch.Parse( "E4" ) } );
    PitchCollection.TryParse( null!, out collection )
                   .Should()
                   .BeFalse();
    collection.Should()
              .BeNull();
    PitchCollection.TryParse( "", out collection )
                   .Should()
                   .BeFalse();
    collection.Should()
              .BeNull();
    PitchCollection.TryParse( "C$4,Z5", out _ )
                   .Should()
                   .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    var y = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    var z = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

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
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
