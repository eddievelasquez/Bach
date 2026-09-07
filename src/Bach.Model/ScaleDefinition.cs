// Module Name: ScaleDefinition.cs
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

/// <summary>
///   Represents a governing pitch-class collection for a key (major, minor, mode, or custom).
/// </summary>
public sealed class ScaleDefinition
{
  #region Constants

  /// <summary>
  ///   Built-in major scale definition.
  /// </summary>
  public static readonly ScaleDefinition Major = new( "Major" );

  /// <summary>
  ///   Built-in natural minor scale definition.
  /// </summary>
  public static readonly ScaleDefinition NaturalMinor = new( "NaturalMinor" );

  /// <summary>
  ///   Built-in harmonic minor scale definition.
  /// </summary>
  public static readonly ScaleDefinition HarmonicMinor = new( "HarmonicMinor" );

  /// <summary>
  ///   Built-in melodic minor scale definition.
  /// </summary>
  public static readonly ScaleDefinition MelodicMinor = new( "MelodicMinor" );

  /// <summary>
  ///   Built-in Ionian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Ionian = new( "Ionian", Interval.Unison );

  /// <summary>
  ///   Built-in Dorian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Dorian = new( "Dorian", Interval.MajorSecond );

  /// <summary>
  ///   Built-in Phrygian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Phrygian = new( "Phrygian", Interval.MajorThird );

  /// <summary>
  ///   Built-in Lydian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Lydian = new( "Lydian", Interval.Fourth );

  /// <summary>
  ///   Built-in Mixolydian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Mixolydian = new( "Mixolydian", Interval.Fifth );

  /// <summary>
  ///   Built-in Aeolian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Aeolian = new( "Aeolian", Interval.MajorSixth );

  /// <summary>
  ///   Built-in Locrian mode definition.
  /// </summary>
  public static readonly ScaleDefinition Locrian = new( "Locrian", Interval.MajorSeventh );

  // Contains a list of all built-in scale definitions for serialization
  // and deserialization purposes (convert to/from an int).
  private static readonly ScaleDefinition[] s_definitions =
  [
    Major, NaturalMinor, HarmonicMinor, MelodicMinor, Ionian, Dorian, Phrygian, Lydian, Mixolydian, Aeolian, Locrian
  ];

  #endregion

  #region Constructors

  private ScaleDefinition(
    string formulaId,
    Interval? relativeMajorInterval = null )
  {
    FormulaId = formulaId ?? throw new ArgumentNullException( nameof( formulaId ) );
    Formula = Registry.ScaleFormulas[FormulaId];
    RelativeMajorInterval = relativeMajorInterval;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Identifier used to resolve a ScaleFormula from the Registry.
  /// </summary>
  public string FormulaId { get; }

  /// <summary>
  ///   The governing formula for the scale definition.
  /// </summary>
  public ScaleFormula Formula { get; }

  /// <summary>
  ///   Gets the interval from a modal tonic down to the tonic of its relative major collection.
  /// </summary>
  public Interval? RelativeMajorInterval { get; }

  /// <summary>
  ///   Human-readable name for the scale definition.
  /// </summary>
  public string Name => Formula.Name;

  /// <summary>
  ///   True when the governing scale definition is major.
  /// </summary>
  public bool IsMajor => Formula.IsMajor;

  /// <summary>
  ///   True when the governing scale definition is minor.
  /// </summary>
  public bool IsMinor => Formula.IsMinor;

  #endregion

  #region Public Methods

  /// <summary>
  ///   Creates a ScaleDefinition wrapper for an arbitrary registry formula id. This allows construction of Key objects
  ///   for registry-provided formulas that do not have a built-in ScaleDefinition static instance.
  /// </summary>
  /// <param name="formulaId">The formula id to wrap.</param>
  /// <param name="relativeMajorInterval">
  ///   The interval from a modal tonic down to the tonic of its relative major collection.
  /// </param>
  public static ScaleDefinition FromFormulaId(
    string formulaId,
    Interval? relativeMajorInterval = null )
  {
    return new ScaleDefinition( formulaId, relativeMajorInterval );
  }

  #endregion

  #region Operators

  /// <summary>
  ///   Converts a ScaleDefinition to its corresponding index in the built-in definitions array.
  /// </summary>
  /// <param name="scaleDefinition">The ScaleDefinition to convert.</param>
  public static explicit operator int(
    ScaleDefinition scaleDefinition )
  {
    return Array.IndexOf( s_definitions, scaleDefinition );
  }

  /// <summary>
  ///   Converts an index to its corresponding ScaleDefinition in the built-in definitions array.
  /// </summary>
  /// <param name="index">The index to convert.</param>
  public static explicit operator ScaleDefinition(
    int index )
  {
    if( index < 0 || index >= s_definitions.Length )
    {
      throw new ArgumentOutOfRangeException(
        nameof( index ),
        index,
        $"Index must be between 0 and {s_definitions.Length - 1}."
      );
    }

    return s_definitions[index];
  }

  #endregion
}
