using System.Linq;

namespace Bach.Model.Test;

public sealed class PublicApiBoundaryTests
{
  [Fact]
  public void Registry_ShouldExposeNamedLookupThroughPublicInterface()
  {
    Registry.Instance.ScaleFormulas.Should()
                    .BeAssignableTo<INamedObjectCollection<ScaleFormula>>();

    Registry.Instance.ScaleFormulas["Major"].Id.Should()
                    .Be( "Major" );
  }

  [Fact]
  public void ScaleFormulaAscendingDegrees_ShouldMatchInheritedIntervals()
  {
    var formula = Registry.Instance.ScaleFormulas["MelodicMinor"];

    formula.AscendingDegrees.Select( step => step.Interval )
            .Should()
            .Equal( formula.Intervals );
  }

  [Fact]
  public void PitchClassGenerators_ShouldStopAtSupportedPitchCount()
  {
    var formula = Registry.Instance.ScaleFormulas["Major"];
    var scale = new Scale( PitchClass.C, formula );
    var mode = new Mode( scale, ModeFormula.Dorian );

    formula.Generate( PitchClass.C ).Should()
            .HaveCount( Pitch.TotalPitchCount );

    scale.GetAscending().Should()
         .HaveCount( Pitch.TotalPitchCount );

    mode.Should()
        .HaveCount( Pitch.TotalPitchCount );
  }

  [Fact]
  public void ScaleFormula_ShouldSnapshotDirectionalDegrees()
  {
    var ascendingIntervals = new[]
    {
      Interval.Unison,
      Interval.MajorSecond,
      Interval.MajorThird,
      Interval.Fourth,
      Interval.Fifth
    };
    var descendingIntervals = new[]
    {
      Interval.Unison,
      Interval.MinorSecond,
      Interval.MinorThird,
      Interval.Fourth,
      Interval.Fifth
    };
    var ascending = ascendingIntervals.Select( ( interval, index ) => new ScaleDegreeStep( index + 1, interval ) ).ToArray();
    var descending = descendingIntervals.Select( ( interval, index ) => new ScaleDegreeStep( index + 1, interval ) ).ToArray();
    var classification = new ScaleClassification( ascending, Array.Empty<ScaleCategory>(), Array.Empty<ScaleTag>() );
    var formula = new ScaleFormula( "snapshot-test", "Snapshot Test", ascending, descending, classification, [] );

    ascending[0] = new ScaleDegreeStep( 1, Interval.MajorSecond );
    descending[0] = new ScaleDegreeStep( 1, Interval.MajorSecond );

    formula.AscendingDegrees[0].Interval.Should()
            .Be( Interval.Unison );

    formula.DescendingDegrees[0].Interval.Should()
            .Be( Interval.Unison );

    formula.AscendingDegrees.Select( step => step.Interval )
            .Should()
            .Equal( formula.Intervals );
  }
}
