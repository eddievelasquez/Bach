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
  #region Constructors

  private ScaleDefinition(
    string formulaId )
  {
    FormulaId = formulaId ?? throw new ArgumentNullException( nameof( formulaId ) );
    Formula = Registry.ScaleFormulas[FormulaId];
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Built-in major scale definition.
  /// </summary>
  public static ScaleDefinition Major { get; } = new( "Major" );

  /// <summary>
  ///   Built-in natural minor scale definition.
  /// </summary>
  public static ScaleDefinition NaturalMinor { get; } = new( "NaturalMinor" );

  /// <summary>
  ///   Built-in harmonic minor scale definition.
  /// </summary>
  public static ScaleDefinition HarmonicMinor { get; } = new( "HarmonicMinor" );

  /// <summary>
  ///   Built-in melodic minor scale definition.
  /// </summary>
  public static ScaleDefinition MelodicMinor { get; } = new( "MelodicMinor" );

  /// <summary>
  ///   Identifier used to resolve a ScaleFormula from the Registry.
  /// </summary>
  public string FormulaId { get; }

  /// <summary>
  ///   The governing formula for the scale definition.
  /// </summary>
  public ScaleFormula Formula { get; }

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
}
