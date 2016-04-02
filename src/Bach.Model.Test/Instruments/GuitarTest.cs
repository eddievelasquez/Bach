﻿//  
// Module Name: GuitarTest.cs
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
  using Bach.Model.Instruments;
  using Xunit;

  public class GuitarTest
  {
    #region Public Methods

    [Fact]
    public void TestConstructor()
    {
      var guitar = new Guitar();
      Assert.Equal(guitar.Name, "Guitar");
      Assert.Equal(guitar.StringCount, 6);
      Assert.NotNull(guitar.Tunings);
      Assert.NotEqual(guitar.Tunings.Count, 0);
    }

    [Fact]
    public void AddTuningTest()
    {
      var guitar = new Guitar();
      int tuningCount = guitar.Tunings.Count;
      guitar.Tunings.Add(new Tuning(guitar, "Drop D Test", NoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
      Assert.Equal(tuningCount + 1, guitar.Tunings.Count);
    }

    [Fact]
    public void AddTuningFailsWithDifferentInstrumentTuningTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar = new Guitar();
                                         guitar.Tunings.Add(new Tuning(new Bass(), "Drop D",
                                                                       NoteCollection.Parse("G2,D2,A1,D1")));
                                       });
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Guitar();
      object y = new Guitar();
      object z = new Guitar();

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
      var x = new Guitar();
      var y = new Guitar();
      var z = new Guitar();

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
      object a = new Guitar();
      object b = new Bass();
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var a = new Guitar();
      var b = new Bass();
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Guitar();
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Guitar();
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Guitar();
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Guitar();
      var expected = new Guitar();
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
