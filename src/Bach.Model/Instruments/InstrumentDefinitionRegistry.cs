﻿//  
// Module Name: InstrumentDefinitionRegistry.cs
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

  public static class InstrumentDefinitionRegistry
  {
    #region Data Members

    private static readonly Dictionary<string, InstrumentDefinition> s_definitions =
      new Dictionary<string, InstrumentDefinition>(StringComparer.CurrentCultureIgnoreCase);

    #endregion

    #region Construction/Destruction

    static InstrumentDefinitionRegistry()
    {
      // Add predefined instruments
      AddDefinition("Guitar", 6,
                    b =>
                      b.AddTuning("Standard", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,E2"))
                       .AddTuning("Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"))
                       .AddTuning("Open D", AbsoluteNoteCollection.Parse("D4,A3,F#3,D3,A2,D2"))
                       .AddTuning("Open G", AbsoluteNoteCollection.Parse("D4,B3,G2,D2,G2,D2"))
                       .AddTuning("Open A", AbsoluteNoteCollection.Parse("E4,C#4,A3,E3,A2,E2")));
      AddDefinition("Bass", 4, b => b.AddTuning("Standard", AbsoluteNoteCollection.Parse("G2,D2,A1,E1")));
    }

    #endregion

    #region Public Methods

    public static InstrumentDefinition Lookup(string name)
    {
      Contract.Requires<ArgumentNullException>(name != null, "Must provide an instrument name");
      Contract.Requires<ArgumentException>(name.Length > 0, "Must provide an instrument name");

      InstrumentDefinition definition;
      s_definitions.TryGetValue(name, out definition);
      return definition;
    }

    public static T Lookup<T>(string name) where T: InstrumentDefinition
    {
      return (T) Lookup(name);
    }

    public static InstrumentDefinition Get(string name)
    {
      Contract.Requires<ArgumentNullException>(name != null, "Must provide an instrument name");
      Contract.Requires<ArgumentException>(name.Length > 0, "Must provide an instrument name");

      InstrumentDefinition definition = s_definitions[name];
      return definition;
    }

    public static T Get<T>(string name) where T: InstrumentDefinition
    {
      return (T) Get(name);
    }

    #endregion

    #region Implementation

    private static void AddDefinition(
      string name,
      int stringCount,
      Action<StringedInstrumentDefinitionBuilder> addTunings)
    {
      Contract.Requires<ArgumentNullException>(addTunings != null);

      var builder = new StringedInstrumentDefinitionBuilder(name, stringCount);
      addTunings(builder);

      StringedInstrumentDefinition definition = builder.Build();
      s_definitions.Add(definition.Name, definition);
    }

    #endregion
  }
}
