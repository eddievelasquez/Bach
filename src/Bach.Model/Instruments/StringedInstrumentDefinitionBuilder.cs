// Module Name: StringedInstrumentDefinitionBuilder.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2026  Eddie Velasquez.
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

using System.Collections.Generic;
using Bach.Model.Instruments.Internal;

namespace Bach.Model.Instruments;

/// <summary>
///   Builds a stringed instrument definition from its identity, string count, and named tunings.
///   Each tuning must provide one pitch per string, and the completed definition must include a
///   standard tuning.
/// </summary>
public sealed class StringedInstrumentDefinitionBuilder
{
  #region Nested Types

  private sealed record TuningInfo(
    string Id,
    string Name,
    IReadOnlyList<Pitch> Pitches );

  #endregion

  #region Fields

  private readonly StringedInstrumentDefinitionState _state;
  private readonly Dictionary<string, TuningInfo> _tuningInfo = new( Comparer.IdComparer );
  private bool _built;

  #endregion

  #region Constructors

  /// <summary>Creates a builder for an instrument whose ID is also its name.</summary>
  /// <param name="id">The instrument's language-neutral identifier.</param>
  /// <param name="stringCount">The number of strings.</param>
  public StringedInstrumentDefinitionBuilder(
    string id,
    int stringCount )
  {
    _state = new StringedInstrumentDefinitionState( id, id, stringCount );
  }

  /// <summary>Creates a builder for an instrument with the specified ID, name, and string count.</summary>
  /// <param name="id">The instrument's language-neutral identifier.</param>
  /// <param name="name">The instrument's localizable name.</param>
  /// <param name="stringCount">The number of strings.</param>
  public StringedInstrumentDefinitionBuilder(
    string id,
    string name,
    int stringCount )
  {
    _state = new StringedInstrumentDefinitionState( id, name, stringCount );
  }

  #endregion

  #region Public Methods

  /// <summary>Adds a tuning from a string of pitch names.</summary>
  /// <param name="id">The language-neutral identifier for the tuning.</param>
  /// <param name="name">The localizable name of the tuning.</param>
  /// <param name="pitches">The pitches in the tuning, one for each string.</param>
  /// <returns>This builder.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string name,
    string pitches )
  {
    return AddTuning( id, name, pitches.ParsePitches() );
  }

  /// <summary>Adds a tuning whose ID is also its name.</summary>
  /// <param name="id">
  ///   The tuning's language-neutral identifier.
  /// </param>
  /// <param name="pitches">The pitches in the tuning, one for each string.</param>
  /// <returns>This builder.</returns>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    params Pitch[] pitches )
  {
    return AddTuning( id, id, pitches );
  }

  /// <summary>Adds a tuning with the specified ID, name, and pitches.</summary>
  /// <param name="id">The language-neutral identifier for the tuning.</param>
  /// <param name="name">The localizable name of the tuning.</param>
  /// <param name="pitches">The pitches in the tuning, one for each string.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> or <paramref name="name"/> is null.</exception>
  /// <exception cref="ArgumentException">
  ///   Thrown when <paramref name="id"/> or <paramref name="name"/> is empty, or when the number
  ///   of pitches does not match the instrument's string count.
  /// </exception>
  public StringedInstrumentDefinitionBuilder AddTuning(
    string id,
    string name,
    ICollection<Pitch> pitches )
  {
    ArgumentException.ThrowIfNullOrEmpty( id );
    ArgumentException.ThrowIfNullOrEmpty( name );
    ArgumentOutOfRangeException.ThrowIfNotEqual( pitches.Count, _state.StringCount );
    CheckBuilderReuse();

    var info = new TuningInfo( id, name, [.. pitches] );
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

    if( _tuningInfo.Empty )
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
