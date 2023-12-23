// Module Name: ChordFormulaTest.cs
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

public sealed class ChordFormulaTest
{
  #region Public Methods

  [Fact]
  public void ConstructorWithFormulaTest()
  {
    const string Id = "Id";
    const string Name = "Name";
    const string Symbol = "Symbol";
    const string Formula = "R,M2,M3";
    var actual = new ChordFormula( Id, Name, Symbol, Formula );

    actual.Id.Should()
          .Be( Id );
    actual.Name.Should()
          .Be( Name );
    actual.Symbol.Should()
          .Be( Symbol );
    actual.Intervals.Should()
          .BeEquivalentTo( new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird } );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void ConstructorWithIntervalsTest()
  {
    const string Id = "Id";
    const string Name = "Name";
    const string Symbol = "Symbol";
    var actual = new ChordFormula(
      Id,
      Name,
      Symbol,
      Interval.Unison,
      Interval.MajorSecond,
      Interval.MajorThird
    );

    actual.Id.Should()
          .Be( Id );
    actual.Name.Should()
          .Be( Name );
    actual.Symbol.Should()
          .Be( Symbol );
    actual.Intervals.Should()
          .BeEquivalentTo( new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird } );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    object y = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    object z = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );

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
    object actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GenerateTest()
  {
    var formula = new ChordFormula( "Id", "Test", "Symbol", "R,M2,M3" );
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
  public void GetHashcodeTest()
  {
    var actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    var expected = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void IntervalsMustBeSortedTest()
  {
    var act = () => new ChordFormula( "Id", "Name", "Symbol", "R,M3,M2" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void IntervalsMustHaveNoDuplicatesTest()
  {
    var act = () => new ChordFormula( "Id", "Name", "Symbol", "R,M2,M2,M3" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    var y = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    var z = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );

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
    var actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new ChordFormula( "Id", "Name", "Symbol", "R,M2,M3" );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
