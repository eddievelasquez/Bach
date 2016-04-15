//  
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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Diagnostics.Contracts;

  public class StringedInstrument: Instrument,
                                   IEquatable<StringedInstrument>
  {
    #region Construction/Destruction

    private StringedInstrument(StringedInstrumentDefinition definition, Tuning tuning, int fretCount)
      : base(definition)
    {
      Contract.Requires<ArgumentNullException>(tuning != null);
      Contract.Requires<ArgumentOutOfRangeException>(fretCount > 0);

      Tuning = tuning;
      FretCount = fretCount;
    }

    #endregion

    #region Factories

    public static StringedInstrument Create(
      StringedInstrumentDefinition definition,
      int fretCount,
      Tuning tuning = null)
    {
      Contract.Requires<ArgumentNullException>(definition != null);
      Contract.Ensures(Contract.Result<StringedInstrument>() != null);

      return new StringedInstrument(definition, tuning ?? definition.Tunings.Standard, fretCount);
    }

    public static StringedInstrument Create(string instrumentName, int fretCount, string tuningName = null)
    {
      Contract.Ensures(Contract.Result<StringedInstrument>() != null);

      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>(instrumentName);
      if( string.IsNullOrEmpty(tuningName) )
      {
        tuningName = "Standard";
      }

      return new StringedInstrument(definition, definition.Tunings[tuningName], fretCount);
    }

    #endregion

    #region Properties

    public new StringedInstrumentDefinition Definition => (StringedInstrumentDefinition) base.Definition;

    public int FretCount { get; }

    public Tuning Tuning { get; }

    #endregion

    #region Public Methods

    public IEnumerable<Fingering> Render(Scale scale, int startString, int startFret, int fretSpan = 4)
    {
      Contract.Requires<ArgumentNullException>(scale != null);
      Contract.Requires<ArgumentOutOfRangeException>(startString > 0 && startString <= Definition.StringCount);
      Contract.Requires<ArgumentOutOfRangeException>(fretSpan > 1 && startFret + fretSpan <= FretCount);

      // Find scale note closest to start string and start fret
      AbsoluteNote startNote = Tuning[startString];
      startNote += startFret;

      // Find the scale note that is closest to the starting fret
      int notePosition;
      while( (notePosition = scale.IndexOf(startNote)) == -1 )
      {
        ++startNote;
      }

      // Skip over the scale notes
      var scaleEnumerator = scale.Render(startNote).GetEnumerator();
      scaleEnumerator.MoveNext();

      int maxFret = startFret + fretSpan;

      for ( int currentString = startString; currentString >= 1; --currentString )
      {
        int nextString = currentString - 1;

        // Calculate the starting note on the next string (if applicable)
        AbsoluteNote? startNoteOnNextString = null;
        if( nextString > 1 )
        {
          startNoteOnNextString = Tuning[nextString] + startFret;
        }

        int fret = startFret;

        while(true)
        {
          // Skip this fingering if the current note is equal to the start note
          // on the next string
          if(startNoteOnNextString.HasValue && scaleEnumerator.Current == startNoteOnNextString )
          {
            break;
          }

          fret = FindNoteOnString(scaleEnumerator.Current, currentString, fret);
          if( fret > maxFret )
          {
            break;
          }
          
          Fingering result = Fingering.Create(this, currentString, fret);
          yield return result;

          scaleEnumerator.MoveNext(); // Will never return false.
          ++fret;
        }
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
      hash = hash * 23 + base.GetHashCode();
      hash = hash * 23 + Tuning.GetHashCode();
      hash = hash * 23 + FretCount.GetHashCode();
      return hash;
    }

    #endregion

    #region IEquatable<StringedInstrument> Members

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

    #endregion

    #region Implementation

    private int FindNoteOnString(AbsoluteNote noteToFind, int stringNumber, int startFretNumber)
    {
      Contract.Requires<ArgumentOutOfRangeException>(stringNumber > 0 && stringNumber <= Definition.StringCount);
      Contract.Requires<ArgumentOutOfRangeException>(startFretNumber >= 0 && startFretNumber <= FretCount);
      Contract.Ensures(Contract.Result<int>() <= FretCount);

      AbsoluteNote openNote = Tuning[stringNumber];
      AbsoluteNote current = openNote + startFretNumber;

      while( current != noteToFind )
      {
        ++current;
      }

      int fret = openNote - current;
      return fret;
    }

    #endregion
  }
}
