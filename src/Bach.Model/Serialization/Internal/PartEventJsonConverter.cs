// Module Name: PartEventJsonConverter.cs
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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bach.Model.Serialization.Internal;

internal class PartEventJsonConverter: JsonConverter<IPartEvent>
{
  #region Public Methods

  /// <summary>
  ///   Reads a part event from JSON.
  /// </summary>
  /// <param name="reader"> The UTF-8 JSON reader. </param>
  /// <param name="typeToConvert"> The type to convert. </param>
  /// <param name="options"> The JSON serializer options. </param>
  /// <returns> The deserialized part event. </returns>
  /// <exception cref="JsonException"> Thrown when the JSON is invalid. </exception>
  public override IPartEvent Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options )
  {
    using var document = JsonDocument.ParseValue( ref reader );
    var root = document.RootElement;

    if( root.ValueKind != JsonValueKind.Object
        || !root.TryGetProperty( "type", out var typeElement )
        || typeElement.ValueKind != JsonValueKind.String )
    {
      throw new JsonException( "A part event must have a string type." );
    }

    return typeElement.GetString() switch
    {
      "Pitch"  => ReadPitch( root ),
      "Chord"  => ReadChord( root ),
      var type => throw new JsonException( $"Unknown event type {type}" )
    };
  }

  /// <summary>
  ///   Writes a part event to JSON.
  /// </summary>
  /// <param name="writer"> The UTF-8 JSON writer. </param>
  /// <param name="value"> The part event to write. </param>
  /// <param name="options"> The JSON serializer options. </param>
  /// <exception cref="JsonException"> Thrown when the part event type is unknown. </exception>
  public override void Write(
    Utf8JsonWriter writer,
    IPartEvent value,
    JsonSerializerOptions options )
  {
    writer.WriteStartObject();

    switch( value )
    {
      case Pitch pitch:
        writer.WriteString( "type", "Pitch" );
        writer.WriteString( "pitch", pitch.ToString() );
        break;

      case PitchChord chord:
        writer.WriteString( "type", "Chord" );
        writer.WriteString( "root", chord.Root.ToString() );
        writer.WriteString( "formula", chord.Formula.Id );
        writer.WriteNumber( "inversion", chord.Inversion );
        break;

      default:
        throw new JsonException( $"Unknown event type {value.GetType().Name}" );
    }

    writer.WriteEndObject();
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Reads a pitch event from JSON.
  /// </summary>
  /// <param name="root"> The JSON element representing the pitch event. </param>
  /// <returns> The deserialized pitch event. </returns>
  /// <exception cref="JsonException"> Thrown when the JSON is invalid. </exception>
  private static Pitch ReadPitch(
    JsonElement root )
  {
    var pitchText = GetRequiredString( root, "pitch" );

    try
    {
      return Pitch.Parse( pitchText );
    }
    catch( FormatException exception )
    {
      throw new JsonException( "The event pitch is invalid.", exception );
    }
  }

  /// <summary>
  ///   Reads a chord event from JSON.
  /// </summary>
  /// <param name="root"> The JSON element representing the chord event. </param>
  /// <returns> The deserialized chord event. </returns>
  /// <exception cref="JsonException"> Thrown when the JSON is invalid. </exception>
  private static PitchChord ReadChord(
    JsonElement root )
  {
    var rootText = GetRequiredString( root, "root" );
    var formulaId = GetRequiredString( root, "formula" );

    if( !root.TryGetProperty( "inversion", out var inversionElement )
        || inversionElement.ValueKind != JsonValueKind.Number
        || !inversionElement.TryGetInt32( out var inversion ) )
    {
      throw new JsonException( "A chord event must have an integer inversion." );
    }

    Pitch pitch;

    try
    {
      pitch = Pitch.Parse( rootText );
    }
    catch( FormatException exception )
    {
      throw new JsonException( "The chord root is invalid.", exception );
    }

    if( !Registry.Instance.TryGetChordFormula( formulaId, out var formula ) )
    {
      throw new JsonException( $"Unknown chord formula {formulaId}." );
    }

    try
    {
      return new PitchChord( pitch, formula, inversion );
    }
    catch( ArgumentOutOfRangeException exception )
    {
      throw new JsonException( "The chord inversion is invalid.", exception );
    }
  }

  /// <summary>
  ///   Gets a required string property from a JSON element.
  /// </summary>
  /// <param name="element"> The JSON element. </param>
  /// <param name="propertyName"> The name of the property to retrieve. </param>
  /// <returns> The value of the property. </returns>
  /// <exception cref="JsonException"> Thrown when the property is missing or not a string. </exception>
  private static string GetRequiredString(
    JsonElement element,
    string propertyName )
  {
    if( !element.TryGetProperty( propertyName, out var value ) || value.ValueKind != JsonValueKind.String )
    {
      throw new JsonException( $"A part event must have a string {propertyName} property." );
    }

    return value.GetString()
           ?? throw new JsonException( $"A part event must have a string {propertyName} property." );
  }

  #endregion
}
