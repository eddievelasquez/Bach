// Module Name: ScaleFormulaBuilder.cs
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Creates scale formulas.</summary>
public sealed class ScaleFormulaBuilder
{
  #region Fields

  private readonly List<int> _steps = [];
  private readonly HashSet<string> _aliases = new( Comparer.NameComparer );
  private readonly HashSet<string> _categories = new( Comparer.NameComparer );

  private string? _id;
  private string? _name;

  #endregion

  #region Constructors

  /// <summary>Initializes a new instance of the <see cref="ScaleFormulaBuilder"/> class.</summary>
  public ScaleFormulaBuilder()
  {
  }

  /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder"/> class.</summary>
  /// <param name="name">The scale formula's name.</param>
  public ScaleFormulaBuilder(
    string name )
  {
    SetName( name );
  }

  /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder"/> class.</summary>
  /// <param name="id">The scale formula's identifier.</param>
  /// <param name="name">The scale formula's name.</param>
  public ScaleFormulaBuilder(
    string id,
    string name )
  {
    SetId( id );
    SetName( name );
  }

  #endregion

  #region Public Methods

  /// <summary>Adds one or more aliases for the scale formula; multiple aliases are separated by semicolons.</summary>
  /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
  /// <param name="alias">The alias.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddAlias(
    string? alias )
  {
    if( string.IsNullOrEmpty( alias ) )
    {
      return this;
    }

    var aliases = alias.Split( ';' );
    return AddAliases( aliases );
  }

  /// <summary>Adds one or more aliases for the scale formula.</summary>
  /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
  /// <param name="aliases">The aliases.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddAliases(
    IEnumerable<string> aliases )
  {
    ArgumentNullException.ThrowIfNull( aliases );

    foreach( var alias in aliases )
    {
      var trimmed = alias.Trim();

      if( trimmed.Length > 0 )
      {
        _aliases.Add( trimmed );
      }
    }

    return this;
  }

  /// <summary>Adds one or more categories for the scale formula.</summary>
  /// <remarks>
  ///   A category is a user defined value that assists in the classification of a scale formula. e.g. Major, Diatonic,
  ///   Pentatonic, etc.
  /// </remarks>
  /// <param name="categories">The categories.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddCategories(
    IEnumerable<string> categories )
  {
    ArgumentNullException.ThrowIfNull( categories );

    foreach( var category in categories )
    {
      var trimmed = category.Trim();

      if( trimmed.Length > 0 )
      {
        _categories.Add( trimmed );
      }
    }

    return this;
  }

  /// <summary>Adds one or more categories for the scale formula; multiple categories are separated by semicolons.</summary>
  /// <remarks>
  ///   A category is a user defined value that assists in the classification of a scale formula. e.g. Major, Diatonic,
  ///   Pentatonic, etc.
  /// </remarks>
  /// <param name="category">The alias.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddCategory(
    string category )
  {
    ArgumentNullException.ThrowIfNull( category );

    var categories = category.Split( ';' );
    return AddCategories( categories );
  }

  /// <summary>Sets the scale formula's id.</summary>
  /// <param name="id">The scale formula's identifier.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetId(
    string id )
  {
    _id = RemoveWhitespace( id );
    return this;
  }

  /// <summary>Sets the scale formula's name.</summary>
  /// <param name="name">The name.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetName(
    string name )
  {
    ArgumentNullException.ThrowIfNull( name );
    _name = name.Trim();

    return this;
  }

  /// <summary>
  ///   Sets the scale formula's steps, which represent the number of semitones between consecutive notes in the scale.
  /// </summary>
  /// <param name="steps">The steps.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetSteps(
    IEnumerable<int> steps )
  {
    ArgumentNullException.ThrowIfNull( steps );

    _steps.Clear();
    _steps.AddRange( steps );

    return this;
  }

  /// <summary>
  ///   Sets the scale formula's steps, which represent the number of semitones between consecutive notes in the scale.
  /// </summary>
  /// <param name="steps">The steps.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetSteps(
    string steps )
  {
    ArgumentNullException.ThrowIfNull( steps );
    SetSteps( StepCollection.Parse( steps ) );
    return this;
  }

  /// <summary>
  ///   Builds a scale formula instance.
  /// </summary>
  /// <remarks>
  ///   The scale formula will have a default id if none was provided. This id is equivalent to the scale formula's name
  ///   without any whitespace characters.
  ///   The "Diatonic", "Major" or "Minor" categories will be automatically added if the provided intervals satisfy the
  ///   category's requirements.
  /// </remarks>
  /// <returns>A scale formula.</returns>
  /// <exception cref="System.InvalidOperationException">
  ///   Missing the scale formula's name, or the interval list is empty, or the
  ///   interval list is unordered or contains duplicate intervals.
  /// </exception>
  public ScaleFormula Build()
  {
    // Validate required values
    if( string.IsNullOrWhiteSpace( _name ) )
    {
      throw new InvalidOperationException( "Must provide a scale name" );
    }

    // Validate steps

    // 1. Check that the number of steps is at least the minimum required for a scale (5 steps for a pentatonic scale)
    if( _steps.Count < Constants.MinimumScaleStepCount )
    {
      throw new InvalidOperationException(
        $"A scale must contain at least {Constants.MinimumScaleStepCount} steps ({Constants.MinimumScaleStepCount - 1} intervals)"
      );
    }

    // 2. Check that the number of steps does not exceed the maximum allowed for a scale (12 steps for a chromatic scale)
    if( _steps.Count > Constants.MaximumScaleStepCount )
    {
      throw new InvalidOperationException(
        $"A scale must contain at most {Constants.MaximumScaleStepCount} steps ({Constants.MaximumScaleStepCount - 1} intervals)"
      );
    }

    // 3. Check that each step is within the valid range of semitones (1 to 4 semitones)
    if( _steps.Any( step => step < Constants.MinimumScaleStepSize || step > Constants.MaximumScaleStepSize ) )
    {
      throw new InvalidOperationException(
        $"A scale step must be between {Constants.MinimumScaleStepSize} and {Constants.MaximumScaleStepSize} semitones"
      );
    }

    // 4. Check that the sum of the steps equals 12 semitones (the total number of semitones in an octave)
    if( _steps.Sum() != Constants.OctaveSemitoneCount )
    {
      throw new InvalidOperationException( $"The sum of the scale steps must be {Constants.OctaveSemitoneCount} semitones" );
    }

    // Add default values
    _id ??= RemoveWhitespace( _name );

    var intervals = new SortedSet<Interval>( _steps.ToIntervals() );
    Categorize( intervals );

    var formula = new ScaleFormula(
      _id,
      _name,
      _steps,
      intervals,
      _categories,
      _aliases
    );
    return formula;
  }

  #endregion

  #region Implementation

  private void Categorize(
    SortedSet<Interval> intervals )
  {
    // Automatically add categories based on the scale's intervals
    if( IsDiatonic() )
    {
      _categories.Add( ScaleCategory.Diatonic );
    }

    // Is the scale major or minor?
    if( intervals.Contains( Interval.Fifth ) )
    {
      // If the scale contains a perfect fifth, it can be either major or minor. We can determine which one it is
      // by checking for the presence of a major third or minor third.
      var isMajor = intervals.Contains( Interval.MajorThird );
      var isMinor = intervals.Contains( Interval.MinorThird );

      if( isMajor )
      {
        _categories.Add( ScaleCategory.Major );
      }

      if( isMinor )
      {
        if( isMajor )
        {
          throw new InvalidOperationException( "A scale cannot be both major and minor" );
        }

        _categories.Add( ScaleCategory.Minor );
      }
    }

    // Automatically add categories based on the number of intervals
    switch( intervals.Count )
    {
      case 5:
        _categories.Add( ScaleCategory.Pentatonic );
        break;

      case 6:
        _categories.Add( ScaleCategory.Hexatonic );
        break;

      case 7:
        _categories.Add( ScaleCategory.Heptatonic );
        break;

      case 8:
        _categories.Add( ScaleCategory.Octatonic );
        break;
    }

    return;

    bool IsDiatonic()
    {
      if( intervals.Count != 7 )
      {
        return false;
      }

      var wholeSteps = 0;
      var halfSteps = 0;

      foreach( var step in _steps )
      {
        if( step == 2 )
        {
          ++wholeSteps;
        }
        else if( step == 1 )
        {
          ++halfSteps;
        }
      }

      return wholeSteps == 5 && halfSteps == 2;
    }
  }

  [return: NotNullIfNotNull( nameof( value ) )]
  private static string? RemoveWhitespace(
    string? value )
  {
    if( value is null )
    {
      return null;
    }

    return new string(
      value.Where( c => !char.IsWhiteSpace( c ) )
           .ToArray()
    );
  }

  #endregion
}
