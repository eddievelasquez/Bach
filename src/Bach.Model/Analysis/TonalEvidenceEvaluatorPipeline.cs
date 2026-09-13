// Module Name: TonalEvidenceEvaluatorPipeline.cs
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
using System.Diagnostics;
using System.Linq;

namespace Bach.Model.Analysis;

/// <summary>
///   Represents a pipeline of tonal evidence evaluators that can be used to evaluate tonal evidence in a given context.
/// </summary>
public class TonalEvidenceEvaluatorPipeline
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="TonalEvidenceEvaluatorPipeline"/> class with the specified
  ///   evaluators.
  /// </summary>
  /// <param name="evaluators">The list of tonal evidence evaluators to include in the pipeline.</param>
  /// <remarks>
  ///   The evaluators are ordered by descending priority, and the maximum priority is calculated.
  /// </remarks>
  internal TonalEvidenceEvaluatorPipeline(
    IReadOnlyList<ITonalEvidenceEvaluator> evaluators )
  {
    ArgumentNullException.ThrowIfNull( evaluators );

    Evaluators = evaluators;
    MaxPriority = Evaluators.Max( e => e.Priority );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the list of tonal evidence evaluators included in the pipeline.
  /// </summary>
  public IReadOnlyList<ITonalEvidenceEvaluator> Evaluators { get; }

  /// <summary>
  ///   Gets the maximum priority value among the evaluators in the pipeline.
  /// </summary>
  /// <remarks>
  ///   The maximum priority is used to normalize the score contributions of individual evaluators.
  /// </remarks>
  public int MaxPriority { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Evaluates tonal evidence in the given context using the evaluators in the pipeline.
  /// </summary>
  /// <param name="context">The context in which to evaluate tonal evidence.</param>
  /// <returns>
  ///   An enumerable of tuples containing the tonal evidence and its corresponding score.
  /// </returns>
  /// <exception cref="InvalidOperationException">Thrown if an evaluator returns null.</exception>
  public IEnumerable<(TonalEvidence Evidence, double WeightedScore)> Evaluate(
    TonalEvidenceContext context )
  {
    // Iterate through each evaluator in the pipeline
    foreach( var evaluator in Evaluators )
    {
      // Evaluate the context using the current evaluator and yield the results
      foreach( var evidence in evaluator.Evaluate( context ) )
      {
        Debug.Assert( evidence is not null, "An tonal evidence evaluator returned null evidence." );

        // Calculate the score for the evidence based on its contribution and the evaluator's priority
        var score = evidence.ScoreContribution * evaluator.Priority / MaxPriority;
        yield return ( evidence, score );
      }
    }
  }

  #endregion
}
