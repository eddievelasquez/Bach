// Module Name: TonalEvidenceEvaluatorPipelineBuilder.cs
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

namespace Bach.Model.Analysis;

/// <summary>
/// Builder class for constructing a <see cref="TonalEvidenceEvaluatorPipeline"/> with a customizable set of
/// <see cref="ITonalEvidenceEvaluator"/> instances.
/// </summary>
public class TonalEvidenceEvaluatorPipelineBuilder
{
  #region Fields

  private readonly List<ITonalEvidenceEvaluator> _evaluators = [];

  #endregion

  #region Public Methods

  /// <summary>
  /// Adds the default set of tonal evidence evaluators to the pipeline builder.
  /// </summary>
  /// <returns>The current instance of <see cref="TonalEvidenceEvaluatorPipelineBuilder"/>.</returns>
  public TonalEvidenceEvaluatorPipelineBuilder AddDefaultEvaluators()
  {
    AddEvaluator<PitchClassEvidenceEvaluator>();
    AddEvaluator<ChordEvidenceEvaluator>();
    AddEvaluator<AppliedFunctionEvidenceEvaluator>();
    AddEvaluator<TonalCenterEvidenceEvaluator>();

    return this;
  }

  /// <summary>
  /// Adds a new <see cref="ITonalEvidenceEvaluator"/> of type <typeparamref name="T"/> to the pipeline builder.
  /// </summary>
  /// <typeparam name="T">The type of the tonal evidence evaluator to add.</typeparam>
  /// <returns>The current instance of <see cref="TonalEvidenceEvaluatorPipelineBuilder"/>.</returns>
  public TonalEvidenceEvaluatorPipelineBuilder AddEvaluator<T>()
    where T: ITonalEvidenceEvaluator, new()
  {
    _evaluators.Add( new T() );
    return this;
  }

  /// <summary>
  /// Adds a new <see cref="ITonalEvidenceEvaluator"/> instance to the pipeline builder.
  /// </summary>
  /// <param name="evaluator">The evaluator to add.</param>
  /// <returns>The current instance of <see cref="TonalEvidenceEvaluatorPipelineBuilder"/>.</returns>
  public TonalEvidenceEvaluatorPipelineBuilder AddEvaluator(
    ITonalEvidenceEvaluator evaluator )
  {
    ArgumentNullException.ThrowIfNull( evaluator );
    _evaluators.Add( evaluator );
    return this;
  }

  /// <summary>
  /// Builds and returns a <see cref="TonalEvidenceEvaluatorPipeline"/> instance with the configured evaluators.
  /// </summary>
  /// <returns>The constructed <see cref="TonalEvidenceEvaluatorPipeline"/> instance.</returns>
  /// <exception cref="InvalidOperationException">Thrown if no evaluators have been added.</exception>
  public TonalEvidenceEvaluatorPipeline Build()
  {
    if( _evaluators.Count == 0 )
    {
      throw new InvalidOperationException( "At least one evidence evaluator is required." );
    }

    // Order the evaluators by descending priority to ensure that higher-priority evaluators are processed first.
    var ordered = _evaluators.OrderByDescending( e => e.Priority )
                             .ToArray();

    return new TonalEvidenceEvaluatorPipeline( ordered );
  }

  #endregion
}
