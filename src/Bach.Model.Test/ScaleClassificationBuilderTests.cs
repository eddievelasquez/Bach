// Module Name: ScaleClassificationBuilderTests.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2026  Eddie Velasquez.
//
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.

namespace Bach.Model.Test;

public sealed class ScaleClassificationBuilderTests
{
  [Fact]
  public void Build_ShouldCreateConfiguredClassification()
  {
    var degrees = new[]
    {
      new ScaleDegreeStep( 1, Interval.Unison ),
      new ScaleDegreeStep( 2, Interval.MinorSecond )
    };

    var classification = new ScaleClassificationBuilder()
      .AddCategories( [ScaleCategory.Diatonic, ScaleCategory.Major] )
      .AddRepertoireTags( [ScaleTag.Jazz, ScaleTag.Modal] )
      .SetParentScaleId( "parent" )
      .SetModalRotationIndex( 1 )
      .SetAscendingDegrees( degrees )
      .SetKeyCandidate( true )
      .Build();

    classification.Cardinality.Should().Be( 2 );
    classification.Categories.Should().BeEquivalentTo( [ScaleCategory.Diatonic, ScaleCategory.Major] );
    classification.RepertoireTags.Should().BeEquivalentTo( [ScaleTag.Jazz, ScaleTag.Modal] );
    classification.ParentScaleId.Should().Be( "parent" );
    classification.ModalRotationIndex.Should().Be( 1 );
    classification.CanonicalDegrees.Should().Equal( "1", "b2" );
    classification.IsKeyCandidate.Should().BeTrue();
  }
}
