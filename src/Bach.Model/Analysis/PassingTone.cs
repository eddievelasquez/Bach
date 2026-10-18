// Module Name: PassingTone.cs
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
///   Represents a non-chord tone that moves by step between surrounding tones.
/// </summary>
public sealed record PassingTone: NonChordTone
{
  #region Constructors

  /// <summary>
  ///   Initializes a passing tone.
  /// </summary>
  /// <param name="target">The passing tone event or pitch.</param>
  /// <param name="direction">The direction of the passing motion.</param>
  /// <param name="evidence">The reason that supports the classification.</param>
  public PassingTone(
    AnalysisTarget target,
    MelodicDirection direction,
    EvidenceReason evidence )
    : base( target, evidence )
  {
    Direction = direction;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the direction of the passing motion.
  /// </summary>
  public MelodicDirection Direction { get; }

  #endregion
}
