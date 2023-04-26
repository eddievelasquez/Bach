// Module Name: ScaleFormulaTest.cs
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
using System.Linq;
using Xunit;

namespace Bach.Model.Test;

public sealed class ScaleFormulaTest
{
#region Public Methods

  [Fact]
  public void CategoriesHeptatonicTest()
  {
    TestFormulaCategory( "Heptatonic", true, formula => formula.Intervals.Count == 7 );
  }

  [Fact]
  public void CategoriesHexatonicTest()
  {
    TestFormulaCategory( "Hexatonic", true, formula => formula.Intervals.Count == 6 );
  }

  [Fact]
  public void CategoriesMajorTest()
  {
    TestFormulaCategory( "Major",
                         false,
                         formula => formula.Intervals.Contains( Interval.MajorThird )
                                    && formula.Intervals.Contains( Interval.Fifth ) );
  }

  [Fact]
  public void CategoriesMinorTest()
  {
    TestFormulaCategory( "Minor",
                         false,
                         formula => formula.Intervals.Contains( Interval.MinorThird )
                                    && formula.Intervals.Contains( Interval.Fifth ) );
  }

  [Fact]
  public void CategoriesOctatonicTest()
  {
    TestFormulaCategory( "Octatonic", true, formula => formula.Intervals.Count == 8 );
  }

  [Fact]
  public void CategoriesPentatonicTest()
  {
    TestFormulaCategory( "Pentatonic", true, formula => formula.Intervals.Count == 5 );
  }

  [Fact]
  public void ConstructorWithFormulaTest()
  {
    const string Id = "Id";
    const string Name = "Name";
    const string Formula = "R,M2,M3";
    var actual = new ScaleFormulaBuilder( Name ).SetId( Id ).SetIntervals( Formula ).Build();

    Assert.Equal( Id, actual.Id );
    Assert.Equal( Name, actual.Name );
    Assert.Equal( new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird }, actual.Intervals );

    Assert.Equal( "Name: 1,2,3", actual.ToString() );
  }

  [Fact]
  public void ConstructorWithIntervalsTest()
  {
    const string Id = "Id";
    const string Name = "Name";

    var actual = new ScaleFormulaBuilder( Name ).SetId( Id )
                                                .SetIntervals( new[]
                                                {
                                                  Interval.Unison,
                                                  Interval.MajorSecond,
                                                  Interval.MajorThird
                                                } )
                                                .Build();

    Assert.Equal( Id, actual.Id );
    Assert.Equal( Name, actual.Name );
    Assert.Equal( new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird }, actual.Intervals );

    Assert.Equal( "Name: 1,2,3", actual.ToString() );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    object y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    object z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();

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
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GenerateTest()
  {
    var formula = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    using var pitches = formula.Generate( Pitch.MinValue ).GetEnumerator();
    var count = 0;

    while( pitches.MoveNext() )
    {
      Assert.True( pitches.Current <= Pitch.MaxValue );
      ++count;
    }

    // 3 pitchClasses per octave, 10 octaves total.
    Assert.Equal( 30, count );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    var expected = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void GetStepsTest()
  {
    TestGetSteps( "Major", 2, 2, 1, 2, 2, 2, 1 );
    TestGetSteps( "NaturalMinor", 2, 1, 2, 2, 1, 2, 2 );
    TestGetSteps( "MelodicMinor", 2, 1, 2, 2, 2, 2, 1 );
    TestGetSteps( "HarmonicMinor", 2, 1, 2, 2, 1, 3, 1 );
    TestGetSteps( "Diminished", 2, 1, 2, 1, 2, 1, 2, 1 );
    TestGetSteps( "WholeTone", 2, 2, 2, 2, 2, 2 );
    TestGetSteps( "MinorBlues", 3, 2, 1, 1, 3, 2 );
    TestGetSteps( "MinorPentatonic", 3, 2, 2, 3, 2 );
    TestGetSteps( "Pentatonic", 2, 2, 3, 2, 3 );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    var y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    var z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();

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
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" ).SetIntervals( "R,M2,M3" ).Build();
    Assert.False( actual.Equals( null ) );
  }

#endregion

#region Implementation

  private static void TestFormulaCategory(
    string category,
    bool strict,
    Predicate<ScaleFormula> predicate )
  {
    foreach( var formula in Registry.ScaleFormulas )
    {
      if( predicate( formula ) )
      {
        Assert.Contains( category, formula.Categories );
      }
      else if( strict )
      {
        Assert.DoesNotContain( category, formula.Categories );
      }
    }
  }

  private static void TestGetSteps(
    string scaleName,
    params int[] expected )
  {
    var scale = Registry.ScaleFormulas[scaleName];
    Assert.Equal( expected, scale.GetRelativeSteps() );
  }

#endregion
}
