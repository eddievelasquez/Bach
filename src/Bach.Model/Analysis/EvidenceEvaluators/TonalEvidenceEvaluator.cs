// Module Name: TonalEvidenceEvaluator.cs
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
///   Provides a base class for tonal evidence providers that evaluate pitch classes against a candidate scale.
/// </summary>
public abstract class TonalEvidenceEvaluator: ITonalEvidenceEvaluator
{
  #region Properties

  /// <inheritdoc/>
  public abstract int Priority { get; }

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public abstract IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context );

  #endregion

  #region Implementation

  /// <summary>
  ///   Returns the scale degrees of the given scale as an array of pitch classes.
  /// </summary>
  /// <param name="scale">The scale to get the degrees from.</param>
  /// <returns>An array of pitch classes representing the scale degrees.</returns>
  protected static PitchClass[] GetDegrees(
    Scale scale )
  {
    return
    [
      .. scale.GetAscending()
              .Take( scale.Formula.AscendingDegrees.Count )
    ];
  }

  #endregion
}
