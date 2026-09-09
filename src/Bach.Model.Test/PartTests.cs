// Module Name: PartTests.cs
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

using System.Collections.Generic;

namespace Bach.Model.Test;

public sealed class PartTests
{
  #region Public Methods

  [Fact]
  public void MeasureBuilder_ShouldBuildImmutableMeasure()
  {
    var builder = new MeasureBuilder().Add( PitchClass.C[4] );
    var first = builder.Build();
    builder.Add( PitchClass.E[4] );

    first.Should()
         .ContainSingle()
         .Which.Should()
         .Be( PitchClass.C[4] );

    builder.Build()
           .Should()
           .HaveCount( 2 );
  }

  [Fact]
  public void PartBuilder_AddMeasure_ShouldPreserveMeasureAndEventOrder()
  {
    var part = new PartBuilder()
               .AddMeasure( measure => measure.Add( PitchClass.C[4] )
                                              .Add( PitchClass.E[4] )
               )
               .AddMeasure( measure => measure.Add( PitchChord.Parse( "G7" ) ) )
               .Build();

    part.Should()
        .HaveCount( 2 );

    part[0]
      .Should()
      .HaveCount( 2 );

    part[1]
      .Should()
      .ContainSingle()
      .Which.Should()
      .Be( PitchChord.Parse( "G7" ) );

    part.Events.Should()
        .Equal( PitchClass.C[4], PitchClass.E[4], PitchChord.Parse( "G7" ) );
  }

  [Fact]
  public void PartBuilder_AddMeasure_ShouldRejectNullCallback()
  {
    Action action = () => new PartBuilder().AddMeasure( (Action<MeasureBuilder>) null! );

    action.Should()
          .Throw<ArgumentNullException>();
  }

  [Fact]
  public void EmptyMeasure_ShouldBeRejected()
  {
    Action action = () => new MeasureBuilder().Build();

    action.Should()
          .Throw<InvalidOperationException>();
  }

  [Fact]
  public void Part_Parse_ShouldCreateMeasuresFromPipeSeparators()
  {
    var part = Part.Parse( "C4,E4|!G7|C4" );

    part.Should()
        .HaveCount( 3 );

    part[0]
      .Should()
      .HaveCount( 2 );

    part[1]
      .Should()
      .ContainSingle()
      .Which.Should()
      .Be( PitchChord.Parse( "G7" ) );

    part.PitchClasses.Should()
        .Equal(
          PitchClass.C,
          PitchClass.E,
          PitchClass.G,
          PitchClass.B,
          PitchClass.D,
          PitchClass.F,
          PitchClass.C
        );
  }

  [Theory]
  [InlineData( "|C4" )]
  [InlineData( "C4|" )]
  [InlineData( "C4||E4" )]
  [InlineData( "invalid" )]
  public void Part_Parse_ShouldRejectMalformedInput(
    string input )
  {
    Action action = () => Part.Parse( input );

    action.Should()
          .Throw<FormatException>();
  }

  [Fact]
  public void Part_ShouldImplementReadOnlyMeasureList()
  {
    var part = new PartBuilder().AddMeasure( measure => measure.Add( PitchClass.C[4] ) )
                                .Build();

    part.Should()
        .BeAssignableTo<IReadOnlyList<Measure>>();

    part.Count.Should()
        .Be( 1 );
  }

  #endregion
}
