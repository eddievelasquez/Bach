// Module Name: Constants.cs
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

namespace Bach.Model.Internal;

/// <summary>
///   Contains constants used throughout the Bach.Model library.
/// </summary>
internal static class Constants
{
  #region Constants

  /// <summary>
  ///   The total number of note names. C, D, E, F, G, A, B
  /// </summary>
  public const int NoteNameCount = 7;

  /// <summary>
  ///   The total number of semitones in an octave. C, C#, D, D#, E, F, F#, G, G#, A, A#, B
  /// </summary>
  public const int OctaveSemitoneCount = 12;

  /// <summary>
  ///   The minimum size of a scale step is 1 semitone.
  /// </summary>
  public const int MinimumScaleStepSize = 1;

  /// <summary>
  ///   The maximum size of a scale step is 4 semitones.
  /// </summary>
  public const int MaximumScaleStepSize = 4;

  /// <summary>
  ///   In Western music, the minimum number of steps in a scale is 5, which corresponds to the pentatonic scale.
  /// </summary>
  public const int MinimumScaleStepCount = 5;

  /// <summary>
  ///   The maximum number of steps in a scale is equal to the number of semitones in an octave.
  /// </summary>
  public const int MaximumScaleStepCount = OctaveSemitoneCount;

  #endregion
}
