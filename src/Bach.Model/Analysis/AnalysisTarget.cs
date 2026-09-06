// Module Name: AnalysisTarget.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Identifies the part event or pitch examined by a tonal analysis item.
/// </summary>
public sealed record AnalysisTarget
{
  #region Constructors

  private AnalysisTarget(
    IPartEvent partEvent,
    Pitch? pitch )
  {
    PartEvent = partEvent;
    Pitch = pitch;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the part event examined by the analysis item.
  /// </summary>
  public IPartEvent PartEvent { get; }

  /// <summary>
  ///   Gets the pitch when the analysis item examines one concrete pitch.
  /// </summary>
  public Pitch? Pitch { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Creates a target for a part event.
  /// </summary>
  /// <param name="partEvent">The part event examined by the analysis item.</param>
  /// <returns>A target for the part event.</returns>
  public static AnalysisTarget ForEvent(
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );

    return partEvent is Pitch { IsValid: true } pitch
      ? new AnalysisTarget( partEvent, pitch )
      : new AnalysisTarget( partEvent, null );
  }

  /// <summary>
  ///   Creates a target for a concrete pitch.
  /// </summary>
  /// <param name="pitch">The pitch examined by the analysis item.</param>
  /// <returns>A target for the pitch.</returns>
  /// <exception cref="ArgumentException">
  ///   Thrown when the pitch is not valid.
  /// </exception>
  public static AnalysisTarget ForPitch(
    Pitch pitch )
  {
    if( !pitch.IsValid )
    {
      throw new ArgumentException( "The pitch must be valid.", nameof( pitch ) );
    }

    return new AnalysisTarget( pitch, pitch );
  }

  #endregion
}
