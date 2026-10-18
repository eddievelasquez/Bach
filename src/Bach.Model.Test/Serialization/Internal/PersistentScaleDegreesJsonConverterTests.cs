// Module Name: PersistentScaleDegreesJsonConverterTests.cs
// Project:     Bach.Model.Test
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

namespace Bach.Model.Serialization.Internal.Test;

public sealed class PersistentScaleDegreesJsonConverterTests
{
  #region Public Methods

  [Fact]
  public void Read_ShouldParseCommaSeparatedIntervals_WhenJsonValueIsString()
  {
    var degrees = JsonSerializer.Deserialize<PersistentScaleDegree[]>(
      "\"1, b3,,5\"",
      CreateOptions()
    );

    degrees.Should()
           .NotBeNull()
           .And.Subject.Select( degree => degree.Interval )
           .Should()
           .Equal( "1", "b3", "5" );
  }

  [Fact]
  public void Read_ShouldParseIntervals_WhenJsonValueIsArray()
  {
    var degrees = JsonSerializer.Deserialize<PersistentScaleDegree[]>(
      "[ {\"Interval\":\"1\"}, {\"Interval\":\"b3\"}, {\"Interval\":\"5\"} ]",
      CreateOptions()
    );

    degrees.Should()
           .NotBeNull()
           .And.Subject.Select( degree => degree.Interval )
           .Should()
           .Equal( "1", "b3", "5" );
  }

  [Fact]
  public void Read_ShouldReturnEmptyArray_WhenJsonValueIsEmptyArray()
  {
    var degrees = JsonSerializer.Deserialize<PersistentScaleDegree[]>( "[]", CreateOptions() );

    degrees.Should()
           .NotBeNull()
           .And.BeEmpty();
  }

  [Theory]
  [InlineData( "\" \"" )]
  [InlineData( "42" )]
  public void Read_ShouldThrowJsonException_WhenJsonValueIsUnsupported(
    string json )
  {
    Action action = () => JsonSerializer.Deserialize<PersistentScaleDegree[]>( json, CreateOptions() );

    action.Should()
          .Throw<JsonException>();
  }

  [Fact]
  public void Write_ShouldEmitCommaSeparatedIntervals_WhenWritingDegreeArray()
  {
    var degrees = new[]
    {
      new PersistentScaleDegree( "1" ), new PersistentScaleDegree( "b3" ), new PersistentScaleDegree( "5" )
    };

    JsonSerializer.Serialize( degrees, CreateOptions() )
                  .Should()
                  .Be( "\"1,b3,5\"" );
  }

  #endregion

  #region Implementation

  private static JsonSerializerOptions CreateOptions()
  {
    var options = new JsonSerializerOptions();
    options.Converters.Add( new PersistentScaleDegreesJsonConverter() );
    return options;
  }

  #endregion
}
