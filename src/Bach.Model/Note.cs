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
  using System.Diagnostics;

  /// <summary>
  /// A Note represents a combination of a <see cref="NoteName"/>
  /// and an optional <see cref="Accidental"/> following the
  /// English naming convention for the 12 note
  /// chromatic scale.
  /// </summary>
  public struct Note : IEquatable<Note>,
                      IComparable<Note>
  {
    #region Constants

    private const ushort TONE_NAME_MASK = 7;
    private const ushort TONE_NAME_SHIFT = 7;
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

      C = Create(NoteName.C);
      CSharp = Create(NoteName.C, Accidental.Sharp);
      DFlat = Create(NoteName.D, Accidental.Flat);
      D = Create(NoteName.D);
      DSharp = Create(NoteName.D, Accidental.Sharp);
      EFlat = Create(NoteName.E, Accidental.Flat);
      E = Create(NoteName.E);
      F = Create(NoteName.F);
      FSharp = Create(NoteName.F, Accidental.Sharp);
      GFlat = Create(NoteName.G, Accidental.Flat);
      G = Create(NoteName.G);
      GSharp = Create(NoteName.G, Accidental.Sharp);
      AFlat = Create(NoteName.A, Accidental.Flat);
      A = Create(NoteName.A);
      ASharp = Create(NoteName.A, Accidental.Sharp);
      BFlat = Create(NoteName.B, Accidental.Flat);
      B = Create(NoteName.B);

      AccidentalMode = AccidentalMode.FavorSharps;
    }

    public Note(NoteName noteName,
                Accidental accidental = Accidental.Natural)
    {
      int interval = CalcInterval(noteName, accidental);
      _encoded = Encode(interval, noteName, accidental);
    }

    private static Note Create(NoteName noteName,
                               Accidental accidental = Accidental.Natural)
    {
      var tone = new Note(noteName, accidental);
      Link link = s_links[tone.Interval];
      if (link == null)
      {
        link = new Link();
        s_links[tone.Interval] = link;
      }

      switch (accidental)
      {
        case Accidental.Flat:
          link.Flat = tone;
          break;

        case Accidental.Natural:
          link.Natural = tone;
          break;

        case Accidental.Sharp:
          link.Sharp = tone;
          break;
      }

      return tone;
    }

    #endregion

    #region Properties

    public static AccidentalMode AccidentalMode { get; set; }

    public int Interval => DecodeInterval(_encoded);

    public NoteName NoteName => DecodeToneName(_encoded);

    public Accidental Accidental => DecodeAccidental(_encoded);

    #endregion

    #region IComparable<Pitch> Members

    public int CompareTo(Note other) => Interval - other.Interval;

    #endregion

    #region IEquatable<Pitch> Members

    public bool Equals(Note other) => Interval == other.Interval;

    #endregion

    #region Public Methods

    public static bool TryParse(string value,
                                out Note note)
    {
      if (string.IsNullOrEmpty(value))
      {
        note = C;
        return false;
      }

      if (!Enum.TryParse(value.Substring(0, 1), true, out NoteName toneName))
      {
        note = C;
        return false;
      }

      var accidental = Accidental.Natural;
      if (value.Length > 1 && !AccidentalExtensions.TryParse(value.Substring(1), out accidental))
      {
        note = C;
        return false;
      }

      note = new Note(toneName, accidental);
      return true;
    }

    public static Note Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if (!TryParse(value, out Note result))
      {
        throw new FormatException($"{value} is not a valid note");
      }

      return result;
    }

    public Note Add(int interval,
                    AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int newInterval = (Interval + interval) % INTERVAL_COUNT;
      return GetNote(newInterval, mode == AccidentalMode.FavorSharps);
    }

    public Note Subtract(int interval,
                         AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      interval %= INTERVAL_COUNT;
      int newInterval = Interval - interval;
      if (newInterval < 0)
      {
        newInterval = INTERVAL_COUNT - interval;
      }

      return GetNote(newInterval, mode == AccidentalMode.FavorSharps);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      return obj.GetType() == GetType() && Equals((Note)obj);
    }

    public override int GetHashCode() => Interval;

    public override string ToString() => $"{NoteName}{Accidental.ToSymbol()}";

    #endregion

    #region Operators

    public static bool operator ==(Note left,
                                  Note right) => Equals(left, right);

    public static bool operator !=(Note left,
                                  Note right) => !Equals(left, right);

    public static bool operator >(Note left,
                                 Note right) => left.CompareTo(right) > 0;

    public static bool operator <(Note left,
                                 Note right) => left.CompareTo(right) < 0;

    public static bool operator >=(Note left,
                                  Note right) => left.CompareTo(right) >= 0;

    public static bool operator <=(Note left,
                                  Note right) => left.CompareTo(right) <= 0;

    public static Note operator +(Note note,
                                 int interval)
    {
      return note.Add(interval, AccidentalMode);
    }

    public static Note operator ++(Note note)
    {
      return note.Add(1, AccidentalMode);
    }

    public static Note operator -(Note note,
                                 int interval)
    {
      return note.Subtract(interval, AccidentalMode);
    }

    public static Note operator --(Note note)
    {
      return note.Subtract(1, AccidentalMode);
    }

    #endregion

    #region Implementation

    private static int CalcInterval(NoteName noteName,
                                    Accidental accidental)
    {
      int interval = (NoteName.C.IntervalBetween(noteName) + (int)accidental) % INTERVAL_COUNT;
      if (interval < 0)
      {
        interval = INTERVAL_COUNT + interval;
      }

      return interval;
    }

    internal static Note GetNote(int index,
                                 bool favorSharps)
    {
      Link link = s_links[index];

      if (link.Natural != null)
      {
        return link.Natural.Value;
      }

      Debug.Assert(link.Sharp.HasValue);
      Debug.Assert(link.Flat.HasValue);

      return favorSharps ? link.Sharp.Value : link.Flat.Value;
    }

    private static ushort Encode(int value,
                                 NoteName noteName,
                                 Accidental accidental)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 11);
      Contract.Requires<ArgumentOutOfRangeException>(noteName >= NoteName.C && noteName <= NoteName.B);
      Contract.Requires<ArgumentOutOfRangeException>(
        accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
      var encoded = (ushort)((((ushort)noteName & TONE_NAME_MASK) << TONE_NAME_SHIFT)
                              | (((ushort)(accidental + 2) & ACCIDENTAL_MASK) << ACCIDENTAL_SHIFT)
                              | ((ushort)value & INTERVAL_MASK));
      return encoded;
    }

    private static int DecodeInterval(ushort encoded)
    {
      int result = encoded & INTERVAL_MASK;
      return result;
    }

    private static NoteName DecodeToneName(ushort encoded)
    {
      int result = (encoded >> TONE_NAME_SHIFT) & TONE_NAME_MASK;
      return (NoteName)result;
    }

    private static Accidental DecodeAccidental(ushort encoded)
    {
      int result = ((encoded >> ACCIDENTAL_SHIFT) & ACCIDENTAL_MASK) - 2;
      return (Accidental)result;
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
