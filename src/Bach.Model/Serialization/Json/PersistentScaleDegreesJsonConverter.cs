// Module Name: PersistentScaleDegreesJsonConverter.cs
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

using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bach.Model.Internal;

namespace Bach.Model.Serialization.Json;

/// <summary>
///   Provides a custom JSON converter for serializing and deserializing arrays of <see cref="PersistentScaleDegree"/>
///   objects.
/// </summary>
internal sealed class PersistentScaleDegreesJsonConverter: JsonConverter<PersistentScaleDegree[]>
{
  #region Public Methods

  /// <summary>
  ///   Reads and converts the JSON representation of an array of <see cref="PersistentScaleDegree"/> objects.
  /// </summary>
  /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
  /// <param name="typeToConvert">The type to convert.</param>
  /// <param name="options">The serializer options.</param>
  /// <returns>An array of <see cref="PersistentScaleDegree"/> objects.</returns>
  /// <exception cref="JsonException">Thrown when the JSON is invalid.</exception>
  public override PersistentScaleDegree[] Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options )
  {
    if( reader.TokenType == JsonTokenType.String )
    {
      var value = reader.GetString();

      if( string.IsNullOrWhiteSpace( value ) )
      {
        throw new JsonException( "A scale degree string must contain at least one interval." );
      }

      return
      [
        .. value.Split( ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries )
                .Select( interval => new PersistentScaleDegree( interval ) )
      ];
    }

    if( reader.TokenType != JsonTokenType.StartArray )
    {
      throw new JsonException( "Scale degrees must be a comma-separated interval string or an array of objects." );
    }

    return JsonSerializer.Deserialize<PersistentScaleDegree[]>( ref reader, options )
           ?? throw new JsonException( "Scale degree array cannot be null." );
  }

  /// <summary>
  ///   Writes the JSON representation of an array of <see cref="PersistentScaleDegree"/> objects.
  /// </summary>
  /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
  /// <param name="value">The array of <see cref="PersistentScaleDegree"/> objects to write.</param>
  /// <param name="options">The serializer options.</param>
  public override void Write(
    Utf8JsonWriter writer,
    PersistentScaleDegree[] value,
    JsonSerializerOptions options )
  {
    ArgumentNullException.ThrowIfNull( value );

    if( value.All( degree => degree.Metadata is null || degree.Metadata.Empty ) )
    {
      writer.WriteStringValue( string.Join( ',', value.Select( degree => degree.Interval ) ) );
      return;
    }

    writer.WriteStartArray();

    foreach( var degree in value )
    {
      JsonSerializer.Serialize( writer, degree, options );
    }

    writer.WriteEndArray();
  }

  #endregion
}
