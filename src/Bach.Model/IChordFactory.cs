// Module Name: IChordFactory.cs
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
///   Represents a factory for creating chords with a specific root element type.
/// </summary>
/// <typeparam name="TChord">The type of the chord itself.</typeparam>
/// <typeparam name="TPitch">The type of the chord's root and bass elements.</typeparam>
public interface IChordFactory<out TChord, in TPitch>
  where TChord: PitchCollection<TPitch>, IChord<TChord, TPitch>, IChordFactory<TChord, TPitch>
  where TPitch: IPitch<TPitch>
{
  #region Public Methods

  /// <summary>
  ///   Creates a new chord with the specified root, formula, and inversion.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion of the chord. Defaults to zero.</param>
  /// <returns>The created chord.</returns>
  static abstract TChord Create(
    TPitch root,
    ChordFormula formula,
    int inversion = 0 );

  /// <summary>
  ///   Creates a new chord with the specified root, formula ID or name, and inversion.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  /// <param name="inversion">The inversion of the chord. Defaults to zero.</param>
  /// <returns>The created chord.</returns>
  static abstract TChord Create(
    TPitch root,
    string formulaIdOrName,
    int inversion = 0 );

  #endregion
}
