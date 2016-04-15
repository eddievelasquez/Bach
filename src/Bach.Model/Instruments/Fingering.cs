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

    private Fingering(int stringNumber, int fretNumber)
    {
      FretNumber = fretNumber;
      StringNumber = stringNumber;
    }

    #endregion

    #region Factories

    public static Fingering Create(StringedInstrument instrument, int stringNumber, int fretNumber)
    {
      Contract.Requires<ArgumentNullException>(instrument != null);
      Contract.Requires<ArgumentOutOfRangeException>(fretNumber >= 0 && fretNumber <= instrument.FretCount);
      Contract.Requires<ArgumentOutOfRangeException>(stringNumber > 0
                                                     && stringNumber <= instrument.Definition.StringCount);

      var result = new Fingering(stringNumber, fretNumber);
      return result;
    }

    #endregion

    #region Properties

    public int StringNumber { get; }
    public int FretNumber { get; }

    #endregion

    #region IEquatable<Fingering> Members

    public bool Equals(Fingering other)
    {
      return StringNumber == other.StringNumber && FretNumber == other.FretNumber;
    }

    #endregion

    #region Public Methods

    public int ToInt()
    {
      if( FretNumber > 9 )
      {
        return StringNumber * 100 + FretNumber;
      }

      return StringNumber * 10 + FretNumber;
    }

    public override string ToString()
    {
      return $"{StringNumber}{FretNumber}";
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
        return (StringNumber * 397) ^ FretNumber;
      }
    }

    #endregion
  }
}
