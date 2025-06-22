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
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
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
  public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType_ObjectVariant()
  {
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull_ObjectVariant()
  {
    object actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparedWithSameObject_TypeSafeVariant()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
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
  public void Parse_ShouldReturnExpectedCollection_WhenInputIsValid()
  {
    PitchClassCollection.Parse( "C,Db" )
                        .Should()
                        .BeEquivalentTo( [PitchClass.C, PitchClass.DFlat] );
  }

  [Theory]
  [MemberData( nameof( ParseInvalidInputData ) )]
  public void Parse_ShouldThrowExpectedException_WhenInputIsInvalid( string input, Type expectedExceptionType )
  {
    Action act = () => PitchClassCollection.Parse( input );
    act.Should().Throw<Exception>().Where( e => e.GetType() == expectedExceptionType );
  }

  [Fact]
  public void TryParse_ShouldReturnTrueAndCollectionOrFalse_WhenInputIsValidOrInvalid()
  {
    PitchClassCollection.TryParse( "C,Db", out var actual )
                        .Should()
                        .BeTrue();
    actual.Should()
          .BeEquivalentTo( [PitchClass.C, PitchClass.DFlat] );
  }

  [Theory]
  [MemberData( nameof( TryParseInvalidInputData ) )]
  public void TryParse_ShouldReturnFalse_WhenInputIsInvalid( string? input )
  {
    PitchClassCollection.TryParse( input!, out _ )
                        .Should()
                        .BeFalse();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
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
  public void Equals_TypeSafeShouldReturnFalse_WhenComparedWithDifferentType_TypeSafeVariant()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_TypeSafeShouldReturnFalse_WhenComparedWithNull_TypeSafeVariant()
  {
    var actual = new PitchClassCollection( PitchClassCollection.Parse( "C,Db" ) );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<string, Type> ParseInvalidInputData => new()
  {
    { null!, typeof(ArgumentNullException) },
    { "", typeof(ArgumentException) },
    { "C$", typeof(FormatException) }
  };

  public static TheoryData<string?> TryParseInvalidInputData => [(string?) null!, "", "C$"];

  #endregion
}
