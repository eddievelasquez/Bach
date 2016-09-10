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
      var scale = new Scale(Tone.C, "Major");
      ModeFormula formula = ModeFormula.Phrygian;
      var target = new Mode(scale, formula);

      Assert.Equal(scale, target.Scale);
      Assert.Equal(formula, target.Formula);
      Assert.Equal("C Phrygian", target.Name);
      Assert.Equal(target.ToArray(), ToneCollection.Parse("E,F,G,A,B,C,D"));
    }

    [Fact]
    public void ModesTest()
    {
      var scale = new Scale(Tone.C, "Major");
      TestMode("C,D,E,F,G,A,B", scale, ModeFormula.Ionian);
      TestMode("D,E,F,G,A,B,C", scale, ModeFormula.Dorian);
      TestMode("E,F,G,A,B,C,D", scale, ModeFormula.Phrygian);
      TestMode("F,G,A,B,C,D,E", scale, ModeFormula.Lydian);
      TestMode("G,A,B,C,D,E,F", scale, ModeFormula.Mixolydian);
      TestMode("A,B,C,D,E,F,G", scale, ModeFormula.Aeolian);
      TestMode("B,C,D,E,F,G,A", scale, ModeFormula.Locrian);
    }

    [Fact]
    public void EqualsContractTest()
    {
      var scale = new Scale(Tone.C, "Major");
      object x = new Mode(scale, ModeFormula.Dorian);
      object y = new Mode(scale, ModeFormula.Dorian);
      object z = new Mode(scale, ModeFormula.Dorian);

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
      var scale = new Scale(Tone.C, "Major");
      var x = new Mode(scale, ModeFormula.Dorian);
      var y = new Mode(scale, ModeFormula.Dorian);
      var z = new Mode(scale, ModeFormula.Dorian);

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
      var scale = new Scale(Tone.C, "Major");
      object actual = new Mode(scale, ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var scale = new Scale(Tone.C, "Major");
      var actual = new Mode(scale, ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      var scale = new Scale(Tone.C, "Major");
      object actual = new Mode(scale, ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var scale = new Scale(Tone.C, "Major");
      var actual = new Mode(scale, ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var scale = new Scale(Tone.C, "Major");
      var actual = new Mode(scale, ModeFormula.Dorian);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var scale = new Scale(Tone.C, "Major");
      var actual = new Mode(scale, ModeFormula.Dorian);
      var expected = new Mode(scale, ModeFormula.Dorian);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void EnumeratorTest()
    {
      var scale = new Scale(Tone.C, "Major");
      var mode = new Mode(scale, ModeFormula.Ionian);
      IEnumerator enumerator = ((IEnumerable) mode).GetEnumerator();
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.D, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.F, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.G, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.A, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Tone.B, enumerator.Current);
      Assert.False(enumerator.MoveNext());
    }

    #endregion

    #region Implementation

    private static void TestMode(string expectedNotes, Scale root, ModeFormula formula)
    {
      var expected = ToneCollection.Parse(expectedNotes).ToArray();
      var mode = new Mode(root, formula);
      var actualNotes = mode.ToArray();
      Assert.Equal(expected, actualNotes);
      Assert.Equal(expectedNotes, mode.ToString());
    }

    #endregion
  }
}
