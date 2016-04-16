//  
// Module Name: FingeringTest.cs
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
  using Model.Instruments;
  using Xunit;

  public class FingeringTest
  {
    #region Public Methods

    [Fact]
    public void CreateTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      Fingering actual = Fingering.Create(instrument, 6, 5);
      Assert.Equal(6, actual.String);
      Assert.Equal(5, actual.Fret);
    }

    [Fact]
    public void CreateThrowsWithOutOfRangeStringNumberTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      Assert.Throws<ArgumentOutOfRangeException>(() => Fingering.Create(instrument, 0, 5));
      Assert.Throws<ArgumentOutOfRangeException>(() => Fingering.Create(instrument, 7, 5));
    }

    [Fact]
    public void CreateThrowsWithOutOfRangeFretNumberTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      Assert.Throws<ArgumentOutOfRangeException>(() => Fingering.Create(instrument, 6, -1));
      Assert.Throws<ArgumentOutOfRangeException>(() => Fingering.Create(instrument, 6, 23));
    }

    [Fact]
    public void EqualsContractTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      object x = Fingering.Create(instrument, 6, 5);
      object y = Fingering.Create(instrument, 6, 5);
      object z = Fingering.Create(instrument, 6, 5);

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
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      var x = Fingering.Create(instrument, 6, 5);
      var y = Fingering.Create(instrument, 6, 5);
      var z = Fingering.Create(instrument, 6, 5);

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
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      object actual = Fingering.Create(instrument, 6, 5);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      var actual = Fingering.Create(instrument, 6, 5);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      object actual = Fingering.Create(instrument, 6, 5);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      var actual = Fingering.Create(instrument, 6, 5);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      var actual = Fingering.Create(instrument, 6, 5);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      var actual = Fingering.Create(instrument, 6, 5);
      var expected = Fingering.Create(instrument, 6, 5);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
