// Module Name: TonalEvidenceEvaluatorProvider.cs
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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Bach.Model.Analysis.EvidenceEvaluators;

/// <summary>
///   Default implementation of <see cref="ITonalEvidenceEvaluatorProvider"/> that provides a set of built-in tonal
///   evidence evaluators.
/// </summary>
public class TonalEvidenceEvaluatorProvider: ITonalEvidenceEvaluatorProvider
{
  #region Constants

  /// <summary>
  ///   Gets the default instance of <see cref="TonalEvidenceEvaluatorProvider"/> with the built-in tonal evidence
  ///   evaluators.
  /// </summary>
  public static readonly ITonalEvidenceEvaluatorProvider Default = new TonalEvidenceEvaluatorProvider();

  #endregion

  #region Fields

  private readonly ConcurrentBag<ITonalEvidenceEvaluator> _evaluators = [];

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="TonalEvidenceEvaluatorProvider"/> class with the default set of tonal
  ///   evidence evaluators.
  /// </summary>
  public TonalEvidenceEvaluatorProvider()
  {
    AddEvaluators(
      new PitchClassEvidenceEvaluator(),
      new ChordEvidenceEvaluator(),
      new AppliedFunctionEvidenceEvaluator(),
      new TonalCenterEvidenceEvaluator()
    );
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="TonalEvidenceEvaluatorProvider"/> class with the specified
  ///   evaluators.
  /// </summary>
  /// <param name="evaluators">The evaluators to add.</param>
  public TonalEvidenceEvaluatorProvider(
    params ReadOnlySpan<ITonalEvidenceEvaluator> evaluators )
  {
    AddEvaluators( evaluators );
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a new <see cref="ITonalEvidenceEvaluator"/> to the provider.
  /// </summary>
  /// <param name="evaluator"> The evaluator to add. </param>
  /// <returns> The current instance of <see cref="TonalEvidenceEvaluatorProvider"/>. </returns>
  public TonalEvidenceEvaluatorProvider AddEvaluator(
    ITonalEvidenceEvaluator evaluator )
  {
    ArgumentNullException.ThrowIfNull( evaluator );

    _evaluators.Add( evaluator );
    return this;
  }

  /// <summary>
  ///   Adds multiple <see cref="ITonalEvidenceEvaluator"/> instances to the provider.
  /// </summary>
  /// <param name="evaluators">The evaluators to add.</param>
  /// <returns>The current instance of <see cref="TonalEvidenceEvaluatorProvider"/>.</returns>
  public TonalEvidenceEvaluatorProvider AddEvaluators(
    params ReadOnlySpan<ITonalEvidenceEvaluator> evaluators )
  {
    foreach( var evaluator in evaluators )
    {
      AddEvaluator( evaluator );
    }

    return this;
  }

  /// <inheritdoc/>
  public IReadOnlyList<ITonalEvidenceEvaluator> GetEvaluators()
  {
    // Return a snapshot of the evaluators to ensure thread safety and avoid potential modification during enumeration.
    var snapshot = _evaluators.ToArray();

    // Ensure that at least one evaluator is present; otherwise, throw an exception.
    if( snapshot.Length == 0 )
    {
      throw new InvalidOperationException( "At least one evidence evaluator is required." );
    }

    // Order the evaluators by descending priority to ensure that higher-priority evaluators are processed first.
    var ordered = snapshot.OrderByDescending( e => e.Priority )
                          .ToArray();

    return Array.AsReadOnly( ordered );
  }

  #endregion
}
