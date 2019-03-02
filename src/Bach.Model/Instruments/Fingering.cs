﻿//
// Module Name: Fingering.cs
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

  public struct Fingering: IEquatable<Fingering>
  {
    private Fingering(Pitch pitch,
                      int @string,
                      int fret)
    {
      Pitch = pitch;
      String = @string;
      Fret = fret;
    }

    public static Fingering Create(StringedInstrument instrument,
                                   int @string,
                                   int fret)
    {
      Contract.Requires<ArgumentNullException>(instrument != null);
      Contract.Requires<ArgumentOutOfRangeException>(fret >= 0 && fret <= instrument.FretCount);
      Contract.Requires<ArgumentOutOfRangeException>(@string > 0 && @string <= instrument.Definition.StringCount);

      Pitch pitch = instrument.Tuning[@string] + fret;
      var result = new Fingering(pitch, @string, fret);
      return result;
    }

    public static Fingering Create(StringedInstrument instrument,
                                   int @string)
    {
      Contract.Requires<ArgumentNullException>(instrument != null);
      Contract.Requires<ArgumentOutOfRangeException>(@string > 0 && @string <= instrument.Definition.StringCount);

      Pitch pitch = Pitch.Empty;
      var result = new Fingering(pitch, @string, -1);
      return result;
    }

    public Pitch Pitch { get; }
    public int String { get; }
    public int Fret { get; }

    public override string ToString()
    {
      if( Fret < 0 )
      {
        return $"{String}x";
      }

      return $"{String}{Fret}";
    }

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
      }

      return obj is Fingering fingering && Equals(fingering);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ( String * 397 ) ^ Fret;
      }
    }

    public bool Equals(Fingering other) => String == other.String && Fret == other.Fret;
  }
}
