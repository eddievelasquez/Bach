// Module Name: ScaleFormulaBuilder.cs
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
  using System.Collections.Generic;
  using System.Collections.Immutable;
  using System.Text;
  using Internal;

  /// <summary>Creates scale formulas.</summary>
  public class ScaleFormulaBuilder
  {
    #region Data Members

    private readonly List<Interval> _intervals = new List<Interval>();
    private readonly HashSet<string> _aliases = new HashSet<string>(Comparer.NameComparer);
    private readonly HashSet<string> _categories = new HashSet<string>(Comparer.NameComparer);

    private string _id;
    private string _name;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
    public ScaleFormulaBuilder()
    {
    }

    /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
    /// <param name="name">The scale formula's name.</param>
    public ScaleFormulaBuilder(string name)
    {
      SetName(name);
    }

    /// <summary>Initializes a new named instance of the <see cref="ScaleFormulaBuilder" /> class.</summary>
    /// <param name="id">The scale formula's identifier.</param>
    /// <param name="name">The scale formula's name.</param>
    public ScaleFormulaBuilder(string id, string name)
    {
      SetId(id);
      SetName(name);
    }

    #endregion

    #region Public Methods

    /// <summary>Sets the scale formula's id.</summary>
    /// <param name="id">The scale formula's identifier.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder SetId(string id)
    {
      _id = RemoveWhitespace(id);
      return this;
    }

    /// <summary>Sets the scale formula's name.</summary>
    /// <param name="name">The name.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder SetName(string name)
    {
      if( name != null )
      {
        _name = name.Trim();
      }

      return this;
    }

    /// <summary>Sets the scale formula's intervals.</summary>
    /// <param name="intervals">The intervals.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder SetIntervals(IEnumerable<Interval> intervals)
    {
      _intervals.Clear();

      if( intervals != null )
      {
        _intervals.AddRange(intervals);
      }

      return this;
    }

    /// <summary>Sets the scale formula's intervals.</summary>
    /// <param name="intervals">The intervals.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder SetIntervals(string intervals)
    {
      _intervals.Clear();

      if( !string.IsNullOrEmpty(intervals) )
      {
        _intervals.AddRange(Formula.ParseIntervals(intervals));
      }

      return this;
    }

    /// <summary>Appends an interval to the scale formula's list of intervals.</summary>
    /// <param name="interval">The interval to append.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder AppendInterval(Interval interval)
    {
      _intervals.Add(interval);
      return this;
    }

    /// <summary>Adds one or more aliases for the scale formula; multiple aliases are separated by semi-colons.</summary>
    /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
    /// <param name="alias">The alias.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder AddAlias(string alias)
    {
      if( alias == null )
      {
        return this;
      }

      string[] aliases = alias.Split(';');
      return AddAliases(aliases);
    }

    /// <summary>Adds one or more aliases for the scale formula.</summary>
    /// <remarks>An alias is an alternative name by which the scale formula might be known as.</remarks>
    /// <param name="aliases">The aliases.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder AddAliases(IEnumerable<string> aliases)
    {
      if( aliases == null )
      {
        return this;
      }

      foreach( string alias in aliases )
      {
        if( alias == null )
        {
          continue;
        }

        string trimmed = alias.Trim();
        if( trimmed.Length > 0 )
        {
          _aliases.Add(trimmed);
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
    public ScaleFormulaBuilder AddCategory(string category)
    {
      if( category == null )
      {
        return this;
      }

      string[] categories = category.Split(';');
      return AddCategories(categories);
    }

    /// <summary>Adds one or more categories for the scale formula.</summary>
    /// <remarks>
    ///   A category is a user defined value that assists in the classification of a scale formula. e.g Major, Diatonic,
    ///   Pentatonic, etc.
    /// </remarks>
    /// <param name="categories">The categories.</param>
    /// <returns>This instance.</returns>
    public ScaleFormulaBuilder AddCategories(IEnumerable<string> categories)
    {
      if( categories == null )
      {
        return this;
      }

      foreach( string category in categories )
      {
        if( category == null )
        {
          continue;
        }

        string trimmed = category.Trim();
        if( trimmed.Length > 0 )
        {
          _categories.Add(trimmed);
        }
      }

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
    /// <exception cref="InvalidOperationException">
    ///   Missing the scale formula's name, or the interval list is empty, or the
    ///   interval list is unordered or contains duplicate intervals.
    /// </exception>
    public ScaleFormula Build()
    {
      // Validate required values
      if( string.IsNullOrWhiteSpace(_name) )
      {
        throw new InvalidOperationException("Must provide a scale name");
      }

      if( _intervals.Count == 0 )
      {
        throw new InvalidOperationException("A scale must contain at least two intervals");
      }

      if( !_intervals.IsSortedUnique() )
      {
        throw new InvalidOperationException("A scale's intervals must be sorted and without duplicates");
      }

      // Add default values
      if( _id == null )
      {
        _id = RemoveWhitespace(_name);
      }

      if( IsDiatonic() )
      {
        _categories.Add("Diatonic");
      }

      if( IsMajor() )
      {
        _categories.Add("Major");
      }

      if( IsMinor() )
      {
        _categories.Add("Minor");
      }

      var formula = new ScaleFormula(_id, _name, _intervals.ToArray(), _categories.ToImmutableHashSet(), _aliases.ToImmutableHashSet());
      return formula;
    }

    #endregion

    #region  Implementation

    internal bool IsDiatonic()
    {
      if( _intervals.Count != 7 )
      {
        return false;
      }

      var wholeSteps = 0;
      var halfSteps = 0;

      foreach( int step in Formula.GetRelativeSteps(_intervals) )
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

    private bool IsMajor() => _intervals[0] == Interval.Unison && _intervals.Contains(Interval.MajorThird) && _intervals.Contains(Interval.Fifth);

    private bool IsMinor() => _intervals[0] == Interval.Unison && _intervals.Contains(Interval.MinorThird) && _intervals.Contains(Interval.Fifth);

    private static string RemoveWhitespace(string value)
    {
      if( value == null )
      {
        return null;
      }

      var builder = new StringBuilder(value.Length);
      foreach( char c in value )
      {
        if( char.IsWhiteSpace(c) )
        {
          continue;
        }

        builder.Append(c);
      }

      return builder.ToString();
    }

    #endregion
  }
}
