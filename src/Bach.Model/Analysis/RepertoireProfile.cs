// Module Name: RepertoireProfile.cs
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
using System.Collections.ObjectModel;
using System.Linq;

namespace Bach.Model.Analysis;

/// <summary>
///   Immutable profile describing which repertoire tags are enabled and their scoring weights.
/// </summary>
public sealed class RepertoireProfile
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="RepertoireProfile"/> class.
  /// </summary>
  /// <param name="tagWeights">Pairs of scale tag and numeric weight. Must be non-empty.</param>
  public RepertoireProfile(
    IEnumerable<(ScaleTag Tag, double Weight)> tagWeights )
  {
    ArgumentNullException.ThrowIfNull( tagWeights );

    var dict = new Dictionary<ScaleTag, double>();

    foreach( var (tag, weight) in tagWeights )
    {
      if( double.IsNaN( weight ) || double.IsInfinity( weight ) || weight < 0.0 )
      {
        throw new ArgumentOutOfRangeException( nameof( tagWeights ), "Weights must be finite and non-negative." );
      }

      dict[tag] = weight;
    }

    if( dict.Count == 0 )
    {
      throw new ArgumentException( "At least one tag weight is required.", nameof( tagWeights ) );
    }

    Weights = new ReadOnlyDictionary<ScaleTag, double>( dict );
    EnabledTags = dict.Keys.ToArray();
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the enabled tags in the profile.
  /// </summary>
  public IReadOnlyList<ScaleTag> EnabledTags { get; }

  /// <summary>
  ///   Gets the scoring weights for enabled tags.
  /// </summary>
  public IReadOnlyDictionary<ScaleTag, double> Weights { get; }

  /// <summary>
  ///   A reasonable default profile used by the evaluator when none is supplied.
  /// </summary>
  public static RepertoireProfile Default =>
    new(
      new[]
      {
        ( ScaleTag.Tonal, 1.0 ),
        ( ScaleTag.CommonPractice, 1.0 ),
        ( ScaleTag.Modal, 0.8 ),
        ( ScaleTag.Jazz, 0.5 ),
        ( ScaleTag.Blues, 0.5 )
      }
    );

  /// <summary>
  ///   Gets a profile for common-practice tonal analysis.
  /// </summary>
  public static RepertoireProfile CommonPractice =>
    new( new[] { ( ScaleTag.Tonal, 1.0 ), ( ScaleTag.CommonPractice, 1.0 ) } );

  /// <summary>
  ///   Gets a profile for modal analysis.
  /// </summary>
  public static RepertoireProfile Modal =>
    new( new[] { ( ScaleTag.Modal, 1.0 ), ( ScaleTag.Tonal, 0.5 ) } );

  #endregion

  #region Public Methods

  /// <summary>
  ///   Gets the weight for a tag, or zero when the tag is not enabled.
  /// </summary>
  public double GetWeight(
    ScaleTag tag )
  {
    return Weights.GetValueOrDefault(tag, 0.0);
  }

  #endregion
}
