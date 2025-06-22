// Module Name: ChordFormulaTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2025  Eddie Velasquez.
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

public sealed class ChordFormulaTest
{
  private const string CHORD_ID = "Id";
  private const string CHORD_NAME = "Name";
  private const string CHORD_SYMBOL = "Symbol";
  private const string CHORD_FORMULA = "R,M2,M3";

  #region Public Methods

  [Fact]
  public void Constructor_WithFormula_ShouldSucceed()
  {
    var actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    actual.Id.Should()
          .Be( CHORD_ID );

    actual.Name.Should()
          .Be( CHORD_NAME );

    actual.Symbol.Should()
          .Be( CHORD_SYMBOL );

    actual.Intervals.Should()
          .BeEquivalentTo( [Interval.Unison, Interval.MajorSecond, Interval.MajorThird] );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void Constructor_WithIntervals_ShouldSucceed()
  {
    var actual = new ChordFormula(
      CHORD_ID,
      CHORD_NAME,
      CHORD_SYMBOL,
      Interval.Unison,
      Interval.MajorSecond,
      Interval.MajorThird
    );

    actual.Id.Should()
          .Be( CHORD_ID );

    actual.Name.Should()
          .Be( CHORD_NAME );

    actual.Symbol.Should()
          .Be( CHORD_SYMBOL );

    actual.Intervals.Should()
          .BeEquivalentTo( [Interval.Unison, Interval.MajorSecond, Interval.MajorThird] );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation()
  {
    object x = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );
    object y = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );
    object z = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

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
  public void Equals_ShouldReturnFalse_WhenComparingObjectOfDifferentType()
  {
    object actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingToNull()
  {
    object actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingTheSameObject()
  {
    var actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Generate_ShouldSucceed()
  {
    var formula = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    using var pitches = formula.Generate( Pitch.MinValue )
                               .GetEnumerator();
    var count = 0;

    while( pitches.MoveNext() )
    {
      ( pitches.Current <= Pitch.MaxValue ).Should()
                                           .BeTrue();
      ++count;
    }

    // 3 pitchClasses per octave, 10 octaves total.
    count.Should()
         .Be( 30 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnTheSameValue_WhenHashingEquivalentObjects()
  {
    var actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );
    var expected = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenIntervalsAreNotSorted()
  {
    var act = () => new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, "R,M3,M2" );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenIntervalsHaveDuplicates()
  {
    var act = () => new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, "R,M2,M2,M3" );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldSatisfyEquivalenceRelation()
  {
    var x = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );
    var y = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );
    var z = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

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
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingObjectOfDifferentType()
  {
    var actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void StronglyTypedEquals_ShouldReturnFalse_WhenComparingToNull()
  {
    var actual = new ChordFormula( CHORD_ID, CHORD_NAME, CHORD_SYMBOL, CHORD_FORMULA );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
