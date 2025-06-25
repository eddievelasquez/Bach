// Module Name: ModeFormula.cs
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

namespace Bach.Model;

using System.Diagnostics;

/// <summary>A mode formula.</summary>
public sealed class ModeFormula: IEquatable<ModeFormula>
{
  #region Constants

  private const int MIN_TONIC = 1;
  private const int MAX_TONIC = 7;

  /// <summary>The Ionian (I) mode.</summary>
  public static readonly ModeFormula Ionian = new( "Ionian", 1 );

  /// <summary>The Dorian (II) mode.</summary>
  public static readonly ModeFormula Dorian = new( "Dorian", 2 );

  /// <summary>The Phrygian (III) mode.</summary>
  public static readonly ModeFormula Phrygian = new( "Phrygian", 3 );

  /// <summary>The Lydian (IV) mode.</summary>
  public static readonly ModeFormula Lydian = new( "Lydian", 4 );

  /// <summary>The Mixolydian (V) mode.</summary>
  public static readonly ModeFormula Mixolydian = new( "Mixolydian", 5 );

  /// <summary>The Aeolian (VI) mode.</summary>
  public static readonly ModeFormula Aeolian = new( "Aeolian", 6 );

  /// <summary>The Locrian (VII).</summary>
  public static readonly ModeFormula Locrian = new( "Locrian", 7 );

  /// <summary>All mode formulas indexed by tonic (1-based, index 0 is unused).</summary>
  public static readonly ModeFormula[] AllModes =
  [
    null!, // index 0 unused
    Ionian, // 1
    Dorian, // 2
    Phrygian, // 3
    Lydian, // 4
    Mixolydian, // 5
    Aeolian, // 6
    Locrian // 7
  ];

  #endregion

  #region Constructors

  private ModeFormula(
    string name,
    int tonic )
  {
    Debug.Assert( !string.IsNullOrWhiteSpace( name ) );
    Debug.Assert( tonic is >= MIN_TONIC and <= MAX_TONIC );

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

  #region Public Methods

  /// <summary>Gets the mode formula by tonic index (1-based).</summary>
  /// <param name="tonic">The tonic index (1-7).</param>
  /// <returns>The corresponding ModeFormula.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If tonic is not in [1,7].</exception>
  public static ModeFormula FromTonic(
    int tonic )
  {
    if( tonic is < MIN_TONIC or > MAX_TONIC )
    {
      throw new ArgumentOutOfRangeException( nameof( tonic ), $"Tonic must be between {MIN_TONIC} and {MAX_TONIC}." );
    }

    return AllModes[tonic];
  }

  /// <inheritdoc />
  public bool Equals(
    ModeFormula? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Tonic == other.Tonic;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is ModeFormula other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return Tonic;
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Name;
  }

  #endregion

  #region Operators

  /// <summary>Explicit cast that converts the given ModeFormula to its tonic index.</summary>
  /// <param name="mode">The mode formula.</param>
  /// <returns>The tonic index (1-7).</returns>
  public static explicit operator int(
    ModeFormula mode )
  {
    return mode.Tonic;
  }

  /// <summary>Explicit cast that converts the given tonic index to a ModeFormula.</summary>
  /// <param name="tonic">The tonic index (1-7).</param>
  /// <returns>The corresponding ModeFormula.</returns>
  public static explicit operator ModeFormula(
    int tonic )
  {
    return FromTonic( tonic );
  }

  #endregion
}
