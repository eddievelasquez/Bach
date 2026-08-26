// Module Name: KeySignature.cs
// Project:     Bach.Model
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

namespace Bach.Model;

/// <summary>Represents the accidentals used by a key signature.</summary>
public readonly struct KeySignature
{
  #region Constants

  private static readonly KeySignature s_empty = new( 0, false );

  /// <summary>Gets the C major key signature.</summary>
  public static readonly KeySignature CMajor = new( 0, true );

  /// <summary>Gets the G major key signature.</summary>
  public static readonly KeySignature GMajor = new( 1, true );

  /// <summary>Gets the D major key signature.</summary>
  public static readonly KeySignature DMajor = new( 2, true );

  /// <summary>Gets the A major key signature.</summary>
  public static readonly KeySignature AMajor = new( 3, true );

  /// <summary>Gets the E major key signature.</summary>
  public static readonly KeySignature EMajor = new( 4, true );

  /// <summary>Gets the B major key signature.</summary>
  public static readonly KeySignature BMajor = new( 5, true );

  /// <summary>Gets the F sharp major key signature.</summary>
  public static readonly KeySignature FSharpMajor = new( 6, true );

  /// <summary>Gets the F major key signature.</summary>
  public static readonly KeySignature FMajor = new( 1, false );

  /// <summary>Gets the B flat major key signature.</summary>
  public static readonly KeySignature BFlatMajor = new( 2, false );

  /// <summary>Gets the E flat major key signature.</summary>
  public static readonly KeySignature EFlatMajor = new( 3, false );

  /// <summary>Gets the A flat major key signature.</summary>
  public static readonly KeySignature AFlatMajor = new( 4, false );

  /// <summary>Gets the D flat major key signature.</summary>
  public static readonly KeySignature DFlatMajor = new( 5, false );

  /// <summary>Gets the G flat major key signature.</summary>
  public static readonly KeySignature GFlatMajor = new( 6, false );

  /// <summary>Gets the C flat major key signature.</summary>
  public static readonly KeySignature CFlatMajor = new( 7, false );

  /// <summary>Gets the A minor key signature.</summary>
  public static readonly KeySignature AMinor = new( 0, true );

  /// <summary>Gets the E minor key signature.</summary>
  public static readonly KeySignature EMinor = new( 1, true );

  /// <summary>Gets the B minor key signature.</summary>
  public static readonly KeySignature BMinor = new( 2, true );

  /// <summary>Gets the F sharp minor key signature.</summary>
  public static readonly KeySignature FSharpMinor = new( 3, true );

  /// <summary>Gets the C sharp minor key signature.</summary>
  public static readonly KeySignature CSharpMinor = new( 4, true );

  /// <summary>Gets the G sharp minor key signature.</summary>
  public static readonly KeySignature GSharpMinor = new( 5, true );

  /// <summary>Gets the D sharp minor key signature.</summary>
  public static readonly KeySignature DSharpMinor = new( 6, true );

  /// <summary>Gets the D minor key signature.</summary>
  public static readonly KeySignature DMinor = new( 1, false );

  /// <summary>Gets the G minor key signature.</summary>
  public static readonly KeySignature GMinor = new( 2, false );

  /// <summary>Gets the C minor key signature.</summary>
  public static readonly KeySignature CMinor = new( 3, false );

  /// <summary>Gets the F minor key signature.</summary>
  public static readonly KeySignature FMinor = new( 4, false );

  /// <summary>Gets the B flat minor key signature.</summary>
  public static readonly KeySignature BFlatMinor = new( 5, false );

  /// <summary>Gets the E flat minor key signature.</summary>
  public static readonly KeySignature EFlatMinor = new( 6, false );

  /// <summary>Gets the A flat minor key signature.</summary>
  public static readonly KeySignature AFlatMinor = new( 7, false );

  #endregion

  #region Constructors

  /// <summary>Initializes a new instance of the <see cref="KeySignature"/> struct.</summary>
  /// <param name="accidentalCount">The number of accidentals.</param>
  /// <param name="isSharp">True when the signature uses sharps; otherwise, flats.</param>
  private KeySignature(
    int accidentalCount,
    bool isSharp )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( accidentalCount, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( accidentalCount, 7 );

    AccidentalCount = accidentalCount;
    Accidental = isSharp ? Accidental.Sharp : Accidental.Flat;
  }

  #endregion

  #region Properties

  /// <summary>Gets the number of accidentals in the signature.</summary>
  public int AccidentalCount { get; }

  /// <summary>Gets the accidental corresponding to the signature's direction.</summary>
  public Accidental Accidental { get; }

  /// <summary>Gets an empty key signature.</summary>
  public static KeySignature Empty => s_empty;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public override string ToString()
  {
    return Accidental == Accidental.Sharp
      ? $"{AccidentalCount} sharps"
      : $"{AccidentalCount} flats";
  }

  #endregion
}
