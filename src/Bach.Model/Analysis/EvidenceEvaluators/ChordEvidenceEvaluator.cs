// Module Name: ChordEvidenceEvaluator.cs
// Project:     Bach.Model
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

namespace Bach.Model.Analysis.EvidenceEvaluators;

/// <summary>
///   Provides harmonic evidence from chord events.
/// </summary>
internal sealed class ChordEvidenceEvaluator: TonalEvidenceEvaluator
{
  #region Constructors

  /// <summary>Initializes the provider with the default chord-evidence priority.</summary>
  public ChordEvidenceEvaluator(
    int priority = 20 )
    : base( priority )
  {
  }

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public override IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context )
  {
    ArgumentNullException.ThrowIfNull( context );
    var scale = context.CandidateKey.Scale;

    foreach( var chordEvent in context.Events.OfType<IChordEvent>() )
    {
      var rootMatches = scale.Contains( chordEvent.Root.PitchClass );
      var bassMatches = scale.Contains( chordEvent.Bass.PitchClass );
      var chordMatches = chordEvent.PitchClasses.All( scale.Contains );

      yield return Evidence(
        rootMatches,
        $"Chord root {chordEvent.Root.PitchClass} is in the candidate scale.",
        $"Chord root {chordEvent.Root.PitchClass} conflicts with the candidate."
      );

      yield return Evidence(
        bassMatches,
        $"Chord bass {chordEvent.Bass.PitchClass} is in the candidate scale.",
        $"Chord bass {chordEvent.Bass.PitchClass} conflicts with the candidate."
      );

      yield return Evidence(
        chordMatches,
        $"Chord formula {chordEvent.Formula.Name} is contained by the candidate scale.",
        $"Chord formula {chordEvent.Formula.Name} conflicts with the candidate."
      );

      if( chordEvent.Inversion == 0 && chordEvent.Bass.PitchClass == chordEvent.Root.PitchClass )
      {
        yield return Supporting( "The chord is in root position." );
      }
      else if( chordEvent.Inversion > 0 )
      {
        yield return Supporting( $"The chord inversion is {chordEvent.Inversion}." );
      }
    }
  }

  #endregion

  #region Implementation

  private static TonalEvidence Evidence(
    bool supports,
    string supporting,
    string conflicting )
  {
    return new TonalEvidence(
      EvidenceReasonCategory.HarmonicContext,
      supports ? supporting : conflicting,
      supports
    );
  }

  private static TonalEvidence Supporting(
    string explanation )
  {
    return new TonalEvidence( EvidenceReasonCategory.HarmonicContext, explanation, true );
  }

  #endregion
}
