﻿// Module Name: Fingering.cs
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
  using Model.Internal;

  public struct Fingering: IEquatable<Fingering>
  {
    #region Constructors

    private Fingering(Pitch pitch,
                      int @string,
                      int position)
    {
      Pitch = pitch;
      String = @string;
      Position = position;
    }

    #endregion

    #region Properties

    public Pitch Pitch { get; }
    public int String { get; }
    public int Position { get; }

    #endregion

    #region IEquatable<Fingering> Members

    public bool Equals(Fingering other) => String == other.String && Position == other.Position;

    #endregion

    #region Public Methods

    public static Fingering Create(StringedInstrument instrument,
                                   int @string,
                                   int position)
    {
      Contract.Requires<ArgumentNullException>(instrument != null);
      Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position <= instrument.PositionCount);
      Contract.Requires<ArgumentOutOfRangeException>(@string > 0 && @string <= instrument.Definition.StringCount);

      Pitch pitch = instrument.Tuning[@string] + position;
      var result = new Fingering(pitch, @string, position);
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

    #endregion

    #region Overrides

    public override string ToString()
    {
      if( Position < 0 )
      {
        return $"{String}x";
      }

      return $"{String}{Position}";
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
        return ( String * 397 ) ^ Position;
      }
    }

    #endregion
  }
}
