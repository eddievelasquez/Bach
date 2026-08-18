// Module Name: Part.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bach.Model.Internal;

/// <summary>A sequential collection of musical events that can contain either pitches or pitch chords.</summary>
public sealed class Part
  : IList<IPartEvent>,
    ISpanConsumingParsable<Part>
{
  #region Fields

  private readonly List<IPartEvent> _events;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class.
  /// </summary>
  public Part()
  {
    _events = [];
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class with the specified collection of part events.
  /// </summary>
  /// <param name="events">The collection of part events to initialize the part with.</param>
  public Part(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );
    _events = [.. events];
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class with the specified initial capacity.
  /// </summary>
  /// <param name="capacity">The initial capacity of the part.</param>
  public Part(
    int capacity )
  {
    _events = new List<IPartEvent>( capacity );
  }

  #endregion

  #region Properties

  /// <inheritdoc />
  public int Count => _events.Count;

  /// <inheritdoc />
  public bool IsReadOnly => false;

  /// <inheritdoc />
  public IPartEvent this[
    int index ]
  {
    get => _events[index];
    set
    {
      ArgumentNullException.ThrowIfNull( value );
      _events[index] = value;
    }
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a musical event to the part.
  /// </summary>
  /// <param name="partEvent">The musical event to add.</param>
  public void Add(
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );
    _events.Add( partEvent );
  }

  /// <inheritdoc />
  public void Clear()
  {
    _events.Clear();
  }

  /// <inheritdoc />
  public bool Contains(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.Contains( item );
  }

  /// <inheritdoc />
  public void CopyTo(
    IPartEvent[] array,
    int arrayIndex )
  {
    ArgumentNullException.ThrowIfNull( array );
    _events.CopyTo( array, arrayIndex );
  }

  /// <inheritdoc />
  public IEnumerator<IPartEvent> GetEnumerator()
  {
    return _events.GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public int IndexOf(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.IndexOf( item );
  }

  /// <inheritdoc />
  public void Insert(
    int index,
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    _events.Insert( index, item );
  }

  /// <inheritdoc />
  public bool Remove(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.Remove( item );
  }

  /// <inheritdoc />
  public void RemoveAt(
    int index )
  {
    _events.RemoveAt( index );
  }

  #endregion

  /// <summary>
  /// Parses a string representation of a musical part and returns a <see cref="Part" /> object.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Part" /> object.</returns>
  public static Part Parse(
    string s,
    IFormatProvider? provider = null )
  {
    return Parse( s.AsSpan(), provider );
  }

  /// <summary>
  /// Parses a span of characters representing a musical part and returns a <see cref="Part" /> object.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Part" /> object.</returns>
  /// <exception cref="FormatException"></exception>
  public static Part Parse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider )
  {
    return TryParse( span, provider, out var part )
      ? part
      : throw new FormatException( $"{span} is not a valid part" );
  }

  /// <summary>
  /// Attempts to parse a string representation of a musical part and returns a boolean indicating success or failure.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="part">The parsed <see cref="Part" /> object.</param>
  /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    [NotNullWhen(true)] out Part? part )
  {
    return TryParse(s.AsSpan(), null, out part);
  }

  /// <summary>
  /// Attempts to parse a string representation of a musical part and returns a boolean indicating success or failure.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part" /> object.</param>
  /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
    [NotNullWhen(true)] out Part? part )
  {
    return TryParse(s.AsSpan(), provider, out part);
  }

  /// <summary>
  /// Attempts to parse a span of characters representing a musical part and returns a boolean indicating success or
  /// failure.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part" /> object.</param>
  /// <returns></returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out Part part )
  {
    // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
    return TryParse(span, provider, out part, out var tail) && tail.IsEmpty;
  }

  /// <summary>
  /// Attempts to parse a span of characters representing a musical part and returns a boolean indicating success or
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part" /> object.</param>
  /// <param name="tail">The remaining unparsed portion of the span.</param>
  /// <returns></returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen(true)] out Part? part,
    out ReadOnlySpan<char> tail )
  {
    tail = span.TrimStart();

    // If the span is empty after trimming, we return an empty part
    if( tail.IsEmpty )
    {
      part = [];
      return true;
    }

    // Count the number of commas in the span to determine how many parts we have.
    var sepCount = span.Count( ',' );

    // Allocate a stack-allocated array of ranges to hold the start and end indices of each pitch in the span.
    Span<Range> ranges = stackalloc Range[sepCount + 1];
    var rangeCount = tail.Split( ranges, ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );
    var partEvents = new List<IPartEvent>( rangeCount );

    // Parse each pitch or PitchChord in the span and add it to the part events list.
    for( var i = 0; i < rangeCount; i++ )
    {
      var currentSpan = tail[ranges[i]];

      // Try to parse the current span as a PitchChord first, and if that fails, try to parse it as a Pitch.
      if( PitchChord.TryParse( currentSpan, provider, out var pitchChord ))
      {
        partEvents.Add( pitchChord );
        continue;
      }

      if( Pitch.TryParse( currentSpan, provider, out var pitch ) )
      {
        partEvents.Add( pitch );
        continue;
      }

      // If neither parsing attempt succeeded, we return false to indicate that the parsing failed.
      part = null;
      return false;
    }

    // Update the tail to point to the remaining unparsed portion of the span after the last Pitch or PitchChord.
    if( rangeCount > 0 )
    {
      tail = tail[ranges[rangeCount - 1].End..];
    }

    part = [.. partEvents];

    return true;
  }
}
