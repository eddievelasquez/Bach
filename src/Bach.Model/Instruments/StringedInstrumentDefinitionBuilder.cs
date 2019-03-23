﻿// Module Name: StringedInstrumentDefinitionBuilder.cs
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
  using System.Collections.Generic;
  using Internal;
  using Model.Internal;

  /// <summary>Creates stringed instrument definitions.</summary>
  public class StringedInstrumentDefinitionBuilder
  {
    #region Nested type: TuningInfo

    private struct TuningInfo
    {
      #region Properties

      public string Name { get; set; }
      public PitchCollection Pitches { get; set; }

      #endregion
    }

    #endregion

    #region Data Members

    private readonly StringedInstrumentDefinitionState _state;

    private readonly Dictionary<string, TuningInfo> _tuningInfo = new Dictionary<string, TuningInfo>(StringComparer.CurrentCultureIgnoreCase);

    private bool _built;

    #endregion

    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key for the instrument to build. The key will be used to create the name.</param>
    /// <param name="stringCount">Number of strings of the instrument to build.</param>
    public StringedInstrumentDefinitionBuilder(string key,
                                               int stringCount)
    {
      _state = new StringedInstrumentDefinitionState(Guid.NewGuid(), key, key, stringCount);
    }

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key for the instrument to build.</param>
    /// <param name="name">The localizable name of the instrument to build.</param>
    /// <param name="stringCount">Number of strings of the instrument to build.</param>
    public StringedInstrumentDefinitionBuilder(string key,
                                               string name,
                                               int stringCount)
    {
      _state = new StringedInstrumentDefinitionState(Guid.NewGuid(), key, name, stringCount);
    }

    #endregion

    #region Public Methods

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">
    ///   The language-neutral key for the tuning. The key will be used to
    ///   create the tunings name.
    /// </param>
    /// <param name="pitches">A string that represents the pitches.</param>
    /// <returns>This instance.</returns>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string pitches)
      => AddTuning(key, key, PitchCollection.Parse(pitches));

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">The language-neutral key for the tuning.</param>
    /// <param name="name">The localizable name of the tuning.</param>
    /// <param name="pitches">A string that represents the pitches.</param>
    /// <returns>This instance.</returns>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         string pitches)
      => AddTuning(key, name, PitchCollection.Parse(pitches));

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">
    ///   The language-neutral key for the tuning. The key will be used to create the
    ///   tunings name.
    /// </param>
    /// <param name="pitches">An array with the tunings pitches.</param>
    /// <returns>This instance.</returns>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         params Pitch[] pitches)
      => AddTuning(key, key, new PitchCollection(pitches));

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">The language-neutral key for the tuning.</param>
    /// <param name="name">The localizable name of the tuning.</param>
    /// <param name="pitches">An array with the tunings pitches.</param>
    /// <returns>This instance.</returns>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         params Pitch[] pitches)
      => AddTuning(key, name, new PitchCollection(pitches));

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">
    ///   The language-neutral key for the tuning. The key will be used to create the
    ///   tunings name.
    /// </param>
    /// <param name="pitches">A pitch collection.</param>
    /// <returns>This instance.</returns>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         PitchCollection pitches)
      => AddTuning(key, key, pitches);

    /// <summary>Adds a tuning to the instrument to build.</summary>
    /// <param name="key">The language-neutral key for the tuning.</param>
    /// <param name="name">The localizable name of the tuning.</param>
    /// <param name="pitches">A pitch collection.</param>
    /// <returns>This instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either the key, name or pitch collection are null.</exception>
    /// <exception cref="ArgumentException">
    ///   Thrown when either the key or name are empty, or when the number of pitches in the
    ///   pitch collection doesn't match the number of string for the instrument.
    /// </exception>
    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         PitchCollection pitches)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(pitches != null, "Must provide a pitch collection");

      if( pitches.Count != _state.StringCount )
      {
        throw new ArgumentException($"Must provide exactly {_state.StringCount} pitches");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Pitches = new PitchCollection(pitches) };
      _tuningInfo.Add(key, info);
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
        throw new InvalidOperationException("A StringedInstrumentDefinition must have at least one Tuning");
      }

      if( !_tuningInfo.ContainsKey("standard") )
      {
        throw new InvalidOperationException("Must provide a standard tuning");
      }

      var definition = new StringedInstrumentDefinition(_state);
      foreach( KeyValuePair<string, TuningInfo> info in _tuningInfo )
      {
        _state.Tunings.Add(new Tuning(definition, info.Key, info.Value.Name, info.Value.Pitches));
      }

      _built = true;
      return definition;
    }

    #endregion

    #region  Implementation

    private void CheckBuilderReuse()
    {
      if( _built )
      {
        throw new InvalidOperationException("Cannot reuse a builder");
      }
    }

    #endregion
  }
}
