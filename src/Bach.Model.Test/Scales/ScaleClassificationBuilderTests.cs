// Module Name: ScaleClassificationBuilderTests.cs
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

namespace Bach.Model.Scales.Test;

public sealed class ScaleClassificationBuilderTests
{
  #region Public Methods

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenAscendingDegreesAreNotSet()
  {
    Action action = () => new ScaleClassificationBuilder().Build();

    action.Should()
          .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Build_ShouldThrowInvalidOperationException_WhenAscendingDegreesAreEmpty()
  {
    Action action = () => new ScaleClassificationBuilder()
                          .SetAscendingDegrees( Array.Empty<ScaleDegreeStep>() )
                          .Build();

    action.Should()
          .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Build_ShouldParseRepertoireTags_WhenStringHasMixedCaseAndSeparators()
  {
    var classification = new ScaleClassificationBuilder()
                         .AddRepertoireTag( " jazz ; modal;; " )
                         .SetAscendingDegrees( [new ScaleDegreeStep( 1, Interval.Unison )] )
                         .Build();

    classification.RepertoireTags.Should()
                  .BeEquivalentTo( [ScaleTag.Jazz, ScaleTag.Modal] );
  }

  [Fact]
  public void AddRepertoireTag_ShouldThrowArgumentException_WhenTagNameIsUnknown()
  {
    Action action = () => new ScaleClassificationBuilder().AddRepertoireTag( "not-a-tag" );

    action.Should()
          .Throw<ArgumentException>();
  }

  [Fact]
  public void Build_ShouldFormatSharpCanonicalDegree_WhenIntervalIsAugmented()
  {
    var classification = new ScaleClassificationBuilder()
                         .SetAscendingDegrees(
                           [new ScaleDegreeStep( 1, Interval.Unison ), new ScaleDegreeStep( 2, Interval.AugmentedFourth )]
                         )
                         .Build();

    classification.CanonicalDegrees.Should()
                  .Equal( "1", "#4" );
  }

  [Fact]
  public void Build_ShouldCreateConfiguredClassification()
  {
    var degrees = new[] { new ScaleDegreeStep( 1, Interval.Unison ), new ScaleDegreeStep( 2, Interval.MinorSecond ) };

    var classification = new ScaleClassificationBuilder()
                         .AddCategories( [ScaleCategory.Diatonic, ScaleCategory.Major] )
                         .AddRepertoireTags( [ScaleTag.Jazz, ScaleTag.Modal] )
                         .SetParentScaleId( "parent" )
                         .SetModalRotationIndex( 1 )
                         .SetAscendingDegrees( degrees )
                         .SetKeyCandidate( true )
                         .Build();

    classification.Cardinality.Should()
                  .Be( 2 );

    classification.Categories.Should()
                  .BeEquivalentTo( [ScaleCategory.Diatonic, ScaleCategory.Major] );

    classification.RepertoireTags.Should()
                  .BeEquivalentTo( [ScaleTag.Jazz, ScaleTag.Modal] );

    classification.ParentScaleId.Should()
                  .Be( "parent" );

    classification.ModalRotationIndex.Should()
                  .Be( 1 );

    classification.CanonicalDegrees.Should()
                  .Equal( "1", "b2" );

    classification.IsKeyCandidate.Should()
                  .BeTrue();
  }

  #endregion
}
