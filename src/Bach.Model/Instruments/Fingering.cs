// Module Name: Fingering.cs
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

namespace Bach.Model.Instruments;

/// <summary>
///   In a stringed instrument, A fingering describes the position in a given string to produce a particular
///   pitch.
/// </summary>
public readonly struct Fingering: IEquatable<Fingering>
{
  private const int MUTED_POSITION = -1;

  #region Constructors

  /// <summary>Creates a new Fingering.</summary>
  /// <param name="instrument">The instrument.</param>
  /// <param name="stringNumber">The string number.</param>
  /// <param name="position">The position.</param>
  /// <exception cref="ArgumentNullException">Thrown when the instrument is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when either the string number or the position are out of range for the given instrument.
  /// </exception>
  internal Fingering(
    StringedInstrument instrument,
    int stringNumber,
    int position )
  {
    ArgumentNullException.ThrowIfNull( instrument );
    ArgumentOutOfRangeException.ThrowIfLessThan( position, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( position, instrument.PositionCount );
    ArgumentOutOfRangeException.ThrowIfLessThan( stringNumber, 1 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( stringNumber, instrument.Definition.StringCount );

    Pitch = instrument.Tuning[stringNumber] + position;
    StringNumber = stringNumber;
    Position = position;
  }

  /// <summary>
  /// Creates a new muted <see cref="Fingering"/> for the given string number.
  /// </summary>
  /// <param name="instrument">The instrument.</param>
  /// <param name="stringNumber">The string number.</param>
  internal Fingering(
    StringedInstrument instrument,
    int stringNumber )
  {
    ArgumentNullException.ThrowIfNull( instrument );
    ArgumentOutOfRangeException.ThrowIfLessThan( stringNumber, 1 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( stringNumber, instrument.Definition.StringCount );

    Pitch = default;
    StringNumber = stringNumber;
    Position = MUTED_POSITION;
  }

  #endregion

  #region Properties

  /// <summary>Gets the fingering's pitch.</summary>
  /// <value>The pitch.</value>
  public Pitch Pitch { get; }

  /// <summary>Gets the string number.</summary>
  /// <value>The string number.</value>
  public int StringNumber { get; }

  /// <summary>Gets the position on the string.</summary>
  /// <remarks>For fretted instruments this corresponds to the fret number.</remarks>
  /// <value>The position.</value>
  public int Position { get; }

  /// <summary>Gets a value indicating whether the fingering is muted.</summary>
  /// <value><c>true</c> if the fingering is muted; otherwise, <c>false</c>.</value>
  public bool IsMuted => Position == MUTED_POSITION;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public bool Equals(
    Fingering other )
  {
    return StringNumber == other.StringNumber && Position == other.Position;
  }

  /// <inheritdoc/>
  public override bool Equals(
    object? obj )
  {
    if( obj is null )
    {
      return false;
    }

    return obj is Fingering other && Equals( other );
  }

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    return HashCode.Combine( StringNumber, Position );
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return Position < 0 ? $"{StringNumber}x" : $"{StringNumber}{Position}";
  }

  #endregion
}
