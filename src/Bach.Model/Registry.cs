// Module Name: Registry.cs
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

namespace Bach.Model
{
  using System;
  using System.IO;
  using System.Reflection;
  using Internal;
  using Instruments;
  using Newtonsoft.Json;
  using Serialization;

  public static class Registry
  {
    #region Constants

    private const string LibraryFileName = "Bach.Model.Library.json";

    #endregion

    #region Constructors

    static Registry()
    {
      // Load the library from the JSON file in the same directory as
      // this assembly.
      Assembly assembly = Assembly.GetExecutingAssembly();
      string directory = Path.GetDirectoryName(new Uri(assembly.CodeBase).LocalPath);
      string path = Path.Combine(directory ?? "", LibraryFileName);
      string s = File.ReadAllText(path);

      // Deserialize
      var library = JsonConvert.DeserializeObject<PersistentLibrary>(s);

      // Load data
      ScaleFormulas = new KeyedObjectCollection<ScaleFormula>();
      foreach( PersistentScale scale in library.Scales )
      {
        ScaleFormulas.Add(new ScaleFormula(scale.Key, scale.Name, scale.Formula));
      }

      ChordFormulas = new KeyedObjectCollection<ChordFormula>();
      foreach( PersistentChord chord in library.Chords )
      {
        ChordFormulas.Add(new ChordFormula(chord.Key, chord.Name, chord.Symbol, chord.Formula));
      }

      StringedInstrumentDefinitions = new KeyedObjectCollection<StringedInstrumentDefinition>();
      foreach( PersistentStringedInstrument instrument in library.StringedInstruments )
      {
        var builder = new StringedInstrumentDefinitionBuilder(instrument.Key, instrument.Name, instrument.StringCount);
        foreach( PersistentTuning tuning in instrument.Tunings )
        {
          builder.AddTuning(tuning.Key, tuning.Name, tuning.Notes);
        }

        StringedInstrumentDefinition definition = builder.Build();
        StringedInstrumentDefinitions.Add(definition);
      }
    }

    #endregion

    #region Properties

    public static KeyedObjectCollection<ScaleFormula> ScaleFormulas { get; }
    public static KeyedObjectCollection<ChordFormula> ChordFormulas { get; }
    public static KeyedObjectCollection<StringedInstrumentDefinition> StringedInstrumentDefinitions { get; }

    #endregion
  }
}
