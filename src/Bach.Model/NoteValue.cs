//  
// Module Name: NoteValue.cs
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
  using System.Text;

  public sealed class NoteValue: IEquatable<NoteValue>,
                                 IComparable<NoteValue>
  {
    #region Data Members

    private static readonly Link[] s_links;

    public static NoteValue C;
    public static NoteValue CSharp;
    public static NoteValue DFlat;
    public static NoteValue D;
    public static NoteValue DSharp;
    public static NoteValue EFlat;
    public static NoteValue E;
    public static NoteValue F;
    public static NoteValue FSharp;
    public static NoteValue GFlat;
    public static NoteValue G;
    public static NoteValue GSharp;
    public static NoteValue AFlat;
    public static NoteValue A;
    public static NoteValue ASharp;
    public static NoteValue BFlat;
    public static NoteValue B;

    #endregion

    #region Construction/Destruction

    static NoteValue()
    {
      // Create notes
      C = new NoteValue(0, Tone.C, Accidental.Natural);
      CSharp = new NoteValue(1, Tone.C, Accidental.Sharp);
      DFlat = new NoteValue(1, Tone.D, Accidental.Flat);
      D = new NoteValue(2, Tone.D, Accidental.Natural);
      DSharp = new NoteValue(3, Tone.D, Accidental.Sharp);
      EFlat = new NoteValue(3, Tone.E, Accidental.Flat);
      E = new NoteValue(4, Tone.E, Accidental.Natural);
      F = new NoteValue(5, Tone.F, Accidental.Natural);
      FSharp = new NoteValue(6, Tone.F, Accidental.Sharp);
      GFlat = new NoteValue(6, Tone.G, Accidental.Flat);
      G = new NoteValue(7, Tone.G, Accidental.Natural);
      GSharp = new NoteValue(8, Tone.G, Accidental.Sharp);
      AFlat = new NoteValue(8, Tone.A, Accidental.Flat);
      A = new NoteValue(9, Tone.A, Accidental.Natural);
      ASharp = new NoteValue(10, Tone.A, Accidental.Sharp);
      BFlat = new NoteValue(10, Tone.B, Accidental.Flat);
      B = new NoteValue(11, Tone.B, Accidental.Natural);

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

    private NoteValue(int value, Tone tone, Accidental accidental)
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

    #region IComparable<NoteValue> Members

    public int CompareTo(NoteValue other)
    {
      return NumericValue - other.NumericValue;
    }

    #endregion

    #region IEquatable<NoteValue> Members

    public bool Equals(NoteValue other)
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

    public static bool operator ==(NoteValue left, NoteValue right) => Equals(left, right);

    public static bool operator !=(NoteValue left, NoteValue right) => !Equals(left, right);

    public static bool operator >(NoteValue left, NoteValue right) => left.CompareTo(right) > 0;

    public static bool operator <(NoteValue left, NoteValue right) => left.CompareTo(right) < 0;

    public static bool operator >=(NoteValue left, NoteValue right) => left.CompareTo(right) >= 0;

    public static bool operator <=(NoteValue left, NoteValue right) => left.CompareTo(right) <= 0;

    public static NoteValue operator +(NoteValue note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(interval, AccidentalMode);
    }

    public static NoteValue operator ++(NoteValue note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(1, AccidentalMode);
    }

    public static NoteValue operator -(NoteValue note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(interval, AccidentalMode);
    }

    public static NoteValue operator --(NoteValue note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(1, AccidentalMode);
    }

    public static bool TryParse(string value, out NoteValue note)
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

    public static NoteValue Parse(string value)
    {
      NoteValue result;
      if( !TryParse(value, out result) )
      {
        throw new ArgumentException($"{value} is not a valid note");
      }

      return result;
    }

    public static NoteValue Get(Tone tone, Accidental accidental = Accidental.Natural)
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

    public NoteValue Add(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int index = (NumericValue + interval) % s_links.Length;
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural;
      }

      return mode == AccidentalMode.FavorSharps ? link.Sharp : link.Flat;
    }

    public NoteValue Subtract(int interval, AccidentalMode mode = AccidentalMode.FavorSharps)
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

      return obj.GetType() == GetType() && Equals((NoteValue) obj);
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

    #region Implementation

    private static void TryGetAccidental(string value, ref int index, out Accidental accidental)
    {
      accidental = Accidental.Natural;

      var buf = new StringBuilder();
      for( int i = index; i < value.Length; ++i )
      {
        char ch = value[i];
        if( ch != '#' && ch != 'b' && ch != 'B' )
        {
          if( buf.Length > 0 )
          {
            break;
          }

          return;
        }

        buf.Append(ch);
      }

      if( buf.Length == 0 )
      {
        return;
      }

      if( AccidentalExtensions.TryParse(buf.ToString(), out accidental) )
      {
        index += buf.Length;
      }
    }

    #endregion

    #region Nested type: Link

    private class Link
    {
      #region Construction/Destruction

      public Link(NoteValue natural)
      {
        Natural = natural;
      }

      public Link(NoteValue flat, NoteValue sharp)
      {
        Flat = flat;
        Sharp = sharp;
      }

      #endregion

      #region Properties

      public NoteValue Natural { get; }
      public NoteValue Sharp { get; }
      public NoteValue Flat { get; }

      #endregion
    }

    #endregion
  }
}
