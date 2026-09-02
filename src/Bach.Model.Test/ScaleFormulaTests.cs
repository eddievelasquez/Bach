// Module Name: ScaleFormulaTest.cs
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

using System.Linq;

namespace Bach.Model.Test;

public sealed class ScaleFormulaTests
{
  private const string PENTATONIC_STEPS = "W-W-3-W-3";

  #region Properties

  public static TheoryData<string, int[]> ScaleStepsData => new()
  {
    { "Major", [2, 2, 1, 2, 2, 2, 1] },
    { "NaturalMinor", [2, 1, 2, 2, 1, 2, 2] },
    { "MelodicMinor", [2, 1, 2, 2, 2, 2, 1] },
    { "HarmonicMinor", [2, 1, 2, 2, 1, 3, 1] },
    { "Diminished", [2, 1, 2, 1, 2, 1, 2, 1] },
    { "WholeTone", [2, 2, 2, 2, 2, 2] },
    { "MinorBlues", [3, 2, 1, 1, 3, 2] },
    { "MinorPentatonic", [3, 2, 2, 3, 2] },
    { "Pentatonic", [2, 2, 3, 2, 3] }
  };

  #endregion

  #region Public Methods

  [Fact]
  public void EqualsShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetSteps( PENTATONIC_STEPS )
                                             .Build();

    var y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetSteps( PENTATONIC_STEPS )
                                             .Build();

    var z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                             .SetSteps( PENTATONIC_STEPS )
                                             .Build();

    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithDifferentType()
  {
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                     .SetSteps( PENTATONIC_STEPS )
                                                     .Build();

    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull()
  {
    object actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                     .SetSteps( PENTATONIC_STEPS )
                                                     .Build();

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingWithSameObject()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetSteps( PENTATONIC_STEPS )
                                                  .Build();

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetSteps( PENTATONIC_STEPS )
                                                .Build();

    object y = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetSteps( PENTATONIC_STEPS )
                                                .Build();

    object z = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                .SetSteps( PENTATONIC_STEPS )
                                                .Build();

    // ReSharper disable once EqualExpressionComparison
    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void Generate_ShouldReturnAllPitches_WhenGivenStartingPitch()
  {
    var formula = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                   .SetSteps( PENTATONIC_STEPS )
                                                   .Build();

    var pitches = formula.Generate( Pitch.MinValue )
                        .ToList();

    pitches.Count.Should()
           .Be( 49 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnSameValue_WhenObjectsAreEqual()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetSteps( PENTATONIC_STEPS )
                                                  .Build();

    var expected = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                    .SetSteps( PENTATONIC_STEPS )
                                                    .Build();

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Theory]
  [MemberData( nameof( ScaleStepsData ) )]
  public void GetRelativeSteps_ShouldReturnCorrectSteps_WhenUsingScale(
    string scaleName,
    int[] expectedSteps )
  {
    var formula = Registry.ScaleFormulas[scaleName];

    formula.Steps
         .Should()
         .BeEquivalentTo( expectedSteps );
  }

  [Fact]
  public void TypeSafeEquals_ShouldReturnFalse_WhenComparingWithDifferentType()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetSteps( PENTATONIC_STEPS )
                                                  .Build();

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEquals_ShouldReturnFalse_WhenComparingWithNull()
  {
    var actual = new ScaleFormulaBuilder( "Name" ).SetId( "Id" )
                                                  .SetSteps( PENTATONIC_STEPS )
                                                  .Build();

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
