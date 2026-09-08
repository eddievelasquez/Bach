// Module Name: TonalEvidenceContext.cs
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
///   Provides immutable context to a tonal evidence provider for one candidate key.
/// </summary>
public sealed class TonalEvidenceContext
{
  #region Constructors

  /// <summary>
  ///   Initializes an evidence context.
  /// </summary>
  public TonalEvidenceContext(
    Key candidateKey,
    PartEventScope scope,
    IEnumerable<IPartEvent> events,
    IEnumerable<PitchClass> uniquePitchClasses,
    TonalEvaluationOptions options )
  {
    ArgumentNullException.ThrowIfNull( candidateKey );
    ArgumentNullException.ThrowIfNull( scope );
    ArgumentNullException.ThrowIfNull( events );
    ArgumentNullException.ThrowIfNull( uniquePitchClasses );
    ArgumentNullException.ThrowIfNull( options );

    CandidateKey = candidateKey;
    Scope = scope;
    Events = Array.AsReadOnly( [.. events] );
    UniquePitchClasses = Array.AsReadOnly( [.. uniquePitchClasses.Distinct()] );
    Options = options;
  }

  #endregion

  #region Properties

  /// <summary>Gets the candidate key being evaluated.</summary>
  public Key CandidateKey { get; }

  /// <summary>Gets the ordered event scope being evaluated.</summary>
  public PartEventScope Scope { get; }

  /// <summary>Gets the immutable events in the scope.</summary>
  public IReadOnlyList<IPartEvent> Events { get; }

  /// <summary>Gets the immutable unique pitch classes observed in the scope.</summary>
  public IReadOnlyList<PitchClass> UniquePitchClasses { get; }

  /// <summary>Gets the validated evaluation options.</summary>
  public TonalEvaluationOptions Options { get; }

  #endregion
}
