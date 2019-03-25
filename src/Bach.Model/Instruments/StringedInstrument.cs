﻿// Module Name: StringedInstrument.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Model.Internal;

  /// <summary>Represents a stringed instrument, such as guitars, basses, ukeleles, etc.</summary>
  public class StringedInstrument
    : Instrument,
      IEquatable<StringedInstrument>
  {
    #region Constructors

    private StringedInstrument(StringedInstrumentDefinition definition,
                               Tuning tuning,
                               int positionCount)
      : base(definition)
    {
      Contract.Requires<ArgumentNullException>(tuning != null);
      Contract.Requires<ArgumentOutOfRangeException>(positionCount > 0);

      Tuning = tuning;
      PositionCount = positionCount;
    }

    #endregion

    #region Properties

    /// <summary>Gets the stringed instruments definition.</summary>
    /// <value>The definition.</value>
    public new StringedInstrumentDefinition Definition => (StringedInstrumentDefinition)base.Definition;

    /// <summary>Gets the number of positions for a string.</summary>
    /// <value>The number of positions.</value>
    public int PositionCount { get; }

    /// <summary>Gets the stringed instruments tuning.</summary>
    /// <value>The tuning.</value>
    public Tuning Tuning { get; }

    #endregion

    #region IEquatable<StringedInstrument> Members

    /// <inheritdoc />
    public bool Equals(StringedInstrument other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }

      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      return Equals(Tuning, other.Tuning) && PositionCount == other.PositionCount && base.Equals(other);
    }

    #endregion

    #region Public Methods

    /// <summary>Creates a new StringedInstrument.</summary>
    /// <param name="definition">The stringed instruments definition.</param>
    /// <param name="positionCount">The number of positions per string.</param>
    /// <param name="tuning">(Optional) The tuning of the instrument. If null, the standard tuning for the instrument is used.</param>
    /// <exception cref="ArgumentNullException">Thrown when instrument definition is null.</exception>
    /// <returns>A StringedInstrument.</returns>
    public static StringedInstrument Create(StringedInstrumentDefinition definition,
                                            int positionCount,
                                            Tuning tuning = null)
    {
      Contract.Requires<ArgumentNullException>(definition != null);

      return new StringedInstrument(definition, tuning ?? definition.Tunings.Standard, positionCount);
    }

    /// <summary>Creates a new StringedInstrument.</summary>
    /// <param name="instrumentKey">The instruments language-neutral key.</param>
    /// <param name="positionCount">The number of positions per string.</param>
    /// <param name="tuningName">(Optional) Name of the tuning. If null, the standard tuning for the instrument is used.</param>
    /// <returns>A StringedInstrument.</returns>
    public static StringedInstrument Create(string instrumentKey,
                                            int positionCount,
                                            string tuningName = null)
    {
      StringedInstrumentDefinition definition = Registry.StringedInstrumentDefinitions[instrumentKey];
      if( string.IsNullOrEmpty(tuningName) )
      {
        tuningName = "Standard";
      }

      return new StringedInstrument(definition, definition.Tunings[tuningName], positionCount);
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
    public IEnumerable<Fingering> Render(Chord chord,
                                         int startPosition,
                                         int positionSpan = 4)
    {
      Contract.Requires<ArgumentNullException>(chord != null);
      Contract.Requires<ArgumentOutOfRangeException>(positionSpan > 1 && startPosition + positionSpan <= PositionCount);

      // Always start at the lowest string
      int startString = Definition.StringCount;

      Pitch startPitch = Tuning[startString] + startPosition;
      int octave = startPitch.Octave;
      if( startPitch.Note > chord.Bass )
      {
        ++octave;
      }

      IEnumerator<Pitch> notes = chord.Render(octave).GetEnumerator();
      notes.MoveNext();

      // Go through all the strings
      for( int currentString = startString; currentString >= 1; --currentString )
      {
        Fingering fingering = GetChordFingering(notes, currentString, startPosition, positionSpan);
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
    public IEnumerable<Fingering> Render(Scale scale,
                                         int startPosition,
                                         int positionSpan = 4)
    {
      Contract.Requires<ArgumentNullException>(scale != null);
      Contract.Requires<ArgumentOutOfRangeException>(positionSpan > 1 && startPosition + positionSpan <= PositionCount);

      // Always start at the lowest string
      int startString = Definition.StringCount;

      // Find the scale pitch that is closest to starting string and position
      Pitch startPitch = Tuning[startString] + startPosition;

      // Adjust the octave if necessary
      int octave = startPitch.Octave;
      if (startPitch.Note < scale.Root)
      {
        --octave;
      }

      var scaleEnumerator = scale.Render(octave).SkipWhile(pitch => pitch < startPitch).GetEnumerator();

      try
      {
        scaleEnumerator.MoveNext();

        // Go through all the strings
        for( int currentString = startString; currentString >= 1; --currentString )
        {
          Pitch low = GetPitchAt(currentString, startPosition);

          // The maximum value that we will use for this string is the minimum
          // between the position span on this string and the value of the pitch
          // before the start of the next string
          Pitch high = Pitch.Min(low + positionSpan, GetPitchAt(currentString - 1, startPosition) - 1);
          while( true )
          {
            Pitch current = scaleEnumerator.Current;
            if( current > high )
            {
              break;
            }

            int position = ( current - low ) + startPosition;
            Fingering fingering = Fingering.Create(this, currentString, position);
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

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
      }

      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj.GetType() == GetType() && Equals((StringedInstrument)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hash = 17;
      hash = ( hash * 23 ) + base.GetHashCode();
      hash = ( hash * 23 ) + Tuning.GetHashCode();
      hash = ( hash * 23 ) + PositionCount.GetHashCode();
      return hash;
    }

    #endregion

    #region  Implementation

    private Fingering GetChordFingering(IEnumerator<Pitch> notes,
                                        int @string,
                                        int startPosition,
                                        int positionSpan)
    {
      Pitch low = GetPitchAt(@string, startPosition);
      Pitch high = low + positionSpan;
      Pitch current = notes.Current;

      while( current < low )
      {
        notes.MoveNext();
        current = notes.Current;
      }

      Fingering fingering;
      if( current >= low && current <= high )
      {
        int position = ( current - low ) + startPosition;
        fingering = Fingering.Create(this, @string, position);
      }
      else
      {
        fingering = Fingering.Create(this, @string);
      }

      return fingering;
    }

    private Pitch GetPitchAt(int @string,
                             int position)
    {
      if( @string < 1 || @string > Definition.StringCount )
      {
        return Pitch.MaxValue;
      }

      return Tuning[@string] + position;
    }

    #endregion
  }
}
