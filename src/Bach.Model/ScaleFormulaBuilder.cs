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

/// <summary>
///   Creates scale formulas.
/// </summary>
public sealed class ScaleFormulaBuilder
{
  #region Fields

  private readonly HashSet<string> _aliases = new( Comparer.NameComparer );
  private readonly ScaleClassificationBuilder _classificationBuilder = new();
  private readonly List<Interval> _ascendingIntervals = [];
  private readonly List<Interval> _descendingIntervals = [];

  private string? _id;
  private string? _name;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ScaleFormulaBuilder"/> class.
  /// </summary>
  public ScaleFormulaBuilder()
  {
  }

  /// <summary>
  ///   Initializes a new named instance of the <see cref="ScaleFormulaBuilder"/> class.
  /// </summary>
  /// <param name="name">
  ///   The scale formula's name.
  /// </param>
  public ScaleFormulaBuilder(
    string name )
  {
    SetName( name );
  }

  /// <summary>
  ///   Initializes a new named instance of the <see cref="ScaleFormulaBuilder"/> class.
  /// </summary>
  /// <param name="id">
  ///   The scale formula's identifier.
  /// </param>
  /// <param name="name">
  ///   The scale formula's name.
  /// </param>
  public ScaleFormulaBuilder(
    string id,
    string name )
  {
    SetId( id );
    SetName( name );
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Sets the scale formula's id.
  /// </summary>
  /// <param name="id">
  ///   The scale formula's identifier.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder SetId(
    string id )
  {
    _id = RemoveWhitespace( id );
    return this;
  }

  /// <summary>
  ///   Sets the scale formula's name.
  /// </summary>
  /// <param name="name">
  ///   The name.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder SetName(
    string name )
  {
    ArgumentNullException.ThrowIfNull( name );
    _name = name.Trim();

    return this;
  }

  /// <summary>
  ///   Adds an ascending interval to the scale formula.
  /// </summary>
  /// <param name="interval">
  ///   The interval to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder AddAscendingInterval(
    Interval interval )
  {
    _ascendingIntervals.Add( interval );
    return this;
  }

  /// <summary>
  ///   Adds multiple ascending intervals to the scale formula.
  /// </summary>
  /// <param name="intervals">
  ///   The intervals to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder AddAscendingIntervals(
    IEnumerable<Interval> intervals )
  {
    ArgumentNullException.ThrowIfNull( intervals );
    _ascendingIntervals.AddRange( intervals );
    return this;
  }

  /// <summary>
  ///   Sets the ascending intervals for the scale formula.
  /// </summary>
  /// <param name="intervals">
  ///   The intervals to set.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  /// <remarks>
  ///   Clears any existing ascending intervals before setting the new ones.
  /// </remarks>
  public ScaleFormulaBuilder SetAscendingIntervals(
    IEnumerable<Interval> intervals )
  {
    ArgumentNullException.ThrowIfNull( intervals );
    _ascendingIntervals.Clear();
    _ascendingIntervals.AddRange( intervals );
    return this;
  }

  /// <summary>
  ///   Adds a descending interval to the scale formula.
  /// </summary>
  /// <param name="interval">
  ///   The interval to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder AddDescendingInterval(
    Interval interval )
  {
    _descendingIntervals.Add( interval );
    return this;
  }

  /// <summary>
  ///   Adds multiple descending intervals to the scale formula.
  /// </summary>
  /// <param name="intervals">
  ///   The intervals to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder AddDescendingIntervals(
    IEnumerable<Interval> intervals )
  {
    ArgumentNullException.ThrowIfNull( intervals );
    _descendingIntervals.AddRange( intervals );
    return this;
  }

  /// <summary>
  ///   Sets the descending intervals for the scale formula.
  /// </summary>
  /// <param name="intervals">
  ///   The intervals to set.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  /// <remarks>
  ///   Clears any existing descending intervals before setting the new ones.
  /// </remarks>
  public ScaleFormulaBuilder SetDescendingIntervals(
    IEnumerable<Interval> intervals )
  {
    ArgumentNullException.ThrowIfNull( intervals );
    _descendingIntervals.Clear();
    _descendingIntervals.AddRange( intervals );
    return this;
  }

  /// <summary>
  ///   Adds one or more aliases for the scale formula; multiple aliases are separated by semicolons.
  /// </summary>
  /// <remarks>
  ///   An alias is an alternative name by which the scale formula might be known as.
  /// </remarks>
  /// <param name="alias">
  ///   The alias.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
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

  /// <summary>
  ///   Adds one or more aliases for the scale formula.
  /// </summary>
  /// <remarks>
  ///   An alias is an alternative name by which the scale formula might be known as.
  /// </remarks>
  /// <param name="aliases">
  ///   The aliases.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleFormulaBuilder AddAliases(
    IEnumerable<string> aliases )
  {
    ArgumentNullException.ThrowIfNull( aliases );

    foreach( var alias in aliases.Select( a => a.Trim() )
                                 .Where( t => t.Length > 0 ) )
    {
      _aliases.Add( alias );
    }

    return this;
  }

  /// <summary>
  /// Configures the scale formula's classification using the provided action.
  /// </summary>
  /// <param name="classifyAction">The action to configure the scale classification.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder Classify( Action<ScaleClassificationBuilder> classifyAction )
  {
    ArgumentNullException.ThrowIfNull( classifyAction );
    classifyAction( _classificationBuilder );
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
  /// <returns>
  ///   A scale formula.
  /// </returns>
  /// <exception cref="System.InvalidOperationException">
  ///   Missing the scale formula's name, or the interval list is empty, or the
  ///   interval list is unordered or contains duplicate intervals.
  /// </exception>
  public ScaleFormula Build()
  {
    if( string.IsNullOrWhiteSpace( _name ) )
    {
      throw new InvalidOperationException( "Must provide a scale name" );
    }

    if( _ascendingIntervals.Empty )
    {
      throw new InvalidOperationException( "Must provide ascending scale intervals" );
    }

    ValidateIntervals( _ascendingIntervals, "ascending" );

    if( !_descendingIntervals.Empty )
    {
      if( _descendingIntervals.Count != _ascendingIntervals.Count )
      {
        throw new InvalidOperationException( "Ascending and descending degree collections must have the same count" );
      }

      ValidateIntervals( _descendingIntervals, "descending" );

      // If descending intervals were provided, we will reverse them to ensure they are in descending order.
      _descendingIntervals.Reverse();
    }
    else
    {
      // If no descending intervals were provided, we will automatically generate them by reversing the ascending intervals.
      _descendingIntervals.AddRange( ( (IEnumerable<Interval>) _ascendingIntervals ).Reverse() );
    }

    // Validate steps

    // 1. Check that the number of intervals is at least the minimum allowed for a scale (5 steps for a pentatonic scale)
    // No need to check the descending intervals because they must match the ascending intervals in count if provided.
    if( _ascendingIntervals.Count < Constants.MinimumScaleIntervalCount )
    {
      throw new InvalidOperationException( $"A scale must contain at least {Constants.MinimumScaleIntervalCount} intervals" );
    }

    // 2. Check that the number of intervals does not exceed the maximum allowed for a scale (12 steps for a chromatic scale)
    // No need to check the descending intervals because they must match the ascending intervals in count if provided.
    if( _ascendingIntervals.Count > Constants.MaximumScaleIntervalCount )
    {
      throw new InvalidOperationException( $"A scale must contain at most {Constants.MaximumScaleIntervalCount} intervals" );
    }

    // Add default values
    _id ??= RemoveWhitespace( _name );

    Categorize( [.. _ascendingIntervals] );

    // Calculate the formula's ascending and descending degrees using the position of each interval as the degree's ordinal
    var ascendingDegrees = _ascendingIntervals.Select( (
                                                         interval,
                                                         index ) => new ScaleDegreeStep( index + 1, interval )
                                              )
                                              .ToList();

    var descendingDegrees = _descendingIntervals.Select( (
                                                           interval,
                                                           index ) => new ScaleDegreeStep( index + 1, interval )
                                                )
                                                .ToList();

    var formula = new ScaleFormula(
      _id,
      _name,
      ascendingDegrees,
      descendingDegrees,
      _classificationBuilder.SetAscendingDegrees( ascendingDegrees )
                            .Build(),
      _aliases
    );

    return formula;
  }

  #endregion

  #region Implementation

  private static void ValidateIntervals(
    IReadOnlyList<Interval> intervals,
    string direction )
  {
    if( intervals[0] != Interval.Unison )
    {
      throw new InvalidOperationException( $"The first {direction} scale interval must be unison (P1)" );
    }

    for( var index = 1; index < intervals.Count; index++ )
    {
      var thisSemitoneCount = intervals[index].SemitoneCount;
      var prevSemitoneCount = intervals[index - 1].SemitoneCount;

      if( thisSemitoneCount <= prevSemitoneCount )
      {
        throw new InvalidOperationException( $"The {direction} scale intervals must be strictly ascending" );
      }
    }
  }

  private void Categorize(
    SortedSet<Interval> intervals )
  {
    // Automatically add categories based on the scale's intervals
    if( IsDiatonic() )
    {
      _classificationBuilder.AddCategory( ScaleCategory.Diatonic );
    }

    // A major scale contains a functional major third and a perfect fifth.
    // A minor scale contains a functional minor third and a perfect fifth.
    // A scale that contains both minor and major third intervals doesn't have
    // a functional third so it's neither major nor minor.
    if( intervals.Contains( Interval.Fifth ) )
    {
      if( intervals.Contains( Interval.MajorThird ) && !intervals.Contains( Interval.MinorThird ) )
      {
        _classificationBuilder.AddCategory( ScaleCategory.Major );
      }

      if( intervals.Contains( Interval.MinorThird ) && !intervals.Contains( Interval.MajorThird ) )
      {
        _classificationBuilder.AddCategory( ScaleCategory.Minor );
      }
    }

    // Automatically add categories based on the number of intervals
    switch( intervals.Count )
    {
      case 5:
        _classificationBuilder.AddCategory( ScaleCategory.Pentatonic );
        break;

      case 6:
        _classificationBuilder.AddCategory( ScaleCategory.Hexatonic );
        break;

      case 7:
        _classificationBuilder.AddCategory( ScaleCategory.Heptatonic );
        break;

      case 8:
        _classificationBuilder.AddCategory( ScaleCategory.Octatonic );
        break;
    }

    return;

    bool IsDiatonic()
    {
      // A diatonic scale is a heptatonic (7 step) scale that consists of five whole steps and two half steps
      if( intervals.Count != 7 )
      {
        return false;
      }

      var wholeSteps = 0;
      var halfSteps = 0;
      var lastStepSize = 0;
      var totalSemitones = 0;

      const int Whole = 2;
      const int Half = 1;

      foreach( var stepSize in _ascendingIntervals.GetSemitoneSteps() )
      {
        switch( stepSize )
        {
          case Whole:
            ++wholeSteps;
            break;

          // If the last step was a half step, then this is not a diatonic scale because
          // the two half steps must be separated by at least one whole step.
          case Half when lastStepSize == Half:
            return false;

          case Half:
            ++halfSteps;
            break;
        }

        lastStepSize = stepSize;
        totalSemitones += stepSize;
      }

      // A diatonic scale must have a total of 12 semitones, 5 whole steps and 2 half steps.
      return totalSemitones == Constants.OctaveSemitoneCount && wholeSteps == 5 && halfSteps == 2;
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

    // Use stackalloc to allocate a buffer on the stack for performance, since we know the
    // maximum size needed is the length of the input string.
    Span<char> buffer = stackalloc char[value.Length];
    var index = 0;

    foreach( var c in value )
    {
      // Only copy non-whitespace characters to the buffer
      if( !char.IsWhiteSpace( c ) )
      {
        buffer[index++] = c;
      }
    }

    // Create a new string from the buffer up to the index of the last non-whitespace character
    return new string( buffer[..index] );
  }

  #endregion
}
