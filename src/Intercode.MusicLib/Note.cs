﻿namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public struct Note: IEquatable<Note>
  {
    private const int INTERVAL_COUNT = 12;
    private const int MIN_OCTAVE = 1;
    private const int MAX_OCTAVE = 8;

    private static readonly string[] s_sharpRepresentations = new[]
    { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

    private static readonly string[] s_flatRepresentations = new[]
    { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };

    #region Data Members

    private readonly byte _index;
    private readonly Accidental _accidental;

    #endregion

    #region Construction

    internal Note( Tone tone, Accidental accidental, int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.A, "tone");
      Contract.Requires<ArgumentOutOfRangeException>(tone <= Tone.AFlat, "tone");
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.None, "accidental");
      Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.Flat, "accidental");

      _index = (byte)(((octave - 1) * INTERVAL_COUNT) + tone);
      _accidental = accidental;
    }

    [ Obsolete ]
    private Note( byte index, Accidental accidental )
    {
      _index = index;
      _accidental = accidental;
    }

    #endregion

    #region Properties

    private byte Index
    {
      get { return _index; }
    }

    public Int32 Octave
    {
      get { return (Index / INTERVAL_COUNT) + 1; }
    }

    public Tone Tone
    {
      get { return (Tone)(Index % INTERVAL_COUNT); }
    }

    public Accidental Accidental
    {
      get { return _accidental; }
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals( Note other )
    {
      return Index == other.Index;
    }

    #endregion

    #region Overrides

    public override bool Equals( object obj )
    {
      if( ReferenceEquals(null, obj) )
        return false;

      return obj is Note && Equals((Note)obj);
    }

    public override int GetHashCode()
    {
      return Index.GetHashCode();
    }

    public override string ToString()
    {
      int pos = Index % INTERVAL_COUNT;
      if( Accidental == Accidental.Flat )
        return s_flatRepresentations[pos] + Octave;

      return s_sharpRepresentations[pos] + Octave;
    }

    #endregion

    #region Implementation

    private static int CalcOctave( int index )
    {
      int octave = (index / INTERVAL_COUNT) + 1;
      return octave;
    }

    private static Accidental CalcAccidental( int index, bool flat )
    {
      var newAccidental = Accidental.None;
      int pos = index % INTERVAL_COUNT;
      if( s_sharpRepresentations[pos].Length > 1 )
        newAccidental = flat ? Accidental.Flat : Accidental.Sharp;

      return newAccidental;
    }

    #endregion

    public static Note A( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.A, Accidental.None, octave);
    }

    public static Note ASharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.ASharp, Accidental.Sharp, octave);
    }

    public static Note BFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.BFlat, Accidental.Flat, octave);
    }

    public static Note B( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.B, Accidental.None, octave);
    }

    public static Note C( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.C, Accidental.None, octave);
    }

    public static Note CSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.CSharp, Accidental.Sharp, octave);
    }

    public static Note DFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.DFlat, Accidental.Flat, octave);
    }

    public static Note D( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.D, Accidental.None, octave);
    }

    public static Note DSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.DSharp, Accidental.Sharp, octave);
    }

    public static Note EFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.EFlat, Accidental.Flat, octave);
    }

    public static Note E( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.E, Accidental.None, octave);
    }

    public static Note F( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.F, Accidental.None, octave);
    }

    public static Note FSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.FSharp, Accidental.Sharp, octave);
    }

    public static Note GFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.GFlat, Accidental.Flat, octave);
    }

    public static Note G( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.G, Accidental.None, octave);
    }

    public static Note GSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.GSharp, Accidental.Sharp, octave);
    }

    public static Note AFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.AFlat, Accidental.Flat, octave);
    }

    public Note AsSharp()
    {
      if( Accidental == Accidental.None )
        return this;

      var note = new Note(Tone, Accidental.Sharp, Octave);
      return note;
    }

    public Note AsFlat()
    {
      if( Accidental == Accidental.None )
        return this;

      var note = new Note(Tone, Accidental.Flat, Octave);
      return note;
    }

    internal Note Next( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int octave = CalcOctave(interval);
      if( octave > MAX_OCTAVE )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes higher than G#{0} are not supported", MAX_OCTAVE));

      int newIndex = Index + interval;

      var newAccidental = CalcAccidental(newIndex, flat);
      var note = new Note((byte)newIndex, newAccidental);
      return note;
    }

    internal Note Previous( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int octave = CalcOctave(interval);
      if( octave < MIN_OCTAVE )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes lower than A{0} are not supported", MIN_OCTAVE));

      int newIndex = Index - interval;
      var newAccidental = CalcAccidental(newIndex, flat);
      var note = new Note((byte)newIndex, newAccidental);
      return note;
    }

    public static bool operator ==( Note left, Note right )
    {
      return left.Equals(right);
    }

    public static bool operator !=( Note left, Note right )
    {
      return !left.Equals(right);
    }
  }
}
