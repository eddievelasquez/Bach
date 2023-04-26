// Module Name: StringedInstrument.cs
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
using System.Collections.Generic;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model.Instruments;

/// <summary>Represents a stringed instrument, such as guitars, basses, ukeleles, etc.</summary>
public sealed class StringedInstrument
  : Instrument,
    IEquatable<StringedInstrument>
{
#region Constructors

  private StringedInstrument(
    StringedInstrumentDefinition definition,
    Tuning tuning,
    int positionCount )
    : base( definition )
  {
    Requires.NotNull( tuning );
    Requires.GreaterThan( positionCount, 0 );

    Tuning = tuning;
    PositionCount = positionCount;
  }

#endregion

#region Properties

  /// <summary>Gets the stringed instruments definition.</summary>
  /// <value>The definition.</value>
  public new StringedInstrumentDefinition Definition => (StringedInstrumentDefinition) base.Definition;

  /// <summary>Gets the number of positions for a string.</summary>
  /// <value>The number of positions.</value>
  public int PositionCount { get; }

  /// <summary>Gets the stringed instruments tuning.</summary>
  /// <value>The tuning.</value>
  public Tuning Tuning { get; }

#endregion

#region Public Methods

  /// <summary>Creates a new StringedInstrument.</summary>
  /// <param name="definition">The stringed instruments definition.</param>
  /// <param name="positionCount">The number of positions per string.</param>
  /// <param name="tuning">(Optional) The tuning of the instrument. If null, the standard tuning for the instrument is used.</param>
  /// <exception cref="ArgumentNullException">Thrown when instrument definition is null.</exception>
  /// <returns>A StringedInstrument.</returns>
  public static StringedInstrument Create(
    StringedInstrumentDefinition definition,
    int positionCount,
    Tuning? tuning = null )
  {
    Requires.NotNull( definition );

    return new StringedInstrument( definition, tuning ?? definition.Tunings.Standard, positionCount );
  }

  /// <summary>Creates a new StringedInstrument.</summary>
  /// <param name="instrumentKey">The instruments language-neutral key.</param>
  /// <param name="positionCount">The number of positions per string.</param>
  /// <param name="tuningName">(Optional) Name of the tuning. If null, the standard tuning for the instrument is used.</param>
  /// <returns>A StringedInstrument.</returns>
  public static StringedInstrument Create(
    string instrumentKey,
    int positionCount,
    string? tuningName = null )
  {
    var definition = Registry.StringedInstrumentDefinitions[instrumentKey];
    if( string.IsNullOrEmpty( tuningName ) )
    {
      tuningName = "Standard";
    }

    return new StringedInstrument( definition, definition.Tunings[tuningName], positionCount );
  }

  /// <inheritdoc />
  public bool Equals( StringedInstrument? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return base.Equals( other ) && Equals( Tuning, other.Tuning ) && PositionCount == other.PositionCount;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    return obj is StringedInstrument other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( base.GetHashCode(), Tuning, PositionCount );
  }

  /// <summary>Render a chord in the starting position with an optional position span.</summary>
  /// <param name="chord">The chord.</param>
  /// <param name="startPosition">The starting position.</param>
  /// <param name="positionSpan">(Optional) The position span.</param>
  /// <exception cref="ArgumentNullException">Thrown when the chord is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the position span is greater than the number of positions in
  ///   a string.
  /// </exception>
  /// <returns>An enumerator for a fingering sequence for the chord.</returns>
  public IEnumerable<Fingering> Render(
    Chord chord,
    int startPosition,
    int positionSpan = 4 )
  {
    Requires.NotNull( chord );
    Requires.GreaterThan( positionSpan, 1 );
    Requires.LessThan( startPosition + positionSpan, PositionCount - 1 );

    // Always start at the lowest string
    var startString = Definition.StringCount;

    var startPitch = Tuning[startString] + startPosition;
    var octave = startPitch.Octave;
    if( startPitch.PitchClass > chord.Bass )
    {
      ++octave;
    }

    var notes = chord.Render( octave ).GetEnumerator();
    notes.MoveNext();

    // Go through all the strings
    for( var currentString = startString; currentString >= 1; --currentString )
    {
      var fingering = GetChordFingering( notes, currentString, startPosition, positionSpan );
      yield return fingering;

      // Only go to the next pitch in the chord if a pitch
      // is to be played in the current string
      if( fingering.Position >= 0 )
      {
        notes.MoveNext();
      }
    }
  }

  /// <summary>Render a scale in the starting position with an optional position span.</summary>
  /// <param name="scale">The scale.</param>
  /// <param name="startPosition">The starting position.</param>
  /// <param name="positionSpan">(Optional) The position span.</param>
  /// <exception cref="ArgumentNullException">Thrown when the scale is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the position span is greater than the number of positions in
  ///   a string.
  /// </exception>
  /// <returns>An enumerator for a fingering sequence for the scale.</returns>
  public IEnumerable<Fingering> Render(
    Scale scale,
    int startPosition,
    int positionSpan = 4 )
  {
    Requires.NotNull( scale );
    Requires.GreaterThan( positionSpan, 1 );
    Requires.LessThan( startPosition + positionSpan, PositionCount - 1 );

    // Always start at the lowest string
    var startString = Definition.StringCount;

    // Find the scale pitch that is closest to starting string and position
    var startPitch = Tuning[startString] + startPosition;

    // Adjust the octave if necessary
    var octave = startPitch.Octave;
    if( startPitch.PitchClass < scale.Root )
    {
      --octave;
    }

    var scaleEnumerator = scale.Render( octave ).SkipWhile( pitch => pitch < startPitch ).GetEnumerator();

    try
    {
      scaleEnumerator.MoveNext();

      // Go through all the strings
      for( var currentString = startString; currentString >= 1; --currentString )
      {
        var low = GetPitchAt( currentString, startPosition );

        // The maximum value that we will use for this string is the minimum
        // between the position span on this string and the value of the pitch
        // before the start of the next string
        var high = Pitch.Min( low + positionSpan, GetPitchAt( currentString - 1, startPosition ) - 1 );
        while( true )
        {
          var current = scaleEnumerator.Current;
          if( current > high )
          {
            break;
          }

          var position = ( current - low ) + startPosition;
          var fingering = Fingering.Create( this, currentString, position );
          yield return fingering;

          scaleEnumerator.MoveNext(); // Will never return false.
        }
      }
    }
    finally
    {
      scaleEnumerator.Dispose();
    }
  }

#endregion

#region Implementation

  private Fingering GetChordFingering(
    IEnumerator<Pitch> notes,
    int stringNumber,
    int startPosition,
    int positionSpan )
  {
    var low = GetPitchAt( stringNumber, startPosition );
    var high = low + positionSpan;
    var current = notes.Current;

    while( current < low )
    {
      notes.MoveNext();
      current = notes.Current;
    }

    Fingering fingering;
    if( current <= high )
    {
      var position = ( current - low ) + startPosition;
      fingering = Fingering.Create( this, stringNumber, position );
    }
    else
    {
      fingering = Fingering.Create( this, stringNumber );
    }

    return fingering;
  }

  private Pitch GetPitchAt(
    int @string,
    int position )
  {
    if( @string < 1 || @string > Definition.StringCount )
    {
      return Pitch.MaxValue;
    }

    return Tuning[@string] + position;
  }

#endregion
}
