//  
// Module Name: StringedInstrumentTest.cs
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

  public class StringedInstrumentTest
  {
    #region Public Methods

    [Fact]
    public void CreateWithDefinitionTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create(definition, 22, expectedTuning);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithDefinitionDefaultTuningTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create(definition, 22);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithDefinitionThrowsOnNullDefinitionTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning expectedTuning = definition.Tunings.Standard;
      Assert.Throws<ArgumentNullException>(() => StringedInstrument.Create(null, 22, expectedTuning));
    }

    [Fact]
    public void CreateWithDefinitionThrowsOnMismatchedStringCountTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Assert.Throws<ArgumentOutOfRangeException>(() => StringedInstrument.Create(definition, 0));
    }

    [Fact]
    public void TestFactoryDefaultTuning()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      StringedInstrument instrument = StringedInstrument.Create(definition, 22);
      Assert.Equal(definition.Tunings.Standard, instrument.Tuning);
    }

    [Fact]
    public void TestFactoryNullDefinition()
    {
      Assert.Throws<ArgumentNullException>(() => { StringedInstrument.Create((StringedInstrumentDefinition) null, 22); });
    }

    [Fact]
    public void TestFactoryInvalidFretCount()
    {
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                 {
                                                   StringedInstrument.Create(
                                                                             Registry.StringedInstrumentDefinitions["bass"],
                                                                             0);
                                                 });
    }

    [Fact]
    public void CreateWithNamesTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create("guitar", 22, "standard");
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithNamesDefaultTuningTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithNamesThrowsOnNullDefinitionNameTest()
    {
      Assert.Throws<ArgumentNullException>(() => StringedInstrument.Create(null, 22, "standard"));
    }

    [Fact]
    public void CreateWithNamesThrowsOnInvalidFretCountTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => StringedInstrument.Create("guitar", 0));
    }

    [Fact]
    public void EqualsContractTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];

      object x = StringedInstrument.Create(definition, 22);
      object y = StringedInstrument.Create(definition, 22);
      object z = StringedInstrument.Create(definition, 22);

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
      var definition = Registry.StringedInstrumentDefinitions["guitar"];

      StringedInstrument x = StringedInstrument.Create(definition, 22);
      StringedInstrument y = StringedInstrument.Create(definition, 22);
      StringedInstrument z = StringedInstrument.Create(definition, 22);

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
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      object a = StringedInstrument.Create(definition, 22);
      object b = StringedInstrument.Create(Registry.StringedInstrumentDefinitions["bass"], 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      StringedInstrument a = StringedInstrument.Create(definition, 22);
      StringedInstrument b =
        StringedInstrument.Create(Registry.StringedInstrumentDefinitions["bass"], 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      object actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      StringedInstrument expected = StringedInstrument.Create(definition, 22);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void RenderMinorPentatonicStartOpenFourFretSpanTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var scale = new Scale(Note.A, Registry.ScaleFormulas["MinorPentatonic"]);
      Assert.Equal("60 63 50 53 40 42 30 32 21 23 10 13", RenderScale(instrument, scale, 0, 4));

      scale = new Scale(Note.G, Registry.ScaleFormulas["MinorPentatonic"]);
      Assert.Equal("61 63 51 53 40 43 30 33 21 23 11 13", RenderScale(instrument, scale, 0, 4));
    }

    [Fact]
    public void RenderMinorPentatonicStartFifthFretFourFretSpanTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var scale = new Scale(Note.A, Registry.ScaleFormulas["MinorPentatonic"]);
      Assert.Equal("65 68 55 57 45 47 35 37 25 28 15 18", RenderScale(instrument, scale, 5, 4));

      scale = new Scale(Note.G, Registry.ScaleFormulas["MinorPentatonic"]);
      Assert.Equal("66 68 55 58 45 48 35 37 26 28 16 18", RenderScale(instrument, scale, 5, 4));
    }

    [Fact]
    public void RenderMelodicMinorStartOpenFourFretSpanTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var scale = new Scale(Note.A, Registry.ScaleFormulas["MelodicMinor"]);
      Assert.Equal("60 62 64 50 52 53 40 42 44 31 32 20 21 23 10 12 14", RenderScale(instrument, scale, 0, 4));

      scale = new Scale(Note.G, Registry.ScaleFormulas["MelodicMinor"]);
      Assert.Equal("60 62 63 50 51 53 40 42 44 30 32 33 21 23 10 12 13", RenderScale(instrument, scale, 0, 4));
    }

    [Fact]
    public void RenderMelodicMinorStartFifthFretFourFretSpanTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var scale = new Scale(Note.A, Registry.ScaleFormulas["MelodicMinor"]);
      Assert.Equal("65 67 68 55 57 59 46 47 49 35 37 25 27 29 15 17 18", RenderScale(instrument, scale, 5, 4));

      scale = new Scale(Note.G, Registry.ScaleFormulas["MelodicMinor"]);
      Assert.Equal("65 66 68 55 57 59 45 47 48 35 37 25 27 28 15 16 18", RenderScale(instrument, scale, 5, 4));
    }

    [Fact]
    public void RenderMustSkipSeveralFretsForStartingNoteTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var scale = new Scale(Note.DSharp, Registry.ScaleFormulas["MinorPentatonic"]);
      Assert.Equal("62 64 51 54 41 44 31 33 22 24 12 14", RenderScale(instrument, scale, 0, 4));
    }

    [Fact]
    public void RenderCMajorChordFret0Test()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var chord = new Chord(Note.C, Registry.ChordFormulas["Major"]);
      Assert.Equal("6x 53 42 30 21 10", RenderChord(instrument, chord, 0, 4));
    }

    [Fact]
    public void RenderCMajorChordFret3Test()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var chord = new Chord(Note.C, Registry.ChordFormulas["Major"]);
      Assert.Equal("6x 53 45 35 25 13", RenderChord(instrument, chord, 3, 4));
    }

    [Fact]
    public void RenderCMajorChordFret5Test()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var chord = new Chord(Note.C, Registry.ChordFormulas["Major"]);
      Assert.Equal("68 57 45 35 25 18", RenderChord(instrument, chord, 5, 4));
    }

    [Fact]
    public void RenderCMajorChordFret8Test()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var chord = new Chord(Note.C, Registry.ChordFormulas["Major"]);
      Assert.Equal("68 510 410 39 28 18", RenderChord(instrument, chord, 8, 4));
    }

    [Fact]
    public void RenderCMajorChordFret12Test()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);

      var chord = new Chord(Note.C, Registry.ChordFormulas["Major"]);
      Assert.Equal("6x 515 414 312 213 112", RenderChord(instrument, chord, 12, 4));
    }


    #endregion

    #region Implementation

    private static string RenderScale(StringedInstrument instrument, Scale scale, int startFret, int fretSpan)
    {
      string result = string.Join(" ",
                                  Array.ConvertAll(instrument.Render(scale, startFret, fretSpan).ToArray(),
                                                   f => f.ToString()));
      return result;
    }

    private static string RenderChord(StringedInstrument instrument, Chord chord, int startFret, int fretSpan)
    {
      string result = string.Join(" ",
                                  Array.ConvertAll(instrument.Render(chord, startFret, fretSpan).ToArray(),
                                                   f => f.ToString()));
      return result;
    }

    #endregion
  }
}
