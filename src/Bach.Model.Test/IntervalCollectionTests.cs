// Module Name: IntervalCollectionTest.cs
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

using System.Collections.Generic;

namespace Bach.Model.Test;

public sealed class IntervalCollectionTests
{
  #region Public Methods

  [Fact]
  public void Constructor_ShouldCreateCollection_WhenGivenIEnumerable()
  {
    var intervals = new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird };
    var collection = new IntervalCollection( intervals );

    collection.Count.Should()
              .Be( 3 );

    collection.Should()
              .ContainInOrder( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
  }

  [Fact]
  public void Constructor_ShouldCreateCollection_WhenGivenParamsArray()
  {
    var collection = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

    collection.Count.Should()
              .Be( 3 );

    collection.Should()
              .ContainInOrder( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenIntervalsAreNotSorted()
  {
    var act = () => new IntervalCollection( Interval.MajorThird, Interval.Unison, Interval.MajorSecond );

    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( "Intervals must be sorted in ascending order and contain no duplicates" );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenIntervalsContainDuplicates()
  {
    var act = () => new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.Unison );

    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( "Intervals must be sorted in ascending order and contain no duplicates" );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenIntervalsIsNull()
  {
    var act = () => new IntervalCollection( (IEnumerable<Interval>) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenIntervalsIsEmpty()
  {
    var act = () => new IntervalCollection();

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType()
  {
    object actual = new IntervalCollection( Interval.Unison, Interval.MajorSecond );

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull_ObjectVariant()
  {
    object actual = new IntervalCollection( Interval.Unison, Interval.MajorSecond );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull_TypeSafeVariant()
  {
    var actual = new IntervalCollection( Interval.Unison, Interval.MajorSecond );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparedWithSameObject()
  {
    var actual = new IntervalCollection( Interval.Unison, Interval.MajorSecond );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
    object y = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
    object z = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

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
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
    var y = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
    var z = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

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
  public void GetHashCode_ShouldReturnSameValue_WhenObjectsAreEqual()
  {
    var actual = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
    var expected = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void IndexOf_ShouldReturnCorrectIndex_WhenIntervalExists()
  {
    var collection = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

    collection.IndexOf( Interval.MajorSecond )
              .Should()
              .Be( 1 );
  }

  [Fact]
  public void IndexOf_ShouldReturnNegativeIndex_WhenIntervalDoesNotExist()
  {
    var collection = new IntervalCollection( Interval.Unison, Interval.MajorSecond );

    collection.IndexOf( Interval.Fifth )
              .Should()
              .BeLessThan( 0 );
  }

  [Fact]
  public void Indexer_ShouldReturnCorrectInterval_WhenIndexIsValid()
  {
    var collection = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

    collection[0]
      .Should()
      .Be( Interval.Unison );

    collection[1]
      .Should()
      .Be( Interval.MajorSecond );

    collection[2]
      .Should()
      .Be( Interval.MajorThird );
  }

  [Fact]
  public void ToString_ShouldReturnExpectedFormat()
  {
    var collection = new IntervalCollection( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );

    collection.ToString()
              .Should()
              .Be( "1,2,3" );
  }

  #endregion
}
