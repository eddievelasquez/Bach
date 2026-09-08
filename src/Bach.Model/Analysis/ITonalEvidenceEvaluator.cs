// Module Name: ITonalEvidenceEvaluator.cs
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
///   Evaluates tonal evidence for a candidate key during tonal evaluation.
/// </summary>
/// <remarks>
///   Evaluators are evaluated in descending <see cref="Priority"/> order. Priority does not suppress
///   lower-priority evaluators; it establishes deterministic evidence order and precedence for aggregation.
/// </remarks>
public interface ITonalEvidenceEvaluator
{
  #region Properties

  /// <summary>
  ///   Gets the evaluator   priority. Higher values run first.
  /// </summary>
  int Priority { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Evaluates one candidate and returns its evidence.
  /// </summary>
  /// <param name="context">The immutable candidate evaluation context.</param>
  /// <returns>The evidence discovered by this provider.</returns>
  IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context );

  #endregion
}
