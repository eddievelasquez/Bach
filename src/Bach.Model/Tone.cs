//
// Module Name: Tone.cs
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

  /// <summary>
  /// A Tone represents a combination of a <see cref="ToneName"/>
  /// and an optional <see cref="Accidental"/> following the
  /// English naming convention for the 12 tone
  /// chromatic scale.
  /// </summary>
  public struct Tone: IEquatable<Tone>,
                      IComparable<Tone>
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

    public static readonly Tone C;
    public static readonly Tone CSharp;
    public static readonly Tone DFlat;
    public static readonly Tone D;
    public static readonly Tone DSharp;
    public static readonly Tone EFlat;
    public static readonly Tone E;
    public static readonly Tone F;
    public static readonly Tone FSharp;
    public static readonly Tone GFlat;
    public static readonly Tone G;
    public static readonly Tone GSharp;
    public static readonly Tone AFlat;
    public static readonly Tone A;
    public static readonly Tone ASharp;
    public static readonly Tone BFlat;
    public static readonly Tone B;

    private readonly ushort _encoded;

    #endregion

    #region Construction/Destruction

    static Tone()
    {
      s_links = new Link[INTERVAL_COUNT];

      C = Create(ToneName.C);
      CSharp = Create(ToneName.C, Accidental.Sharp);
      DFlat = Create(ToneName.D, Accidental.Flat);
      D = Create(ToneName.D);
      DSharp = Create(ToneName.D, Accidental.Sharp);
      EFlat = Create(ToneName.E, Accidental.Flat);
      E = Create(ToneName.E);
      F = Create(ToneName.F);
      FSharp = Create(ToneName.F, Accidental.Sharp);
      GFlat = Create(ToneName.G, Accidental.Flat);
      G = Create(ToneName.G);
      GSharp = Create(ToneName.G, Accidental.Sharp);
      AFlat = Create(ToneName.A, Accidental.Flat);
      A = Create(ToneName.A);
      ASharp = Create(ToneName.A, Accidental.Sharp);
      BFlat = Create(ToneName.B, Accidental.Flat);
      B = Create(ToneName.B);

      AccidentalMode = AccidentalMode.FavorSharps;
    }

    public Tone(ToneName toneName,
                Accidental accidental = Accidental.Natural)
    {
      int interval = CalcInterval(toneName, accidental);
      _encoded = Encode(interval, toneName, accidental);
    }

    private static Tone Create(ToneName toneName,
                               Accidental accidental = Accidental.Natural)
    {
      var tone = new Tone(toneName, accidental);
      Link link = s_links[tone.Interval];
      if( link == null )
      {
        link = new Link();
        s_links[tone.Interval] = link;
      }

      switch( accidental )
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

    public ToneName ToneName => DecodeToneName(_encoded);

    public Accidental Accidental => DecodeAccidental(_encoded);

    #endregion

    #region IComparable<Note> Members

    public int CompareTo(Tone other) => Interval - other.Interval;

    #endregion

    #region IEquatable<Note> Members

    public bool Equals(Tone other) => Interval == other.Interval;

    #endregion

    #region Public Methods

    public static bool TryParse(string value,
                                out Tone tone)
    {
      if( string.IsNullOrEmpty(value) )
      {
        tone = C;
        return false;
      }

      ToneName toneName;
      if( !Enum.TryParse(value.Substring(0, 1), true, out toneName) )
      {
        tone = C;
        return false;
      }

      var accidental = Accidental.Natural;
      if( value.Length > 1 && !AccidentalExtensions.TryParse(value.Substring(1), out accidental) )
      {
        tone = C;
        return false;
      }

      tone = new Tone(toneName, accidental);
      return true;
    }

    public static Tone Parse(string value)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);

      Tone result;
      if( !TryParse(value, out result) )
      {
        throw new FormatException($"{value} is not a valid tone");
      }

      return result;
    }

    [Pure]
    public Tone Add(int interval,
                    AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int newInterval = (Interval + interval) % INTERVAL_COUNT;
      return GetNote(newInterval, mode == AccidentalMode.FavorSharps);
    }

    [Pure]
    public Tone Subtract(int interval,
                         AccidentalMode mode = AccidentalMode.FavorSharps)
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

      return obj.GetType() == GetType() && Equals((Tone) obj);
    }

    public override int GetHashCode() => Interval;

    public override string ToString() => $"{ToneName}{Accidental.ToSymbol()}";

    #endregion

    #region Operators

    public static bool operator==(Tone left,
                                  Tone right) => Equals(left, right);

    public static bool operator!=(Tone left,
                                  Tone right) => !Equals(left, right);

    public static bool operator>(Tone left,
                                 Tone right) => left.CompareTo(right) > 0;

    public static bool operator<(Tone left,
                                 Tone right) => left.CompareTo(right) < 0;

    public static bool operator>=(Tone left,
                                  Tone right) => left.CompareTo(right) >= 0;

    public static bool operator<=(Tone left,
                                  Tone right) => left.CompareTo(right) <= 0;

    public static Tone operator+(Tone tone,
                                 int interval)
    {
      Contract.Requires<ArgumentNullException>(tone != null);
      return tone.Add(interval, AccidentalMode);
    }

    public static Tone operator++(Tone tone)
    {
      Contract.Requires<ArgumentNullException>(tone != null);
      return tone.Add(1, AccidentalMode);
    }

    public static Tone operator-(Tone tone,
                                 int interval)
    {
      Contract.Requires<ArgumentNullException>(tone != null);
      return tone.Subtract(interval, AccidentalMode);
    }

    public static Tone operator--(Tone tone)
    {
      Contract.Requires<ArgumentNullException>(tone != null);
      return tone.Subtract(1, AccidentalMode);
    }

    #endregion

    #region Implementation

    private static int CalcInterval(ToneName toneName,
                                    Accidental accidental)
    {
      int interval = (ToneName.C.IntervalBetween(toneName) + (int) accidental) % INTERVAL_COUNT;
      if( interval < 0 )
      {
        interval = INTERVAL_COUNT + interval;
      }

      return interval;
    }

    internal static Tone GetNote(int index,
                                 bool favorSharps)
    {
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural.Value;
      }

      return favorSharps ? link.Sharp.Value : link.Flat.Value;
    }

    private static ushort Encode(int value,
                                 ToneName toneName,
                                 Accidental accidental)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 11);
      Contract.Requires<ArgumentOutOfRangeException>(toneName >= ToneName.C && toneName <= ToneName.B);
      Contract.Requires<ArgumentOutOfRangeException>(
        accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
      var encoded = (ushort) ((((ushort) toneName & TONE_NAME_MASK) << TONE_NAME_SHIFT)
                              | (((ushort) (accidental + 2) & ACCIDENTAL_MASK) << ACCIDENTAL_SHIFT)
                              | ((ushort) value & INTERVAL_MASK));
      return encoded;
    }

    private static int DecodeInterval(ushort encoded)
    {
      int result = encoded & INTERVAL_MASK;
      return result;
    }

    private static ToneName DecodeToneName(ushort encoded)
    {
      int result = (encoded >> TONE_NAME_SHIFT) & TONE_NAME_MASK;
      return (ToneName) result;
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

      public Tone? Natural { get; set; }
      public Tone? Sharp { get; set; }
      public Tone? Flat { get; set; }

      #endregion
    }

    #endregion
  }
}
