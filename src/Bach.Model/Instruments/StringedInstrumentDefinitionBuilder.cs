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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.Contracts;
  using Internal;

  public class StringedInstrumentDefinitionBuilder
  {
    #region Data Members

    private readonly StringedInstrumentDefinitionState _state;

    private readonly Dictionary<string, TuningInfo> _tuningInfo =
      new Dictionary<string, TuningInfo>(StringComparer.CurrentCultureIgnoreCase);

    private bool _built;

    #endregion

    #region Construction/Destruction

    public StringedInstrumentDefinitionBuilder(string key, string name, int stringCount)
    {
      _state = new StringedInstrumentDefinitionState(Guid.NewGuid(), key, name, stringCount);
    }

    #endregion

    #region Public Methods

    public StringedInstrumentDefinitionBuilder AddTuning(string key, string name, string notes, int defaultOctave = 4)
    {
      Contract.Requires<ArgumentNullException>(key != null, "Must provide a tuning key");
      Contract.Requires<ArgumentException>(key.Length > 0, "Must provide a tuning key");
      Contract.Requires<ArgumentNullException>(name != null, "Must provide a tuning name");
      Contract.Requires<ArgumentException>(name.Length > 0, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(notes != null, "Must provide tuning notes");
      Contract.Requires<ArgumentException>(notes.Length > 0, "Must provide tuning notes");
      Contract.Ensures(Contract.Result<StringedInstrumentDefinitionBuilder>() != null);

      AbsoluteNoteCollection collection = AbsoluteNoteCollection.Parse(notes, defaultOctave);
      if( collection.Count != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} notes");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Notes = new AbsoluteNoteCollection(collection) };
      _tuningInfo.Add(key, info);
      return this;
    }

    public StringedInstrumentDefinitionBuilder AddTuning(string key, string name, params AbsoluteNote[] notes)
    {
      Contract.Requires<ArgumentNullException>(key != null, "Must provide a tuning key");
      Contract.Requires<ArgumentException>(key.Length > 0, "Must provide a tuning key");
      Contract.Requires<ArgumentNullException>(name != null, "Must provide a tuning name");
      Contract.Requires<ArgumentException>(name.Length > 0, "Must provide a tuning name");
      Contract.Ensures(Contract.Result<StringedInstrumentDefinitionBuilder>() != null);

      if( notes.Length != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} notes");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Notes = new AbsoluteNoteCollection(notes) };
      _tuningInfo.Add(key, info);
      return this;
    }

    public StringedInstrumentDefinitionBuilder AddTuning(string key, string name, AbsoluteNoteCollection notes)
    {
      Contract.Requires<ArgumentNullException>(key != null, "Must provide a tuning key");
      Contract.Requires<ArgumentException>(key.Length > 0, "Must provide a tuning key");
      Contract.Requires<ArgumentNullException>(name != null, "Must provide a tuning name");
      Contract.Requires<ArgumentException>(name.Length > 0, "Must provide a tuning name");
      Contract.Requires<ArgumentNullException>(notes != null, "Must provide a note collection");
      Contract.Ensures(Contract.Result<StringedInstrumentDefinitionBuilder>() != null);

      if( notes.Count != _state.StringCount )
      {
        throw new ArgumentException("Must provide exactly {_state.StringCount} notes");
      }

      CheckBuilderReuse();

      var info = new TuningInfo { Name = name, Notes = new AbsoluteNoteCollection(notes) };
      _tuningInfo.Add(key, info);
      return this;
    }

    public StringedInstrumentDefinition Build()
    {
      Contract.Ensures(Contract.Result<StringedInstrumentDefinition>() != null);

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
      foreach( var info in _tuningInfo )
      {
        _state.Tunings.Add(new Tuning(definition, info.Key, info.Value.Name, info.Value.Notes));
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
        throw new InvalidOperationException("Cannot reuse a builder");
      }
    }

    #endregion

    #region Nested type: TuningInfo

    private struct TuningInfo
    {
      #region Properties

      public string Name { get; set; }
      public AbsoluteNoteCollection Notes { get; set; }

      #endregion
    }

    #endregion
  }
}
