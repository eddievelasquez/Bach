// Module Name: PitchClassEvidenceEvaluator.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Provides scale-membership evidence for observed pitch classes.
/// </summary>
public sealed class PitchClassEvidenceEvaluator: TonalEvidenceEvaluator
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchClassEvidenceEvaluator"/> class.
  /// </summary>
  public PitchClassEvidenceEvaluator()
  {
  }

  #endregion

  #region Public Methods


  /// <inheritdoc/>
  public override int Priority => 10;

  /// <inheritdoc/>
  public override IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context )
  {
    ArgumentNullException.ThrowIfNull( context );

    var degrees = GetDegrees( context.CandidateKey.Scale );

    foreach( var pitchClass in context.UniquePitchClasses )
    {
      var degree = Array.IndexOf( degrees, pitchClass );

      yield return new TonalEvidence(
        EvidenceReasonCategory.ScaleContext,
        degree >= 0
          ? $"Pitch class {pitchClass} is scale degree {degree + 1} in the candidate scale."
          : $"Pitch class {pitchClass} is outside the candidate scale.",
        degree >= 0
      );
    }
  }

  #endregion
}
