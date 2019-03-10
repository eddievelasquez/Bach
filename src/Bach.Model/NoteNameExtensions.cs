//
// Module Name: NoteNameExtensions.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  /// <summary>
  /// Provides extension to for <see cref="NoteName"/>.
  /// </summary>
  public static class NoteNameExtensions
  {
    private const int NoteNameCount = (int)NoteName.B + 1;

    private static readonly int[] s_intervals =
    {
      2, // C-D
      2, // D-E
      1, // E-F
      2, // F-G
      2, // G-A
      2, // A-B
      1 // B-C
    };

    /// <summary>
    /// Returns the next <see cref="NoteName"/>.
    /// </summary>
    /// <param name="noteName">The starting note name.</param>
    /// <returns>The next note name.</returns>
    public static NoteName Next(this NoteName noteName) => Add(noteName, 1);

    /// <summary>Adds a number of steps to a note name.</summary>
    /// <param name="noteName">The starting note name.</param>
    /// <param name="steps">The number of steps to add.</param>
    /// <returns>A NoteName.</returns>
    public static NoteName Add(this NoteName noteName,
                               int steps)
    {
      var result = (NoteName)ArrayExtensions.WrapIndex(NoteNameCount, (int)noteName + steps);
      return result;
    }

    /// <summary>
    /// Returns the previous <see cref="NoteName"/>.
    /// </summary>
    /// <param name="noteName">The starting note name.</param>
    /// <returns>The previous note name.</returns>
    public static NoteName Previous(this NoteName noteName) => Add(noteName, -1);

    /// <summary>Subtracts a number of steps from a note name.</summary>
    /// <param name="noteName">The starting note name.</param>
    /// <param name="steps">The number of steps to subtract.</param>
    /// <returns>A NoteName.</returns>
    public static NoteName Subtract(this NoteName noteName,
                                    int steps)
      => Add(noteName, -steps);

    /// <summary>
    /// Calculates the semitone distance between two note names.
    /// </summary>
    /// <param name="noteName">The first note name</param>
    /// <param name="end">The last not name</param>
    /// <returns>The distance in semitones between the two note names.</returns>
    public static int SemitonesBetween(this NoteName noteName,
                                       NoteName end)
    {
      var semitones = 0;
      while( noteName != end )
      {
        semitones += s_intervals[(int)noteName];
        noteName = (NoteName)s_intervals.WrapIndex((int)noteName + 1);
      }

      return semitones;
    }
  }
}
