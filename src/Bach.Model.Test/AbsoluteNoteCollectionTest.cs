//  
// Module Name: AbsoluteNoteCollectionTest.cs
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
  using System.Linq;
  using Xunit;

  public class AbsoluteNoteCollectionTest
  {
    #region Public Methods

    [Fact]
    public void ParseTest()
    {
      var expected = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.Equal(expected, AbsoluteNoteCollection.Parse("C4,C5")); // Using notes
      Assert.Equal(expected, AbsoluteNoteCollection.Parse("60,72")); // Using midi
      Assert.Throws<ArgumentNullException>(() => AbsoluteNoteCollection.Parse(null));
      Assert.Throws<ArgumentException>(() => AbsoluteNoteCollection.Parse(""));
      Assert.Throws<ArgumentOutOfRangeException>(() => AbsoluteNoteCollection.Parse("C4,C5", Int32.MinValue));
      Assert.Throws<ArgumentOutOfRangeException>(() => AbsoluteNoteCollection.Parse("C4,C5", Int32.MaxValue));
      Assert.Throws<FormatException>(() => AbsoluteNoteCollection.Parse("C$4,Z5"));
    }

    [Fact]
    public void ToStringTest()
    {
      var actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.Equal("C4,C5", actual.ToString());
    }

    [Fact]
    public void TryParseTest()
    {
      AbsoluteNoteCollection collection;
      Assert.True(AbsoluteNoteCollection.TryParse("C4,E4", out collection));
      Assert.Equal(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("E4") }, collection);
      Assert.False(AbsoluteNoteCollection.TryParse(null, out collection));
      Assert.Null(collection);
      Assert.False(AbsoluteNoteCollection.TryParse("", out collection));
      Assert.Null(collection);
      Assert.Throws<ArgumentOutOfRangeException>(() => AbsoluteNoteCollection.TryParse("C4", out collection, int.MaxValue));
      Assert.Throws<ArgumentOutOfRangeException>(() => AbsoluteNoteCollection.TryParse("C4", out collection, int.MinValue));
      Assert.False(AbsoluteNoteCollection.TryParse("C$4,Z5", out collection));
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      object y = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      object z = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });

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
      var x = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      var y = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      var z = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });

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
      object actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      var expected = new AbsoluteNoteCollection(new[] { AbsoluteNote.Parse("C4"), AbsoluteNote.Parse("C5") });
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
