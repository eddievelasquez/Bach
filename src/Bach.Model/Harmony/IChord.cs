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

using System.Collections.Generic;

namespace Bach.Model.Harmony;

/// <summary>
///   Defines a chord as an ordered list of pitch elements, with its root, bass, formula, inversion,
///   and display name. Implementations can use pitch classes or pitches as their elements.
/// </summary>
/// <typeparam name="TPitch">The type of the chord's root and bass elements.</typeparam>
public interface IChord<out TPitch>
  : IReadOnlyList<TPitch>
  where TPitch: struct, IPitch
{
  #region Properties

  /// <summary>
  ///   Gets the display name of the chord.
  /// </summary>
  string Name { get; }

  /// <summary>
  ///   Gets the chord formula.
  /// </summary>
  ChordFormula Formula { get; }

  /// <summary>
  ///   Gets the inversion number of the chord.
  /// </summary>
  int Inversion { get; }

  /// <summary>
  ///   Gets the root of the chord.
  /// </summary>
  TPitch Root { get; }

  /// <summary>
  ///   Gets the bass of the chord.
  /// </summary>
  TPitch Bass { get; }

  /// <summary>
  ///   Gets a value indicating whether the chord formula's highest interval extends beyond an octave,
  ///   as in a ninth or eleventh chord. A formula whose highest interval is a seventh is not extended.
  /// </summary>
  bool IsExtended { get; }

  #endregion
}
