namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public struct Note: IEquatable<Note>, IComparable<Note>
  {
    #region Private Constants

    private const int INTERVAL_COUNT = 12;
    private const int TONE_COUNT = Tone.B - Tone.C + 1;
    private const int MIN_OCTAVE = 1;
    private const int MAX_OCTAVE = 8;
    private const int MIN_INDEX = 0;
    private const int MAX_INDEX = (MAX_OCTAVE * INTERVAL_COUNT) - 1;

    private static readonly Accidental[] s_accidentals = new[]
    {
      Accidental.None, // C
      Accidental.Sharp, // C#
      Accidental.Flat, // Db
      Accidental.None, // D
      Accidental.Sharp, // D#
      Accidental.Flat, // Eb
      Accidental.None, // E
      Accidental.None, // F
      Accidental.Sharp, // F#
      Accidental.Flat, // Gb
      Accidental.None, // G
      Accidental.Sharp, // G#
      Accidental.Flat, // Ab
      Accidental.None, // A
      Accidental.Sharp, // A#
      Accidental.Flat, // Bb
      Accidental.None // B
    };

    private static readonly int[] s_intervals = new[]
    {
      0, // C
      1, // C#
      1, // Db
      2, // D
      3, // D#
      3, // Eb
      4, // E
      5, // F
      6, // F#
      6, // Gb
      7, // G
      8, // G#
      8, // Ab
      9, // A
      10, // A#
      10, // Bb
      11 // B
    };

    private static readonly Tone[] s_tones = new[]
    {
      Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.G,
      Tone.GSharp, Tone.A, Tone.ASharp, Tone.B
    };

    private static readonly Tone[] s_tonesNoAccidentals = new[]
    {
      Tone.C, // C
      Tone.C, // C#
      Tone.D, // Db
      Tone.D, // D
      Tone.D, // D#
      Tone.E, // Eb
      Tone.E, // E
      Tone.F, // F
      Tone.F, // F#
      Tone.G, // Gb
      Tone.G, // G
      Tone.G, // G#
      Tone.A, // Ab
      Tone.A, // A
      Tone.A, // A#
      Tone.B, // Bb
      Tone.B, // B
    };

    private static readonly string[] s_representations = new[]
    {
      "C", // C
      "C#", // C#
      "Db", // Db
      "D", // D
      "D#", // D#
      "Eb", // Eb
      "E", // E
      "F", // F
      "F#", // F#
      "Gb", // Gb
      "G", // G
      "G#", // G#
      "Ab", // Ab
      "A", // A
      "A#", // A#
      "Bb", // Bb
      "B", // B
    };

    #endregion

    #region Data Members
    
    public static readonly Note Invalid = new Note((Tone)(-1), -1);
    private readonly byte _tone;
    private readonly byte _octave;

    #endregion

    #region Construction

    private Note( Tone tone, int octave )
    {
      _tone = (byte)tone;
      _octave = (byte)octave;
    }

    #endregion

    #region Factories

    public static Note C( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.C, octave);
    }

    public static Note CSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.CSharp, octave);
    }

    public static Note DFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.DFlat, octave);
    }

    public static Note D( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.D, octave);
    }

    public static Note DSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.DSharp, octave);
    }

    public static Note EFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.EFlat, octave);
    }

    public static Note E( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.E, octave);
    }

    public static Note F( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.F, octave);
    }

    public static Note FSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.FSharp, octave);
    }

    public static Note GFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.GFlat, octave);
    }

    public static Note G( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.G, octave);
    }

    public static Note GSharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.GSharp, octave);
    }

    public static Note AFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.AFlat, octave);
    }

    public static Note A( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.A, octave);
    }

    public static Note ASharp( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.ASharp, octave);
    }

    public static Note BFlat( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.BFlat, octave);
    }

    public static Note B( int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      return new Note(Tone.B, octave);
    }

    #endregion

    #region Properties

    private int Index
    {
      get { return ((Octave - 1) * INTERVAL_COUNT) + s_intervals[_tone]; }
    }

    public Int32 Octave
    {
      get { return _octave; }
    }

    public Tone Tone
    {
      get { return (Tone)_tone; }
    }

    public Tone ToneWithoutAccidental
    {
      get { return s_tonesNoAccidentals[_tone]; }
    }

    public Accidental Accidental
    {
      get { return s_accidentals[_tone]; }
    }

    #endregion

    #region Public Methods

    public Note AsSharp()
    {
      if( Accidental != Accidental.Flat )
        return this;

      var tone = (Tone)((int)(Tone - 1) % TONE_COUNT);
      var note = new Note(tone, Octave);
      return note;
    }

    public Note AsFlat()
    {
      if( Accidental != Accidental.Sharp )
        return this;

      var tone = (Tone)((int)(Tone + 1) % TONE_COUNT);
      var note = new Note(tone, Octave);
      return note;
    }

    #endregion

    #region Internal Methods

    internal bool TryNext( int interval, out Note note )
    {
      if( interval == 0 )
      {
        note = this;
        return true;
      }

      int index = Index + interval;
      if( index < MIN_INDEX || index > MAX_INDEX )
      {
        note = Invalid;
        return false;
      }

      int octave = (index / INTERVAL_COUNT) + 1;
      var tone = (int)s_tones[index % INTERVAL_COUNT];

      note = new Note((Tone)tone, octave);
      return true;
    }

    internal Note Next( int interval )
    {
      Note result;
      if( !TryNext(interval, out result) )
      {
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes higher than B{0} are not supported", MAX_OCTAVE));
      }

      return result;
    }

    internal Note Previous( int interval )
    {
      Note result;
      if( !TryNext(-interval, out result) )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes lower than C{0} are not supported", MIN_OCTAVE));

      return result;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals( Note other )
    {
      return Index == other.Index;
    }

    #endregion

    #region Object Members

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
      return s_representations[(int)Tone] + Octave;
    }

    #endregion

    #region IComparable<Note> Members

    public int CompareTo( Note other )
    {
      return Index - other.Index;
    }

    #endregion

    #region Equality Operators

    public static bool operator ==( Note left, Note right )
    {
      return left.Equals(right);
    }

    public static bool operator !=( Note left, Note right )
    {
      return !left.Equals(right);
    }

    #endregion
  }
}
