// Module Name: CircularArrayTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2026  Eddie Velasquez.
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
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test;

using Bach.Model.Internal;

public sealed class CircularArrayTest
{
  [Fact]
  public void Head_ShouldWrapToValidIndex_WhenSetOutsideRange()
  {
    var array = new CircularArray<int>( new[] { 1, 2, 3 } );

    array.Head = 4;

    array.Head.Should()
              .Be( 1 );
    array[0].Should()
           .Be( 2 );
    array[1].Should()
           .Be( 3 );
    array[2].Should()
           .Be( 1 );
  }

  [Fact]
  public void Indexer_ShouldSupportSetAndEnumerateInCircularOrder()
  {
    var array = new CircularArray<int>( new[] { 1, 2, 3 } );
    array.Head = 1;
    array[1] = 10;

    array[0].Should()
           .Be( 2 );
    array[1].Should()
           .Be( 10 );
    array[2].Should()
           .Be( 1 );
    array.Should()
         .Equal( 2, 10, 1 );
  }

  [Fact]
  public void Indexer_ShouldThrowArgumentOutOfRangeException_WhenIndexIsOutOfRange()
  {
    var array = new CircularArray<int>( new[] { 1, 2 } );

    var act = () => _ = array[-1];
    act.Should()
       .Throw<ArgumentOutOfRangeException>();

    var act2 = () => _ = array[2];
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }
}
