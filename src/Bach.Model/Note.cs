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

  public struct Note: IEquatable<Note>,
                      IComparable<Note>
  {
    #region Data Members

    private static readonly Link[] s_links;

    public static readonly Note C;
    public static readonly Note CSharp;
    public static readonly Note DFlat;
    public static readonly Note D;
    public static readonly Note DSharp;
    public static readonly Note EFlat;
    public static readonly Note E;
    public static readonly Note F;
    public static readonly Note FSharp;
    public static readonly Note GFlat;
    public static readonly Note G;
    public static readonly Note GSharp;
    public static readonly Note AFlat;
    public static readonly Note A;
    public static readonly Note ASharp;
    public static readonly Note BFlat;
    public static readonly Note B;

    private readonly sbyte _tone;
    private readonly sbyte _accidental;
    private readonly sbyte _value;
    private sbyte _reserved;

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
      _value = (sbyte) value;
      _tone = (sbyte) tone;
      _accidental = (sbyte) accidental;
      _reserved = 0;
    }

    #endregion

    #region Properties

    public static AccidentalMode AccidentalMode { get; set; }

    public int NumericValue => _value;
    public Tone Tone => (Tone) _tone;
    public Accidental Accidental => (Accidental) _accidental;

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
      return NumericValue == other.NumericValue;
    }

    #endregion

    #region Public Methods

    public static bool TryParse(string value, out Note note)
    {
      if( string.IsNullOrEmpty(value) )
      {
        note = C;
        return false;
      }

      Tone tone;
      if( !Enum.TryParse(value.Substring(0, 1), true, out tone) )
      {
        note = C;
        return false;
      }

      var accidental = Accidental.Natural;
      if( value.Length > 1 && !AccidentalExtensions.TryParse(value.Substring(1), out accidental) )
      {
        note = C;
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

    private static Note GetNote(int index, bool favorSharps)
    {
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural.Value;
      }

      return favorSharps ? link.Sharp.Value : link.Flat.Value;
    }

    public static Note Get(Tone tone, Accidental accidental = Accidental.Natural)
    {
      int index = (Tone.C.IntervalBetween(tone) + (int) accidental) % s_links.Length;
      if( index < 0 )
      {
        index = s_links.Length + index;
      }

      return GetNote(index, accidental >= Accidental.Natural);
    }

    public Note Add(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int index = (NumericValue + interval) % s_links.Length;
      return GetNote(index, mode == AccidentalMode.FavorSharps);
    }

    public Note Subtract(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      interval %= s_links.Length;
      int index = NumericValue - interval;
      if( index < 0 )
      {
        index = s_links.Length - interval;
      }

      return GetNote(index, mode == AccidentalMode.FavorSharps);
    }

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
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

      public Note? Natural { get; }
      public Note? Sharp { get; }
      public Note? Flat { get; }

      #endregion
    }

    #endregion
  }
}
