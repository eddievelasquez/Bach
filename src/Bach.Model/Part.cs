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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   An immutable sequential collection of measures containing pitches or pitch chords.
/// </summary>
public sealed class Part
  : IReadOnlyList<Measure>,
    ISpanConsumingParsable<Part>
{
  #region Fields

  private readonly IReadOnlyList<Measure> _measures;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part"/> class.
  /// </summary>
  public Part()
  {
    _measures = Array.Empty<Measure>();
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part"/> class with the specified measures.
  /// </summary>
  /// <param name="measures">The measures to initialize the part with.</param>
  public Part(
    IEnumerable<Measure> measures )
  {
    ArgumentNullException.ThrowIfNull( measures );

    var measureArray = measures.ToArray();

    if( measureArray.Any( measure => measure is null ) )
    {
      throw new ArgumentException( "The measure collection contains a null measure.", nameof( measures ) );
    }

    _measures = Array.AsReadOnly( measureArray );
  }

  #endregion

  #region Properties

  /// <inheritdoc/>
  public int Count => _measures.Count;

  /// <summary>
  ///   Gets the events contained in the part, in measure and event order.
  /// </summary>
  public IEnumerable<IPartEvent> Events => _measures.SelectMany( measure => measure );

  /// <summary>
  ///   Gets each event with its measure and event location.
  /// </summary>
  internal IEnumerable<(PartEventLocation Location, IPartEvent Event)> MeasuresWithLocations()
  {
    for( var measureIndex = 0; measureIndex < _measures.Count; measureIndex++ )
    {
      var measure = _measures[measureIndex];
      for( var eventIndex = 0; eventIndex < measure.Count; eventIndex++ )
      {
        yield return (new PartEventLocation( measureIndex, eventIndex ), measure[eventIndex]);
      }
    }
  }

  /// <summary>
  ///   Gets the pitch classes contained in the part, in event order.
  /// </summary>
  /// <remarks>
  ///   The result preserves duplicate pitch classes and the order of pitch classes within each event.
  /// </remarks>
  public IEnumerable<PitchClass> PitchClasses => _measures.SelectMany( measure => measure.PitchClasses );

  /// <inheritdoc/>
  public Measure this[
    int index ] => _measures[index];

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public IEnumerator<Measure> GetEnumerator()
  {
    return _measures.GetEnumerator();
  }

  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <summary>
  ///   Parses a string representation of a musical part and returns a <see cref="Part"/> object.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Part"/> object.</returns>
  public static Part Parse(
    string s,
    IFormatProvider? provider = null )
  {
    return Parse( s.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a span of characters representing a musical part and returns a <see cref="Part"/> object.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Part"/> object.</returns>
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
  ///   Attempts to parse a string representation of a musical part and returns a boolean indicating success or failure.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="part">The parsed <see cref="Part"/> object.</param>
  /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    [NotNullWhen( true )] out Part? part )
  {
    return TryParse( s.AsSpan(), null, out part );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a musical part and returns a boolean indicating success or failure.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part"/> object.</param>
  /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
    [NotNullWhen( true )] out Part? part )
  {
    return TryParse( s.AsSpan(), provider, out part );
  }

  /// <summary>
  ///   Attempts to parse a span of characters representing a musical part and returns a boolean indicating success or
  ///   failure.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part"/> object.</param>
  /// <returns></returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [MaybeNullWhen( false )] out Part part )
  {
    // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
    return TryParse( span, provider, out part, out var tail ) && tail.IsEmpty;
  }

  /// <summary>
  ///   Attempts to parse a span of characters representing a musical part and returns a boolean indicating success or
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="part">The parsed <see cref="Part"/> object.</param>
  /// <param name="tail">The remaining unparsed portion of the span.</param>
  /// <returns></returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out Part? part,
    out ReadOnlySpan<char> tail )
  {
    tail = ReadOnlySpan<char>.Empty;
    var text = span.Trim().ToString();

    if (text.Length == 0)
    {
      part = new Part();
      return true;
    }

    var partBuilder = new PartBuilder();
    var measureTexts = text.Split('|');

    try
    {
      foreach (var measureText in measureTexts)
      {
        if (string.IsNullOrWhiteSpace(measureText))
        {
          part = null;
          return false;
        }

        var eventTexts = measureText.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (eventTexts.Length == 0)
        {
          part = null;
          return false;
        }

        partBuilder.AddMeasure(measureBuilder =>
        {
          foreach (var eventText in eventTexts)
          {
            var eventSpan = eventText.AsSpan();

            if (eventSpan.StartsWith("!"))
            {
              if (!TryParseExplicitChord(eventSpan[1..], provider, out var explicitChord))
              {
                throw new FormatException($"{eventText} is not a valid part event");
              }

              measureBuilder.Add(explicitChord);
            }
            else if (Pitch.TryParse(eventSpan, provider, out var pitch))
            {
              measureBuilder.Add(pitch);
            }
            else
            {
              throw new FormatException($"{eventText} is not a valid part event");
            }
          }
        });
      }
    }
    catch (FormatException)
    {
      part = null;
      return false;
    }

    part = partBuilder.Build();
    return true;
  }

  /// <summary>
  ///   Attempts to parse an explicitly marked chord token without the leading marker.
  /// </summary>
  /// <param name="span">The chord token without its leading marker.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="chord">The parsed chord, if successful.</param>
  /// <returns>true when the token is a valid chord; otherwise, false.</returns>
  private static bool TryParseExplicitChord(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchChord? chord )
  {
    // If the span is empty, we cannot parse a chord.
    if( span.IsEmpty )
    {
      chord = null;
      return false;
    }

    // Attempt to parse the span as a PitchChord. If successful and the tail is empty, we have a valid chord.
    if( PitchChord.TryParse( span, provider, out chord, out var tail ) && tail.IsEmpty )
    {
      return true;
    }

    // If the span does not represent a valid PitchChord, we attempt to parse it as a root note followed by an optional bass note.
    if( !PitchClass.TryParse( span, provider, out var root, out tail ) )
    {
      chord = null;
      return false;
    }

    // If the tail is empty, we assume a default major chord for the root note.
    if( tail.IsEmpty )
    {
      chord = PitchChord.Create( root, ChordFormula.Major );
      return true;
    }

    // If the tail does not start with a '/', we cannot parse a valid chord with a bass note.
    if( tail[0] != '/' )
    {
      chord = null;
      return false;
    }

    // Attempt to parse the bass note from the tail. If successful and the tail is empty, we have a valid chord with a bass note.
    var bassSpan = tail[1..];
    if( !Pitch.TryParse( bassSpan, provider, out var bass, out var bassTail ) )
    {
      if( !PitchClass.TryParse( bassSpan, provider, out var bassClass, out bassTail ) || !bassTail.IsEmpty )
      {
        chord = null;
        return false;
      }

      bass = Pitch.Create( bassClass, 4 );
    }
    else if( !bassTail.IsEmpty )
    {
      chord = null;
      return false;
    }

    // If we have a valid root and bass note, we create a chord with the root note and determine the inversion based on the bass note.
    var rootPosition = Chord.Create( root, ChordFormula.Major );
    var inversion = rootPosition.IndexOf( bass.PitchClass );
    if( inversion < 0 )
    {
      chord = null;
      return false;
    }

    chord = PitchChord.Create( root, ChordFormula.Major, bass.Octave, inversion );
    return true;
  }

  #endregion
}
