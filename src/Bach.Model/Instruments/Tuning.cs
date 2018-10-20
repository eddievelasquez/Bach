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
  using System.Linq;

  public class Tuning: IEquatable<Tuning>
  {
    #region Data Members

    private static readonly StringComparer s_nameComparer = StringComparer.CurrentCultureIgnoreCase;

    #endregion

    #region Construction/Destruction

    internal Tuning(StringedInstrumentDefinition instrumentDefinition,
                    string key,
                    string name,
                    NoteCollection notes)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null, "Must provide an instrument definition");
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(notes != null, "Must provide a note collection");
      Contract.Requires<ArgumentOutOfRangeException>(notes.Count == instrumentDefinition.StringCount,"The number of note must match the instrument's string count");

      InstrumentDefinition = instrumentDefinition;
      Key = key;
      Name = name;
      Notes = notes.ToArray();
    }

    #endregion

    #region Properties

    public StringedInstrumentDefinition InstrumentDefinition { get; }
    public string Key { get; }
    public string Name { get; }
    public Note[] Notes { get; }

    public Note this[int stringNumber]
    {
      get
      {
        Contract.Requires<ArgumentOutOfRangeException>(stringNumber >= 1);
        Contract.Requires<ArgumentOutOfRangeException>(stringNumber <= InstrumentDefinition.StringCount);
        return Notes[stringNumber - 1];
      }
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
      hash = (hash * 23) + InstrumentDefinition.GetHashCode();
      hash = (hash * 23) + s_nameComparer.GetHashCode(Key);
      hash = (hash * 23) + s_nameComparer.GetHashCode(Name);
      return hash;
    }

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

      return InstrumentDefinition.Equals(other.InstrumentDefinition) && s_nameComparer.Equals(Key, other.Key)
             && s_nameComparer.Equals(Name, other.Name) && Notes.SequenceEqual(other.Notes);
    }

    #endregion
  }
}
