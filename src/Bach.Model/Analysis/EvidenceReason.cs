// Module Name: EvidenceReason.cs
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
///   Describes why an analysis item is supported by the available musical evidence.
/// </summary>
public sealed record EvidenceReason
{
  #region Constructors

  /// <summary>
  ///   Initializes an evidence reason.
  /// </summary>
  /// <param name="category">The type of evidence.</param>
  /// <param name="explanation">A specific explanation for the evidence.</param>
  public EvidenceReason(
    EvidenceReasonCategory category,
    string explanation )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace( explanation );

    Category = category;
    Explanation = explanation;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the type of evidence.
  /// </summary>
  public EvidenceReasonCategory Category { get; }

  /// <summary>
  ///   Gets the specific explanation for the evidence.
  /// </summary>
  public string Explanation { get; }

  #endregion
}
