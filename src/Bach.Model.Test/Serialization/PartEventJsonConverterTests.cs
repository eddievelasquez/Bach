// Module Name: PartEventJsonConverterTests.cs
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

using System.Collections.Generic;
using System.Text.Json;

namespace Bach.Model.Serialization.Test;

public sealed class PartEventJsonConverterTests
{
  #region Public Methods

  [Fact]
  public void AddConverters_ShouldReturnConfiguredOptions_WhenOptionsAreValid()
  {
    var options = new JsonSerializerOptions();

    options.AddConverters()
           .Should()
           .BeSameAs( options );

    JsonSerializer.Serialize<IPartEvent>( Pitch.Parse( "C4" ), options )
                  .Should()
                  .Be( "{\"type\":\"Pitch\",\"pitch\":\"C4\"}" );
  }

  [Fact]
  public void AddConverters_ShouldThrowArgumentNullException_WhenOptionsAreNull()
  {
    Action action = () => ( (JsonSerializerOptions) null! ).AddConverters();

    action.Should()
          .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Serialize_ShouldWriteChordFields_WhenEventIsPitchChord()
  {
    var chord = new PitchChord( Pitch.Parse( "C4" ), "Major", 1 );
    var json = JsonSerializer.Serialize<IPartEvent>( chord, new JsonSerializerOptions().AddConverters() );

    json.Should()
        .Be( "{\"type\":\"Chord\",\"root\":\"C4\",\"formula\":\"Major\",\"inversion\":1}" );
  }

  [Fact]
  public void Read_ShouldReconstructPitch_WhenTypeIsPitch()
  {
    var partEvent = JsonSerializer.Deserialize<IPartEvent>(
      "{\"type\":\"Pitch\",\"pitch\":\"C4\"}",
      new JsonSerializerOptions().AddConverters()
    );

    partEvent.Should()
             .BeOfType<Pitch>()
             .Which.ToString()
             .Should()
             .Be( "C4" );
  }

  [Fact]
  public void Read_ShouldReconstructChord_WhenTypeIsChord()
  {
    var partEvent = JsonSerializer.Deserialize<IPartEvent>(
      "{\"type\":\"Chord\",\"root\":\"C4\",\"formula\":\"Major\",\"inversion\":1}",
      new JsonSerializerOptions().AddConverters()
    );

    var chord = partEvent.Should()
                         .BeOfType<PitchChord>()
                         .Which;

    chord.Root.ToString()
         .Should()
         .Be( "C4" );

    chord.Formula.Id.Should()
         .Be( "Major" );

    chord.Inversion.Should()
         .Be( 1 );
  }

  [Theory]
  [InlineData( "{\"type\":\"Unknown\"}" )]
  [InlineData( "{\"type\":1}" )]
  [InlineData( "{}" )]
  public void Read_ShouldThrowJsonException_WhenDiscriminatorIsUnknownOrMalformed(
    string json )
  {
    Action action = () => JsonSerializer.Deserialize<IPartEvent>( json, new JsonSerializerOptions().AddConverters() );

    action.Should()
          .Throw<JsonException>();
  }

  [Fact]
  public void Serialize_ShouldThrowJsonException_WhenEventTypeIsUnsupported()
  {
    Action action = () => JsonSerializer.Serialize<IPartEvent>(
      new UnsupportedPartEvent(),
      new JsonSerializerOptions().AddConverters()
    );

    action.Should()
          .Throw<JsonException>()
          .WithMessage( "Unknown event type UnsupportedPartEvent" );
  }

  #endregion

  #region Nested Types

  private sealed class UnsupportedPartEvent: IPartEvent
  {
    #region Public Methods

    public bool Any(
      PitchClass pitchClass ) => pitchClass == PitchClass.C;

    #endregion

    #region IPartEvent Implementation

    public IEnumerable<PitchClass> PitchClasses => [PitchClass.C];

    #endregion
  }

  #endregion
}
