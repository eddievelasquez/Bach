// Module Name: ModeFormula.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Diagnostics;

  /// <summary>A mode formula.</summary>
  public class ModeFormula: IEquatable<ModeFormula>
  {
    #region Constants

    private const int MinTonic = 1;
    private const int MaxTonic = 7;

    /// <summary>The Ionian (I) mode.</summary>
    public static readonly ModeFormula Ionian = new ModeFormula("Ionian", 1);

    /// <summary>The Dorian (II) mode.</summary>
    public static readonly ModeFormula Dorian = new ModeFormula("Dorian", 2);

    /// <summary>The Phrygian (III) mode.</summary>
    public static readonly ModeFormula Phrygian = new ModeFormula("Phrygian", 3);

    /// <summary>The Lydian (IV) mode.</summary>
    public static readonly ModeFormula Lydian = new ModeFormula("Lydian", 4);

    /// <summary>The Mixolydian (V) mode.</summary>
    public static readonly ModeFormula Mixolydian = new ModeFormula("Mixolydian", 5);

    /// <summary>The Aeolian (VI) mode.</summary>
    public static readonly ModeFormula Aeolian = new ModeFormula("Aeolian", 6);

    /// <summary>The Locrian (VII).</summary>
    public static readonly ModeFormula Locrian = new ModeFormula("Locrian", 7);

    #endregion

    #region Constructors

    private ModeFormula(string name,
                        int tonic)
    {
      Debug.Assert(!string.IsNullOrWhiteSpace(name));
      Debug.Assert(tonic >= MinTonic && tonic <= MaxTonic);

      Name = name;
      Tonic = tonic;
    }

    #endregion

    #region Properties

    /// <summary>Gets the mode formula name.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the mode formula's tonic.</summary>
    /// <value>The tonic.</value>
    public int Tonic { get; }

    #endregion

    #region IEquatable<ModeFormula> Members

    /// <inheritdoc />
    public bool Equals(ModeFormula other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( other is null )
      {
        return false;
      }

      return Tonic == other.Tonic;
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(obj, this) )
      {
        return true;
      }

      return obj is ModeFormula other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Tonic;

    /// <inheritdoc />
    public override string ToString() => Name;

    #endregion
  }
}
