//  
// Module Name: StringedInstrument.cs
// Project:     Bach.Model
// Copyright (c) 2014  Eddie Velasquez.
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

  public abstract class StringedInstrument: Instrument,
                                            IEquatable<StringedInstrument>
  {
    #region Construction/Destruction

    protected StringedInstrument(string name, int stringCount, int fretCount)
      : base(name)
    {
      Contract.Requires<ArgumentOutOfRangeException>(stringCount > 0);
      Contract.Requires<ArgumentOutOfRangeException>(fretCount > 0);

      StringCount = stringCount;
      FretCount = fretCount;
      Tunings = new TuningCollection(this);
    }

    #endregion

    #region Properties

    public int StringCount { get; }

    public int FretCount { get; }

    public TuningCollection Tunings { get; }

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

      return StringCount == other.StringCount && base.Equals(other);
    }

    #endregion

    #region Public Methods

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
      hash = hash * 23 + StringCount;
      return hash;
    }

    #endregion
  }
}
