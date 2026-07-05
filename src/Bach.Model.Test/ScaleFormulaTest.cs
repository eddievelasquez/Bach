// Module Name: ScaleFormulaTest.cs
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

namespace Bach.Model.Test;

using System.Linq;

public sealed class ScaleFormulaTest
{
  #region Properties

  public static TheoryData<string, int[]> ScaleStepsData => new()
  {
    { "Major", [2, 2, 1, 2, 2, 2, 1] },
    { "NaturalMinor", [2, 1, 2, 2, 1, 2, 2] },
    { "MelodicMinor", [2, 1, 2, 2, 2, 2, 1] },
    { "HarmonicMinor", [2, 1, 2, 2, 1, 3, 1] },
    { "Diminished", [2, 1, 2, 1, 2, 1, 2, 1] },
    { "WholeTone", [2, 2, 2, 2, 2, 2] },
    { "MinorBlues", [3, 2, 1, 1, 3, 2] },
    { "MinorPentatonic", [3, 2, 2, 3, 2] },
    { "Pentatonic", [2, 2, 3, 2, 3] }
  };

  public static TheoryData<string, int> CategoryByIntervalCountData => new()
  {
    { "Pentatonic", 5 },
    { "Hexatonic", 6 },
    { "Heptatonic", 7 },
    { "Octatonic", 8 }
  };

  public static TheoryData<string, Interval[]> CategoryByIntervalsData => new()
  {
    { "Major", [Interval.MajorThird, Interval.Fifth] },
    { "Minor", [Interval.MinorThird, Interval.Fifth] }
  };

  #endregion

  #region Public Methods

  [Fact]
  public void AppendInterval_ShouldAddIntervalToFormula_WhenIntervalIsValid()
  {
    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .AppendInterval( Interval.Unison )
                 .AppendInterval( Interval.MajorSecond )
                 .AppendInterval( Interval.MajorThird )
                 .Build();

    actual.Intervals.Should()
          .ContainInOrder( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithAliases_WhenAliasesAreAdded()
  {
    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddAlias( "Alternate Name" )
                 .Build();

    actual.Aliases.Should()
          .Contain( "Alternate Name" );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithAliases_WhenAliasesCollectionIsAdded()
  {
    var aliases = new[] { "Alias1", "Alias2", "Alias3" };

    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddAliases( aliases )
                 .Build();

    actual.Aliases.Should()
          .Contain( "Alias1" )
          .And.Contain( "Alias2" )
          .And.Contain( "Alias3" );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithCategories_WhenCategoriesAreAdded()
  {
    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddCategory( "Custom" )
                 .Build();

    actual.Categories.Should()
          .Contain( "Custom" );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithCategories_WhenCategoriesCollectionIsAdded()
  {
    var categories = new[] { "Cat1", "Cat2", "Cat3" };

    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddCategories( categories )
                 .Build();

    actual.Categories.Should()
          .Contain( "Cat1" )
          .And.Contain( "Cat2" )
          .And.Contain( "Cat3" );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithMultipleAliases_WhenAliasesAreSemicolonDelimited()
  {
    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddAlias( "Alias1;Alias2;Alias3" )
                 .Build();

    actual.Aliases.Should()
          .Contain( "Alias1" )
          .And.Contain( "Alias2" )
          .And.Contain( "Alias3" );
  }

  [Fact]
  public void Build_ShouldCreateScaleFormulaWithMultipleCategories_WhenCategoriesAreSemicolonDelimited()
  {
    var actual = new ScaleFormulaBuilder( "Test Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .AddCategory( "Custom1;Custom2;Custom3" )
                 .Build();

    actual.Categories.Should()
          .Contain( "Custom1" )
          .And.Contain( "Custom2" )
          .And.Contain( "Custom3" );
  }

  [Fact]
  public void Build_ShouldGenerateDefaultId_WhenIdNotProvided()
  {
    var actual = new ScaleFormulaBuilder( "My Custom Scale" )
                 .SetIntervals( "R,M2,M3" )
                 .Build();

    actual.Id.Should()
          .Be( "MyCustomScale" );
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenIntervalCountIsLessThanTwo()
  {
    var builder = new ScaleFormulaBuilder( "Test" ).AppendInterval( Interval.Unison );

    var act = () => builder.Build();

    act.Should()
       .Throw<InvalidOperationException>()
       .WithMessage( "A scale must contain at least two intervals" );
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenNameNotProvided()
  {
    var builder = new ScaleFormulaBuilder().SetIntervals( "R,M2,M3" );

    var act = () => builder.Build();

    act.Should()
       .Throw<InvalidOperationException>()
       .WithMessage( "Must provide a scale name" );
  }

  [Theory]
  [MemberData( nameof( CategoryByIntervalsData ) )]
  public void Categories_ShouldContainCategory_WhenFormulaContainsIntervals(
    string category,
    Interval[] requiredIntervals )
  {
    var matchingFormulas = Registry.ScaleFormulas
                                   .Where( formula => requiredIntervals.All( i => formula.Intervals.Contains( i ) ) );

    foreach( var formula in matchingFormulas )
    {
      formula.Categories.Should()
             .Contain( category );
    }
  }

  [Theory]
  [MemberData( nameof( CategoryByIntervalCountData ) )]
  public void Categories_ShouldContainCategory_WhenFormulaHasIntervalCount(
    string category,
    int intervalCount )
  {
    var matchingFormulas = Registry.ScaleFormulas
                                   .Where( formula => formula.Intervals.Count == intervalCount );

    foreach( var formula in matchingFormulas )
    {
      formula.Categories.Should()
             .Contain( category );
    }
  }

  [Fact]
  public void Constructor_ShouldCreateScaleFormula_WhenGivenIntervalArray()
  {
    const string Id = "Id";
    const string Name = "Name";

    var actual = new ScaleFormulaBuilder( Name ).SetId( Id )
                                                .SetIntervals( [Interval.Unison, Interval.MajorSecond, Interval.MajorThird] )
                                                .Build();

    actual.Id.Should()
          .Be( Id );

    actual.Name.Should()
          .Be( Name );

    actual.Intervals.Should()
          .BeEquivalentTo( [Interval.Unison, Interval.MajorSecond, Interval.MajorThird] );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void Constructor_ShouldCreateScaleFormula_WhenGivenIntervalString()
  {
    const string Id = "Id";
    const string Name = "Name";
    const string Formula = "R,M2,M3";

    var actual = new ScaleFormulaBuilder( Name ).SetId( Id )
                                                .SetIntervals( Formula )
                                                .Build();

    actual.Id.Should()
          .Be( Id );

    actual.Name.Should()
          .Be( Name );

    actual.Intervals.Should()
          .BeEquivalentTo( [Interval.Unison, Interval.MajorSecond, Interval.MajorThird] );

    actual.ToString()
          .Should()
          .Be( "Name: 1,2,3" );
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithDifferentType()
  {
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                     .SetIntervals( "R,M2,M3" )
                                                     .Build();

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull()
  {
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                     .SetIntervals( "R,M2,M3" )
                                                     .Build();

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingWithSameObject()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetIntervals( "R,M2,M3" )
                                                  .Build();

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetIntervals( "R,M2,M3" )
                                                .Build();

    object y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetIntervals( "R,M2,M3" )
                                                .Build();

    object z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetIntervals( "R,M2,M3" )
                                                .Build();

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
  public void EqualsShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetIntervals( "R,M2,M3" )
                                             .Build();

    var y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetIntervals( "R,M2,M3" )
                                             .Build();

    var z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetIntervals( "R,M2,M3" )
                                             .Build();

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
  public void Generate_ShouldReturnAllPitches_WhenGivenStartingPitch()
  {
    var formula = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                   .SetIntervals( "R,M2,M3" )
                                                   .Build();

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
  public void GetHashCode_ShouldReturnSameValue_WhenObjectsAreEqual()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetIntervals( "R,M2,M3" )
                                                  .Build();

    var expected = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                    .SetIntervals( "R,M2,M3" )
                                                    .Build();

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Theory]
  [MemberData( nameof( ScaleStepsData ) )]
  public void GetRelativeSteps_ShouldReturnCorrectSteps_WhenUsingScale(
    string scaleName,
    int[] expectedSteps )
  {
    var scale = Registry.ScaleFormulas[scaleName];

    scale.GetRelativeSteps()
         .Should()
         .BeEquivalentTo( expectedSteps );
  }

  [Fact]
  public void TypeSafeEquals_ShouldReturnFalse_WhenComparingWithDifferentType()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetIntervals( "R,M2,M3" )
                                                  .Build();

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEquals_ShouldReturnFalse_WhenComparingWithNull()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetIntervals( "R,M2,M3" )
                                                  .Build();

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
