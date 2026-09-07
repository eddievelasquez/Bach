// Module Name: TonalAnalysisResultBuilder.cs
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
using Bach.Model.Internal;

namespace Bach.Model.Analysis;

/// <summary>
///   Builds one ranked or inconclusive tonal analysis result.
/// </summary>
/// <remarks>
///   This builder constructs result data. It does not infer tonality or evaluate evidence.
/// </remarks>
public sealed class TonalAnalysisResultBuilder
{
  #region Fields

  private readonly PartEventScope _scope;
  private readonly List<EvidenceReason> _supportingEvidence = [];
  private readonly List<EvidenceReason> _conflictingEvidence = [];

  private Key? _key;
  private PitchClass? _tonic;
  private int _rank;
  private double _confidence;
  private TonalCandidateStatus _status = TonalCandidateStatus.Accepted;
  private string? _inconclusiveReason;
  private bool _isInconclusive;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a builder for the specified ordered analysis scope.
  /// </summary>
  /// <param name="scope">The immutable part and range examined by the analysis.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="scope"/> is null.</exception>
  public TonalAnalysisResultBuilder(
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
  public TonalAnalysisResultBuilder(
    Part source,
    Range range )
    : this( new PartEventScope( source, range ) )
  {
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Sets the candidate tonic and key.
  /// </summary>
  /// <param name="tonic">The spelled candidate tonic.</param>
  /// <param name="key">The key represented by the candidate.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when the tonic does not match <see cref="Key.Tonic"/>.</exception>
  public TonalAnalysisResultBuilder For(
    PitchClass tonic,
    Key key )
  {
    ArgumentNullException.ThrowIfNull( key );
    ArgumentException.ThrowIfNotEqualTo( tonic, key.Tonic, "The candidate tonic must agree with Key.Tonic." );

    _tonic = tonic;
    _key = key;
    _isInconclusive = false;
    return this;
  }

  /// <summary>
  ///   Sets the one-based rank for the candidate.
  /// </summary>
  /// <param name="rank">The candidate rank. Lower values are better.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="rank"/> is less than one.</exception>
  public TonalAnalysisResultBuilder WithRank(
    int rank )
  {
    if( rank < 1 )
    {
      throw new ArgumentOutOfRangeException( nameof( rank ), "Rank must be positive." );
    }

    _rank = rank;
    return this;
  }

  /// <summary>
  ///   Sets the candidate confidence score.
  /// </summary>
  /// <param name="confidence">A score from 0.0 through 1.0.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the score is outside the inclusive range 0.0 through 1.0.</exception>
  public TonalAnalysisResultBuilder WithConfidence(
    double confidence )
  {
    ArgumentOutOfRangeException.ThrowIfOutOfRange( confidence, 0.0, 1.0, "Confidence must be between 0.0 and 1.0." );

    _confidence = confidence;
    return this;
  }

  /// <summary>
  ///   Adds supporting evidence for the candidate.
  /// </summary>
  /// <param name="evidence">The supporting evidence.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="evidence"/> is null.</exception>
  public TonalAnalysisResultBuilder Supporting(
    EvidenceReason evidence )
  {
    ArgumentNullException.ThrowIfNull( evidence );
    _supportingEvidence.Add( evidence );
    return this;
  }

  /// <summary>
  ///   Adds conflicting evidence for the candidate or inconclusive result.
  /// </summary>
  /// <param name="evidence">The conflicting evidence.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="evidence"/> is null.</exception>
  public TonalAnalysisResultBuilder Conflicting(
    EvidenceReason evidence )
  {
    ArgumentNullException.ThrowIfNull( evidence );
    _conflictingEvidence.Add( evidence );
    return this;
  }

  /// <summary>
  ///   Sets the status for the candidate.
  /// </summary>
  /// <param name="status">The candidate status.</param>
  /// <returns>This builder.</returns>
  public TonalAnalysisResultBuilder WithStatus(
    TonalCandidateStatus status )
  {
    _status = status;
    return this;
  }

  /// <summary>
  ///   Configures this builder to create an inconclusive result.
  /// </summary>
  /// <param name="reason">An optional explanation for the inconclusive result.</param>
  /// <returns>This builder.</returns>
  public TonalAnalysisResultBuilder Inconclusive(
    string? reason = null )
  {
    _isInconclusive = true;
    _inconclusiveReason = reason;
    return this;
  }

  /// <summary>
  ///   Creates the configured tonal analysis result.
  /// </summary>
  /// <returns>A ranked candidate result or an inconclusive result.</returns>
  /// <exception cref="InvalidOperationException">
  ///   Thrown when the builder does not contain enough data for the selected
  ///   result.
  /// </exception>
  public TonalAnalysisResult Build()
  {
    if( _isInconclusive )
    {
      return new InconclusiveTonalAnalysisResult( _scope, _inconclusiveReason, _conflictingEvidence );
    }

    if( _tonic is null || _key is null )
    {
      throw new InvalidOperationException( "A candidate tonic and key are required." );
    }

    return new RankedTonalCandidateResult(
      _rank,
      _tonic.Value,
      _key,
      _confidence,
      _supportingEvidence,
      _conflictingEvidence,
      _scope,
      _status
    );
  }

  #endregion
}
