//  
// Module Name: Note.cs
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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;
  using System.Diagnostics.Contracts;

  public sealed class Note: IEquatable<Note>,
                            IComparable<Note>
  {
    #region Data Members

    private static readonly Link[] s_links;

    public static Note C;
    public static Note CSharp;
    public static Note DFlat;
    public static Note D;
    public static Note DSharp;
    public static Note EFlat;
    public static Note E;
    public static Note F;
    public static Note FSharp;
    public static Note GFlat;
    public static Note G;
    public static Note GSharp;
    public static Note AFlat;
    public static Note A;
    public static Note ASharp;
    public static Note BFlat;
    public static Note B;

    #endregion

    #region Construction/Destruction

    static Note()
    {
      // Create notes
      C = new Note(0, Tone.C, Accidental.Natural);
      CSharp = new Note(1, Tone.C, Accidental.Sharp);
      DFlat = new Note(1, Tone.D, Accidental.Flat);
      D = new Note(2, Tone.D, Accidental.Natural);
      DSharp = new Note(3, Tone.D, Accidental.Sharp);
      EFlat = new Note(3, Tone.E, Accidental.Flat);
      E = new Note(4, Tone.E, Accidental.Natural);
      F = new Note(5, Tone.F, Accidental.Natural);
      FSharp = new Note(6, Tone.F, Accidental.Sharp);
      GFlat = new Note(6, Tone.G, Accidental.Flat);
      G = new Note(7, Tone.G, Accidental.Natural);
      GSharp = new Note(8, Tone.G, Accidental.Sharp);
      AFlat = new Note(8, Tone.A, Accidental.Flat);
      A = new Note(9, Tone.A, Accidental.Natural);
      ASharp = new Note(10, Tone.A, Accidental.Sharp);
      BFlat = new Note(10, Tone.B, Accidental.Flat);
      B = new Note(11, Tone.B, Accidental.Natural);

      // Link all notes
      s_links = new Link[12];
      s_links[0] = new Link(C);
      s_links[1] = new Link(DFlat, CSharp);
      s_links[2] = new Link(D);
      s_links[3] = new Link(EFlat, DSharp);
      s_links[4] = new Link(E);
      s_links[5] = new Link(F);
      s_links[6] = new Link(GFlat, FSharp);
      s_links[7] = new Link(G);
      s_links[8] = new Link(AFlat, GSharp);
      s_links[9] = new Link(A);
      s_links[10] = new Link(BFlat, ASharp);
      s_links[11] = new Link(B);

      AccidentalMode = AccidentalMode.FavorSharps;
    }

    private Note(int value, Tone tone, Accidental accidental)
    {
      NumericValue = value;
      Tone = tone;
      Accidental = accidental;
    }

    #endregion

    #region Properties

    public static AccidentalMode AccidentalMode { get; set; }
    public int NumericValue { get; }
    public Tone Tone { get; }
    public Accidental Accidental { get; }

    #endregion

    #region IComparable<Note> Members

    public int CompareTo(Note other)
    {
      return NumericValue - other.NumericValue;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals(Note other)
    {
      if( ReferenceEquals(null, other) )
      {
        return false;
      }
      if( ReferenceEquals(this, other) )
      {
        return true;
      }
      return NumericValue == other.NumericValue;
    }

    #endregion

    #region Public Methods

    public static bool TryParse(string value, out Note note)
    {
      if( string.IsNullOrEmpty(value) )
      {
        note = null;
        return false;
      }

      Tone tone;
      if( !Enum.TryParse(value.Substring(0, 1), true, out tone) )
      {
        note = null;
        return false;
      }

      var accidental = Accidental.Natural;
      if( value.Length > 1 && !AccidentalExtensions.TryParse(value.Substring(1), out accidental) )
      {
        note = null;
        return false;
      }

      note = Get(tone, accidental);
      return true;
    }

    public static Note Parse(string value)
    {
      Note result;
      if( !TryParse(value, out result) )
      {
        throw new ArgumentException($"{value} is not a valid note");
      }

      return result;
    }

    public static Note Get(Tone tone, Accidental accidental = Accidental.Natural)
    {
      int index = (Tone.C.IntervalBetween(tone) + (int) accidental) % s_links.Length;
      if( index < 0 )
      {
        index = s_links.Length + index;
      }

      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural;
      }

      return accidental < Accidental.Natural ? link.Flat : link.Sharp;
    }

    public Note Add(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int index = (NumericValue + interval) % s_links.Length;
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural;
      }

      return mode == AccidentalMode.FavorSharps ? link.Sharp : link.Flat;
    }

    public Note Subtract(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      interval %= s_links.Length;
      int index = NumericValue - interval;
      if( index < 0 )
      {
        index = s_links.Length - interval;
      }

      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural;
      }

      return mode == AccidentalMode.FavorSharps ? link.Sharp : link.Flat;
    }

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
      }

      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj.GetType() == GetType() && Equals((Note) obj);
    }

    public override int GetHashCode()
    {
      return NumericValue;
    }

    public override string ToString()
    {
      return $"{Tone}{Accidental.ToSymbol()}";
    }

    #endregion

    #region Operators

    public static bool operator ==(Note left, Note right) => Equals(left, right);

    public static bool operator !=(Note left, Note right) => !Equals(left, right);

    public static bool operator >(Note left, Note right) => left.CompareTo(right) > 0;

    public static bool operator <(Note left, Note right) => left.CompareTo(right) < 0;

    public static bool operator >=(Note left, Note right) => left.CompareTo(right) >= 0;

    public static bool operator <=(Note left, Note right) => left.CompareTo(right) <= 0;

    public static Note operator +(Note note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(interval, AccidentalMode);
    }

    public static Note operator ++(Note note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(1, AccidentalMode);
    }

    public static Note operator -(Note note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(interval, AccidentalMode);
    }

    public static Note operator --(Note note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(1, AccidentalMode);
    }

    #endregion

    #region Nested type: Link

    private class Link
    {
      #region Construction/Destruction

      public Link(Note natural)
      {
        Natural = natural;
      }

      public Link(Note flat, Note sharp)
      {
        Flat = flat;
        Sharp = sharp;
      }

      #endregion

      #region Properties

      public Note Natural { get; }
      public Note Sharp { get; }
      public Note Flat { get; }

      #endregion
    }

    #endregion
  }
}