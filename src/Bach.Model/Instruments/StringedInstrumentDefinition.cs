// Module Name: StringedInstrumentDefinition.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System;
using Bach.Model.Instruments.Internal;

namespace Bach.Model.Instruments;

/// <summary>A stringed instrument definition.</summary>
public sealed class StringedInstrumentDefinition
  : InstrumentDefinition,
    IEquatable<StringedInstrumentDefinition>
{
#region Constructors

  internal StringedInstrumentDefinition( StringedInstrumentDefinitionState state )
    : base( state )
  {
  }

#endregion

#region Properties

  /// <summary>Gets the number of strings for the instrument.</summary>
  /// <value>The number of strings.</value>
  public int StringCount => State.StringCount;

  /// <summary>Gets the instruments tunings.</summary>
  /// <value>The tunings.</value>
  public TuningCollection Tunings => State.Tunings;

  private new StringedInstrumentDefinitionState State => (StringedInstrumentDefinitionState) base.State;

#endregion

#region Public Methods

  /// <inheritdoc />
  public bool Equals( StringedInstrumentDefinition? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return base.Equals( other ) && StringCount == other.StringCount;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    return obj is StringedInstrumentDefinition other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( base.GetHashCode(), StringCount );
  }

#endregion
}
