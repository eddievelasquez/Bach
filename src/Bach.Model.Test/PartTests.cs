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
using System.Linq;

namespace Bach.Model.Test;

public sealed class PartTests
{
  #region Public Methods

  [Fact]
  public void AddAndEnumerate_ShouldStoreEventsAndPreserveChordsAsSingleEvents()
  {
    var part = Part.Parse( "C4,C" );

    var events = part.ToArray();

    events.Should()
          .HaveCount( 2 );

    events[0]
      .Should()
      .Be( PitchClass.C[4] );

    events[1]
      .Should()
      .Be( PitchChord.Parse( "C" ) );
  }

  [Fact]
  public void Constructor_Should_CopyEventsAndPreserveOrder()
  {
    var source = new List<IPartEvent>
    {
      PitchClass.C[4],
      PitchChord.Parse( "C" )
    };
    var part = new Part( source );

    source.Clear();

    part.Should()
        .HaveCount( 2 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchChord.Parse( "C" ) );
  }

  [Fact]
  public void EmptyPart_ShouldHaveNoEvents()
  {
    new Part().Should()
              .BeEmpty();
  }

  [Fact]
  public void Part_Should_ImplementReadOnlyList()
  {
    var part = new Part( new IPartEvent[] { PitchClass.C[4] } );

    part.Should()
        .BeAssignableTo<IReadOnlyList<IPartEvent>>();

    part.Count.Should()
        .Be( 1 );
  }

  [Fact]
  public void PartBuilder_Should_BuildImmutablePart()
  {
    var builder = new PartBuilder()
                  .Add( PitchClass.C[4] )
                  .Add( PitchClass.E[4] )
                  .Insert( 1, PitchClass.D[4] );

    var part = builder.Build();

    part.Select( partEvent => partEvent )
        .Should()
        .Equal( PitchClass.C[4], PitchClass.D[4], PitchClass.E[4] );
  }

  [Fact]
  public void PartBuilder_Should_RejectNullEvents()
  {
    var builder = new PartBuilder();

    Action action = () => builder.Add( null! );

    action.Should()
          .Throw<ArgumentNullException>();
  }

  [Fact]
  public void PartBuilder_Should_CopyEventsOnBuild()
  {
    var builder = new PartBuilder().Add( PitchClass.C[4] );
    var first = builder.Build();

    builder.Add( PitchClass.D[4] );
    var second = builder.Build();

    first.Should()
         .HaveCount( 1 );

    second.Should()
          .HaveCount( 2 );
  }

  [Fact]
  public void Parse_ShouldCreateImmutablePart()
  {
    var part = Part.Parse( "C4,C" );

    part.Should()
        .HaveCount( 2 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchChord.Parse( "C" ) );
  }

  [Fact]
  public void Parse_ShouldThrowFormatExceptionForInvalidInput()
  {
    Action action = () => Part.Parse( "invalid" );

    action.Should()
          .Throw<FormatException>();
  }

  [Fact]
  public void PitchClasses_ShouldPreserveEventAndChordOrderAndDuplicates()
  {
    var part = new PartBuilder()
               .Add( PitchClass.C[4] )
               .Add( PitchChord.Create( PitchClass.C, ChordFormula.Major, inversion: 1 ) )
               .Add( PitchClass.C[4] )
               .Build();

    part.PitchClasses.Should()
        .Equal( PitchClass.C, PitchClass.E, PitchClass.G, PitchClass.C, PitchClass.C );
  }

  [Theory]
  [InlineData( "C4", 1, "" )]
  [InlineData( "C4, E4, G4", 3, "" )]
  [InlineData( "Cmaj7", 1, "" )]
  [InlineData( "C4, Am, G5", 3, "" )]
  [InlineData( "", 0, "" )]
  [InlineData( "  ", 0, "" )]
  [InlineData( " C4 , E4 ", 2, " " )]
  [InlineData( "C4,,E4", 2, "" )]
  [InlineData( "C4, E4, ", 2, ", " )]
  public void TryParse_ShouldParseValidInputs(
    string input,
    int expectedCount,
    string expectedTail )
  {
    var result = Part.TryParse( input.AsSpan(), null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .NotBeNull();

    part!.Count.Should()
         .Be( expectedCount );

    tail.ToString()
        .Should()
        .Be( expectedTail );
  }

  [Theory]
  [InlineData( "invalid" )]
  [InlineData( "C4, invalid" )]
  public void TryParse_ShouldReturnFalseForInvalidInputs(
    string input )
  {
    var result = Part.TryParse( input.AsSpan(), null, out var part, out var tail );

    result.Should()
          .BeFalse();
  }

  [Fact]
  public void TryParse_WithOnlyCommas_ShouldReturnTrueAndEmptyPart()
  {
    var input = ", , ,".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .BeEmpty();

    // Since no events were parsed, tail should be the trimmed input.
    tail.ToString()
        .Should()
        .Be( ", , ," );
  }

  #endregion
}
