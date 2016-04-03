//  
// Module Name: TuningCollection.cs
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
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;

  public class TuningCollection: KeyedCollection<string, Tuning>
  {
    #region Construction/Destruction

    public TuningCollection(StringedInstrumentDefinition instrumentDefinition)
      : base(StringComparer.CurrentCultureIgnoreCase)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null);
      InstrumentDefinition = instrumentDefinition;
    }

    #endregion

    #region Properties

    public StringedInstrumentDefinition InstrumentDefinition { get; }

    public Tuning Standard
    {
      get
      {
        Contract.Requires<ArgumentOutOfRangeException>(Count > 0);
        return this[0];
      }
    }

    #endregion

    #region Overrides

    protected override void InsertItem(int index, Tuning item)
    {
      VerifyInstrument(item);
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, Tuning item)
    {
      VerifyInstrument(item);
      base.SetItem(index, item);
    }

    protected override string GetKeyForItem(Tuning item)
    {
      return item.Name;
    }

    #endregion

    #region Implementation

    private void VerifyInstrument(Tuning tuning)
    {
      if( !InstrumentDefinition.Equals(tuning.InstrumentDefinition) )
      {
        throw new ArgumentException($"\"{tuning.Name}\" is not a \"{InstrumentDefinition.Name}\" tuning");
      }
    }

    #endregion
  }
}
