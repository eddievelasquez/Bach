// Module Name: Fingering.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System;
using Bach.Model.Internal;

namespace Bach.Model.Instruments;

/// <summary>
///   In a stringed instrument, A fingering describes the position in a given string to produce a particular
///   pitch.
/// </summary>
public readonly struct Fingering: IEquatable<Fingering>
{
#region Constructors

  private Fingering(
    Pitch pitch,
    int stringNumber,
    int position )
  {
    Pitch = pitch;
    StringNumber = stringNumber;
    Position = position;
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

#endregion

#region Public Methods

  /// <summary>Creates a new Fingering.</summary>
  /// <param name="instrument">The instrument.</param>
  /// <param name="stringNumber">The string number.</param>
  /// <param name="position">The position.</param>
  /// <exception cref="ArgumentNullException">Thrown when the instrument is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when either the string number or the position are out of range for the given instrument.
  /// </exception>
  /// <returns>A Fingering.</returns>
  public static Fingering Create(
    StringedInstrument instrument,
    int stringNumber,
    int position )
  {
    Requires.NotNull( instrument );
    Requires.Between( position, 0, instrument.PositionCount );
    Requires.Between( stringNumber, 1, instrument.Definition.StringCount );

    var pitch = instrument.Tuning[stringNumber] + position;
    var result = new Fingering( pitch, stringNumber, position );
    return result;
  }

  /// <inheritdoc />
  public bool Equals( Fingering other )
  {
    return StringNumber == other.StringNumber && Position == other.Position;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( obj is null )
    {
      return false;
    }

    return obj is Fingering other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( StringNumber, Position );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Position < 0 ? $"{StringNumber}x" : $"{StringNumber}{Position}";
  }

#endregion

#region Implementation

  internal static Fingering Create(
    StringedInstrument instrument,
    int stringNumber )
  {
    Requires.NotNull( instrument );
    Requires.Between( stringNumber, 1, instrument.Definition.StringCount );

    var pitch = Pitch.Empty;
    var result = new Fingering( pitch, stringNumber, -1 );
    return result;
  }

#endregion
}
