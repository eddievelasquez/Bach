// Module Name: ScaleDegreeStep.cs
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

using System.Diagnostics;

namespace Bach.Model.Scales;

/// <summary>
///   Represents a single ordered step in a scale formula with spelling-aware information.
/// </summary>
[DebuggerDisplay( "{Ordinal}. {Interval}" )]
public readonly struct ScaleDegreeStep
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of <see cref="ScaleDegreeStep"/>.
  /// </summary>
  /// <param name="ordinal">The degree ordinal (1..n).</param>
  /// <param name="interval">The spelled interval to the next degree.</param>
  public ScaleDegreeStep(
    int ordinal,
    Interval interval )
  {
    if( ordinal < 1 )
    {
      throw new ArgumentOutOfRangeException( nameof( ordinal ), "Ordinal must be >= 1" );
    }

    Ordinal = ordinal;
    Interval = interval;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   The 1-based ordinal of the source degree. For an n-degree scale the final step will have
  ///   ordinal n and represents the transition from degree n to the next occurrence of degree 1.
  /// </summary>
  public int Ordinal { get; }

  /// <summary>
  ///   The spelled interval between the source degree and the next degree in the formula.
  /// </summary>
  public Interval Interval { get; }

  /// <summary>
  ///   Gets the semitone count of the spelled interval.
  /// </summary>
  public int SemitoneCount => Interval.SemitoneCount;

  /// <summary>
  ///   Gets the chromatic alteration relative to the interval's base quantity (e.g. major/perfect).
  ///   This is a convenience derived value and may be negative for diminished intervals.
  /// </summary>
  public int ChromaticAlteration => Interval.ChromaticAlteration;

  #endregion
}
