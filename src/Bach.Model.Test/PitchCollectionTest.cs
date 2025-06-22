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
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
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
  public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType()
  {
    object actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull()
  {
    object actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparedWithSameObject()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashCode_ShouldReturnSameValue_WhenObjectsAreEqual()
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
  public void Parse_ShouldReturnExpectedValue_WhenInputIsPitchClasses()
  {
    var expected = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    PitchCollection.Parse( "C4,C5" )
                   .Should()
                   .BeEquivalentTo( expected ); // Using pitches
  }

  [Fact]
  public void Parse_ShouldReturnExpectedValue_WhenInputIsMidiNotes()
  {
    var expected = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    PitchCollection.Parse( "60,72" )
                   .Should()
                   .BeEquivalentTo( expected ); // Using midi
  }

  [Theory]
  [MemberData( nameof( UnparsableValues ) )]
  public void Parse_ShouldThrow_WhenInputIsInvalid(
    string input,
    Type expectedExceptionType )
  {
    Action act = () => PitchCollection.Parse( input );

    act.Should()
       .Throw<Exception>()
       .Which.Should()
       .BeOfType( expectedExceptionType );
  }

  [Fact]
  public void ToString_ShouldReturnExpectedValue()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    actual.ToString()
          .Should()
          .Be( "C4,C5" );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
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
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithDifferentType()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithNull()
  {
    var actual = new PitchCollection( [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )] );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<string, Type> UnparsableValues =>
    new()
    {
      { null!, typeof( ArgumentNullException ) },
      { "", typeof( ArgumentException ) },
      { "C$4,Z5", typeof( FormatException ) }
    };

  #endregion
}
