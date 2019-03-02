//
// Module Name: StringedInstrumentDefinitionBuilderTest.cs
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
  using System.Linq;
  using Model.Instruments;
  using Xunit;

  public class StringedInstrumentDefinitionBuilderTest
  {
    private const string EXPECTED_INSTRUMENT_KEY = "InstrumentKey";
    private const string EXPECTED_INSTRUMENT_NAME = "InstrumentName";
    private const int EXPECTED_INSTRUMENT_STRING_COUNT = 3;
    private const string EXPECTED_TUNING_KEY = "Standard";
    private const string EXPECTED_TUNING_NAME = "Standard";

    [Fact]
    public void BuildTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches);
      StringedInstrumentDefinition definition = builder.Build();
      Assert.NotNull(definition);
      Assert.Equal(EXPECTED_INSTRUMENT_NAME, definition.InstrumentName);
      Assert.Equal(EXPECTED_INSTRUMENT_STRING_COUNT, definition.StringCount);
      Assert.NotNull(definition.Tunings);
      Assert.Single(definition.Tunings);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(pitches, definition.Tunings.Standard.Pitches);
    }

    [Fact]
    public void AddTuningWithNoteArrayTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches);
      StringedInstrumentDefinition definition = builder.Build();
      Assert.NotNull(definition);
      Assert.Equal(EXPECTED_INSTRUMENT_NAME, definition.InstrumentName);
      Assert.Equal(EXPECTED_INSTRUMENT_STRING_COUNT, definition.StringCount);
      Assert.NotNull(definition.Tunings);
      Assert.Single(definition.Tunings);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(pitches, definition.Tunings.Standard.Pitches);
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnNullKeyTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(null, EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnNullNameTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, null, pitches));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnEmptyKeyTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning("", EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnEmptyNameTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, "", pitches));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnMismatchedNoteCountTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4,F4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnNullKeyTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(null, EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnNullNameTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, null, pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnEmptyKeyTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning("", EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnEmptyNameTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, "", pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnMismatchedNoteCountTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4,F4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches));
    }

    [Fact]
    public void BuildThrowOnEmptyTuningsTest()
    {
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void BuildThrowOnMissingStandardTuningTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning("ATuning", "A tuning", pitches);
      Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void AddTuningThrowsOnStringMismatchTest()
    {
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(
        () => builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, "C4,D4,E4,F4"));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnBuilderReuseTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches);
      builder.Build();

      Assert.Throws<InvalidOperationException>(() => builder.AddTuning("ATuning", "A tuning", pitches));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnBuilderReuseTest()
    {
      Pitch[] pitches = PitchCollection.Parse("C4,D4,E4").ToArray();

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches);
      builder.Build();

      Assert.Throws<InvalidOperationException>(() => builder.AddTuning("ATuning", "A tuning", pitches));
    }

    [Fact]
    public void BuildThrowsOnBuilderReuseTest()
    {
      PitchCollection pitches = PitchCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, pitches);
      builder.Build();
      Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
  }
}
