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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
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
    [Fact]
    public void StringConstructorTest()
    {
      var target = new Chord(Note.C, "Minor");
      Assert.Equal(Note.C, target.Root);
      Assert.Equal(Registry.ChordFormulas["Minor"], target.Formula);
      Assert.Equal("Cm", target.Name);
      Assert.Equal(NoteCollection.Parse("C,Eb,G"), target.Notes);
      Assert.Equal(target.Name, target.ToString());
    }

    [Fact]
    public void StringConstructorThrowsOnNullFormulaNameTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Chord(Note.C, (string) null));
    }

    [Fact]
    public void StringConstructorThrowsOnEmptyFormulaNameTest()
    {
      Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => new Chord(Note.C, ""));
    }

    [Fact]
    public void FormulaConstructorTest()
    {
      ChordFormula formula = Registry.ChordFormulas["Minor"];
      var target = new Chord(Note.C, formula);
      Assert.Equal(Note.C, target.Root);
      Assert.Equal(Registry.ChordFormulas["Minor"], target.Formula);
      Assert.Equal("Cm", target.Name);
      Assert.Equal(NoteCollection.Parse("C,Eb,G"), target.Notes);
      Assert.Equal(target.Name, target.ToString());
    }

    [Fact]
    public void FormulaConstructorThrowsOnNullFormulaTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Chord(Note.C, (ChordFormula) null));
    }

    [Fact]
    public void ChordsTest()
    {
      ChordTestImpl("C,E,G", Note.C, "Major");
      ChordTestImpl("C,E,G,B", Note.C, "Major7");
      ChordTestImpl("C,E,G,B,D", Note.C, "Major9");
      ChordTestImpl("C,E,G,B,D,F", Note.C, "Major11");
      ChordTestImpl("C,E,G,B,D,F,A", Note.C, "Major13");
      ChordTestImpl("C,Eb,G", Note.C, "Minor");
      ChordTestImpl("C,Eb,G,Bb", Note.C, "Minor7");
      ChordTestImpl("C,Eb,G,Bb,D", Note.C, "Minor9");
      ChordTestImpl("C,Eb,G,Bb,D,F", Note.C, "Minor11");
      ChordTestImpl("C,Eb,G,Bb,D,F,A", Note.C, "Minor13");
      ChordTestImpl("C,E,G,Bb", Note.C, "Dominant7");
      ChordTestImpl("C,E,G,Bb,D", Note.C, "Dominant9");
      ChordTestImpl("C,E,G,Bb,D,F", Note.C, "Dominant11");
      ChordTestImpl("C,E,G,Bb,D,F,A", Note.C, "Dominant13");
      ChordTestImpl("C,E,G,A,D", Note.C, "SixNine");
      ChordTestImpl("C,E,G,D", Note.C, "AddNine");
      ChordTestImpl("C,Eb,Gb", Note.C, "Diminished");
      ChordTestImpl("C,Eb,Gb,A", Note.C, "Diminished7");
      ChordTestImpl("C,Eb,Gb,Bb", Note.C, "HalfDiminished");
      ChordTestImpl("C,E,G#", Note.C, "Augmented");
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Chord(Note.C, "Major");
      object y = new Chord(Note.C, "Major");
      object z = new Chord(Note.C, "Major");

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
      var x = new Chord(Note.C, "Major");
      var y = new Chord(Note.C, "Major");
      var z = new Chord(Note.C, "Major");

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
      object actual = new Chord(Note.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Chord(Note.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Chord(Note.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Chord(Note.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Chord(Note.C, "Major");
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Chord(Note.C, "Major");
      var expected = new Chord(Note.C, "Major");
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void GetInversionTest()
    {
      var cMajor = new Chord(Note.C, "Major");
      Chord firstInversion = cMajor.GetInversion(1);
      Assert.NotNull(firstInversion);
      Assert.Equal("C/E", firstInversion.Name);
      Assert.Equal(NoteCollection.Parse("E,G,C"), firstInversion.Notes);

      Chord secondInversion = cMajor.GetInversion(2);
      Assert.NotNull(secondInversion);
      Assert.Equal("C/G", secondInversion.Name);
      Assert.Equal(NoteCollection.Parse("G,C,E"), secondInversion.Notes);

      Assert.Throws<ArgumentOutOfRangeException>(() => cMajor.GetInversion(3));
    }

    [Fact]
    public void EnumeratorTest()
    {
      var cMajor = new Chord(Note.C, "Major");
      IEnumerator enumerator = ((IEnumerable) cMajor).GetEnumerator();
      Assert.NotNull(enumerator);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.G, enumerator.Current);
      Assert.False(enumerator.MoveNext());
    }

    private static void ChordTestImpl(string expectedNotes,
                                      Note root,
                                      string formulaName)
    {
      var chord = new Chord(root, formulaName);
      Note[] actualNotes = chord.Take(NoteCollection.Parse(expectedNotes).Count).ToArray();
      Assert.Equal(NoteCollection.Parse(expectedNotes), actualNotes);
    }
  }
}
