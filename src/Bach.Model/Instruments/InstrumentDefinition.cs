// Module Name: InstrumentDefinition.cs
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
  using Internal;
  using Model.Internal;

  /// <summary>A base class for an instrument definition.</summary>
  public abstract class InstrumentDefinition
    : IKeyNameObject,
      IEquatable<InstrumentDefinition>
  {
    #region Constructors

    internal InstrumentDefinition(InstrumentDefinitionState state)
    {
      Contract.Requires<ArgumentNullException>(state != null);
      State = state;
    }

    #endregion

    #region Properties

    private static StringComparer Comparer { get; } = StringComparer.CurrentCultureIgnoreCase;

    internal InstrumentDefinitionState State { get; }

    /// <summary>Gets the unique identifier of the instrument.</summary>
    /// <value>The identifier of the instrument.</value>
    public Guid InstrumentId => State.InstrumentId;

    /// <summary>Gets the name of the instrument.</summary>
    /// <value>The name of the instrument.</value>
    public string InstrumentName => State.InstrumentName;

    #endregion

    #region IEquatable<InstrumentDefinition> Members

    /// <inheritdoc />
    public bool Equals(InstrumentDefinition other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }

      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      // InstrumentId is only used for hashcode calculation,
      // don't used it for equality
      return Comparer.Equals(InstrumentName, other.InstrumentName);
    }

    #endregion

    #region IKeyNameObject Members

    /// <summary>Returns the language-neutral key for the instrument.</summary>
    /// <value>The key.</value>
    /// <inheritdoc />
    public string Key => State.InstrumentKey;

    /// <inheritdoc />
    public string Name => State.InstrumentName;

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override int GetHashCode() => InstrumentId.GetHashCode();

    #endregion
  }
}
