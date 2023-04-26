// Module Name: StringedInstrumentDefinitionBuilder.cs
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
using System.Collections.Generic;
using Bach.Model.Instruments.Internal;
using Bach.Model.Internal;

namespace Bach.Model.Instruments;

/// <summary>Creates stringed instrument definitions.</summary>
public sealed class StringedInstrumentDefinitionBuilder
{
#region Nested Types

  private sealed record TuningInfo(
    string Id,
    string Name,
    PitchCollection Pitches );

#endregion

#region Fields

  private readonly StringedInstrumentDefinitionState _state;
  private readonly Dictionary<string, TuningInfo> _tuningInfo = new( Comparer.IdComparer );
  private bool _built;

#endregion

#region Constructors

  /// <summary>Constructor.</summary>
  /// <param name="id">The language-neutral identifier for the instrument to build. The id will be used to create the name.</param>
  /// <param name="stringCount">Number of strings of the instrument to build.</param>
  public StringedInstrumentDefinitionBuilder(
    string id,
    int stringCount )
  {
    _state = new StringedInstrumentDefinitionState( id, id, stringCount );
  }

  /// <summary>Constructor.</summary>
  /// <param name="id">The language-neutral id for the instrument to build.</param>
  /// <param name="name">The localizable name of the instrument to build.</param>
  /// <param name="stringCount">Number of strings of the instrument to build.</param>
  public StringedInstrumentDefinitionBuilder(
    string id,
    string name,
    int stringCount )
  {
    _state = new StringedInstrumentDefinitionState( id, name, stringCount );
  }

#endregion

#region Public Methods

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">
  ///   The language-neutral identifier for the tuning. The id will be used to
  ///   create the tunings name.
  /// </param>
  /// <param name="pitches">A string that represents the pitches.</param>
  /// <returns>This instance.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string pitches )
  {
    return AddTuning( id, id, PitchCollection.Parse( pitches ) );
  }

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">The language-neutral identifier for the tuning.</param>
  /// <param name="name">The localizable name of the tuning.</param>
  /// <param name="pitches">A string that represents the pitches.</param>
  /// <returns>This instance.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string name,
    string pitches )
  {
    return AddTuning( id, name, PitchCollection.Parse( pitches ) );
  }

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">
  ///   The language-neutral identifier for the tuning. The id will be used to create the
  ///   tunings name.
  /// </param>
  /// <param name="pitches">An array with the tunings pitches.</param>
  /// <returns>This instance.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    params Pitch[] pitches )
  {
    return AddTuning( id, id, new PitchCollection( pitches ) );
  }

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">The language-neutral identifier for the tuning.</param>
  /// <param name="name">The localizable name of the tuning.</param>
  /// <param name="pitches">An array with the tunings pitches.</param>
  /// <returns>This instance.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string name,
    params Pitch[] pitches )
  {
    return AddTuning( id, name, new PitchCollection( pitches ) );
  }

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">
  ///   The language-neutral identifier for the tuning. The identifier will be used to create the
  ///   name of the tuning.
  /// </param>
  /// <param name="pitches">A pitch collection.</param>
  /// <returns>This instance.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    PitchCollection pitches )
  {
    return AddTuning( id, id, pitches );
  }

  /// <summary>Adds a tuning to the instrument to build.</summary>
  /// <param name="id">The language-neutral identifier for the tuning.</param>
  /// <param name="name">The localizable name of the tuning.</param>
  /// <param name="pitches">A pitch collection.</param>
  /// <returns>This instance.</returns>
  /// <exception cref="ArgumentNullException">Thrown when either the id, name or pitch collection are null.</exception>
  /// <exception cref="ArgumentException">
  ///   Thrown when either the id or name are empty, or when the number of pitches in the
  ///   pitch collection doesn't match the number of string for the instrument.
  /// </exception>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string name,
    PitchCollection pitches )
  {
    Requires.NotNullOrEmpty( id );
    Requires.NotNullOrEmpty( name );
    Requires.ExactCount( pitches, _state.StringCount, $"Must provide exactly {_state.StringCount} pitches" );
    CheckBuilderReuse();

    var info = new TuningInfo( id, name, pitches );
    _tuningInfo.Add( id, info );

    return this;
  }

  /// <summary>Builds the new instrument definition.</summary>
  /// <returns>A StringedInstrumentDefinition.</returns>
  /// <exception cref="InvalidOperationException">
  ///   Thrown when no tunings have been added, a tuning named "standard" has not
  ///   been found, or this method has already been called.
  /// </exception>
  public StringedInstrumentDefinition Build()
  {
    CheckBuilderReuse();

    if( _tuningInfo.Count == 0 )
    {
      throw new InvalidOperationException( "A StringedInstrumentDefinition must have at least one Tuning" );
    }

    if( !_tuningInfo.ContainsKey( "standard" ) )
    {
      throw new InvalidOperationException( "Must provide a standard tuning" );
    }

    var definition = new StringedInstrumentDefinition( _state );
    foreach( var info in _tuningInfo )
    {
      _state.Tunings.Add( new Tuning( definition, info.Value.Id, info.Value.Name, info.Value.Pitches ) );
    }

    _built = true;
    return definition;
  }

#endregion

#region Implementation

  private void CheckBuilderReuse()
  {
    if( _built )
    {
      throw new InvalidOperationException( "Cannot reuse a builder" );
    }
  }

#endregion
}
