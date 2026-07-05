namespace Bach.Model.Test;

using System.Collections.Generic;
using System.Linq;

public sealed class FormulaTest
{
  [Fact]
  public void Contains_ShouldUseEnharmonicMatching_WhenRequested()
  {
    var formula = new TestFormula( "test", "Test", [Interval.Unison, Interval.AugmentedFourth] );

    formula.Contains( [Interval.DiminishedFifth], IntervalMatch.Enharmonic )
           .Should()
           .BeTrue();
  }

  [Fact]
  public void GetRelativeSteps_ShouldReturnSemitoneStepsBetweenIntervals()
  {
    var formula = new TestFormula( "test", "Test", [Interval.Unison, Interval.MajorSecond, Interval.MajorThird, Interval.Fourth, Interval.Fifth] );

    formula.GetRelativeSteps()
           .Should()
           .Equal( 2, 2, 1, 2, 5 );
  }

  [Fact]
  public void ParseIntervals_ShouldParseIntervalList_WhenFormulaUsesCsv()
  {
    var intervals = Formula.ParseIntervals( "R,M2,M3" );

    intervals.Should()
             .Equal( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
  }

  [Fact]
  public void ParseIntervals_ShouldThrowFormatException_WhenFormulaContainsInvalidInterval()
  {
    var act = () => Formula.ParseIntervals( "R,ZZ" );

    act.Should()
       .Throw<FormatException>()
       .WithMessage( "ZZ is not a valid interval" );
  }

  [Fact]
  public void ScaleFormula_ShouldExposeCategoriesAliasesAndFlags()
  {
    var formula = new ScaleFormula(
      "custom",
      "Custom",
      [Interval.Unison, Interval.MajorSecond],
      new HashSet<string> { ScaleCategory.Diatonic, ScaleCategory.Major },
      new HashSet<string> { "custom-name" } );

    formula.Categories.Should()
           .Contain( ScaleCategory.Diatonic )
           .And.Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .Contain( "custom-name" );

    formula.IsDiatonic.Should()
           .BeTrue();

    formula.IsMajor.Should()
           .BeTrue();

    formula.IsMinor.Should()
           .BeFalse();
  }

  [Fact]
  public void ToString_ShouldHonorFormatSpecifier_WhenCustomFormatIsProvided()
  {
    var formula = new TestFormula( "test", "Test", [Interval.Unison, Interval.MajorThird] );

    formula.ToString( "N:I" )
           .Should()
           .Be( "Test:1,3" );
  }

  private sealed class TestFormula : Formula
  {
    public TestFormula(
      string id,
      string name,
      Interval[] intervals )
      : base( id, name, intervals )
    {
    }
  }
}
