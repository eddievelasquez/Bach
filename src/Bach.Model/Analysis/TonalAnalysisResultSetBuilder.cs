// Module Name: TonalAnalysisResultSetBuilder.cs
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
///   Builds an ordered set of ranked tonal analysis candidates.
/// </summary>
/// <remarks>
///   This builder constructs result data. It does not infer tonality or evaluate evidence.
/// </remarks>
public sealed class TonalAnalysisResultSetBuilder
{
  #region Fields

  private readonly PartEventScope _scope;
  private readonly List<RankedTonalCandidateResult> _candidates = [];

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a builder for the specified ordered analysis scope.
  /// </summary>
  /// <param name="scope">The immutable part and range examined by the analysis.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="scope"/> is null.</exception>
  public TonalAnalysisResultSetBuilder(
    PartEventScope scope )
  {
    _scope = scope ?? throw new ArgumentNullException( nameof( scope ) );
  }

  /// <summary>
  ///   Initializes a builder for a range in a part.
  /// </summary>
  /// <param name="source">The immutable source part.</param>
  /// <param name="range">The start-inclusive and end-exclusive event range.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is invalid for the source.</exception>
  public TonalAnalysisResultSetBuilder(
    Part source,
    Range range )
    : this( new PartEventScope( source, range ) )
  {
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a candidate configured by a child builder.
  /// </summary>
  /// <param name="configure">The callback that configures the candidate.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
  /// <exception cref="ArgumentException">
  ///   Thrown when the callback creates an inconclusive result or a candidate with a different scope.
  /// </exception>
  /// <exception cref="InvalidOperationException">Thrown when the candidate rank is not greater than the previous rank.</exception>
  public TonalAnalysisResultSetBuilder AddCandidate(
    Action<TonalAnalysisResultBuilder> configure )
  {
    ArgumentNullException.ThrowIfNull( configure );

    var candidateBuilder = new TonalAnalysisResultBuilder( _scope );
    configure( candidateBuilder );

    if( candidateBuilder.Build() is not RankedTonalCandidateResult candidate )
    {
      throw new ArgumentException( "The callback must configure a ranked candidate.", nameof( configure ) );
    }

    return AddCandidate( candidate );
  }

  /// <summary>
  ///   Adds an already-built candidate.
  /// </summary>
  /// <param name="candidate">The candidate to add.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="candidate"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when the candidate does not use this builder's scope.</exception>
  /// <exception cref="InvalidOperationException">Thrown when the candidate rank is not greater than the previous rank.</exception>
  public TonalAnalysisResultSetBuilder AddCandidate(
    RankedTonalCandidateResult candidate )
  {
    ArgumentNullException.ThrowIfNull( candidate );

    if( !ReferenceEquals( candidate.Scope.Source, _scope.Source ) || !candidate.Scope.Range.Equals( _scope.Range ) )
    {
      throw new ArgumentException( "The candidate must use the builder's analysis scope.", nameof( candidate ) );
    }

    if( _candidates.Count > 0 && candidate.Rank <= _candidates[^1].Rank )
    {
      throw new InvalidOperationException( "Candidate ranks must be strictly increasing." );
    }

    _candidates.Add( candidate );
    return this;
  }

  /// <summary>
  ///   Creates the immutable result set.
  /// </summary>
  /// <returns>The candidates in rank order.</returns>
  /// <exception cref="InvalidOperationException">Thrown when no candidates have been added.</exception>
  public TonalAnalysisResultSet Build()
  {
    if( _candidates.Count == 0 )
    {
      throw new InvalidOperationException( "At least one candidate is required." );
    }

    return new TonalAnalysisResultSet( _candidates, _scope );
  }

  /// <summary>
  ///   Creates an explicit inconclusive result for this builder's scope.
  /// </summary>
  /// <param name="reason">An optional explanation for the inconclusive result.</param>
  /// <param name="conflictingEvidence">Evidence that prevents a conclusive result.</param>
  /// <returns>An inconclusive tonal analysis result.</returns>
  /// <exception cref="InvalidOperationException">Thrown when candidates have already been added.</exception>
  public InconclusiveTonalAnalysisResult BuildInconclusive(
    string? reason = null,
    IEnumerable<EvidenceReason>? conflictingEvidence = null )
  {
    if( _candidates.Count > 0 )
    {
      throw new InvalidOperationException( "An inconclusive result cannot be combined with candidates." );
    }

    return new InconclusiveTonalAnalysisResult( _scope, reason, conflictingEvidence?.ToArray() );
  }

  #endregion
}
