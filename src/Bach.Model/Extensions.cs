// Module Name: Extensions.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System.Collections.Generic;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Provides common extensions.</summary>
public static class Extensions
{
#region Public Methods

  /// <summary>Returns the intervals that separate the provided pitch classes.</summary>
  /// <param name="pitchClasses">The pitch classes.</param>
  /// <returns>An intervals iterator.</returns>
  public static IEnumerable<Interval> Intervals( this IEnumerable<PitchClass> pitchClasses )
  {
    Requires.NotNull( pitchClasses );

    using var e = pitchClasses.GetEnumerator();
    yield return Interval.Unison;

    if( !e.MoveNext() )
    {
      yield break;
    }

    var root = e.Current;
    if( !e.MoveNext() )
    {
      yield break;
    }

    do
    {
      var pitchClass = e.Current;
      var interval = root - pitchClass;
      yield return interval;
    } while( e.MoveNext() );
  }

#endregion
}
