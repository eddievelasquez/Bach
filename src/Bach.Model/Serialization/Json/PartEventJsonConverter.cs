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

namespace Bach.Model.Serialization.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
///   Provides extension methods for configuring JsonSerializerOptions to handle IPartEvent serialization and
///   deserialization.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
  #region Implementation

  /// <param name="options">The JsonSerializerOptions to which the converters will be added.</param>
  extension(
    JsonSerializerOptions options )
  {
    #region Public Methods

    /// <summary>
    ///   Adds the converters for serializing and deserializing IPartEvent objects to the provided JsonSerializerOptions.
    /// </summary>
    /// <returns>The updated JsonSerializerOptions with the converters added.</returns>
    public JsonSerializerOptions AddConverters()
    {
      options.Converters.Add( new PartEventJsonConverter() );
      return options;
    }

    #endregion
  }

  #endregion
}

internal class PartEventJsonConverter: JsonConverter<IPartEvent>
{
  #region Public Methods

  public override IPartEvent? Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options )
  {
    throw new NotImplementedException();
  }

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
}
