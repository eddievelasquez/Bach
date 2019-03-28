// Module Name: Tuning.cs
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
  using System.Collections.ObjectModel;
  using System.Linq;
  using Model.Internal;

  /// <summary>A tuning is the set of pitches for a stringed instrument when no string has been pressed.</summary>
  public class Tuning: IEquatable<Tuning>
  {
    #region Constants

    private static readonly StringComparer s_nameComparer = StringComparer.CurrentCultureIgnoreCase;

    #endregion

    #region Data Members

    private readonly Pitch[] _pitches;

    #endregion

    #region Constructors

    internal Tuning(StringedInstrumentDefinition instrumentDefinition,
                    string key,
                    string name,
                    PitchCollection pitches)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null, "Must provide an instrument definition");
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(pitches != null, "Must provide a pitch collection");
      Contract.Requires<ArgumentOutOfRangeException>(pitches.Count == instrumentDefinition.StringCount,
                                                     "The number of pitch must match the instrument's string count");

      InstrumentDefinition = instrumentDefinition;
      Key = key;
      Name = name;
      _pitches = pitches.ToArray();
    }

    #endregion

    #region Properties

    /// <summary>Gets the instruments definition.</summary>
    /// <value>The instrument definition.</value>
    public StringedInstrumentDefinition InstrumentDefinition { get; }

    /// <summary>Gets the language-neutral key for the tuning.</summary>
    /// <value>The key.</value>
    public string Key { get; }

    /// <summary>Gets the localizable name for the tuning.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the tunings pitches.</summary>
    /// <value>The pitches.</value>
    public ReadOnlyCollection<Pitch> Pitches => new ReadOnlyCollection<Pitch>(_pitches);

    /// <summary>Indexer to get pitches within this tuning using array index syntax.</summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown when the string number is outside the
    ///   string range.
    /// </exception>
    /// <param name="stringNumber">The string number.</param>
    /// <returns>A pitch.</returns>
    public Pitch this[int stringNumber]
    {
      get
      {
        Contract.Requires<ArgumentOutOfRangeException>(stringNumber >= 1);
        Contract.Requires<ArgumentOutOfRangeException>(stringNumber <= InstrumentDefinition.StringCount);
        return Pitches[stringNumber - 1];
      }
    }

    #endregion

    #region IEquatable<Tuning> Members

    /// <inheritdoc />
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

      return InstrumentDefinition.Equals(other.InstrumentDefinition)
             && s_nameComparer.Equals(Key, other.Key)
             && s_nameComparer.Equals(Name, other.Name)
             && Pitches.SequenceEqual(other.Pitches);
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
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

      return obj.GetType() == GetType() && Equals((Tuning)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hash = 17;
      hash = ( hash * 23 ) + InstrumentDefinition.GetHashCode();
      hash = ( hash * 23 ) + s_nameComparer.GetHashCode(Key);
      hash = ( hash * 23 ) + s_nameComparer.GetHashCode(Name);
      return hash;
    }

    #endregion
  }
}
