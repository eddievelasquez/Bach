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
    #region Constants

    private const ushort TONE_MASK = 7;
    private const ushort TONE_SHIFT = 7;
    private const ushort ACCIDENTAL_MASK = 7;
    private const ushort ACCIDENTAL_SHIFT = 4;
    private const ushort INTERVAL_MASK = 0x0F;
    private const int INTERVAL_COUNT = 12;

    #endregion

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

    private readonly ushort _encoded;

    #endregion

    #region Construction/Destruction

    static Note()
    {
      s_links = new Link[INTERVAL_COUNT];

      C = Create(Tone.C);
      CSharp = Create(Tone.C, Accidental.Sharp);
      DFlat = Create(Tone.D, Accidental.Flat);
      D = Create(Tone.D);
      DSharp = Create(Tone.D, Accidental.Sharp);
      EFlat = Create(Tone.E, Accidental.Flat);
      E = Create(Tone.E);
      F = Create(Tone.F);
      FSharp = Create(Tone.F, Accidental.Sharp);
      GFlat = Create(Tone.G, Accidental.Flat);
      G = Create(Tone.G);
      GSharp = Create(Tone.G, Accidental.Sharp);
      AFlat = Create(Tone.A, Accidental.Flat);
      A = Create(Tone.A);
      ASharp = Create(Tone.A, Accidental.Sharp);
      BFlat = Create(Tone.B, Accidental.Flat);
      B = Create(Tone.B);

      AccidentalMode = AccidentalMode.FavorSharps;
    }

    public Note(Tone tone, Accidental accidental = Accidental.Natural)
    {
      int interval = CalcInterval(tone, accidental);
      _encoded = Encode(interval, tone, accidental);
    }

    private static Note Create(Tone tone, Accidental accidental = Accidental.Natural)
    {
      var note = new Note(tone, accidental);
      Link link = s_links[note.Interval];
      if( link == null )
      {
        link = new Link();
        s_links[note.Interval] = link;
      }

      switch( accidental )
      {
        case Accidental.Flat:
          link.Flat = note;
          break;

        case Accidental.Natural:
          link.Natural = note;
          break;

        case Accidental.Sharp:
          link.Sharp = note;
          break;
      }

      return note;
    }

    #endregion

    #region Properties

    public static AccidentalMode AccidentalMode { get; set; }

    public int Interval => DecodeInterval(_encoded);
    public Tone Tone => DecodeTone(_encoded);
    public Accidental Accidental => DecodeAccidental(_encoded);

    #endregion

    #region IComparable<Note> Members

    public int CompareTo(Note other)
    {
      return Interval - other.Interval;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals(Note other)
    {
      return Interval == other.Interval;
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

      note = new Note(tone, accidental);
      return true;
    }

    public static Note Parse(string value)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);

      Note result;
      if( !TryParse(value, out result) )
      {
        throw new FormatException($"{value} is not a valid note");
      }

      return result;
    }

    [Pure]
    public Note Add(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int newInterval = (Interval + interval) % INTERVAL_COUNT;
      return GetNote(newInterval, mode == AccidentalMode.FavorSharps);
    }

    [Pure]
    public Note Subtract(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      interval %= INTERVAL_COUNT;
      int newInterval = Interval - interval;
      if( newInterval < 0 )
      {
        newInterval = INTERVAL_COUNT - interval;
      }

      return GetNote(newInterval, mode == AccidentalMode.FavorSharps);
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
      return Interval;
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

    #region Implementation

    private static int CalcInterval(Tone tone, Accidental accidental)
    {
      int interval = (Tone.C.IntervalBetween(tone) + (int) accidental) % INTERVAL_COUNT;
      if( interval < 0 )
      {
        interval = INTERVAL_COUNT + interval;
      }

      return interval;
    }

    internal static Note GetNote(int index, bool favorSharps)
    {
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural.Value;
      }

      return favorSharps ? link.Sharp.Value : link.Flat.Value;
    }

    private static ushort Encode(int value, Tone tone, Accidental accidental)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 11);
      Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.C && tone <= Tone.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat
                                                     && accidental <= Accidental.DoubleSharp);
      var encoded =
        (ushort)
          ((((ushort) tone & TONE_MASK) << TONE_SHIFT)
           | (((ushort) (accidental + 2) & ACCIDENTAL_MASK) << ACCIDENTAL_SHIFT) | ((ushort) value & INTERVAL_MASK));
      return encoded;
    }

    private static int DecodeInterval(ushort encoded)
    {
      int result = encoded & INTERVAL_MASK;
      return result;
    }

    private static Tone DecodeTone(ushort encoded)
    {
      int result = (encoded >> TONE_SHIFT) & TONE_MASK;
      return (Tone) result;
    }

    private static Accidental DecodeAccidental(ushort encoded)
    {
      int result = ((encoded >> ACCIDENTAL_SHIFT) & ACCIDENTAL_MASK) - 2;
      return (Accidental) result;
    }

    #endregion

    #region Nested type: Link

    private class Link
    {
      #region Properties

      public Note? Natural { get; set; }
      public Note? Sharp { get; set; }
      public Note? Flat { get; set; }

      #endregion
    }

    #endregion
  }
}
