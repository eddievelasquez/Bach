// Module Name: PartEventScopeTests.cs
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

namespace Bach.Model.Structure.Test;

public class PartEventScopeTests
{
  #region Public Methods

  [Fact]
  public void Events_Should_CalculateSelectedRange()
  {
    var part = Part.Parse( "C4,D4,E4" );

    var scope = new PartEventScope( part, 1..3 );

    ReferenceEquals( scope.Source, part )
      .Should()
      .BeTrue();

    scope.Events.Should()
         .Equal( Pitch.Parse( "D4" ), Pitch.Parse( "E4" ) );
  }

  [Fact]
  public void Events_Should_SupportFromEndRange()
  {
    var part = Part.Parse( "C4,D4,E4" );

    var scope = new PartEventScope( part, ^2.. );

    scope.Events.Should()
         .Equal( Pitch.Parse( "D4" ), Pitch.Parse( "E4" ) );
  }

  [Fact]
  public void Scope_Should_DefaultToWholePart()
  {
    var part = Part.Parse( "C4,D4" );

    var scope = new PartEventScope( part );

    scope.Events.Should()
         .Equal( part.Events );
  }

  [Fact]
  public void Scope_Should_RejectInvalidRange()
  {
    var part = Part.Parse( "C4" );

    Action action = () => new PartEventScope( part, ..2 );

    action.Should()
          .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Locations_Should_IdentifyMeasureAndEventIndexes()
  {
    var part = Part.Parse( "C4,D4|E4" );

    var locations = new PartEventScope(part).Locations;

    locations.Should().Equal(
      new PartEventLocation(0, 0),
      new PartEventLocation(0, 1),
      new PartEventLocation(1, 0));
  }

  #endregion
}
