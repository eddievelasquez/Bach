// Module Name: StringedInstrument.cs
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

namespace Bach.Model.Instruments;

/// <summary>
///   Represents a stringed instrument, such as guitars, basses, ukuleles, etc.
/// </summary>
public sealed class StringedInstrument
  : Instrument,
    IEquatable<StringedInstrument>
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="StringedInstrument"/> class.
  /// </summary>
  /// <param name="definition">The stringed instrument definition.</param>
  /// <param name="positionCount">The number of positions for a string.</param>
  /// <param name="tuning">The stringed instrument's tuning.</param>
  public StringedInstrument(
    StringedInstrumentDefinition definition,
    int positionCount,
    Tuning? tuning = null )
    : base( definition )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( positionCount, 1 );

    Tuning = tuning ?? definition.Tunings.Standard;
    PositionCount = positionCount;
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="StringedInstrument"/> class.
  /// </summary>
  /// <param name="instrumentDefinition">The stringed instrument definition.</param>
  /// <param name="positionCount">The number of positions for a string.</param>
  /// <param name="tuningId">The tuning ID.</param>
  public StringedInstrument(
    StringedInstrumentDefinition instrumentDefinition,
    int positionCount,
    string? tuningId = null )
    : this( instrumentDefinition, positionCount, tuningId != null ? instrumentDefinition.Tunings[tuningId] : null )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="StringedInstrument"/> class.
  /// </summary>
  /// <param name="instrumentDefinitionId">The stringed instrument definition ID.</param>
  /// <param name="positionCount">The number of positions for a string.</param>
  /// <param name="tuningId">The tuning ID.</param>
  public StringedInstrument(
    string instrumentDefinitionId,
    int positionCount,
    string? tuningId = null )
    : this( Registry.StringedInstrumentDefinitions[instrumentDefinitionId], positionCount, tuningId )
  {
  }

  #endregion

  #region Properties

  /// <summary>Gets the stringed instruments definition.</summary>
  /// <value>The definition.</value>
  public new StringedInstrumentDefinition Definition => (StringedInstrumentDefinition) base.Definition;

  /// <summary>Gets the number of positions for a string.</summary>
  /// <value>The number of positions.</value>
  public int PositionCount { get; }

  /// <summary>Gets the stringed instruments tuning.</summary>
  /// <value>The tuning.</value>
  public Tuning Tuning { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Creates a new <see cref="Fingering"/> for the specified string number and position.
  /// </summary>
  /// <param name="stringNumber">The string number.</param>
  /// <param name="position">The position.</param>
  /// <returns>A new <see cref="Fingering"/>.</returns>
  public Fingering CreateFingering(
    int stringNumber,
    int position )
  {
    return new Fingering( this, stringNumber, position );
  }

  /// <summary>
  ///   Creates a new muted <see cref="Fingering"/> for the specified string number.
  /// </summary>
  /// <param name="stringNumber">The string number.</param>
  /// <returns>A new muted <see cref="Fingering"/>.</returns>
  public Fingering CreateMutedFingering(
    int stringNumber )
  {
    return new Fingering( this, stringNumber );
  }

  /// <summary>
  ///   Indicates whether the current object is equal to another object of the same type.
  /// </summary>
  /// <param name="other">The other <see cref="StringedInstrument"/> to compare.</param>
  /// <returns>
  ///   <c>true</c> if the current object is equal to the other object; otherwise, <c>false</c>.
  /// </returns>
  public bool Equals(
    StringedInstrument? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return base.Equals( other ) && Equals( Tuning, other.Tuning ) && PositionCount == other.PositionCount;
  }

  /// <summary>
  ///   Indicates whether the current object is equal to another object.
  /// </summary>
  /// <param name="obj">The other object to compare.</param>
  /// <returns>
  ///   <c>true</c> if the current object is equal to the other object; otherwise, <c>false</c>.
  /// </returns>
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    return obj is StringedInstrument other && Equals( other );
  }

  /// <summary>
  ///   Returns a hash code for the current object.
  /// </summary>
  /// <returns>A hash code for the current object.</returns>
  public override int GetHashCode()
  {
    return HashCode.Combine( base.GetHashCode(), Tuning, PositionCount );
  }

  /// <summary>Get a chord fingering in the starting position with an optional position span.</summary>
  /// <param name="chord">The chord.</param>
  /// <param name="startPosition">The starting position.</param>
  /// <param name="positionSpan">(Optional) The position span.</param>
  /// <exception cref="ArgumentNullException">Thrown when the chord is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the position span is greater than the number of positions in
  ///   a string.
  /// </exception>
  /// <returns>An enumerator for a fingering sequence for the chord.</returns>
  public IEnumerable<Fingering> GetFingering(
    Chord chord,
    int startPosition,
    int positionSpan = 4 )
  {
    ArgumentNullException.ThrowIfNull( chord );
    ArgumentOutOfRangeException.ThrowIfLessThan( positionSpan, 2 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( startPosition + positionSpan, PositionCount );

    // Always start at the lowest string
    var startString = Definition.StringCount;

    // Find the chord pitch that is closest to starting string and position
    var startPitch = Tuning[startString] + startPosition;

    // Adjust the octave if necessary. Use chromatic pitch height (not spelling) since the goal is to
    // detect whether the starting pitch class sounds lower than the chord's bass.

    // Use chromatic pitch height (not spelling) since the goal is to detect whether the starting
    // pitch class sounds higher than the chord's bass.
    if( startPitch.PitchClass.EnharmonicCompareTo( chord.Bass ) > 0 )
    {
      startPitch = startPitch.Transpose( Interval.Octave );
    }

    // Materialize every chord pitch from the starting pitch through the highest string's pitch at the
    // end of the requested position span.
    var pitches = chord.Render( startPitch.Octave )
                       .ToArray();
    var pitchIndex = 0;

    // Go through all the strings
    for( var currentString = startString; currentString >= 1; --currentString )
    {
      // Get the fingering for the current string. This will either be a pitch in the chord or a muted string.
      var fingering = GetChordFingering( currentString );
      yield return fingering;

      // If the string is not muted, then move to the next pitch in the chord for the next string.
      // If the string is muted, then keep the same pitch for the next string.
      if( !fingering.IsMuted && pitchIndex < pitches.Length - 1 )
      {
        ++pitchIndex;
      }
    }

    yield break;

    Fingering GetChordFingering(
      int stringNumber )
    {
      // Look for all the string pitches that are within the string's span
      var low = GetPitchAt( stringNumber, startPosition );
      var high = low + positionSpan;

      // Find the first pitch in the chord that is within the string's span
      while( pitchIndex < pitches.Length - 1 && pitches[pitchIndex] < low )
      {
        ++pitchIndex;
      }

      var current = pitches[pitchIndex];

      // If the current pitch is higher than the high end of the span, then mute the string
      if( current > high )
      {
        return CreateMutedFingering( stringNumber );
      }

      // Otherwise, return the fingering for the current pitch
      var position = current - low + startPosition;
      return CreateFingering( stringNumber, position );
    }
  }

  /// <summary>Get a scale fingering in the starting position with an optional position span.</summary>
  /// <param name="scale">The scale.</param>
  /// <param name="startPosition">The starting position.</param>
  /// <param name="positionSpan">(Optional) The position span.</param>
  /// <exception cref="ArgumentNullException">Thrown when the scale is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the position span is greater than the number of positions in
  ///   a string.
  /// </exception>
  /// <returns>An enumerator for a fingering sequence for the scale.</returns>
  public IEnumerable<Fingering> GetFingering(
    Scale scale,
    int startPosition,
    int positionSpan = 4 )
  {
    ArgumentNullException.ThrowIfNull( scale );
    ArgumentOutOfRangeException.ThrowIfLessThan( positionSpan, 2 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( startPosition + positionSpan, PositionCount );

    // Always start at the lowest string
    var startString = Definition.StringCount;

    // Find the scale pitch that is closest to starting string and position
    var startPitch = Tuning[startString] + startPosition;

    // Adjust the octave if necessary. Use chromatic pitch height (not spelling) since the goal is to
    // detect whether the starting pitch class sounds lower than the scale's root.
    var octave = startPitch.Octave;

    if( startPitch.PitchClass.EnharmonicCompareTo( scale.Root ) < 0 )
    {
      --octave;
    }

    // Fingering proceeds from the lowest string to the highest string. Therefore, the final pitch
    // that can be used is the highest string's pitch at the end of the requested position span.
    // Materialize every scale pitch from the starting pitch through that endpoint so the loop can
    // advance across the gaps between adjacent strings. Those gaps depend on the tuning, so the
    // required array length cannot be calculated from string count and position span alone.
    var endPitch = GetPitchAt( 1, startPosition + positionSpan );

    var pitches = scale.Render( octave )
                       .SkipWhile( pitch => pitch < startPitch )
                       .TakeWhile( pitch => pitch.EnharmonicCompareTo( endPitch ) <= 0 )
                       .ToArray();
    var pitchIndex = 0;

    // Go through all the strings
    for( var currentString = startString; currentString >= 1; --currentString )
    {
      // The lowest pitch that can be used on this string is the pitch at the starting position
      var low = GetPitchAt( currentString, startPosition );

      // Keep each pitch within this string's position span. When the next string starts within
      // that span, stop one pitch below its starting pitch so that the next string owns that pitch.
      // For the highest string, GetPitchAt returns Pitch.MaxValue because there is no next string.
      var highThisString = low + positionSpan;
      var lowNextString = GetPitchAt( currentString - 1, startPosition ) - 1;
      var high = highThisString <= lowNextString ? highThisString : lowNextString;

      while( pitchIndex < pitches.Length )
      {
        var current = pitches[pitchIndex];

        // If the current pitch is higher than the high end of the span, then we will move on to
        // the next string. Stop processing this string.
        if( current.EnharmonicCompareTo( high ) > 0 )
        {
          break;
        }

        // Otherwise, return the fingering for the current pitch
        var position = current - low + startPosition;
        var fingering = CreateFingering( currentString, position );
        yield return fingering;

        ++pitchIndex;
      }
    }
  }

  #endregion

  #region Implementation

  private Pitch GetPitchAt(
    int @string,
    int position )
  {
    // If the string number is out of range, return Pitch.MaxValue to indicate
    // that there is no pitch for that string. Using Pitch.MaxValue ensures that
    // the loop behaves correctly even when the string number is out of range.
    if( @string < 1 || @string > Definition.StringCount )
    {
      return Pitch.MaxValue;
    }

    return Tuning[@string] + position;
  }

  #endregion
}
