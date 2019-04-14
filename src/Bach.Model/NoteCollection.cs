// Module Name: NoteCollection.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Internal;

  /// <summary>Collection of notes.</summary>
  public class NoteCollection
    : IReadOnlyList<Note>,
      IEquatable<NoteCollection>
  {
    #region Data Members

    private readonly Note[] _notes;

    #endregion

    #region Constructors

    /// <inheritdoc />
    public NoteCollection(IEnumerable<Note> notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);
      _notes = notes.ToArray();
    }

    /// <inheritdoc />
    public NoteCollection(Note[] notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);
      _notes = notes;
    }

    #endregion

    #region IEquatable<NoteCollection> Members

    /// <inheritdoc />
    public bool Equals(NoteCollection other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }

      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      return _notes.SequenceEqual(other._notes);
    }

    #endregion

    #region IReadOnlyList<Note> Members

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<Note> GetEnumerator() => ( (IEnumerable<Note>)_notes ).GetEnumerator();

    /// <inheritdoc />
    public int Count => _notes.Length;

    /// <inheritdoc />
    public Note this[int index] => _notes[index];

    #endregion

    #region Public Methods

    /// <summary>Attempts to parse a Note collection from the given string.</summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="notes">[out] The note collection.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string value,
                                out NoteCollection notes)
    {
      if( string.IsNullOrEmpty(value) )
      {
        notes = null;
        return false;
      }

      var tmp = new List<Note>();

      foreach( string s in value.Split(new[]{ ',', ' ' }, StringSplitOptions.RemoveEmptyEntries) )
      {
        if( !Note.TryParse(s, out Note note) )
        {
          notes = null;
          return false;
        }

        tmp.Add(note);
      }

      notes = new NoteCollection(tmp.ToArray());
      return true;
    }

    /// <summary>Parses the provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a note collection.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A NoteCollection.</returns>
    public static NoteCollection Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if( !TryParse(value, out NoteCollection notes) )
      {
        throw new FormatException($"{value} contains invalid notes");
      }

      return notes;
    }

    public int IndexOf(Note note) => Array.IndexOf(_notes, note);

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString() => string.Join(",", _notes);

    /// <inheritdoc />
    public override bool Equals(object other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }

      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      if( other.GetType() != GetType() )
      {
        return false;
      }

      return Equals((NoteCollection)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        var hash = 17;
        foreach( Note note in _notes )
        {
          hash = ( hash * 23 ) + note.GetHashCode();
        }

        return hash;
      }
    }

    #endregion
  }
}
