// Module Name: NoteNameExtensions.cs
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

using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Provides operations for <see cref="NoteName"/> values.</summary>
public static class NoteNameExtensions
{
  #region Constants

  private const string NOTE_NAMES = "CDEFGAB";

  #endregion

  #region Implementation

  extension(
    NoteName noteName )
  {
    #region Public Methods

    /// <summary>Adds a number of steps to a note name.</summary>
    /// <param name="steps">The number of steps to add.</param>
    /// <returns>A note name.</returns>
    public NoteName Add(
      int steps )
    {
      Validate( noteName );
      return (NoteName) ( (int) noteName + steps ).Wrap( Constants.NoteNameCount );
    }

    /// <summary>Returns the signed distance from another note name to this note name.</summary>
    /// <param name="name">The other note name.</param>
    /// <returns>The distance between the note names.</returns>
    public int Subtract(
      NoteName name )
    {
      Validate( noteName );
      Validate( name );
      return (int) noteName.Add( -(int) name );
    }

    /// <summary>Subtracts a number of steps from a note name.</summary>
    /// <param name="steps">The number of steps to subtract.</param>
    /// <returns>A note name.</returns>
    public NoteName Subtract(
      int steps )
    {
      return noteName.Add( -steps );
    }

    #endregion
  }

  extension(
    NoteName )
  {
    #region Public Methods

    /// <summary>
    /// Parses a note name.
    /// </summary>
    /// <param name="value">The string representation of the note name.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="NoteName"/>.</returns>
    public static NoteName Parse(
      string value,
      IFormatProvider? provider = null )
    {
      ArgumentException.ThrowIfNullOrEmpty( value );
      return NoteName.Parse( value.AsSpan(), provider );
    }

    /// <summary>
    /// Parses a note name from a span of characters.
    /// </summary>
    /// <param name="value">The span of characters representing the note name.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="NoteName"/>.</returns>
    /// <exception cref="FormatException"></exception>
    public static NoteName Parse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider = null )
    {
      return NoteName.TryParse( value, provider, out var result )
        ? result
        : throw new FormatException( $"{value} is not a valid note name" );
    }

    /// <summary>
    /// Attempts to parse a note name.
    /// </summary>
    /// <param name="value">The string representation of the note name.</param>
    /// <param name="noteName">The parsed <see cref="NoteName"/> if successful.</param>
    /// <returns><c>true</c> if the note name was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      string? value,
      out NoteName noteName )
    {
      return NoteName.TryParse( value, null, out noteName );
    }

    /// <summary>
    /// Attempts to parse a note name with an optional format provider.
    /// </summary>
    /// <param name="value">The string representation of the note name.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="noteName">The parsed <see cref="NoteName"/> if successful.</param>
    /// <returns><c>true</c> if the note name was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      string? value,
      IFormatProvider? provider,
      out NoteName noteName )
    {
      return NoteName.TryParse( value.AsSpan(), provider, out noteName );
    }

    /// <summary>
    /// Attempts to parse a note name.
    /// </summary>
    /// <param name="value">The span of characters representing the note name.</param>
    /// <param name="noteName">The parsed <see cref="NoteName"/> if successful.</param>
    /// <returns><c>true</c> if the note name was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      out NoteName noteName )
    {
      return NoteName.TryParse( value, null, out noteName );
    }

    /// <summary>
    /// Attempts to parse a note name with an optional format provider.
    /// </summary>
    /// <param name="value">The span of characters representing the note name.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="noteName">The parsed <see cref="NoteName"/> if successful.</param>
    /// <returns><c>true</c> if the note name was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider,
      out NoteName noteName )
    {
      // Attempt to parse the note name and check if there is any unconsumed input.
      return NoteName.TryParse( value, provider, out noteName, out var tail ) && tail.IsEmpty;
    }

    /// <summary>
    /// Attempts to parse a note name and returns any unconsumed input.
    /// </summary>
    /// <param name="value">The span of characters representing the note name.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="noteName">The parsed <see cref="NoteName"/> if successful.</param>
    /// <param name="tail">The span of characters that were not consumed during parsing.</param>
    /// <returns><c>true</c> if the note name was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider,
      out NoteName noteName,
      out ReadOnlySpan<char> tail )
    {
      value = value.TrimStart();

      // Must have at least one character to parse a note name.
      if( value.IsEmpty )
      {
        noteName = NoteName.C;
        tail = ReadOnlySpan<char>.Empty;
        return false;
      }

      // Attempt to parse the note name by finding its index in the NOTE_NAMES string.
      var parsed = NOTE_NAMES.IndexOf( char.ToUpperInvariant( value[0] ) );

      // If the character is not found, return false and set the note name to C and the tail to the original value.
      if( parsed < 0 )
      {
        noteName = NoteName.C;
        tail = value;
        return false;
      }

      noteName = (NoteName) parsed;
      tail = value[1..];
      return true;
    }

    #endregion
  }

  /// <summary>
  /// Validates that a <see cref="NoteName"/> value is within the defined range of note names (C to B).
  /// </summary>
  /// <param name="noteName">The note name to validate.</param>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the note name is not within the valid range.</exception>
  private static void Validate(
    NoteName noteName )
  {
    if (noteName is < NoteName.C or > NoteName.B)
    {
      throw new ArgumentOutOfRangeException( nameof( noteName ), noteName, "The note name is not defined." );
    }
  }

  #endregion
}
