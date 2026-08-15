// Module Name: PitchClassCollection.cs
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

using System.Collections;
using System.Collections.Generic;

/// <summary>Collection of <see cref="PitchClass" />.</summary>
public abstract class PitchClassCollection
  : IReadOnlyList<PitchClass>
{
  #region Fields

  private readonly PitchClass[] _pitchClasses;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchClassCollection" /> class.
  /// </summary>
  /// <param name="pitchClasses">The collection of pitch classes.</param>
  protected PitchClassCollection(
    IEnumerable<PitchClass> pitchClasses )
  {
    ArgumentNullException.ThrowIfNull( pitchClasses );
    _pitchClasses = [.. pitchClasses];
  }

  #endregion

  #region Properties

  /// <inheritdoc />
  public int Count => _pitchClasses.Length;

  /// <inheritdoc />
  public PitchClass this[
    int index ] => _pitchClasses[index];

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public IEnumerator<PitchClass> GetEnumerator()
  {
    return ( (IEnumerable<PitchClass>) _pitchClasses ).GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <summary>
  ///   Returns the index of the specified pitch class in the collection.
  /// </summary>
  /// z
  /// <param name="pitchClass">The pitch class to search for.</param>
  /// <returns>The index of the pitch class, or -1 if not found.</returns>
  public int IndexOf(
    PitchClass pitchClass )
  {
    return Array.IndexOf( _pitchClasses, pitchClass );
  }

  /// <summary>Renders the collection as <see cref="Pitch" /> instances for the provided octave.</summary>
  /// <param name="octave">The octave to render from.</param>
  /// <returns>The rendered pitches.</returns>
  public abstract IEnumerable<Pitch> Render(
    int octave );

  #endregion
}
