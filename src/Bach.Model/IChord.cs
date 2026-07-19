// Module Name: IChord.cs
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
/// Represents a chord with a specific root element type.
/// </summary>
/// <typeparam name="TSelf">The type of the chord itself.</typeparam>
/// <typeparam name="T">The type of the chord's root and bass elements.</typeparam>
public interface IChord<out TSelf, out T>
  where TSelf: IChord<TSelf, T>
  where T: IPitchClass<T>
{
  #region Properties

  /// <summary>
  ///   Gets the root of the chord.
  /// </summary>
  T Root { get; }

  /// <summary>
  ///   Gets the bass of the chord.
  /// </summary>
  T Bass { get; }

  /// <summary>
  ///   Gets the inversion number of the chord.
  /// </summary>
  int Inversion { get; }

  /// <summary>
  ///   Gets the chord formula.
  /// </summary>
  ChordFormula Formula { get; }

  /// <summary>
  ///   Gets the display name of the chord.
  /// </summary>
  string Name { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Creates an inversion of the current chord.
  /// </summary>
  /// <param name="inversion">The inversion to create.</param>
  /// <returns>An inverted chord.</returns>
  TSelf GetInversion(
    int inversion );

  #endregion
}
