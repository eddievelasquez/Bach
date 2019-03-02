//
// Module Name: TuningTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test.Instruments
{
  using System;
  using Model.Instruments;
  using Xunit;

  public class TuningTest
  {
    private const string EXPECTED_INSTRUMENT_KEY = "guitar";
    private const string EXPECTED_TUNING_KEY = "DropD";
    private const string EXPECTED_TUNING_NAME = "Drop D";

    [Fact]
    public void TestConstructor()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var actual = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.Equal(guitar, actual.InstrumentDefinition);
      Assert.Equal(EXPECTED_TUNING_NAME, actual.Name);
      Assert.NotNull(actual.Pitches);
      Assert.Equal(6, actual.Pitches.Length);
    }

    [Fact]
    public void ConstructorFailsWithNullInstrumentTest()
    {
      Assert.Throws<ArgumentNullException>(
        () => new Tuning(null, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2")));
    }

    [Fact]
    public void ConstructorFailsWithNullKeyTest()
    {
      Assert.Throws<ArgumentNullException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, null, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
        });
    }

    [Fact]
    public void ConstructorFailsWithNullNameTest()
    {
      Assert.Throws<ArgumentNullException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, EXPECTED_TUNING_KEY, null, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
        });
    }

    [Fact]
    public void ConstructorFailsWithEmptyKeyTest()
    {
      Assert.Throws<ArgumentException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, "", EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
        });
    }

    [Fact]
    public void ConstructorFailsWithEmptyNameTest()
    {
      Assert.Throws<ArgumentException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, EXPECTED_TUNING_KEY, "", PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
        });
    }

    [Fact]
    public void ConstructorFailsWithNullNoteCollectionTest()
    {
      Assert.Throws<ArgumentNullException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, null);
        });
    }

    [Fact]
    public void ConstructorFailsWithInvalidNoteCollectionCountTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () =>
        {
          StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
          new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2"));
        });
    }

    [Fact]
    public void EqualsContractTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      object x = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object y = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object z = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void TypeSafeEqualsContractTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var x = new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var y = new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var z = new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void EqualsFailsWithDifferentTypeTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      object a = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object b = guitar;
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var a = new Tuning(guitar, EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      StringedInstrumentDefinition b = guitar;
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      object actual = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var actual = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var actual = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      StringedInstrumentDefinition guitar = Registry.StringedInstrumentDefinitions[EXPECTED_INSTRUMENT_KEY];
      var actual = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var expected = new Tuning(
        guitar,
        EXPECTED_TUNING_KEY,
        EXPECTED_TUNING_NAME,
        PitchCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }
  }
}
