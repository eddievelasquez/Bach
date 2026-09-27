// Module Name: Registry.cs
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using PersistentLibrary = Bach.Model.Serialization.Internal.Library;

namespace Bach.Model.Library;

/// <summary>
///   The registry provides access to all the predefined formulas and definitions that can be found in the
///   Bach.Model.Library.json file.
/// </summary>
public sealed class Registry
{
  #region Constants

  private const string LIBRARY_FILE_NAME = "Bach.Model.Library.json";

  private static readonly JsonSerializerOptions s_jsonSerializerOptions =
    new() { Converters = { new PersistentScaleDegreesJsonConverterFactory() } };

  private readonly Lookup<ChordFormula> _chordSymbolLookup;

  /// <summary>
  ///   Gets the shared registry instance.
  /// </summary>
  public static Registry Instance { get; } = new();

  #endregion

  #region Constructors

  [SuppressMessage(
    "Blocker Code Smell",
    "S3877:Exceptions should not be thrown from unexpected methods",
    Justification = "Must abort if the library cannot be loaded"
  )]
  private Registry()
  {
    var path = GetLibraryPath();
    var library = LoadLibrary( path );

    if( library is null )
    {
      // NOTE: An exception in a static constructor stops the application.
      throw new InvalidOperationException( $"Could not load the library from {path}" );
    }

    ScaleFormulas = LoadScaleFormulas( library );
    ChordFormulas = LoadChordFormulas( library );
    StringedInstrumentDefinitions = LoadStringedInstruments( library );

    // Create a lookup for chord formulas by symbol for fast access
    _chordSymbolLookup = new Lookup<ChordFormula>( ChordFormulas, cf => cf.Symbol );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the collection of scale formulas.
  /// </summary>
  /// <value>
  ///   The scale formulas.
  /// </value>
  public NamedObjectCollection<ScaleFormula> ScaleFormulas { get; }

  /// <summary>
  ///   Gets the collection of chord formulas.
  /// </summary>
  /// <value>
  ///   The chord formulas.
  /// </value>
  public NamedObjectCollection<ChordFormula> ChordFormulas { get; }

  /// <summary>
  ///   Gets the collection of stringed instrument definitions.
  /// </summary>
  /// <value>
  ///   The stringed instrument definitions.
  /// </value>
  public NamedObjectCollection<StringedInstrumentDefinition> StringedInstrumentDefinitions { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Tries to get a chord formula by ID or name.
  /// </summary>
  /// <param name="idOrName">
  ///   The ID or name of the chord formula.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the chord formula associated with the specified ID or name, if found;
  ///   otherwise, null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if the chord formula is found; otherwise, false.
  /// </returns>
  public bool TryGetChordFormula(
    string idOrName,
    [MaybeNullWhen( false )] out ChordFormula result )
  {
    return ChordFormulas.TryGetValue( idOrName, out result );
  }

  /// <summary>
  ///   Tries to get a chord formula by symbol.
  /// </summary>
  /// <param name="symbol">
  ///   The symbol of the chord formula.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the chord formula associated with the specified symbol, if found;
  ///   otherwise, null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if the chord formula is found; otherwise, false.
  /// </returns>
  public bool TryGetChordFormulaBySymbol(
    string symbol,
    [MaybeNullWhen( false )] out ChordFormula result )
  {
    return _chordSymbolLookup.TryGetValue( symbol, out result );
  }

  /// <summary>
  ///   Tries to get a chord formula by symbol.
  /// </summary>
  /// <param name="symbol">
  ///   The symbol of the chord formula.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the chord formula associated with the specified symbol, if found;
  ///   otherwise, null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if the chord formula is found; otherwise, false.
  /// </returns>
  public bool TryGetChordFormulaBySymbol(
    ReadOnlySpan<char> symbol,
    [MaybeNullWhen( false )] out ChordFormula result )
  {
    return _chordSymbolLookup.TryGetValue( symbol, out result );
  }

  /// <summary>
  ///   Tries to get a scale formula by ID or name.
  /// </summary>
  /// <param name="idOrName">
  ///   The ID or name of the scale formula.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the scale formula associated with the specified ID or name, if found;
  ///   otherwise, null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if the scale formula is found; otherwise, false.
  /// </returns>
  public bool TryGetScaleFormula(
    string idOrName,
    [MaybeNullWhen( false )] out ScaleFormula result )
  {
    return ScaleFormulas.TryGetValue( idOrName, out result );
  }

  /// <summary>
  ///   Tries to get a stringed instrument definition by ID or name.
  /// </summary>
  /// <param name="idOrName">
  ///   The ID or name of the stringed instrument definition.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the stringed instrument definition associated with the specified ID or name, if
  ///   found;
  ///   otherwise, null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if the stringed instrument definition is found; otherwise, false.
  /// </returns>
  public bool TryGetStringedInstrumentDefinition(
    string idOrName,
    [MaybeNullWhen( false )] out StringedInstrumentDefinition result )
  {
    return StringedInstrumentDefinitions.TryGetValue( idOrName, out result );
  }

  #endregion

  #region Implementation

  private static NamedObjectCollection<StringedInstrumentDefinition> LoadStringedInstruments(
    PersistentLibrary library )
  {
    List<StringedInstrumentDefinition> definitions = [];

    foreach( var instrument in library.StringedInstruments )
    {
      var builder = new StringedInstrumentDefinitionBuilder( instrument.Id, instrument.Name, instrument.StringCount );

      foreach( var tuning in instrument.Tunings )
      {
        builder.AddTuning( tuning.Id, tuning.Name, tuning.Pitches );
      }

      var definition = builder.Build();
      definitions.Add( definition );
    }

    return new NamedObjectCollection<StringedInstrumentDefinition>( definitions );
  }

  private static NamedObjectCollection<ChordFormula> LoadChordFormulas(
    PersistentLibrary library )
  {
    List<ChordFormula> formulas = [];

    foreach( var chord in library.Chords )
    {
      var formula = new ChordFormula( chord.Id, chord.Name, chord.Symbol, chord.Formula );
      formulas.Add( formula );
    }

    return new NamedObjectCollection<ChordFormula>( formulas );
  }

  private static NamedObjectCollection<ScaleFormula> LoadScaleFormulas(
    PersistentLibrary library )
  {
    List<ScaleFormula> formulas = [];

    foreach( var scale in library.Scales )
    {
      var builder = new ScaleFormulaBuilder( scale.Id, scale.Name )
        .SetAscendingIntervals( ParseDegreeIntervals( scale.AscendingDegrees ) );

      if( scale.DescendingDegrees is not null )
      {
        builder.SetDescendingIntervals( ParseDegreeIntervals( scale.DescendingDegrees ) );
      }

      if( scale.Alias is not null )
      {
        builder.AddAlias( scale.Alias );
      }

      var classification = scale.Classification;

      if( classification is not null )
      {
        builder.Classify( b =>
          {
            b.AddRepertoireTag( classification.RepertoireTags )
             .SetParentScaleId( classification.ParentScaleId )
             .SetModalRotationIndex( classification.ModalRotationIndex )
             .SetKeyCandidate( classification.IsKeyCandidate );
          }
        );
      }

      var formula = builder.Build();
      formulas.Add( formula );
    }

    return new NamedObjectCollection<ScaleFormula>( formulas );

    static IEnumerable<Interval> ParseDegreeIntervals(
      IEnumerable<PersistentScaleDegree> degrees )
    {
      return degrees.Select( degree => Interval.Parse( degree.Interval ) );
    }
  }

  private static string GetLibraryPath()
  {
    // Load the library from the JSON file in the same directory as
    // this assembly.
    var assembly = Assembly.GetExecutingAssembly();
    var directory = Path.GetDirectoryName( new Uri( assembly.Location ).LocalPath );
    var path = Path.Combine( directory ?? string.Empty, LIBRARY_FILE_NAME );
    return path;
  }

  private static PersistentLibrary? LoadLibrary(
    string path )
  {
    var json = File.ReadAllText( path );

    // Deserialize
    var library = JsonSerializer.Deserialize<PersistentLibrary>( json, s_jsonSerializerOptions );
    return library;
  }

  #endregion
}
