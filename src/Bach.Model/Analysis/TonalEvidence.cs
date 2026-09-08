// Module Name: TonalEvidence.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Represents one supporting or conflicting observation produced by a tonal evidence provider.
/// </summary>
/// <param name="Reason">The explanation for the observation.</param>
/// <param name="Supports">Whether the observation supports the candidate.</param>
/// <param name="ScoreContribution">An optional score contribution before provider-priority weighting.</param>
public sealed record TonalEvidence(
  EvidenceReason Reason,
  bool Supports,
  double ScoreContribution = 0.0 )
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="TonalEvidence"/> record with the specified reason, support status,
  ///   and optional score contribution.
  /// </summary>
  /// <param name="category">The category of the evidence reason.</param>
  /// <param name="explanation">The explanation for the evidence reason.</param>
  /// <param name="supports">Whether the evidence supports the candidate.</param>
  /// <param name="scoreContribution">An optional score contribution before provider-priority weighting.</param>
  public TonalEvidence(
    EvidenceReasonCategory category,
    string explanation,
    bool supports,
    double scoreContribution = 0.0 )
    : this( new EvidenceReason( category, explanation ), supports, scoreContribution )
  {
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Validates the evidence value and its optional score contribution.
  /// </summary>
  public void Validate()
  {
    ArgumentNullException.ThrowIfNull( Reason );

    if( double.IsNaN( ScoreContribution ) || double.IsInfinity( ScoreContribution ) )
    {
      throw new ArgumentOutOfRangeException( nameof( ScoreContribution ), "The score contribution must be finite." );
    }
  }

  #endregion
}
