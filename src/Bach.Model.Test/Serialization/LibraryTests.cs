// Module Name: LibraryTest.cs
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

using System.Text.Json;
using Bach.Model.Serialization;

namespace Bach.Model.Test.Serialization;

public sealed class LibraryTests
{
  #region Public Methods

  [Fact]
  public void Library_ShouldRoundTripNestedPersistentData()
  {
    var expected = new Library(
      new Version( 1, 0 ),
      [new PersistentScale( "major", "Major", "W-W-H-W-W-W-H",  "Ionian", "Diatonic;Major" )],
      [new PersistentChord( "maj", "Major", "R,M3,5", "Δ" )],
      [
        new PersistentStringedInstrument(
          "guitar",
          "Guitar",
          6,
          [new PersistentTuning( "standard", "Standard", "E2,A2,D3,G3,B3,E4" )]
        )
      ]
    );

    var json = JsonSerializer.Serialize( expected );
    var actual = JsonSerializer.Deserialize<Library>( json );

    actual.Should()
          .NotBeNull();

    actual!.Version.Should()
           .Be( expected.Version );

    actual.Scales.Should()
          .ContainSingle()
          .Which.Alias.Should()
          .Be( "Ionian" );

    actual.Chords.Should()
          .ContainSingle()
          .Which.Symbol.Should()
          .Be( "Δ" );

    actual.StringedInstruments.Should()
          .ContainSingle()
          .Which.Tunings.Should()
          .ContainSingle()
          .Which.Pitches.Should()
          .Be( "E2,A2,D3,G3,B3,E4" );
  }

  [Fact]
  public void PersistentScale_ShouldPreserveNullOptionalValuesWhenSerializing()
  {
    var expected = new PersistentScale( "modes", "Modes", "W-W-H-W-W-W-H" );

    var json = JsonSerializer.Serialize( expected );
    var actual = JsonSerializer.Deserialize<PersistentScale>( json );

    actual.Should()
          .NotBeNull();

    actual.Alias.Should()
           .BeNull();

    actual.Categories.Should()
          .BeNull();
  }

  #endregion
}
