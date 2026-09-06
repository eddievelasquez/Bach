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

  private const string DEFAULT_FORMULA_NAME = "Name";

  private static readonly Interval[] s_pentatonicIntervals =
  [
    Interval.Unison, Interval.MajorSecond, Interval.MajorThird, Interval.Fifth, Interval.MajorSixth
  ];

  #endregion

  #region Public Methods

  [Fact]
  public void Build_ShouldCreateFormula()
  {
    var formula = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetAscendingIntervals( s_pentatonicIntervals )
                                                                 .Build();

    formula.Should()
           .NotBeNull();

    formula.Name.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.Id.Should()
           .BeEquivalentTo( DEFAULT_FORMULA_NAME );

    formula.AscendingDegrees.Should()
           .HaveCount( s_pentatonicIntervals.Length );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Categories.Should()
           .Contain( ScaleCategory.Pentatonic );

    formula.Aliases.Should()
           .BeEmpty();
  }

  [Fact]
  public void AddAlias_ShouldAddMultipleAliases_WhenGivenSemicolonSeparatedString()
  {
    const string Alias = "Alias1;Alias2";

    var formula = CreateFormula( builder => builder.AddAlias( Alias ) );

    formula.Aliases.Should()
           .Contain( "Alias1" );

    formula.Aliases.Should()
           .Contain( "Alias2" );
  }

  [Fact]
  public void AddAlias_ShouldAddSingleAlias_WhenGivenValidString()
  {
    const string Alias = "Alias";

    var formula = CreateFormula( builder => builder.AddAlias( Alias ) );

    formula.Aliases.Should()
           .Contain( Alias );
  }

  [Fact]
  public void AddAlias_ShouldAddTrimmedAliases_WhenGivenPaddedStrings()
  {
    const string Alias = "   Alias1   ; Alias2  ";

    var formula = CreateFormula( builder => builder.AddAlias( Alias ) );

    formula.Aliases.Should()
           .Contain( "Alias1" );

    formula.Aliases.Should()
           .Contain( "Alias2" );
  }

  [Fact]
  public void AddAliases_ShouldAddMultipleAliases_WhenGivenEnumerableOfStrings()
  {
    string[] aliases = ["Alias1", "Alias2"];

    var formula = CreateFormula( builder => builder.AddAliases( aliases ) );

    formula.Aliases.Should()
           .Contain( aliases[0] );

    formula.Aliases.Should()
           .Contain( aliases[1] );
  }

  [Fact]
  public void AddCategories_ShouldAddMultipleCategories_WhenGivenEnumerableOfStrings()
  {
    string[] categories = ["Category1", "Category2"];

    var formula = CreateFormula( builder => builder.AddCategories( categories ) );

    formula.Categories.Should()
           .Contain( ScaleCategory.Major );

    formula.Categories.Should()
           .Contain( ScaleCategory.Pentatonic );

    formula.Categories.Should()
           .Contain( categories[0] );

    formula.Categories.Should()
           .Contain( categories[1] );
  }

  [Fact]
  public void AddCategory_ShouldAddCategory_WhenGivenValidString()
  {
    const string Category = "Category";

    var formula = CreateFormula( builder => builder.AddCategory( Category ) );

    formula.Categories.Should()
           .Contain( Category );
  }

  [Fact]
  public void AddCategory_ShouldAddMultipleCategories_WhenGivenSemicolonSeparatedString()
  {
    const string Categories = "Category1;Category2";

    var formula = CreateFormula( builder => builder.AddCategory( Categories ) );

    formula.Categories.Should()
           .Contain( "Category1" );

    formula.Categories.Should()
           .Contain( "Category2" );
  }

  [Fact]
  public void AddCategory_ShouldAddTrimmedCategories_WhenGivenPaddedStrings()
  {
    const string Categories = "   Category1   ;  Category2  ";

    var formula = CreateFormula( builder => builder.AddCategory( Categories ) );

    formula.Categories.Should()
           .Contain( "Category1" );

    formula.Categories.Should()
           .Contain( "Category2" );
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenIntervalsAreNotSet()
  {
    var builder = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME );
    var act = builder.Build;

    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenNameIsNotSet()
  {
    var builder = new ScaleFormulaBuilder().SetAscendingIntervals( s_pentatonicIntervals );
    var act = builder.Build;

    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void SetId_ShouldUseProvidedId_WhenIdIsSet()
  {
    const string Id = "Id";

    var formula = CreateFormula( builder => builder.SetId( Id ) );

    formula.Id.Should()
           .BeEquivalentTo( Id );
  }

  [Fact]
  public void SetAscendingIntervals_ShouldBuildScaleWithIntervals_WhenGivenIntervals()
  {
    Interval[] minorPentatonic =
    [
      Interval.Unison, Interval.MinorThird, Interval.Fourth, Interval.Fifth, Interval.MinorSeventh
    ];
    var formula = CreateFormula( builder => builder.SetAscendingIntervals( minorPentatonic ) );

    formula.AscendingDegrees.Should()
           .HaveCount( minorPentatonic.Length );

    formula.Categories.Should()
           .Contain( ScaleCategory.Minor );

    formula.Categories.Should()
           .Contain( ScaleCategory.Pentatonic );
  }

  [Fact]
  public void SetName_ShouldSetIdWithoutSpaces_WhenNameContainsSpaces()
  {
    const string Name = "Name With Spaces";

    var formula = CreateFormula( builder => builder.SetName( Name ) );

    formula.Name.Should()
           .BeEquivalentTo( Name );

    formula.Id.Should()
           .BeEquivalentTo( "NameWithSpaces" );
  }

  [Fact]
  public void SetName_ShouldUseTrimmedName_WhenNameContainsPadding()
  {
    var formula = CreateFormula( builder => builder.SetName( "   ScaleName    " ) );

    formula.Name.Should()
           .BeEquivalentTo( "ScaleName" );

    formula.Id.Should()
           .BeEquivalentTo( "ScaleName" );
  }

  #endregion

  #region Implementation

  private static ScaleFormula CreateFormula(
    Action<ScaleFormulaBuilder>? configure = null )
  {
    var builder = new ScaleFormulaBuilder( DEFAULT_FORMULA_NAME ).SetAscendingIntervals( s_pentatonicIntervals );
    configure?.Invoke( builder );

    var formula = builder.Build();
    return formula;
  }

  #endregion
}
