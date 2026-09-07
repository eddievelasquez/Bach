// Module Name: TonalAnalysisResultSet.cs
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
using Bach.Model.Internal;

namespace Bach.Model.Analysis;

/// <summary>
///   Contains an ordered set of ranked tonal analysis candidates.
/// </summary>
/// <remarks>
///   Candidates are immutable after construction. This type represents candidate results only;
///   an inconclusive result is represented by <see cref="InconclusiveTonalAnalysisResult"/>.
/// </remarks>
public sealed record class TonalAnalysisResultSet: TonalAnalysisResult
{
  #region Constructors

  /// <summary>
  ///   Initializes a result set from ranked candidates.
  /// </summary>
  /// <param name="candidates">The candidates in rank order.</param>
  /// <param name="scope">The scope of the part events.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="candidates"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when the collection contains a null candidate.</exception>
  public TonalAnalysisResultSet(
    IEnumerable<RankedTonalCandidateResult> candidates,
    PartEventScope scope )
    : base( scope )
  {
    ArgumentNullException.ThrowIfNull( candidates );

    var copiedCandidates = candidates.ToArray();
    ArgumentException.ThrowIfContainsNulls( copiedCandidates, "Candidates contains a null result." );

    Candidates = Array.AsReadOnly( copiedCandidates );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the candidates in their analysis rank order.
  /// </summary>
  public IReadOnlyList<RankedTonalCandidateResult> Candidates { get; }

  /// <summary>
  ///   Gets the number of candidates in the result set.
  /// </summary>
  public int Count => Candidates.Count;

  #endregion
}
