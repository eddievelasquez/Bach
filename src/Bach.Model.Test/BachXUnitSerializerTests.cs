// Module Name: BachXUnitSerializerTests.cs
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
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Bach.Model;

namespace Bach.Model.Test;

public sealed class BachXUnitSerializerTests
{
  #region Fields

  private static readonly (Type Type, object Value)[] s_supportedValues =
  [
    (typeof( Accidental ), Accidental.DoubleSharp),
    (typeof( NoteName ), NoteName.G),
    (typeof( Pitch ), Pitch.Create( PitchClass.CSharp, 4 )),
    (typeof( PitchClass ), PitchClass.EFlat),
    (typeof( Interval ), Interval.MajorThird),
    (typeof( ModeFormula ), ModeFormula.Dorian),
    (typeof( ScaleDegree ), ScaleDegree.Dominant),
    (typeof( ScaleDefinition ), ScaleDefinition.NaturalMinor)
  ];

  #endregion

  #region Public Methods

  [Fact]
  public void IsSerializable_ShouldReturnTrue_ForAllSupportedTypes()
  {
    var serializer = new BachXUnitSerializer();

    foreach( var (type, value) in s_supportedValues )
    {
      serializer.IsSerializable( type, value, out var failureReason )
                .Should()
                .BeTrue();
      failureReason.Should().BeNull();
    }
  }

  [Fact]
  public void SerializeAndDeserialize_ShouldRoundTrip_AllSupportedTypes()
  {
    var serializer = new BachXUnitSerializer();

    foreach( var (type, value) in s_supportedValues )
    {
      var serializedValue = serializer.Serialize( value );
      var deserializedValue = serializer.Deserialize( type, serializedValue );

      deserializedValue.Should().Be( value );
    }
  }

  #endregion
}
