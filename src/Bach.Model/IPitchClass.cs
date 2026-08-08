// Module Name: IPitchClass.cs
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

namespace Bach.Model;

/// <summary>
///   Defines the shared contract for pitch-like values.
/// </summary>
/// <typeparam name="TSelf">The concrete pitch-like type.</typeparam>
public interface IPitchClass<TSelf>
  : IEquatable<TSelf>,
    IComparable<TSelf>,
    ISpanParsable<TSelf>,
    IFormattable
  where TSelf: IPitchClass<TSelf>
{
  #region Properties

  /// <summary>
  ///   Gets the note name of the pitch-like value.
  /// </summary>
  NoteName NoteName { get; }

  /// <summary>
  ///   Gets the accidental of the pitch-like value.
  /// </summary>
  Accidental Accidental { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a number of semitones to the current instance.
  /// </summary>
  /// <param name="semitoneCount">The number of semitones to transpose by.</param>
  /// <returns>The resulting pitch-like value after transposing by the semitones.</returns>
  TSelf Transpose(
    int semitoneCount );

  /// <summary>
  ///   Adds an interval to the current instance.
  /// </summary>
  /// <param name="interval">The interval to transpose by.</param>
  /// <returns>The resulting pitch-like value after transposing by the interval.</returns>
  TSelf Transpose(
    Interval interval );

  #endregion
}
