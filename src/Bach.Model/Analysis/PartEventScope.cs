// Module Name: PartEventScope.cs
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
using System.Linq;
using Bach.Model.Analysis;
using Bach.Model.Internal;

namespace Bach.Model.Analysis;

/// <summary>
///   Identifies a range of flattened events in an immutable, measure-based <see cref="Part"/>.
/// </summary>
/// <remarks>
///   The range is start-inclusive and end-exclusive. From-end indexes use the standard .NET
///   <see cref="System.Index"/> semantics. Events are calculated from the source when requested.
/// </remarks>
public sealed class PartEventScope
{
  #region Constructors

  /// <summary>
  ///   Initializes a scope for all events in the source part.
  /// </summary>
  /// <param name="source">The immutable source part.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
  public PartEventScope(
    Part source )
    : this( source, .. )
  {
  }

  /// <summary>
  ///   Initializes a scope for a range of events in the source part.
  /// </summary>
  /// <param name="source">The immutable source part.</param>
  /// <param name="range">The start-inclusive and end-exclusive event range.</param>
  /// <param name="appliedFunctions">The immutable applied-function annotations assigned to events in this scope.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the range cannot be resolved against the source count.</exception>
  public PartEventScope(
    Part source,
    Range range,
    IEnumerable<AppliedFunction>? appliedFunctions = null )
  {
    Source = source ?? throw new ArgumentNullException( nameof( source ) );
    Range = range;

    // Validate the range against the source count.
    var events = source.Events.ToArray();
    var (offset, length) = range.GetOffsetAndLength( events.Length );

    var scopedEvents = events.Skip( offset )
                             .Take( length )
                             .ToArray();
    var annotations = ( appliedFunctions ?? Array.Empty<AppliedFunction>() ).ToArray();

    ArgumentException.ThrowIfContainsNulls(
      annotations,
      "The applied-function collection contains a null annotation.",
      nameof( appliedFunctions )
    );

    if( annotations.Any( appliedFunction =>
                           !scopedEvents.Any( partEvent => ReferenceEquals( partEvent, appliedFunction.Target.PartEvent ) )
       ) )
    {
      throw new ArgumentException(
        "Each applied-function target must belong to the event scope.",
        nameof( appliedFunctions )
      );
    }

    AppliedFunctions = Array.AsReadOnly( annotations );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the immutable source part.
  /// </summary>
  public Part Source { get; }

  /// <summary>
  ///   Gets the event range represented by this scope.
  /// </summary>
  public Range Range { get; }

  /// <summary>
  ///   Gets the immutable applied-function annotations assigned to events in this scope.
  /// </summary>
  public IReadOnlyList<AppliedFunction> AppliedFunctions { get; }

  /// <summary>
  ///   Gets the events selected by <see cref="Range"/> from <see cref="Source"/>.
  /// </summary>
  public IEnumerable<IPartEvent> Events
  {
    get
    {
      var events = Source.Events.ToArray();
      var (offset, length) = Range.GetOffsetAndLength( events.Length );

      for( var index = 0; index < length; index++ )
      {
        yield return events[offset + index];
      }
    }
  }

  /// <summary>
  ///   Gets the measure and event location for each event in the scope.
  /// </summary>
  public IEnumerable<PartEventLocation> Locations
  {
    get
    {
      var locations = Source.MeasuresWithLocations().ToArray();
      var (offset, length) = Range.GetOffsetAndLength( locations.Length );

      for( var index = 0; index < length; index++ )
      {
        yield return locations[offset + index].Location;
      }
    }
  }

  #endregion
}
