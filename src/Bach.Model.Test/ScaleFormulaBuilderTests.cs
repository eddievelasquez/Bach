// Module Name: ScaleFormulaBuilderTests.cs
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

public sealed class ScaleFormulaBuilderTests
{
  #region Constants

  private const string PENTATONIC_STEPS = "W-W-3-W-3";
  private const string DEFAULT_FORMULA_NAME = "Name";
  private static readonly StepCollection s_pentatonicSteps = StepCollection.Parse( PENTATONIC_STEPS );

  #endregion

  #region Public Methods

  [Fact]
  public void AddAlias_ShouldAddMultipleAliases_WhenGivenSemicolonSeparatedString()
  {
    const string Alias = "Alias1;Alias2";
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddAlias( Alias )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

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
    const string Alias = "Alias";
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddAlias( Alias )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .NotBeEmpty();

    formula.Aliases.Should()
           .Contain( Alias );
  }

  [Fact]
  public void AddAlias_ShouldAddSingleAlias_WhenGivenValidString()
  {
    const string Alias = "Alias";
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddAlias( Alias )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .NotBeEmpty();

    formula.Aliases.Should()
           .Contain( Alias );
  }

  [Fact]
  public void AddAlias_ShouldAddTrimmedAliases_WhenGivenPaddedStrings()
  {
    const string Alias = "   Alias1   ; Alias2  ";
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddAlias( Alias )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .NotBeEmpty();

    formula.Aliases.Should()
           .Contain( "Alias1" );

    formula.Aliases.Should()
           .Contain( "Alias2" );
  }

  [Fact]
  public void AddAliases_ShouldAddMultipleAliases_WhenGivenEnumerableOfStrings()
  {
    string[] aliases = ["Alias1", "Alias2"];
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddAliases( aliases )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .NotBeEmpty();

    formula.Aliases.Should()
           .Contain( aliases[0] );

    formula.Aliases.Should()
           .Contain( aliases[1] );
  }

  [Fact]
  public void AddCategories_ShouldAddMultipleCategories_WhenGivenEnumerableOfStrings()
  {
    string[] categories = ["Category1", "Category2"];
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddCategories( categories )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

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
    const string Category = "Category";
    var steps = s_pentatonicSteps;

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddCategory( Category )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

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
    const string Categories = "Category1;Category2";

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddCategory( Categories )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

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
    const string Category = "Category";

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddCategory( Category )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .NotBeEmpty();

    formula.Categories.Should()
           .Contain( Category );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddCategory_ShouldAddTrimmedCategories_WhenGivenPaddedStrings()
  {
    const string Categories = "   Category1   ;  Category2  ";

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS )
                                                                 .AddCategory( Categories )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

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
  public void Build_ShouldThrowInvalidOperationException_WhenIntervalsAreNotSet()
  {
    var builder = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME );
    var act = () => builder.Build();

    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenNameIsNotSet()
  {
    var builder = new ScaleFormulaBuilder().SetSteps( PENTATONIC_STEPS );
    var act = () => builder.Build();

    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void ParseIntervals_ShouldReturnSameIntervals_WhenUsingStringOrSpan()
  {
    var steps = StepCollection.Parse( PENTATONIC_STEPS.AsSpan() );

    steps.Should()
         .BeEquivalentTo( s_pentatonicSteps );
  }

  [Fact]
  public void SetId_ShouldUseProvidedId_WhenIdIsSet()
  {
    const string Id = "Id";

    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetId( Id )
                                                                 .SetSteps( PENTATONIC_STEPS )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( Id );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetIntervals_ShouldBuildScaleWithIntervals_WhenGivenIntervalArray()
  {
    int[] steps = [2, 2, 3, 2, 3];

    var builder = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( steps );
    var formula = builder.Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( steps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetIntervals_ShouldBuildScaleWithIntervals_WhenGivenIntervalString()
  {
    var builder = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetSteps( PENTATONIC_STEPS );
    var formula = builder.Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldBuildScaleWithNameAndIntervals_WhenNameAndIntervalsAreSet()
  {
    var formula = new ScaleFormulaBuilder().SetName( DEFAULT_FORMULA_NAME )
                                           .SetSteps( PENTATONIC_STEPS )
                                           .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldSetIdWithoutSpaces_WhenNameContainsSpaces()
  {
    const string Name = "Name With Spaces";

    var formula = new ScaleFormulaBuilder( Name ).SetSteps( PENTATONIC_STEPS )
                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( Name );

    formula.Id.Should()
           .BeEquivalentTo( "NameWithSpaces" );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void SetName_ShouldUseTrimmedName_WhenNameContainsPadding()
  {
    var formula = new ScaleFormulaBuilder().SetName( "   Name    " )
                                           .SetSteps( PENTATONIC_STEPS )
                                           .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Steps.Should()
           .BeEquivalentTo( s_pentatonicSteps );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .BeEmpty();
  }

  #endregion
}
