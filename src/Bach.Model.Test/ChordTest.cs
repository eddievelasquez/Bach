//
// Module Name: ChordTest.cs
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

namespace Bach.Model.Test
{
  using System;
  using System.Collections;
  using System.Linq;
  using Xunit;

  public class ChordTest
  {
    #region Public Methods

    [Fact]
    public void StringConstructorTest()
    {
      var target = new Chord(Tone.C, "Minor");
      Assert.Equal(Tone.C, target.Root);
      Assert.Equal(Registry.ChordFormulas["Minor"], target.Formula);
      Assert.Equal("Cm", target.Name);
      Assert.Equal(ToneCollection.Parse("C,Eb,G"), target.Tones);
      Assert.Equal(target.Name, target.ToString());
    }

    [Fact]
    public void StringConstructorThrowsOnNullFormulaNameTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Chord(Tone.C, (string) null));
    }

    [Fact]
    public void StringConstructorThrowsOnEmptyFormulaNameTest()
    {
      Assert.Throws<ArgumentException>(() => new Chord(Tone.C, ""));
    }

    [Fact]
    public void FormulaConstructorTest()
    {
      ChordFormula formula = Registry.ChordFormulas["Minor"];
      var target = new Chord(Tone.C, formula);
      Assert.Equal(Tone.C, target.Root);
      Assert.Equal(Registry.ChordFormulas["Minor"], target.Formula);
      Assert.Equal("Cm", target.Name);
      Assert.Equal(ToneCollection.Parse("C,Eb,G"), target.Tones);
      Assert.Equal(target.Name, target.ToString());
    }

    [Fact]
    public void FormulaConstructorThrowsOnNullFormulaTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Chord(Tone.C, (ChordFormula) null));
    }

    [Fact]
    public void ChordsTest()
    {
      ChordTestImpl("C,E,G", Tone.C, "Major");
      ChordTestImpl("C,E,G,B", Tone.C, "Major7");
      ChordTestImpl("C,E,G,B,D", Tone.C, "Major9");
      ChordTestImpl("C,E,G,B,D,F", Tone.C, "Major11");
      ChordTestImpl("C,E,G,B,D,F,A", Tone.C, "Major13");
      ChordTestImpl("C,Eb,G", Tone.C, "Minor");
      ChordTestImpl("C,Eb,G,Bb", Tone.C, "Minor7");
      ChordTestImpl("C,Eb,G,Bb,D", Tone.C, "Minor9");
      ChordTestImpl("C,Eb,G,Bb,D,F", Tone.C, "Minor11");
      ChordTestImpl("C,Eb,G,Bb,D,F,A", Tone.C, "Minor13");
      ChordTestImpl("C,E,G,Bb", Tone.C, "Dominant7");
      ChordTestImpl("C,E,G,Bb,D", Tone.C, "Dominant9");
      ChordTestImpl("C,E,G,Bb,D,F", Tone.C, "Dominant11");
      ChordTestImpl("C,E,G,Bb,D,F,A", Tone.C, "Dominant13");
      ChordTestImpl("C,E,G,A,D", Tone.C, "SixNine");
      ChordTestImpl("C,E,G,D", Tone.C, "AddNine");
      ChordTestImpl("C,Eb,Gb", Tone.C, "Diminished");
      ChordTestImpl("C,Eb,Gb,A", Tone.C, "Diminished7");
      ChordTestImpl("C,Eb,Gb,Bb", Tone.C, "HalfDiminished");
      ChordTestImpl("C,E,G#", Tone.C, "Augmented");
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Chord(Tone.C, "Major");
      object y = new Chord(Tone.C, "Major");
      object z = new Chord(Tone.C, "Major");

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
      var x = new Chord(Tone.C, "Major");
      var y = new Chord(Tone.C, "Major");
      var z = new Chord(Tone.C, "Major");

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
      object actual = new Chord(Tone.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Chord(Tone.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Chord(Tone.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Chord(Tone.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Chord(Tone.C, "Major");
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Chord(Tone.C, "Major");
      var expected = new Chord(Tone.C, "Major");
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void InvertTest()
    {
      var cMajor = new Chord(Tone.C, "Major");
      Chord firstInversion = cMajor.Invert(1);
      Assert.NotNull(firstInversion);
      Assert.Equal("C/E", firstInversion.Name);
      Assert.Equal(ToneCollection.Parse("E,G,C"), firstInversion.Tones);

      Chord secondInversion = cMajor.Invert(2);
      Assert.NotNull(secondInversion);
      Assert.Equal("C/G", secondInversion.Name);
      Assert.Equal(ToneCollection.Parse("G,C,E"), secondInversion.Tones);

      Assert.Throws<ArgumentOutOfRangeException>(() => cMajor.Invert(3));
    }

    [Fact]
    public void EnumeratorTest()
    {
      var cMajor = new Chord(Tone.C, "Major");
      IEnumerator enumerator = ((IEnumerable) cMajor).GetEnumerator();
      Assert.NotNull(enumerator);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.G, enumerator.Current);
      Assert.False(enumerator.MoveNext());
    }

    #endregion

    #region Implementation

    private static void ChordTestImpl(string expectedNotes,
                                      Tone root,
                                      string formulaName)
    {
      var chord = new Chord(root, formulaName);
      Tone[] actualNotes = chord.Take(ToneCollection.Parse(expectedNotes).Count).ToArray();
      Assert.Equal(ToneCollection.Parse(expectedNotes), actualNotes);
    }

    #endregion
  }
}
