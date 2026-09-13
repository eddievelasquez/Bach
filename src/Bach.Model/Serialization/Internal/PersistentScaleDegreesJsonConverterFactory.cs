// Module Name: PersistentScaleDegreesJsonConverterFactory.cs
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

/// <summary>
///   Provides a factory for creating <see cref="PersistentScaleDegreesJsonConverter"/> instances for arrays of
///   <see cref="PersistentScaleDegree"/>.
/// </summary>
internal class PersistentScaleDegreesJsonConverterFactory: JsonConverterFactory
{
  #region Public Methods

  /// <summary>
  ///   Determines whether the converter can convert the specified type.
  /// </summary>
  /// <param name="typeToConvert">The type to convert.</param>
  /// <returns><c>true</c> if the converter can convert the specified type; otherwise, <c>false</c>.</returns>
  public override bool CanConvert(
    Type typeToConvert )
  {
    return typeToConvert.IsArray && typeToConvert.GetElementType() == typeof( PersistentScaleDegree );
  }

  /// <summary>
  ///   Creates a converter for the specified type.
  /// </summary>
  /// <param name="typeToConvert">The type to convert.</param>
  /// <param name="options">The serializer options.</param>
  /// <returns>A <see cref="JsonConverter"/> instance for the specified type.</returns>
  public override JsonConverter? CreateConverter(
    Type typeToConvert,
    JsonSerializerOptions options )
  {
    return new PersistentScaleDegreesJsonConverter();
  }

  #endregion
}
