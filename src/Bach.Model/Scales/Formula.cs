// Module Name: Formula.cs
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
using System.Text;

namespace Bach.Model.Scales;

/// <summary>
///   Provides the shared identity, name, and ordered interval pattern for chord and scale formulas.
///   Derived formulas use these intervals to define their pitch-class content.
/// </summary>
public abstract class Formula
  : INamedObject,
    IEquatable<Formula>,
    IFormattable
{
  #region Nested Types

  private sealed class IntervalComparer: IComparer<Interval>
  {
    #region Public Methods

    public int Compare(
      Interval lhs,
      Interval rhs )
    {
      return lhs.CompareTo( rhs );
    }

    #endregion
  }

  private sealed class SemitoneCountIntervalComparer: IComparer<Interval>
  {
    #region Public Methods

    public int Compare(
      Interval lhs,
      Interval rhs )
    {
      return lhs.SemitoneCount - rhs.SemitoneCount;
    }

    #endregion
  }

  #endregion

  #region Constants

  private const string NAME_INTERVALS_TO_STRING_FORMAT = "N: I";

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
    ArgumentException.ThrowIfNullOrWhiteSpace( id );
    ArgumentException.ThrowIfNullOrWhiteSpace( name );
    ArgumentNullException.ThrowIfNull( intervals );
    ArgumentOutOfRangeException.ThrowIfZero( intervals.Length );

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
    IComparer<Interval> comparer = match == IntervalMatch.Exact ? s_intervalComparer : s_semitoneComparer;
    return intervals.All( interval => Intervals.IndexOf( interval, comparer ) >= 0 );
  }

  /// <inheritdoc/>
  public bool Equals(
    Formula? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    // Formulas are entity objects considered equal if they have the same id, as the id is unique for each formula.
    return Comparer.IdComparer.Equals( Id, other.Id );
  }

  /// <inheritdoc/>
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Formula other && Equals( other );
  }

  /// <summary>
  ///   Generates a sequence of pitches based on the formula's intervals, starting from the provided root pitch.
  /// </summary>
  /// <typeparam name="TPitch">The type of pitch to generate.</typeparam>
  /// <param name="root">The root pitch.</param>
  /// <returns>An enumerator for a sequence of pitches.</returns>
  /// <exception cref="ArgumentException">Thrown when the root pitch is not supported.</exception>
  public IEnumerable<TPitch> Generate<TPitch>(
    TPitch root )
  {
    return root switch
    {
      Pitch pitch           => (IEnumerable<TPitch>) Generate( pitch ),
      PitchClass pitchClass => (IEnumerable<TPitch>) Generate( pitchClass ),
      _                     => throw new ArgumentException( "Unsupported pitch type", nameof( root ) )
    };
  }

  /// <summary>
  /// Generates a sequence of pitches based on the formula's intervals, starting from the provided root pitch.
  /// </summary>
  /// <param name="root">The root pitch.</param>
  /// <returns>An enumerator for a sequence of pitches.</returns>
  public IEnumerable<Pitch> Generate(
    Pitch root )
  {
    var currentRoot = root;

    while( true )
    {
      foreach( var interval in Intervals )
      {
        // Stop when the next transposition is outside the supported pitch range.
        if( !Pitch.TryTranspose( currentRoot, interval, out var pitch ) )
        {
          yield break;
        }

        yield return pitch;
      }

      // Formula intervals repeat from the root one octave higher on the next cycle.
      if( !Pitch.TryTranspose( currentRoot, Interval.Octave, out currentRoot ) )
      {
        yield break;
      }
    }
  }

  /// <summary>Generates a sequence of pitch classes based on the formula's intervals.</summary>
  /// <param name="root">The root pitch class.</param>
  /// <returns> An enumerator for a sequence of pitch classes.</returns>
  public IEnumerable<PitchClass> Generate(
    PitchClass root )
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
  ///   Generates pitch classes from a root pitch class and an interval sequence.
  /// </summary>
  /// <param name="root">The root pitch class.</param>
  /// <param name="intervals">The intervals to apply to the root.</param>
  /// <returns>The generated pitch classes.</returns>
  public static IEnumerable<PitchClass> Generate(
    PitchClass root,
    IEnumerable<Interval> intervals )
  {
    ArgumentNullException.ThrowIfNull( intervals );
    return intervals.Select( interval => root + interval );
  }

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    return Comparer.IdComparer.GetHashCode( Id );
  }

  /// <summary>Parses intervals from a comma-separated string.</summary>
  /// <param name="formula">The string that contains the intervals.</param>
  /// <returns>The parsed intervals, or an empty array if the string is empty.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="formula"/> is null.</exception>
  /// <exception cref="FormatException">Thrown when the string contains an invalid interval.</exception>
  public static Interval[] ParseIntervals(
    string formula )
  {
    ArgumentNullException.ThrowIfNull( formula );
    return ParseIntervals( formula.AsSpan() );
  }

  /// <summary>Parses intervals from a comma-separated character span.</summary>
  /// <param name="formula">The span that contains the intervals.</param>
  /// <returns>The parsed intervals, or an empty array if the span is empty.</returns>
  /// <exception cref="FormatException">Thrown when the span contains an invalid interval.</exception>
  public static Interval[] ParseIntervals(
    ReadOnlySpan<char> formula )
  {
    if( formula.IsEmpty )
    {
      return [];
    }

    var buf = new List<Interval>();

    // There can be at most (n / 2) + 1 intervals in an n-character string formula
    // in which intervals are separated by commas
    Span<Range> ranges = stackalloc Range[formula.Length / 2 + 1];
    var count = formula.Split( ranges, ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );

    for( var i = 0; i < count; ++i )
    {
      var value = formula[ranges[i]];

      if( !Interval.TryParse( value, out var interval ) )
      {
        throw new FormatException( value.ToString() + " is not a valid interval" );
      }

      buf.Add( interval );
    }

    return [.. buf];
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return ToString( NAME_INTERVALS_TO_STRING_FORMAT, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula"/> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "Major".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string format )
  {
    return ToString( format, null! );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula"/> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. Not used.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "Major".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      format = NAME_INTERVALS_TO_STRING_FORMAT;
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
}
