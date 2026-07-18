// Module Name: ArgumentExceptionExtensionsTest.cs
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
using System.Collections.Generic;

public sealed class ArgumentExceptionExtensionsTest
{
  [Fact]
  public void ThrowIfNullOrEmpty_ShouldThrowArgumentNullException_WhenSourceIsNull()
  {
    var act = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty<int>( null, "source" );

    act.Should()
       .Throw<ArgumentNullException>()
       .WithParameterName( "source" );
  }

  [Fact]
  public void ThrowIfNullOrEmpty_ShouldThrowArgumentException_WhenSourceIsEmpty()
  {
    var act = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty( Array.Empty<int>(), "source" );

    act.Should()
       .Throw<ArgumentException>()
       .WithMessage( "*Sequence must not be empty.*" );
  }

  [Fact]
  public void ThrowIfNullOrEmpty_ShouldSucceed_WhenSourceContainsItems()
  {
    var act = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty( new[] { 1, 2, 3 }, "source" );

    act.Should()
       .NotThrow();
  }
}
