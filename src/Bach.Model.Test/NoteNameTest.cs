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

  [Theory]
  [MemberData( nameof( NoteSteps ) )]
  public void AdditionOperator_ShouldReturnExpectedValue_WhenAddingSteps( NoteName start, int steps, NoteName expected )
  {
    ( start + steps ).Should().Be( expected );
  }


  [Theory]
  [MemberData( nameof( NoteSteps ) )]
  public void Add_ShouldReturnExpectedValue_WhenAddingSteps( NoteName start, int steps, NoteName expected )
  {
    start.Add( steps )
         .Should()
         .Be( expected );
  }

  [Theory]
  [MemberData( nameof( DecrementSteps ) )]
  public void DecrementOperator_ShouldReturnExpectedValue( NoteName start, int times, NoteName expected )
  {
    var noteName = start;
    for( var i = 0; i < times; i++ )
    {
      --noteName;
    }
    noteName.Should().Be( expected );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = NoteName.C;
    object y = new NoteName();
    object z = ( NoteName ) 0;

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
  public void Equals_ShouldReturnFalse_WhenComparingWithDifferentType()
  {
    object actual = NoteName.C;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull_ObjectVariant()
  {
    object actual = NoteName.C;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingWithSameObject()
  {
    var actual = NoteName.C;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashCode_ShouldReturnSameValue_WhenObjectsAreEqual()
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

  [Theory]
  [MemberData( nameof( IncrementSteps ) )]
  public void IncrementOperator_ShouldReturnExpectedValue_WhenIncrementing( NoteName start, int times, NoteName expected )
  {
    var noteName = start;
    for( var i = 0; i < times; i++ )
    {
      ++noteName;
    }
    noteName.Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( NoteSteps ) )]
  public void IntegerSubtractionOperator_ShouldReturnExpectedValue( NoteName start, int steps, NoteName expected )
  {
    ( start - -steps ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( NoteNameSubtractionData ) )]
  public void NoteNameSubtractionOperator_ShouldReturnExpectedValue_WhenSubtractingNoteNames( NoteName left, NoteName right, int expected )
  {
    ( left - right ).Should().Be( expected );
  }

  [Theory]
  [MemberData( nameof( InvalidParseData ) )]
  public void Parse_ShouldThrowArgumentException_WhenInputIsInvalid( string? input, Type expectedExceptionType )
  {
    var act = () => PitchClassCollection.Parse( input! );
    act.Should().Throw<Exception>().Where( e => e.GetType() == expectedExceptionType );
  }

  [Theory]
  [MemberData( nameof( NoteNames ) )]
  public void Parse_ShouldReturnExpectedValue_WhenInputIsValid( string input, NoteName expected )
  {
    NoteName.Parse( input )
            .Should()
            .Be( expected );
  }

  [Theory]
  [MemberData( nameof( SubtractStepsData ) )]
  public void Subtract_ShouldReturnExpectedValue_WhenSubtractingSteps( NoteName start, int steps, NoteName expected )
  {
    start.Subtract( steps )
         .Should()
         .Be( expected );
  }

  [Theory]
  [MemberData( nameof( NoteNameSubtractionData ) )]
  public void Subtract_ShouldReturnExpectedValue_WhenSubtractingNoteName( NoteName left, NoteName right, int expected )
  {
    left.Subtract( right )
        .Should()
        .Be( expected );
  }

  [Theory]
  [InlineData( null )]
  [InlineData( "" )]
  public void TryParse_ShouldReturnFalse_WhenInputIsEmpty( string? input )
  {
    NoteName.TryParse( input!, out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = NoteName.C;
    var y = new NoteName();
    var z = ( NoteName ) 0;

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
    x.Equals( (NoteName?)null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull_TypeSafeVariant()
  {
    var actual = NoteName.C;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

#pragma warning disable 1718
  [Fact]
  public void RelationalOperators_ShouldSatisfyOrdering()
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
  public static TheoryData<NoteName, int, NoteName> NoteSteps =>
    new()
    {
      { NoteName.C, 0, NoteName.C },
      { NoteName.C, 1, NoteName.D },
      { NoteName.C, 2, NoteName.E },
      { NoteName.C, 3, NoteName.F },
      { NoteName.C, 4, NoteName.G },
      { NoteName.C, 5, NoteName.A },
      { NoteName.C, 6, NoteName.B },
      { NoteName.C, 7, NoteName.C },
      { NoteName.C, 8, NoteName.D },
      { NoteName.B, 0, NoteName.B },
      { NoteName.B, 1, NoteName.C },
      { NoteName.B, 2, NoteName.D },
      { NoteName.B, 3, NoteName.E },
      { NoteName.B, 4, NoteName.F },
      { NoteName.B, 5, NoteName.G },
      { NoteName.B, 6, NoteName.A },
      { NoteName.B, 7, NoteName.B },
      { NoteName.B, 8, NoteName.C },
      { NoteName.D, 0, NoteName.D },
      { NoteName.D, 1, NoteName.E },
      { NoteName.D, 2, NoteName.F },
      { NoteName.D, 3, NoteName.G },
      { NoteName.D, 4, NoteName.A },
      { NoteName.D, 5, NoteName.B },
      { NoteName.D, 6, NoteName.C },
      { NoteName.D, 7, NoteName.D },
      { NoteName.D, 8, NoteName.E },
      { NoteName.E, 0, NoteName.E },
      { NoteName.E, 1, NoteName.F },
      { NoteName.E, 2, NoteName.G },
      { NoteName.E, 3, NoteName.A },
      { NoteName.E, 4, NoteName.B },
      { NoteName.E, 5, NoteName.C },
      { NoteName.E, 6, NoteName.D },
      { NoteName.E, 7, NoteName.E },
      { NoteName.E, 8, NoteName.F },
      { NoteName.F, 0, NoteName.F },
      { NoteName.F, 1, NoteName.G },
      { NoteName.F, 2, NoteName.A },
      { NoteName.F, 3, NoteName.B },
      { NoteName.F, 4, NoteName.C },
      { NoteName.F, 5, NoteName.D },
      { NoteName.F, 6, NoteName.E },
      { NoteName.F, 7, NoteName.F },
      { NoteName.F, 8, NoteName.G },
      { NoteName.G, 0, NoteName.G },
      { NoteName.G, 1, NoteName.A },
      { NoteName.G, 2, NoteName.B },
      { NoteName.G, 3, NoteName.C },
      { NoteName.G, 4, NoteName.D },
      { NoteName.G, 5, NoteName.E },
      { NoteName.G, 6, NoteName.F },
      { NoteName.G, 7, NoteName.G },
      { NoteName.G, 8, NoteName.A }
    };

  public static TheoryData<NoteName, int, NoteName> DecrementSteps =>
    new()
    {
      { NoteName.C, 1, NoteName.B },
      { NoteName.C, 2, NoteName.A },
      { NoteName.C, 3, NoteName.G },
      { NoteName.C, 4, NoteName.F },
      { NoteName.C, 5, NoteName.E },
      { NoteName.C, 6, NoteName.D },
      { NoteName.C, 7, NoteName.C },
      { NoteName.C, 8, NoteName.B }
    };

  public static TheoryData<NoteName, int, NoteName> IncrementSteps =>
    new()
    {
      { NoteName.C, 1, NoteName.D },
      { NoteName.C, 2, NoteName.E },
      { NoteName.C, 3, NoteName.F },
      { NoteName.C, 4, NoteName.G },
      { NoteName.C, 5, NoteName.A },
      { NoteName.C, 6, NoteName.B },
      { NoteName.C, 7, NoteName.C }
    };

  public static TheoryData<NoteName, NoteName, int> NoteNameSubtractionData =>
    new()
    {
      { NoteName.B, NoteName.B, 0 },
      { NoteName.B, NoteName.A, 1 },
      { NoteName.B, NoteName.D, 5 },
      { NoteName.B, NoteName.G, 2 },
      { NoteName.B, NoteName.C, 6 },
      { NoteName.D, NoteName.C, 1 },
      { NoteName.D, NoteName.B, 2 },
      { NoteName.C, NoteName.B, 1 },
      { NoteName.C, NoteName.C, 0 },
      { NoteName.C, NoteName.D, 6 },
      { NoteName.C, NoteName.E, 5 },
      { NoteName.C, NoteName.F, 4 },
      { NoteName.C, NoteName.G, 3 },
      { NoteName.C, NoteName.A, 2 },
      { NoteName.D, NoteName.D, 0 },
      { NoteName.D, NoteName.E, 6 },
      { NoteName.D, NoteName.F, 5 },
      { NoteName.D, NoteName.G, 4 },
      { NoteName.D, NoteName.A, 3 },
      { NoteName.D, NoteName.B, 2 },
      { NoteName.E, NoteName.C, 2 },
      { NoteName.E, NoteName.D, 1 },
      { NoteName.E, NoteName.E, 0 },
      { NoteName.E, NoteName.F, 6 },
      { NoteName.E, NoteName.G, 5 },
      { NoteName.E, NoteName.A, 4 },
      { NoteName.E, NoteName.B, 3 },
      { NoteName.F, NoteName.C, 3 },
      { NoteName.F, NoteName.D, 2 },
      { NoteName.F, NoteName.E, 1 },
      { NoteName.F, NoteName.F, 0 },
      { NoteName.F, NoteName.G, 6 },
      { NoteName.F, NoteName.A, 5 },
      { NoteName.F, NoteName.B, 4 },
      { NoteName.G, NoteName.C, 4 },
      { NoteName.G, NoteName.D, 3 },
      { NoteName.G, NoteName.E, 2 },
      { NoteName.G, NoteName.F, 1 },
      { NoteName.G, NoteName.G, 0 },
      { NoteName.G, NoteName.A, 6 },
      { NoteName.G, NoteName.B, 5 },
      { NoteName.A, NoteName.C, 5 },
      { NoteName.A, NoteName.D, 4 },
      { NoteName.A, NoteName.E, 3 },
      { NoteName.A, NoteName.F, 2 },
      { NoteName.A, NoteName.G, 1 },
      { NoteName.A, NoteName.A, 0 },
      { NoteName.A, NoteName.B, 6 },
      { NoteName.B, NoteName.E, 4 },
      { NoteName.B, NoteName.F, 3 },
      { NoteName.B, NoteName.A, 1 }
    };

  public static TheoryData<string, NoteName> NoteNames =>
    new()
    {
      { "c", NoteName.C },
      { "d", NoteName.D },
      { "e", NoteName.E },
      { "f", NoteName.F },
      { "g", NoteName.G },
      { "a", NoteName.A },
      { "b", NoteName.B },
      { "C", NoteName.C },
      { "D", NoteName.D },
      { "E", NoteName.E },
      { "F", NoteName.F },
      { "G", NoteName.G },
      { "A", NoteName.A },
      { "B", NoteName.B }
    };

  public static TheoryData<NoteName, int, NoteName> SubtractStepsData =>
    new()
    {
      { NoteName.B, 0, NoteName.B },
      { NoteName.B, 1, NoteName.A },
      { NoteName.B, 2, NoteName.G },
      { NoteName.B, 6, NoteName.C },
      { NoteName.B, 7, NoteName.B },
      { NoteName.B, 8, NoteName.A }
    };

  public static TheoryData<string?, Type> InvalidParseData =>
    new()
    {
      { null, typeof(ArgumentNullException) },
      { "", typeof(ArgumentException) },
      { "Z", typeof(FormatException) }
    };
}
