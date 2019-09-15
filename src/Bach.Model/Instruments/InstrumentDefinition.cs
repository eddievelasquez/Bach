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
    : INamedObject,
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

    internal InstrumentDefinitionState State { get; }

    #endregion

    #region IEquatable<InstrumentDefinition> Members

    /// <inheritdoc />
    public bool Equals(InstrumentDefinition other)
    {
      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      if( other is null )
      {
        return false;
      }

      return Comparer.IdComparer.Equals(Id, other.Id);
    }

    #endregion

    #region IKeyNameObject Members

    /// <inheritdoc />
    public string Id => State.Id;

    /// <inheritdoc />
    public string Name => State.Name;

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj is InstrumentDefinition other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();

    #endregion
  }
}
