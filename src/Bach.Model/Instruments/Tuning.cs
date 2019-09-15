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
  using System.Linq;
  using Model.Internal;

  /// <summary>A tuning is the set of pitches for a stringed instrument when no string has been pressed.</summary>
  public class Tuning: IEquatable<Tuning>
  {
    #region Constructors

    internal Tuning(StringedInstrumentDefinition instrumentDefinition,
                    string id,
                    string name,
                    PitchCollection pitches)
    {
      Contract.Requires<ArgumentNullException>(instrumentDefinition != null, "Must provide an instrument definition");
      Contract.RequiresNotNullOrEmpty(id, "Must provide a tuning id");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(pitches != null, "Must provide a pitch collection");
      Contract.Requires<ArgumentOutOfRangeException>(pitches.Count == instrumentDefinition.StringCount,
                                                     "The number of pitch must match the instrument's string count");

      InstrumentDefinition = instrumentDefinition;
      Id = id;
      Name = name;
      Pitches = pitches;
    }

    #endregion

    #region Properties

    /// <summary>Gets the instruments definition.</summary>
    /// <value>The instrument definition.</value>
    public StringedInstrumentDefinition InstrumentDefinition { get; }

    /// <summary>Gets the language-neutral identifier for the tuning.</summary>
    /// <value>The id.</value>
    public string Id { get; }

    /// <summary>Gets the localizable name for the tuning.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the tunings pitches.</summary>
    /// <value>The pitches.</value>
    public PitchCollection Pitches { get; }

    /// <summary>Indexer to get pitches within this tuning using array index syntax.</summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown when the string number is outside the string range.
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
      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      if( other is null )
      {
        return false;
      }

      return InstrumentDefinition.Equals(other.InstrumentDefinition)
             && Comparer.IdComparer.Equals(Id, other.Id)
             && Comparer.NameComparer.Equals(Name, other.Name)
             && Pitches.SequenceEqual(other.Pitches);
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj is Tuning other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hash = 17;
      hash = ( hash * 23 ) + InstrumentDefinition.GetHashCode();
      hash = ( hash * 23 ) + Comparer.IdComparer.GetHashCode(Id);
      hash = ( hash * 23 ) + Comparer.NameComparer.GetHashCode(Name);
      return hash;
    }

    #endregion
  }
}
