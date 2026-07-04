// Module Name: ChordChart.cs
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

using System.Collections.Generic;
using System.Linq;

/// <summary>
///   Represents a chord chart, which consists of a key and a chord progression.
/// </summary>
public sealed class ChordChart
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ChordChart" /> class with the specified key and chord progression.
  /// </summary>
  /// <param name="key">The key for the chord chart.</param>
  /// <param name="progression">The chord progression for the chord chart.</param>
  public ChordChart(
    Key key,
    ChordProgression progression )
  {
    ArgumentNullException.ThrowIfNull( key );
    ArgumentNullException.ThrowIfNull( progression );

    Key = key;
    Progression = progression;

    Chords = progression.ScaleDegrees.Select( degree => degree.ResolveDiatonicTriad( key ) )
                        .ToList();
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="ChordChart" /> class with the specified key and chord progression
  ///   string.
  /// </summary>
  /// <param name="key">The key for the chord chart.</param>
  /// <param name="progression">The chord progression string for the chord chart.</param>
  public ChordChart(
    Key key,
    string progression )
    : this( key, ChordProgression.Parse( progression ) )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="ChordChart" /> class with the specified key and scale degrees.
  /// </summary>
  /// <param name="key">The key for the chord chart.</param>
  /// <param name="scaleDegrees">The scale degrees for the chord chart.</param>
  public ChordChart(
    Key key,
    params ScaleDegree[] scaleDegrees )
    :
    this( key, new ChordProgression( scaleDegrees ) )
  {
  }

  #endregion

  #region Properties

  /// <summary>Gets the key for the chord chart.</summary>
  public Key Key { get; }

  /// <summary>Gets the chord progression for the chord chart.</summary>
  public ChordProgression Progression { get; }

  /// <summary>Gets the chords in the progression.</summary>
  public IReadOnlyList<Chord> Chords { get; }

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public override string ToString()
  {
    return string.Join( "-", Chords.Select( chord => chord.ToString() ) );
  }

  #endregion
}
