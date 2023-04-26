// Module Name: ScaleFormula.cs
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
using System.Diagnostics;

namespace Bach.Model;

/// <summary>A scale formula defines how the pitchClasses of a scale relate to each other.</summary>
public class ScaleFormula: Formula
{
#region Constructors

  /// <inheritdoc />
  internal ScaleFormula(
    string id,
    string name,
    Interval[] intervals,
    IReadOnlySet<string> categories,
    IReadOnlySet<string> aliases )
    : base( id, name, intervals )
  {
    Debug.Assert( categories != null );
    Debug.Assert( aliases != null );

    Categories = categories;
    Aliases = aliases;
  }

#endregion

#region Properties

  /// <summary>Gets the optional categories for this scale formula.</summary>
  /// <value>The categories.</value>
  public IReadOnlySet<string> Categories { get; }

  /// <summary>Gets the optional aliases for this scale formula.</summary>
  /// <value>The aliases.</value>
  public IReadOnlySet<string> Aliases { get; }

  /// <summary>Gets a value indicating whether this instance is diatonic.</summary>
  /// <value><c>true</c> if this instance is diatonic, <c>false</c> if not.</value>
  public bool IsDiatonic => Categories.Contains( ScaleCategory.Diatonic );

  /// <summary>Query if this instance is a major scale formula.</summary>
  /// <value><c>true</c> if major; otherwise, <c>false</c>.</value>
  public bool IsMajor => Categories.Contains( ScaleCategory.Major );

  /// <summary>Query if this instance is a minor scale formula.</summary>
  /// <value><c>true</c> if minor; otherwise, <c>false</c>.</value>
  public bool IsMinor => Categories.Contains( ScaleCategory.Minor );

#endregion
}
