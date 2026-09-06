// Module Name: Suspension.cs
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
///   Represents a prepared tone that resolves after a harmony changes.
/// </summary>
public sealed record Suspension: NonChordTone
{
  #region Constructors

  /// <summary>
  ///   Initializes a suspension.
  /// </summary>
  /// <param name="target">The suspended event or pitch.</param>
  /// <param name="preparation">The event or pitch that prepares the suspension.</param>
  /// <param name="resolution">The event or pitch that resolves the suspension.</param>
  /// <param name="evidence">The reason that supports the classification.</param>
  public Suspension(
    AnalysisTarget target,
    AnalysisTarget preparation,
    AnalysisTarget resolution,
    EvidenceReason evidence )
    : base( target, evidence )
  {
    Preparation = preparation ?? throw new ArgumentNullException( nameof( preparation ) );
    Resolution = resolution ?? throw new ArgumentNullException( nameof( resolution ) );
  }

  #endregion

  #region Properties

  /// <summary>Gets the event or pitch that prepares the suspension.</summary>
  public AnalysisTarget Preparation { get; }

  /// <summary>Gets the event or pitch that resolves the suspension.</summary>
  public AnalysisTarget Resolution { get; }

  #endregion
}
