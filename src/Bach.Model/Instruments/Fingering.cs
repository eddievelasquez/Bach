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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Instruments
{
  using System;
  using System.Diagnostics.Contracts;

  public struct Fingering: IEquatable<Fingering>
  {
    #region Construction/Destruction

    private Fingering(AbsoluteNote note, int @string, int fret)
    {
      Note = note;
      String = @string;
      Fret = fret;
    }

    #endregion

    #region Factories

    public static Fingering Create(StringedInstrument instrument, int @string, int fret)
    {
      Contract.Requires<ArgumentNullException>(instrument != null);
      Contract.Requires<ArgumentOutOfRangeException>(fret >= 0 && fret <= instrument.FretCount);
      Contract.Requires<ArgumentOutOfRangeException>(@string > 0 && @string <= instrument.Definition.StringCount);

      var note = instrument.Tuning[@string] + fret;
      var result = new Fingering(note, @string, fret);
      return result;
    }

    #endregion

    #region Properties

    public AbsoluteNote Note { get; }
    public int String { get; }
    public int Fret { get; }

    #endregion

    #region Public Methods

    public override string ToString()
    {
      return $"{String}{Fret}";
    }

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
      }

      return obj is Fingering && Equals((Fingering) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (String * 397) ^ Fret;
      }
    }

    #endregion

    #region IEquatable<Fingering> Members

    public bool Equals(Fingering other)
    {
      return String == other.String && Fret == other.Fret;
    }

    #endregion
  }
}
