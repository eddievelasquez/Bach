// Module Name: PartBuilder.cs
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

namespace Bach.Model.Structure;

/// <summary>
///   Builds immutable <see cref="Part"/> instances from ordered measures.
/// </summary>
public sealed class PartBuilder
{
  #region Fields

  private readonly List<Measure> _measures;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes an empty part builder.
  /// </summary>
  public PartBuilder()
  {
    _measures = [];
  }

  /// <summary>
  ///   Initializes a part builder with the specified initial capacity.
  /// </summary>
  /// <param name="capacity">The initial measure capacity.</param>
  public PartBuilder(
    int capacity )
  {
    _measures = new List<Measure>( capacity );
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a configured measure to the part.
  /// </summary>
  /// <param name="configureMeasure">The action that configures the measure builder.</param>
  /// <returns>This builder.</returns>
  public PartBuilder AddMeasure(
    Action<MeasureBuilder> configureMeasure )
  {
    ArgumentNullException.ThrowIfNull( configureMeasure );

    var measureBuilder = new MeasureBuilder();
    configureMeasure( measureBuilder );
    _measures.Add( measureBuilder.Build() );
    return this;
  }

  /// <summary>
  ///   Adds a completed <see cref="Measure"/> to the part.
  /// </summary>
  /// <param name="measure">The measure to add.</param>
  /// <returns>This builder.</returns>
  public PartBuilder AddMeasure(
    Measure measure )
  {
    ArgumentNullException.ThrowIfNull( measure );
    _measures.Add( measure );
    return this;
  }

  /// <summary>
  ///   Adds a collection of completed <see cref="Measure"/> instances to the part.
  /// </summary>
  /// <param name="measures">The measures to add.</param>
  /// <returns>This builder.</returns>
  public PartBuilder AddMeasures(
    params ReadOnlySpan<Measure> measures )
  {
    foreach( var measure in measures )
    {
      AddMeasure( measure );
    }

    return this;
  }

  /// <summary>
  ///   Creates an immutable part from the measures currently in the builder.
  /// </summary>
  public Part Build()
  {
    return new Part( _measures );
  }

  #endregion
}
