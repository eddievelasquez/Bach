// Module Name: ChordFormula.cs
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

using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>A chord formula defines how the pitch classes of a chord relate to each other.</summary>
public sealed class ChordFormula: Formula
{
#region Constructors

  /// <summary>Constructor.</summary>
  /// <param name="id">The language-neutral id of the chord.</param>
  /// <param name="name">The localizable name of the chord.</param>
  /// <param name="symbol">The symbol for the chord.</param>
  /// <param name="intervals">
  ///   The intervals that describe the relationship between the pitch classes that
  ///   compose the chord.
  /// </param>
  public ChordFormula(
    string id,
    string name,
    string? symbol,
    params Interval[] intervals )
    : base( id, name, intervals )
  {
    // A chord is composed by three or more pitch classes...
    Requires.GreaterThan( intervals.Length, 1 );
    Symbol = symbol ?? string.Empty;
  }

  /// <summary>Constructor.</summary>
  /// <param name="id">The language-neutral id of the chord.</param>
  /// <param name="name">The localizable name of the chord.</param>
  /// <param name="symbol">The symbol for the chord.</param>
  /// <param name="formula">
  ///   The string representation of the formula for the chord. The formula is a
  ///   sequence of comma-separated intervals. See
  ///   <see cref="Interval.ToString()" /> for the format of an interval.
  /// </param>
  public ChordFormula(
    string id,
    string name,
    string? symbol,
    string formula )
    : this( id, name, symbol, ParseIntervals( formula ) )
  {
  }

#endregion

#region Properties

  /// <summary>Gets the symbol for the chord.</summary>
  /// <value>The symbol.</value>
  public string Symbol { get; }

#endregion
}
