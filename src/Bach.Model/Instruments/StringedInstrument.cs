﻿//
// Module Name: StringedInstrument.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
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

  public class StringedInstrument
    : Instrument,
      IEquatable<StringedInstrument>
  {
    private StringedInstrument(StringedInstrumentDefinition definition,
                               Tuning tuning,
                               int fretCount)
      : base(definition)
    {
      Contract.Requires<ArgumentNullException>(tuning != null);
      Contract.Requires<ArgumentOutOfRangeException>(fretCount > 0);

      Tuning = tuning;
      FretCount = fretCount;
    }

    public static StringedInstrument Create(StringedInstrumentDefinition definition,
                                            int fretCount,
                                            Tuning tuning = null)
    {
      Contract.Requires<ArgumentNullException>(definition != null);

      return new StringedInstrument(definition, tuning ?? definition.Tunings.Standard, fretCount);
    }

    public static StringedInstrument Create(string instrumentKey,
                                            int fretCount,
                                            string tuningName = null)
    {
      StringedInstrumentDefinition definition = Registry.StringedInstrumentDefinitions[instrumentKey];
      if( string.IsNullOrEmpty(tuningName) )
      {
        tuningName = "Standard";
      }

      return new StringedInstrument(definition, definition.Tunings[tuningName], fretCount);
    }

    public new StringedInstrumentDefinition Definition => (StringedInstrumentDefinition) base.Definition;

    public int FretCount { get; }

    public Tuning Tuning { get; }

    public IEnumerable<Fingering> Render(Chord chord,
                                         int startFret,
                                         int fretSpan = 4)
    {
      Contract.Requires<ArgumentNullException>(chord != null);
      Contract.Requires<ArgumentOutOfRangeException>(fretSpan > 1 && startFret + fretSpan <= FretCount);

      // Always start at the lowest string
      int startString = Definition.StringCount;

      Pitch startPitch = Tuning[startString] + startFret;
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
        Fingering fingering = GetChordFingering(notes, currentString, startFret, fretSpan);
        yield return fingering;

        // Only go to the next pitch in the chord if a pitch
        // is to be played in the current string
        if( fingering.Fret >= 0 )
        {
          notes.MoveNext();
        }
      }
    }

    public IEnumerable<Fingering> Render(Scale scale,
                                         int startFret,
                                         int fretSpan = 4)
    {
      Contract.Requires<ArgumentNullException>(scale != null);
      Contract.Requires<ArgumentOutOfRangeException>(fretSpan > 1 && startFret + fretSpan <= FretCount);

      // Always start at the lowest string
      int startString = Definition.StringCount;

      // Find scale pitch closest to start string and start fret
      Pitch startPitch = Tuning[startString] + startFret;
      while( scale.IndexOf(startPitch) == -1 )
      {
        ++startPitch;
      }

      // Start rendering the scale at the pitch closest to the
      // start string and fret
      IEnumerator<Pitch> scaleEnumerator = scale.Render(startPitch).GetEnumerator();

      try
      {
        scaleEnumerator.MoveNext();

        // Go through all the strings
        for( int currentString = startString; currentString >= 1; --currentString )
        {
          Pitch low = GetNote(currentString, startFret);

          // The maximum value that we will use for this string is the minimum
          // between the fret span on this string and the value of the pitch
          // before the start of the next string
          Pitch high = Pitch.Min(low + fretSpan, GetNote(currentString - 1, startFret) - 1);
          while( true )
          {
            Pitch current = scaleEnumerator.Current;
            if( current > high )
            {
              break;
            }

            int fret = ( current - low ) + startFret;
            Fingering fingering = Fingering.Create(this, currentString, fret);
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

      return obj.GetType() == GetType() && Equals((StringedInstrument) obj);
    }

    public override int GetHashCode()
    {
      var hash = 17;
      hash = ( hash * 23 ) + base.GetHashCode();
      hash = ( hash * 23 ) + Tuning.GetHashCode();
      hash = ( hash * 23 ) + FretCount.GetHashCode();
      return hash;
    }

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

      return Equals(Tuning, other.Tuning) && FretCount == other.FretCount && base.Equals(other);
    }

    private Fingering GetChordFingering(IEnumerator<Pitch> notes,
                                        int @string,
                                        int startFret,
                                        int fretSpan)
    {
      Pitch low = GetNote(@string, startFret);
      Pitch high = low + fretSpan;
      Pitch current = notes.Current;

      while( current < low )
      {
        notes.MoveNext();
        current = notes.Current;
      }

      Fingering fingering;
      if( current >= low && current <= high )
      {
        int fret = ( current - low ) + startFret;
        fingering = Fingering.Create(this, @string, fret);
      }
      else
      {
        fingering = Fingering.Create(this, @string);
      }

      return fingering;
    }

    private Pitch GetNote(int @string,
                          int fret)
    {
      if( @string < 1 || @string > Definition.StringCount )
      {
        return Pitch.MaxValue;
      }

      return Tuning[@string] + fret;
    }
  }
}
