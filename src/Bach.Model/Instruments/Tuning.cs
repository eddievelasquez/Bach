//  
// Module Name: Tuning.cs
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
  using System.Linq;

  public class Tuning: IEquatable<Tuning>
  {
    #region Data Members

    private static readonly StringComparer s_nameComparer = StringComparer.CurrentCultureIgnoreCase;

    #endregion

    #region Construction/Destruction

    public Tuning(StringedInstrumentDefinition instrumentDefinition, string name, params AbsoluteNote[] notes)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null);
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(notes.Length == instrumentDefinition.StringCount);

      InstrumentDefinition = instrumentDefinition;
      Name = name;
      Notes = notes;
    }

    public Tuning(StringedInstrumentDefinition instrumentDefinition, string name, AbsoluteNoteCollection notes)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null);
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(notes.Count == instrumentDefinition.StringCount);

      InstrumentDefinition = instrumentDefinition;
      Name = name;
      Notes = notes.ToArray();
    }

    #endregion

    #region Properties

    public StringedInstrumentDefinition InstrumentDefinition { get; }
    public string Name { get; }
    public AbsoluteNote[] Notes { get; }

    #endregion

    #region IEquatable<Tuning> Members

    public bool Equals(Tuning other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }

      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      return InstrumentDefinition.Equals(other.InstrumentDefinition) && s_nameComparer.Equals(Name, other.Name)
             && Notes.SequenceEqual(other.Notes);
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

      return obj.GetType() == GetType() && Equals((Tuning) obj);
    }

    public override int GetHashCode()
    {
      var hash = 17;
      hash = hash * 23 + InstrumentDefinition.GetHashCode();
      hash = hash * 23 + s_nameComparer.GetHashCode(Name);
      return hash;
    }

    #endregion
  }
}
