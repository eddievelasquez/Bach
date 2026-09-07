// Module Name: ChordEventTests.cs
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

namespace Bach.Model.Analysis.Test;

public sealed class ChordEventTests
{
  #region Public Methods

  [Fact]
  public void PitchChord_ShouldExposeChordMetadata_ViaIChordEvent()
  {
    var root = Pitch.Create( PitchClass.C, 4 );
    var chord = PitchChord.Create( root, ChordFormula.Major, 1 );

    ( chord as IChordEvent ).Should()
                            .NotBeNull();

    var chordEvent = (IChordEvent) chord;

    chordEvent.Root.Should()
              .Be( root );

    chordEvent.Inversion.Should()
              .Be( 1 );

    chordEvent.Formula.Should()
              .Be( ChordFormula.Major );

    chordEvent.Bass.Should()
              .Be( chord.Bass );
  }

  [Fact]
  public void InvertedChord_ShouldReportDistinctBass()
  {
    var root = Pitch.Create( PitchClass.G, 3 );
    var chord = PitchChord.Create( root, ChordFormula.Major, 2 );

    var chordEvent = (IChordEvent) chord;

    chordEvent.Bass.Should()
              .Be( Pitch.Create( PitchClass.D, 4 ) );
  }

  [Fact]
  public void Part_ShouldAllowPatternMatching_ForChordEvents()
  {
    var part = Part.Parse( "C4,!C" );

    IChordEvent? found = null;

    foreach( var ev in part )
    {
      if( ev is IChordEvent ce )
      {
        found = ce;
        break;
      }
    }

    found.Should()
         .NotBeNull();

    found!.Formula.Should()
          .Be( ChordFormula.Major );
  }

  #endregion
}
