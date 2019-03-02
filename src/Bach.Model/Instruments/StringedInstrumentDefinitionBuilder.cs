//
// Module Name: StringedInstrumentDefinitionBuilder.cs
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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections.Generic;
  using Internal;

  public class StringedInstrumentDefinitionBuilder
  {
    private readonly StringedInstrumentDefinitionState _state;

    private readonly Dictionary<string, TuningInfo> _tuningInfo = new Dictionary<string, TuningInfo>(StringComparer.CurrentCultureIgnoreCase);

    private bool _built;

    public StringedInstrumentDefinitionBuilder(string key,
                                               string name,
                                               int stringCount)
    {
      _state = new StringedInstrumentDefinitionState(Guid.NewGuid(), key, name, stringCount);
    }

    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         string notes,
                                                         int defaultOctave = 4)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.RequiresNotNullOrEmpty(notes, "Must provide tuning pitches");

      PitchCollection collection = PitchCollection.Parse(notes, defaultOctave);
      if( collection.Count != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} pitches");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Pitches = new PitchCollection(collection) };
      _tuningInfo.Add(key, info);
      return this;
    }

    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         params Pitch[] pitches)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");

      if( pitches.Length != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} pitches");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Pitches = new PitchCollection(pitches) };
      _tuningInfo.Add(key, info);
      return this;
    }

    public StringedInstrumentDefinitionBuilder AddTuning(string key,
                                                         string name,
                                                         PitchCollection pitches)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a tuning key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(pitches != null, "Must provide a pitch collection");

      if( pitches.Count != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} pitches");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Pitches = new PitchCollection(pitches) };
      _tuningInfo.Add(key, info);
      return this;
    }

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

    private void CheckBuilderReuse()
    {
      if( _built )
      {
        throw new InvalidOperationException("Cannot reuse a builder");
      }
    }

    private struct TuningInfo
    {
      public string Name { get; set; }
      public PitchCollection Pitches { get; set; }
    }
  }
}
