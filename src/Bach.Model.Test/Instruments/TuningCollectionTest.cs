//  
// Module Name: TuningCollectionTest.cs
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
  using System.Collections;
  using System.Linq;
  using Bach.Model.Instruments;
  using Xunit;

  public class TuningCollectionTest
  {
    #region Public Methods

    [Fact]
    public void ContainsKeyTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Assert.True(definition.Tunings.ContainsKey("standard"));
      Assert.False(definition.Tunings.ContainsKey("DoesNotExist!!!"));
    }

    [Fact]
    public void ContainsKeyThrowsOnNullKeyTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Assert.Throws<ArgumentNullException>(() => definition.Tunings.ContainsKey(null));
    }

    [Fact]
    public void TryGetValueTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];

      Tuning tuning;
      Assert.True(definition.Tunings.TryGetValue("standard", out tuning));
      Assert.Equal(definition.Tunings.Standard, tuning);
    }

    [Fact]
    public void TryGetValueThrowsOnNullKeyTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Tuning tuning;
      Assert.Throws<ArgumentNullException>(() => definition.Tunings.TryGetValue(null, out tuning));
    }

    [Fact]
    public void KeysTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Assert.Equal(definition.Tunings.Count, definition.Tunings.Keys.Count());
    }

    [Fact]
    public void ValuesTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      Assert.Equal(definition.Tunings.Count, definition.Tunings.Values.Count());
    }

    [Fact]
    public void GetEnumeratorTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      var tunings = definition.Tunings.ToArray();

      var enumerator = definition.Tunings.GetEnumerator();
      for( int i = 0; i < tunings.Length; i++ )
      {
        Assert.True(enumerator.MoveNext());
        Assert.Equal(tunings[i], enumerator.Current);
      }

      Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void GetEnumeratorOfObjectTest()
    {
      var definition = Registry.StringedInstrumentDefinitions["guitar"];
      var tunings = definition.Tunings.ToArray();

      IEnumerator enumerator = ((IEnumerable) definition.Tunings).GetEnumerator();
      for( int i = 0; i < tunings.Length; i++ )
      {
        Assert.True(enumerator.MoveNext());
        Assert.Equal(tunings[i], enumerator.Current);
      }

      Assert.False(enumerator.MoveNext());
    }

    #endregion
  }
}
