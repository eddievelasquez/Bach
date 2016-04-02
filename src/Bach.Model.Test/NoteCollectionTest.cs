//  
// Module Name: NoteCollectionTest.cs
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
  using System.Linq;
  using Xunit;

  public class NoteCollectionTest
  {
    #region Public Methods

    [Fact]
    public void ParseWithNotesTest()
    {
      var expected = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      NoteCollection actual = NoteCollection.Parse("C4,C5");
      Assert.True(actual.SequenceEqual(expected));
    }

    [Fact]
    public void ParseWithMidiTest()
    {
      var expected = new NoteCollection(new[] { Note.FromMidi(60), Note.FromMidi(70) });
      NoteCollection actual = NoteCollection.Parse("60,70");
      Assert.True(actual.SequenceEqual(expected));
    }

    [Fact]
    public void ToStringTest()
    {
      var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.Equal("C4,C5", actual.ToString());
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      object y = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      object z = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });

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
      var x = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      var y = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      var z = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });

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
      object actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      var expected = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
