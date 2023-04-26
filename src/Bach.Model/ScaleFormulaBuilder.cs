// Module Name: ScaleFormulaBuilder.cs
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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Creates scale formulas.</summary>
public sealed class ScaleFormulaBuilder
{
#region Fields

  private readonly SortedSet<Interval> _intervals = new();
  private readonly HashSet<string> _aliases = new( Comparer.NameComparer );
  private readonly HashSet<string> _categories = new( Comparer.NameComparer );
  private string? _id;
  private string? _name;

#endregion

#region Constructors

  /// <summary>Initializes a new instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
  public ScaleFormulaBuilder()
  {
  }

  /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
  /// <param name="name">The scale formula's name.</param>
  public ScaleFormulaBuilder( string name )
  {
    SetName( name );
  }

  /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
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

  /// <summary>Adds one or more aliases for the scale formula; multiple aliases are separated by semi-colons.</summary>
  /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
  /// <param name="alias">The alias.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddAlias( string? alias )
  {
    Requires.NotNullOrEmpty( alias );

    var aliases = alias.Split( ';' );
    return AddAliases( aliases );
  }

  /// <summary>Adds one or more aliases for the scale formula.</summary>
  /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
  /// <param name="aliases">The aliases.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddAliases( IEnumerable<string> aliases )
  {
    Requires.NotNull( aliases );

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
  ///   A category is a user defined value that assists in the classification of a scale formula. e.g Major, Diatonic,
  ///   Pentatonic, etc.
  /// </remarks>
  /// <param name="categories">The categories.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddCategories( IEnumerable<string> categories )
  {
    Requires.NotNull( categories );

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

  /// <summary>Adds one or more categories for the scale formula; multiple categories are separated by semi-colons.</summary>
  /// <remarks>
  ///   A category is a user defined value that assists in the classification of a scale formula. e.g Major, Diatonic,
  ///   Pentatonic, etc.
  /// </remarks>
  /// <param name="category">The alias.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AddCategory( string category )
  {
    Requires.NotNull( category );

    var categories = category.Split( ';' );
    return AddCategories( categories );
  }

  /// <summary>Appends an interval to the scale formula's list of intervals.</summary>
  /// <param name="interval">The interval to append.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder AppendInterval( Interval interval )
  {
    _intervals.Add( interval );
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

    if( _intervals.Count < 2 )
    {
      throw new InvalidOperationException( "A scale must contain at least two intervals" );
    }

    // Add default values
    _id ??= RemoveWhitespace( _name );

    if( IsDiatonic() )
    {
      _categories.Add( ScaleCategory.Diatonic );
    }

    if( IsMajor() )
    {
      _categories.Add( ScaleCategory.Major );
    }

    if( IsMinor() )
    {
      _categories.Add( ScaleCategory.Minor );
    }

    switch( _intervals.Count )
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

    var formula = new ScaleFormula( _id, _name, _intervals.ToArray(), _categories.ToHashSet(), _aliases.ToHashSet() );
    return formula;
  }

  /// <summary>Sets the scale formula's id.</summary>
  /// <param name="id">The scale formula's identifier.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetId( string id )
  {
    _id = RemoveWhitespace( id );
    return this;
  }

  /// <summary>Sets the scale formula's intervals.</summary>
  /// <param name="intervals">The intervals.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetIntervals( IEnumerable<Interval> intervals )
  {
    Requires.NotNull(intervals);

    _intervals.Clear();

    foreach( var interval in intervals )
    {
      _intervals.Add( interval );
    }

    return this;
  }

  /// <summary>Sets the scale formula's intervals.</summary>
  /// <param name="intervals">The intervals.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetIntervals( string intervals )
  {
    Requires.NotNull(intervals);
    SetIntervals( Formula.ParseIntervals( intervals ) );

    return this;
  }

  /// <summary>Sets the scale formula's name.</summary>
  /// <param name="name">The name.</param>
  /// <returns>This instance.</returns>
  public ScaleFormulaBuilder SetName( string name )
  {
    Requires.NotNull(name);
    _name = name.Trim();

    return this;
  }

#endregion

#region Implementation

  private bool IsDiatonic()
  {
    if( _intervals.Count != 7 )
    {
      return false;
    }

    var wholeSteps = 0;
    var halfSteps = 0;

    foreach( var step in Formula.GetRelativeSteps( _intervals.ToArray() ) )
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

  private bool IsMajor()
  {
    return _intervals.Contains( Interval.MajorThird ) && _intervals.Contains( Interval.Fifth );
  }

  private bool IsMinor()
  {
    return _intervals.Contains( Interval.MinorThird ) && _intervals.Contains( Interval.Fifth );
  }

  [return: NotNullIfNotNull( nameof( value ) )]
  private static string? RemoveWhitespace( string? value )
  {
    if( value == null )
    {
      return null;
    }

    var builder = new StringBuilder( value.Length );
    foreach( var c in value )
    {
      if( char.IsWhiteSpace( c ) )
      {
        continue;
      }

      builder.Append( c );
    }

    return builder.ToString();
  }

#endregion
}
