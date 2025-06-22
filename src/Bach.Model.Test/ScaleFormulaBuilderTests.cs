// Module Name: ScaleFormulaBuilderTests.cs
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

public sealed class ScaleFormulaBuilderTest
{
  #region Public Methods

  [Fact]
  public void AddAliases_ShouldAddMultipleAliases_WhenGivenEnumerableOfStrings()
  {
    const string Name = "Name";
    string[] aliases = ["Alias1", "Alias2"];
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddAliases( aliases );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .NotBeEmpty();
    formula.Aliases.Should()
           .Contain( aliases[0] );
    formula.Aliases.Should()
           .Contain( aliases[1] );
  }

  [Fact]
  public void AddAlias_ShouldAddSingleAlias_WhenGivenValidString()
  {
    const string Name = "Name";
    const string Alias = "Alias";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddAlias( Alias );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .NotBeEmpty();
    formula.Aliases.Should()
           .Contain( Alias );
  }

  [Fact]
  public void AddAlias_ShouldAddMultipleAliases_WhenGivenSemicolonSeparatedString()
  {
    const string Name = "Name";
    const string Alias = "Alias1;Alias2";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddAlias( Alias );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .NotBeEmpty();
    formula.Aliases.Should()
           .Contain( "Alias1" );
    formula.Aliases.Should()
           .Contain( "Alias2" );
  }

  [Fact]
  public void AddAlias_ShouldAddTrimmedAliases_WhenGivenPaddedStrings()
  {
    const string Name = "Name";
    const string Alias = "   Alias1   ; Alias2  ";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddAlias( Alias );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .NotBeEmpty();
    formula.Aliases.Should()
           .Contain( "Alias1" );
    formula.Aliases.Should()
           .Contain( "Alias2" );
  }

  [Fact]
  public void AddAlias_ShouldAddSingleAlias_WhenGivenNonSeparatedString()
  {
    const string Name = "Name";
    const string Alias = "Alias";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddAlias( Alias );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .NotBeEmpty();
    formula.Aliases.Should()
           .Contain( Alias );
  }

  [Fact]
  public void AddCategory_ShouldAddTrimmedCategories_WhenGivenPaddedStrings()
  {
    const string Name = "Name";
    const string Categories = "   Category1   ;  Category2  ";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddCategory( Categories );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( "Category1" );
    formula.Categories.Should()
           .Contain( "Category2" );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddCategories_ShouldAddMultipleCategories_WhenGivenEnumerableOfStrings()
  {
    const string Name = "Name";
    string[] categories = ["Category1", "Category2"];
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddCategories( categories );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( categories[0] );
    formula.Categories.Should()
           .Contain( categories[1] );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddCategory_ShouldAddCategory_WhenGivenValidString()
  {
    const string Name = "Name";
    const string Category = "Category";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddCategory( Category );

    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( Category );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddCategory_ShouldAddMultipleCategories_WhenGivenSemicolonSeparatedString()
  {
    const string Name = "Name";
    const string Categories = "Category1;Category2";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddCategory( Categories );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( "Category1" );
    formula.Categories.Should()
           .Contain( "Category2" );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddCategory_ShouldAddSingleCategory_WhenGivenNonSeparatedString()
  {
    const string Name = "Name";
    const string Category = "Category";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString )
                                                 .AddCategory( Category );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( Category );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AppendInterval_ShouldBuildScaleWithIntervals_WhenAddingIntervalsSequentially()
  {
    const string Name = "Name";
    Interval[] intervals = [Interval.Unison, Interval.MajorSecond, Interval.MinorSixth];

    var builder = new ScaleFormulaBuilder( Name );
    foreach( var interval in intervals )
    {
      builder.AppendInterval( interval );
    }

    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void Build_ShouldAddDiatonicCategory_WhenScaleHasSevenNoteMajorPattern()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,M3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name )
                                           .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( "Diatonic" );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenIntervalsAreNotSet()
  {
    var builder = new ScaleFormulaBuilder( "Name" );
    var act = () => builder.Build();
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Build_ShouldAddMajorCategory_WhenScaleHasMajorThirdAndFifth()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,M3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name )
                                           .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( "Major" );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void Build_ShouldAddMinorCategory_WhenScaleHasMinorThirdAndFifth()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name )
                                           .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .NotBeEmpty();
    formula.Categories.Should()
           .Contain( "Minor" );
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenNameIsNotSet()
  {
    var builder = new ScaleFormulaBuilder().SetIntervals( "R,M2,m6" );
    var act = () => builder.Build();
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void ParseIntervals_ShouldReturnSameIntervals_WhenUsingStringOrSpan()
  {
    const string IntervalString = "R,M2,m6";
    var intervalsFromString = Formula.ParseIntervals( IntervalString );
    var intervalsFromSpan = Formula.ParseIntervals( IntervalString.AsSpan() );

    intervalsFromSpan.Should()
                     .BeEquivalentTo( intervalsFromString );
  }

  [Fact]
  public void SetIntervals_ShouldBuildScaleWithIntervals_WhenGivenIntervalArray()
  {
    const string Name = "Name";
    Interval[] intervals = [Interval.Unison, Interval.MajorSecond, Interval.MinorSixth];

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( intervals );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetIntervals_ShouldBuildScaleWithIntervals_WhenGivenIntervalString()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldSetIdWithoutSpaces_WhenNameContainsSpaces()
  {
    const string Name = "Name With Spaces";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( "NameWithSpaces" );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetId_ShouldUseProvidedId_WhenIdIsSet()
  {
    const string Name = "Name";
    const string Id = "Id";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetId( Id )
                                                 .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Id );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldBuildScaleWithNameAndIntervals_WhenNameAndIntervalsAreSet()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name )
                                           .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( Name );
    formula.Id.Should()
           .BeEquivalentTo( Name );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldUseTrimmedName_WhenNameContainsPadding()
  {
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( "   Name    " )
                                           .SetIntervals( IntervalString );
    var formula = builder.Build();
    formula.Should()
           .NotBeNull();
    formula.Name.Should()
           .BeEquivalentTo( "Name" );
    formula.Id.Should()
           .BeEquivalentTo( "Name" );
    formula.Intervals.Should()
           .BeEquivalentTo( intervals );
    formula.Categories.Should()
           .BeEmpty();
    formula.Aliases.Should()
           .BeEmpty();
  }

  #endregion
}
