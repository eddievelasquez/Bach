// Module Name: SpellingRegressionTests.cs
// Project:     Bach.Model.Test

namespace Bach.Model.Test;

public sealed class SpellingRegressionTests
{
  [Fact]
  public void AugmentedSecond_IsDistinctFrom_MinorThird()
  {
    var aug2 = Interval.AugmentedSecond;
    var m3 = Interval.MinorThird;

    aug2.SemitoneCount.Should().Be( 3 );
    m3.SemitoneCount.Should().Be( 3 );

    aug2.Quantity.Should().Be( IntervalQuantity.Second );
    m3.Quantity.Should().Be( IntervalQuantity.Third );

    // Chromatic alteration relative to base quantity
    aug2.ChromaticAlteration.Should().Be( 1 );
    m3.ChromaticAlteration.Should().Be( -1 );
  }
}
