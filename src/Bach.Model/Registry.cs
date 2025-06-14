// Module Name: Registry.cs
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

namespace Bach.Model;

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Instruments;
using Internal;
using Serialization;

/// <summary>
///   The registry provides access to all the predefined formulas and definitions that can be found in the
///   Bach.Model.Library.json file.
/// </summary>
public static class Registry
{
  #region Constants

  private const string LIBRARY_FILE_NAME = "Bach.Model.Library.json";

  #endregion

  #region Constructors

  [SuppressMessage(
    "Blocker Code Smell",
    "S3877:Exceptions should not be thrown from unexpected methods",
    Justification = "Must abort if the library cannot be loaded"
  )]
  static Registry()
  {
    var path = GetLibraryPath();
    var library = LoadLibrary( path );
    if( library is null )
    {
      // NOTE: Throwing in a static ctor will cause the application to terminate
      throw new InvalidOperationException( $"Could not load the library from {path}" );
    }

    ScaleFormulas = [];
    ChordFormulas = [];
    StringedInstrumentDefinitions = [];

    // Load scales
    foreach( var scale in library.Scales )
    {
      var builder = new ScaleFormulaBuilder( scale.Id, scale.Name ).SetIntervals( scale.Formula );

      if( scale.Alias is not null )
      {
        builder.AddAlias( scale.Alias );
      }

      if( scale.Categories is not null )
      {
        builder.AddCategory( scale.Categories );
      }

      var formula = builder.Build();
      ScaleFormulas.Add( formula );
    }

    // Load Chords
    foreach( var chord in library.Chords )
    {
      ChordFormulas.Add( new ChordFormula( chord.Id, chord.Name, chord.Symbol, chord.Formula ) );
    }

    // Load Instrument definitions
    foreach( var instrument in library.StringedInstruments )
    {
      var builder = new StringedInstrumentDefinitionBuilder( instrument.Id, instrument.Name, instrument.StringCount );

      foreach( var tuning in instrument.Tunings )
      {
        builder.AddTuning( tuning.Id, tuning.Name, tuning.Pitches );
      }

      var definition = builder.Build();
      StringedInstrumentDefinitions.Add( definition );
    }
  }

  #endregion

  #region Properties

  /// <summary>Gets the collection of scale formulas.</summary>
  /// <value>The scale formulas.</value>
  public static NamedObjectCollection<ScaleFormula> ScaleFormulas { get; }

  /// <summary>Gets the collection of chord formulas.</summary>
  /// <value>The chord formulas.</value>
  public static NamedObjectCollection<ChordFormula> ChordFormulas { get; }

  /// <summary>Gets the collection of stringed instrument definitions.</summary>
  /// <value>The stringed instrument definitions.</value>
  public static NamedObjectCollection<StringedInstrumentDefinition> StringedInstrumentDefinitions { get; }

  #endregion

  #region Implementation

  private static string GetLibraryPath()
  {
    // Load the library from the JSON file in the same directory as
    // this assembly.
    var assembly = Assembly.GetExecutingAssembly();
    var directory = Path.GetDirectoryName( new Uri( assembly.Location ).LocalPath );
    var path = Path.Combine( directory ?? string.Empty, LIBRARY_FILE_NAME );
    return path;
  }

  private static Library? LoadLibrary(
    string path )
  {
    var json = File.ReadAllText( path );

    // Deserialize
    var library = JsonSerializer.Deserialize<Library>( json );
    return library;
  }

  #endregion
}
