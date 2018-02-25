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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
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
    #region Constants

    private const string EXPECTED_INSTRUMENT_KEY = "InstrumentKey";
    private const string EXPECTED_INSTRUMENT_NAME = "InstrumentName";
    private const int EXPECTED_INSTRUMENT_STRING_COUNT = 3;
    private const string EXPECTED_TUNING_KEY = "Standard";
    private const string EXPECTED_TUNING_NAME = "Standard";

    #endregion

    #region Public Methods

    [Fact]
    public void BuildTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes);
      StringedInstrumentDefinition definition = builder.Build();
      Assert.NotNull(definition);
      Assert.Equal(EXPECTED_INSTRUMENT_NAME, definition.InstrumentName);
      Assert.Equal(EXPECTED_INSTRUMENT_STRING_COUNT, definition.StringCount);
      Assert.NotNull(definition.Tunings);
      Assert.Single(definition.Tunings);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(notes, definition.Tunings.Standard.Notes);
    }

    [Fact]
    public void AddTuningWithNoteArrayTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes);
      StringedInstrumentDefinition definition = builder.Build();
      Assert.NotNull(definition);
      Assert.Equal(EXPECTED_INSTRUMENT_NAME, definition.InstrumentName);
      Assert.Equal(EXPECTED_INSTRUMENT_STRING_COUNT, definition.StringCount);
      Assert.NotNull(definition.Tunings);
      Assert.Single(definition.Tunings);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(EXPECTED_TUNING_NAME, definition.Tunings.Standard.Name);
      Assert.Equal(notes, definition.Tunings.Standard.Notes);
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnNullKeyTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(null, EXPECTED_TUNING_NAME, notes));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnNullNameTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, null, notes));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnEmptyKeyTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning("", EXPECTED_TUNING_NAME, notes));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnEmptyNameTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, "", notes));
    }

    [Fact]
    public void AddTuningWithNoteCollectionThrowsOnMismatchedNoteCountTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4,F4");
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnNullKeyTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(null, EXPECTED_TUNING_NAME, notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnNullNameTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentNullException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, null, notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnEmptyKeyTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning("", EXPECTED_TUNING_NAME, notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnEmptyNameTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, "", notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnMismatchedNoteCountTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4,F4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      Assert.Throws<ArgumentException>(() => builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes));
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
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();
      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning("ATuning", "A tuning", notes);
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
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes);
      builder.Build();

      Assert.Throws<InvalidOperationException>(() => builder.AddTuning("ATuning", "A tuning", notes));
    }

    [Fact]
    public void AddTuningWithNoteArrayThrowsOnBuilderReuseTest()
    {
      Note[] notes = NoteCollection.Parse("C4,D4,E4").ToArray();

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes);
      builder.Build();

      Assert.Throws<InvalidOperationException>(() => builder.AddTuning("ATuning", "A tuning", notes));
    }

    [Fact]
    public void BuildThrowsOnBuilderReuseTest()
    {
      NoteCollection notes = NoteCollection.Parse("C4,D4,E4");

      var builder = new StringedInstrumentDefinitionBuilder(
        EXPECTED_INSTRUMENT_KEY,
        EXPECTED_INSTRUMENT_NAME,
        EXPECTED_INSTRUMENT_STRING_COUNT);
      builder.AddTuning(EXPECTED_TUNING_KEY, EXPECTED_TUNING_NAME, notes);
      builder.Build();
      Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    #endregion
  }
}
