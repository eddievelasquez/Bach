// Module Name: InconclusiveTonalAnalysisResult.cs
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
///   Represents an inconclusive tonal analysis result.
/// </summary>
/// <remarks>
///   This result has no accepted tonic or key. It records the scope and any evidence that prevents
///   a conclusive result. Evidence collections are copied during construction.
/// </remarks>
public sealed record InconclusiveTonalAnalysisResult: TonalAnalysisResult
{
  #region Constructors

  /// <summary>
  ///   Initializes an inconclusive tonal analysis result.
  /// </summary>
  /// <param name="scope">The ordered part events examined by the analysis.</param>
  /// <param name="reason">An optional explanation for the inconclusive result.</param>
  /// <param name="conflictingEvidence">Evidence that prevents a conclusive result.</param>
  /// <exception cref="ArgumentNullException">Thrown when scope is null.</exception>
  /// <exception cref="ArgumentException">Thrown when scope or evidence contains a null item.</exception>
  public InconclusiveTonalAnalysisResult(
    PartEventScope scope,
    string? reason = null,
    IReadOnlyList<EvidenceReason>? conflictingEvidence = null )
    : base( scope )
  {
    Reason = reason;
    ConflictingEvidence = Array.AsReadOnly( [.. conflictingEvidence ?? Array.Empty<EvidenceReason>()] );

    if (ConflictingEvidence.Any(e => e is null))
    {
      throw new ArgumentException( "ConflictingEvidence cannot be null.", nameof( conflictingEvidence ) );
    }
  }

  #endregion

  #region Properties

  /// <summary>Gets the optional human-readable reason for the inconclusive result.</summary>
  public string? Reason { get; init; }

  /// <summary>
  ///   Gets the evidence that conflicts with or prevents a conclusive result. Never null.
  /// </summary>
  public IReadOnlyList<EvidenceReason> ConflictingEvidence { get; init; }

  #endregion
}
