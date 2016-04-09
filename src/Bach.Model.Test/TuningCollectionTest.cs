﻿//  
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

namespace Bach.Model.Test
{
  using Bach.Model.Instruments;
  using Xunit;

  public class TuningCollectionTest
  {
    #region Public Methods

    [Fact]
    public void AddSetItemsTest()
    {
      var guitar = new GuitarDefinition();
      var actual = new TuningCollection(guitar);
      Assert.Equal(0, actual.Count);
      actual.Add(guitar.Tunings.Standard);
      Assert.Equal(1, actual.Count);
      Assert.Equal(guitar.Tunings.Standard, actual[0]);
      actual[0] = guitar.Tunings[1];
      Assert.Equal(guitar.Tunings[1], actual[0]);
    }

    #endregion
  }
}
