// Module Name: AppliedFunctionEvidenceEvaluator.cs
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

namespace Bach.Model.Analysis.Internal;

/// <summary>
///   Provides evidence from annotated applied harmonic functions.
/// </summary>
internal sealed class AppliedFunctionEvidenceEvaluator: TonalEvidenceEvaluator
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="AppliedFunctionEvidenceEvaluator"/> class.
  /// </summary>
  public AppliedFunctionEvidenceEvaluator()
  {
  }

  #endregion

  #region Properties

  /// <inheritdoc/>
  public override int Priority => 30;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public override IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context )
  {
    ArgumentNullException.ThrowIfNull( context );
    var scale = context.CandidateKey.Scale;

    foreach( var appliedFunction in context.Scope.AppliedFunctions )
    {
      var degrees = GetDegrees( scale );
      var degreeIndex = appliedFunction.TargetDegree.Degree - 1;

      var supported = degreeIndex >= 0
                      && degreeIndex < degrees.Length
                      && appliedFunction.Target.PartEvent.Any( degrees[degreeIndex] );

      yield return new TonalEvidence(
        EvidenceReasonCategory.HarmonicContext,
        $"{appliedFunction.Label}: {appliedFunction.Evidence.Explanation}",
        supported
      );
    }
  }

  #endregion
}
