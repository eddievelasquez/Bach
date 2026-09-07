// Module Name: RankedTonalCandidateResult.cs
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
using Bach.Model.Internal;

namespace Bach.Model.Analysis;

/// <summary>
///   Represents a single ranked tonal candidate produced by an analysis pass.
/// </summary>
/// <remarks>
///   Rank is one-based and lower values indicate stronger candidates. Confidence is a value from
///   0.0 through 1.0. Evidence collections are copied during construction.
/// </remarks>
[DebuggerDisplay("{Rank} - {Key} ({Confidence})")]
public sealed record RankedTonalCandidateResult: TonalAnalysisResult
{
  #region Constructors

  /// <summary>
  ///   Initializes a ranked tonal candidate result.
  /// </summary>
  /// <param name="rank">The one-based candidate rank. Lower values indicate stronger candidates.</param>
  /// <param name="tonic">The spelled candidate tonic.</param>
  /// <param name="key">The key represented by the candidate.</param>
  /// <param name="confidence">The confidence score from 0.0 through 1.0.</param>
  /// <param name="supportingEvidence">Evidence that supports the candidate.</param>
  /// <param name="conflictingEvidence">Evidence that conflicts with the candidate.</param>
  /// <param name="scope">The ordered part events examined by the analysis.</param>
  /// <param name="status">The candidate status.</param>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when rank is less than one or confidence is outside 0.0 through
  ///   1.0.
  /// </exception>
  /// <exception cref="ArgumentNullException">Thrown when key or scope is null.</exception>
  /// <exception cref="ArgumentException">
  ///   Thrown when the tonic does not match the key tonic, or an evidence collection contains null.
  /// </exception>
  public RankedTonalCandidateResult(
    int rank,
    PitchClass tonic,
    Key key,
    double confidence,
    IReadOnlyList<EvidenceReason>? supportingEvidence,
    IReadOnlyList<EvidenceReason>? conflictingEvidence,
    PartEventScope scope,
    TonalCandidateStatus status = TonalCandidateStatus.Accepted )
    : base( scope )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( rank, 1 );
    ArgumentOutOfRangeException.ThrowIfOutOfRange( confidence, 0.0, 1.0, "Confidence must be between 0.0 and 1.0." );
    ArgumentNullException.ThrowIfNull( key );

    ArgumentException.ThrowIfNotEqualTo(
      key.Tonic,
      tonic,
      "The candidate tonic must agree with Key.Tonic.",
      nameof( tonic )
    );

    Rank = rank;
    Tonic = tonic;
    Key = key;
    Confidence = confidence;
    Status = status;

    SupportingEvidence = Array.AsReadOnly( [.. supportingEvidence ?? Array.Empty<EvidenceReason>()] );
    ConflictingEvidence = Array.AsReadOnly( [.. conflictingEvidence ?? Array.Empty<EvidenceReason>()] );

    ArgumentException.ThrowIfContainsNulls(
      SupportingEvidence,
      "SupportingEvidence contains a null.",
      nameof( supportingEvidence )
    );

    ArgumentException.ThrowIfContainsNulls(
      ConflictingEvidence,
      "ConflictingEvidence contains a null.",
      nameof( conflictingEvidence )
    );
  }

  #endregion

  #region Properties

  /// <summary>Rank (1-based) for the candidate. Lower is better.</summary>
  public int Rank { get; init; }

  /// <summary>Gets the spelled tonic for the candidate.</summary>
  public PitchClass Tonic { get; init; }

  /// <summary>
  ///   Gets the resolved key for the candidate, including its governing scale definition.
  /// </summary>
  public Key Key { get; init; }

  /// <summary>Gets the confidence score in the range 0.0 through 1.0.</summary>
  public double Confidence { get; init; }

  /// <summary>Gets the supporting evidence for the candidate. Never null.</summary>
  public IReadOnlyList<EvidenceReason> SupportingEvidence { get; init; }

  /// <summary>Gets the conflicting evidence for the candidate. Never null.</summary>
  public IReadOnlyList<EvidenceReason> ConflictingEvidence { get; init; }

  /// <summary>Gets the status for the candidate.</summary>
  public TonalCandidateStatus Status { get; init; }

  #endregion
}
