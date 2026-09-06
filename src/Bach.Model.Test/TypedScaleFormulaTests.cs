// Module Name: TypedScaleFormulaTests.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2026  Eddie Velasquez.
//
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.

using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model.Test;

public sealed class TypedScaleFormulaTests
{
  [Fact]
  public void Build_ShouldInferOrdinalsAndSemitones_WhenGivenAscendingIntervals()
  {
    var formula = new ScaleFormulaBuilder( "Major" )
      .SetAscendingIntervals( [
        Interval.Unison, Interval.MajorSecond, Interval.MajorThird,
        Interval.Fourth, Interval.Fifth, Interval.MajorSixth, Interval.MajorSeventh
      ] )
      .Build();

    formula.AscendingDegrees.Should().HaveCount( 7 );
    formula.AscendingDegrees.Select( step => step.Ordinal ).Should().Equal( 1, 2, 3, 4, 5, 6, 7 );
    formula.GetSemitoneSteps().Should().Equal( 2, 2, 1, 2, 2, 2, 1 );
  }

  [Fact]
  public void GetDescending_ShouldUseExplicitCollection_WhenMelodicMinorIsDirectionDependent()
  {
    var scale = new Scale( PitchClass.C, "MelodicMinor" );

    scale.GetDescending().Take( 7 ).Should().Equal(
      PitchClass.C, PitchClass.BFlat, PitchClass.AFlat, PitchClass.G, PitchClass.F, PitchClass.EFlat, PitchClass.D );
  }

  [Fact]
  public void Registry_ShouldLoadAllConvertedScales()
  {
    Registry.ScaleFormulas.Should().HaveCount( 52 );
    Registry.ScaleFormulas.Should().OnlyContain( formula => formula.AscendingDegrees.Count > 0 );
  }
}
