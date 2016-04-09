//  
// Module Name: ModeTest.cs
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
  using System.Collections;
  using System.Linq;
  using Xunit;

  public class ModeTest
  {
    #region Public Methods

    [Fact]
    public void ModeConstructorTest()
    {
      Note root = Note.C;
      ModeFormula formula = ModeFormula.Phrygian;
      var target = new Mode(root, formula);

      Assert.Equal(root, target.Root);
      Assert.Equal(formula, target.Formula);
      Assert.Equal("C Phrygian", target.Name);
      Assert.Equal(target.ToArray(), NoteCollection.Parse("E,F,G,A,B,C,D"));
    }

    [Fact]
    public void ModesTest()
    {
      Note root = Note.C;
      TestMode("C,D,E,F,G,A,B", root, ModeFormula.Ionian);
      TestMode("D,E,F,G,A,B,C", root, ModeFormula.Dorian);
      TestMode("E,F,G,A,B,C,D", root, ModeFormula.Phrygian);
      TestMode("F,G,A,B,C,D,E", root, ModeFormula.Lydian);
      TestMode("G,A,B,C,D,E,F", root, ModeFormula.Mixolydian);
      TestMode("A,B,C,D,E,F,G", root, ModeFormula.Aeolian);
      TestMode("B,C,D,E,F,G,A", root, ModeFormula.Locrian);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Mode(Note.C, ModeFormula.Dorian);
      object y = new Mode(Note.C, ModeFormula.Dorian);
      object z = new Mode(Note.C, ModeFormula.Dorian);

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
      var x = new Mode(Note.C, ModeFormula.Dorian);
      var y = new Mode(Note.C, ModeFormula.Dorian);
      var z = new Mode(Note.C, ModeFormula.Dorian);

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
      object actual = new Mode(Note.C, ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Mode(Note.C, ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Mode(Note.C, ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Mode(Note.C, ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Mode(Note.C, ModeFormula.Dorian);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Mode(Note.C, ModeFormula.Dorian);
      var expected = new Mode(Note.C, ModeFormula.Dorian);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void EnumeratorTest()
    {
      var mode = new Mode(Note.C, ModeFormula.Ionian);
      var enumerator = ((IEnumerable) mode).GetEnumerator();
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.D, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.F, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.G, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.A, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.B, enumerator.Current);
      Assert.False(enumerator.MoveNext());
    }


    #endregion

    #region Implementation

    private static void TestMode(string expectedNotes, Note root, ModeFormula formula)
    {
      var expected = NoteCollection.Parse(expectedNotes).ToArray();
      var mode = new Mode(root, formula);
      var actualNotes = mode.ToArray();
      Assert.Equal(expected, actualNotes);
      Assert.Equal(expectedNotes, mode.ToString());
    }

    #endregion
  }
}
