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

using System;
using Xunit;

namespace Bach.Model.Test;

public sealed class ScaleFormulaBuilderTest
{
#region Public Methods

  [Fact]
  public void AddAliasEnumerableStringTest()
  {
    const string Name = "Name";
    string[] aliases = { "Alias1", "Alias2" };
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddAliases( aliases );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.NotEmpty( formula.Aliases );
    Assert.Contains( aliases[0], formula.Aliases );
    Assert.Contains( aliases[1], formula.Aliases );
  }

  [Fact]
  public void AddAliasNullStringTest()
  {
    const string Name = "Name";
    const string Alias = "Alias";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddAlias( Alias );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.NotEmpty( formula.Aliases );
    Assert.Contains( Alias, formula.Aliases );
  }

  [Fact]
  public void AddAliasStringMultipleTest()
  {
    const string Name = "Name";
    const string Alias = "Alias1;Alias2";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddAlias( Alias );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.NotEmpty( formula.Aliases );
    Assert.Contains( "Alias1", formula.Aliases );
    Assert.Contains( "Alias2", formula.Aliases );
  }

  [Fact]
  public void AddAliasStringsAreTrimmedTest()
  {
    const string Name = "Name";
    const string Alias = "   Alias1   ; Alias2  ";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddAlias( Alias );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.NotEmpty( formula.Aliases );
    Assert.Contains( "Alias1", formula.Aliases );
    Assert.Contains( "Alias2", formula.Aliases );
  }

  [Fact]
  public void AddAliasStringSingleTest()
  {
    const string Name = "Name";
    const string Alias = "Alias";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddAlias( Alias );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.NotEmpty( formula.Aliases );
    Assert.Contains( Alias, formula.Aliases );
  }

  [Fact]
  public void AddCategoriesAreTrimmedTest()
  {
    const string Name = "Name";
    const string Categories = "   Category1   ;  Category2  ";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddCategory( Categories );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( "Category1", formula.Categories );
    Assert.Contains( "Category2", formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void AddCategoriesEnumerableStringTest()
  {
    const string Name = "Name";
    string[] categories = { "Category1", "Category2" };
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddCategories( categories );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( categories[0], formula.Categories );
    Assert.Contains( categories[1], formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void AddCategoryNullStringTest()
  {
    const string Name = "Name";
    const string Category = "Category";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddCategory( Category );

    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( Category, formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void AddCategoryStringMultipleTest()
  {
    const string Name = "Name";
    const string Categories = "Category1;Category2";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddCategory( Categories );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( "Category1", formula.Categories );
    Assert.Contains( "Category2", formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void AddCategoryStringSingleTest()
  {
    const string Name = "Name";
    const string Category = "Category";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString ).AddCategory( Category );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( Category, formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void AppendIntervalTest()
  {
    const string Name = "Name";
    Interval[] intervals = { Interval.Unison, Interval.MajorSecond, Interval.MinorSixth };

    var builder = new ScaleFormulaBuilder( Name );
    foreach( var interval in intervals )
    {
      builder.AppendInterval( interval );
    }

    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void DiatonicCategoryIsAddedTest()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,M3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( "Diatonic", formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void IntervalsAreRequiredTest()
  {
    var builder = new ScaleFormulaBuilder( "Name" );
    Assert.Throws<InvalidOperationException>( () => builder.Build() );
  }

  [Fact]
  public void MajorCategoryIsAddedTest()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,M3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( "Major", formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void MinorCategoryIsAddedTest()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m3,4,5,M6,M7";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.NotEmpty( formula.Categories );
    Assert.Contains( "Minor", formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void NameIsRequiredTest()
  {
    var builder = new ScaleFormulaBuilder().SetIntervals( "R,M2,m6" );
    Assert.Throws<InvalidOperationException>( () => builder.Build() );
  }

  [Fact]
  public void ParseSpanTest()
  {
    const string IntervalString = "R,M2,m6";
    var intervalsFromString = Formula.ParseIntervals( IntervalString );
    var intervalsFromSpan = Formula.ParseIntervals( IntervalString.AsSpan() );

    Assert.Equal( intervalsFromString, intervalsFromSpan );
  }

  [Fact]
  public void SetIntervalEnumerableIntervalTest()
  {
    const string Name = "Name";
    Interval[] intervals = { Interval.Unison, Interval.MajorSecond, Interval.MinorSixth };

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( intervals );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void SetIntervalsStringTest()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void SetKeyIsNameWithoutSpacesTest()
  {
    const string Name = "Name With Spaces";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( "NameWithSpaces", formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void SetKeyTest()
  {
    const string Name = "Name";
    const string Id = "Id";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder( Name ).SetId( Id ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Id, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void SetNameAndIntervalsStringTest()
  {
    const string Name = "Name";
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( Name ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( Name, formula.Name );
    Assert.Equal( Name, formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

  [Fact]
  public void SetNameIsTrimmedTest()
  {
    const string IntervalString = "R,M2,m6";
    var intervals = Formula.ParseIntervals( IntervalString );

    var builder = new ScaleFormulaBuilder().SetName( "   Name    " ).SetIntervals( IntervalString );
    var formula = builder.Build();
    Assert.NotNull( formula );
    Assert.Equal( "Name", formula.Name );
    Assert.Equal( "Name", formula.Id );
    Assert.Equal( intervals, formula.Intervals );
    Assert.Empty( formula.Categories );
    Assert.Empty( formula.Aliases );
  }

#endregion
}
