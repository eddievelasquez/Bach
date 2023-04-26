// Module Name: Formula.cs
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
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>A formula is a base class for constructing a sequence of pitch classes based on a series of intervals.</summary>
public abstract class Formula
  : INamedObject,
    IEquatable<Formula>
{
#region Nested Types

  private sealed class IntervalComparer: IComparer<Interval>
  {
#region Public Methods

    public int Compare(
      Interval x,
      Interval y )
    {
      return x.CompareTo( y );
    }

#endregion
  }

  private sealed class SemitoneCountIntervalComparer: IComparer<Interval>
  {
#region Public Methods

    public int Compare(
      Interval x,
      Interval y )
    {
      return x.SemitoneCount - y.SemitoneCount;
    }

#endregion
  }

#endregion

#region Constants

  private const string NameIntervalsToStringFormat = "N: I";

  private static readonly IntervalComparer s_intervalComparer = new();
  private static readonly SemitoneCountIntervalComparer s_semitoneComparer = new();

#endregion

#region Constructors

  /// <summary>Specialized constructor for use only by derived classes.</summary>
  /// <exception cref="ArgumentNullException">Thrown when either the id, name or interval arguments are null.</exception>
  /// <exception cref="ArgumentException">Thrown when the id or name are empty.</exception>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when interval array is empty.</exception>
  /// <param name="id">
  ///   The language-neutral identifier for the formula. The id is used as the unique identifier for a formula in
  ///   the registry.
  /// </param>
  /// <param name="name">The localizable name for the formula.</param>
  /// <param name="intervals">The intervals that compose the formula.</param>
  protected Formula(
    string id,
    string name,
    Interval[] intervals )
  {
    Requires.NotNullOrEmpty( id );
    Requires.NotNullOrEmpty( name );
    Requires.NotNullOrEmpty( intervals );

    Id = id;
    Name = name;
    Intervals = new IntervalCollection( intervals );
  }

#endregion

#region Properties

  /// <summary>Gets the intervals that compose this formula.</summary>
  /// <value>The intervals.</value>
  public IntervalCollection Intervals { get; }

  /// <summary>Returns the language-neutral id for the formula.</summary>
  /// <value>The id.</value>
  public string Id { get; }

  /// <summary>Gets the localizable name for the formula.</summary>
  /// <value>The name.</value>
  public string Name { get; }

#endregion

#region Public Methods

  /// <summary>Determines whether this instance contains the provided intervals.</summary>
  /// <param name="intervals">The intervals to evaluate.</param>
  /// <param name="match">Interval matching strategy.</param>
  /// <returns>
  ///   <c>true</c> if the formula contains the specified intervals; otherwise, <c>false</c>.
  /// </returns>
  public bool Contains(
    IEnumerable<Interval> intervals,
    IntervalMatch match = IntervalMatch.Exact )
  {
    var comparer = ( match == IntervalMatch.Exact ) ? (IComparer<Interval>) s_intervalComparer : s_semitoneComparer;

    return intervals.All( interval => Intervals.IndexOf( interval, comparer ) >= 0 );
  }

  /// <inheritdoc />
  public bool Equals( Formula? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Comparer.IdComparer.Equals( Id, other.Id )
           && Comparer.NameComparer.Equals( Name, other.Name )
           && Intervals.SequenceEqual( other.Intervals );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Formula other && Equals( other );
  }

  /// <summary>Generates a sequence of pitches based on the formula's intervals.</summary>
  /// <param name="root">The root pitch.</param>
  /// <returns> An enumerator for a sequence of pitches.</returns>
  public IEnumerable<Pitch> Generate( Pitch root )
  {
    var intervalCount = Intervals.Count;
    var index = 0;

    while( true )
    {
      var interval = Intervals[index % intervalCount];
      var pitch = root + interval;

      // Do we need to change the pitch's octave?
      var octaveAdd = index / intervalCount;
      if( octaveAdd > 0 )
      {
        pitch += octaveAdd * Pitch.IntervalsPerOctave;
      }

      if( pitch > Pitch.MaxValue )
      {
        yield break;
      }

      yield return pitch;

      ++index;
    }
  }

  /// <summary>Generates a sequence of pitch classes based on the formula's intervals.</summary>
  /// <param name="root">The root pitch class.</param>
  /// <returns> An enumerator for a sequence of pitch classes.</returns>
  public IEnumerable<PitchClass> Generate( PitchClass root )
  {
    // maxIterationCount provides a way to break out of an otherwise infinite
    // loop, as it doesn't make sense to generate more pitch classes than
    // the number of pitches that are supported.
    var maxIterationCount = Pitch.TotalPitchCount;
    var intervalCount = Intervals.Count;
    var index = 0;

    while( maxIterationCount-- >= 0 )
    {
      var interval = Intervals[index % intervalCount];
      var pitchClass = root + interval;
      yield return pitchClass;

      ++index;
    }
  }

  /// <summary>
  ///   Generates a sequence of pitchClasses based on the provided formula's intervals and starting on the provided
  ///   <see cref="PitchClass" />.
  /// </summary>
  /// <param name="root">The root pitch class.</param>
  /// <param name="intervals">The intervals.</param>
  /// <returns> An enumerator for a sequence of pitchClasses.</returns>
  public static IEnumerable<PitchClass> Generate(
    PitchClass root,
    IEnumerable<Interval> intervals )
  {
    Requires.NotNull( intervals );
    return intervals.Select( interval => root + interval );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return Comparer.IdComparer.GetHashCode( Id );
  }

  /// <summary>Gets the relative steps in terms of semitones between the intervals that compose the formula.</summary>
  /// <returns>An array of integral semitone counts.</returns>
  public int[] GetRelativeSteps()
  {
    var steps = new int[Intervals.Count];
    var lastCount = 0;

    for( var i = 1; i < Intervals.Count; i++ )
    {
      var semitoneCount = Intervals[i].SemitoneCount;
      var step = semitoneCount - lastCount;
      steps[i - 1] = step;
      lastCount = semitoneCount;
    }

    // Add last step between the root octave and the
    // last interval
    steps[^1] = 12 - lastCount;
    return steps;
  }

  /// <summary>Parse intervals.</summary>
  /// <param name="formula">The formula.</param>
  /// <returns>.</returns>
  public static Interval[] ParseIntervals( string formula )
  {
    Requires.NotNull( formula );
    return ParseIntervals( formula.AsSpan() );
  }

  /// <summary>Parse intervals.</summary>
  /// <exception cref="FormatException">Thrown when the format of the formula is incorrect.</exception>
  /// <param name="formula">The formula.</param>
  /// <returns>.</returns>
  public static Interval[] ParseIntervals( ReadOnlySpan<char> formula )
  {
    if( formula.IsEmpty )
    {
      return Array.Empty<Interval>();
    }

    var buf = new List<Interval>();
    foreach( var value in formula.Split( ',' ) )
    {
      if( !Interval.TryParse( formula[value.Start.Value..value.End.Value], out var interval ) )
      {
        throw new FormatException( value + " is not a valid interval" );
      }

      buf.Add( interval );
    }

    return buf.ToArray();
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return ToString( NameIntervalsToStringFormat, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "Major".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString( string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. (Currently unused)</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "Major".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      format = NameIntervalsToStringFormat;
    }

    var buf = new StringBuilder();
    foreach( var f in format )
    {
      switch( f )
      {
        case 'N':
          buf.Append( Name );
          break;

        case 'I':
          buf.Append( Intervals );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

#endregion

#region Implementation

  internal static int[] GetRelativeSteps( IList<Interval> intervals )
  {
    Requires.NotNullOrEmpty( intervals );
    var steps = new int[intervals.Count];
    var lastCount = 0;

    for( var i = 1; i < intervals.Count; i++ )
    {
      var semitoneCount = intervals[i].SemitoneCount;
      var step = semitoneCount - lastCount;
      steps[i - 1] = step;
      lastCount = semitoneCount;
    }

    // Add last step between the root octave and the
    // last interval
    steps[^1] = 12 - lastCount;
    return steps;
  }

#endregion
}
