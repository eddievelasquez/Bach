namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public struct Note: IEquatable<Note>, IComparable<Note>
  {
    #region Constants

    private const int INTERVAL_COUNT = 12;
    private const int TONE_COUNT = Tone.AFlat - Tone.A + 1;
    private const int MIN_OCTAVE = 1;
    private const int MAX_OCTAVE = 8;
    private const int MIN_INDEX = 0;
    private const int MAX_INDEX = (MAX_OCTAVE * INTERVAL_COUNT) - 1;

    private static readonly Accidental[] s_accidentals = new[]
    {
      Accidental.None, // A
      Accidental.Sharp, // A#
      Accidental.Flat, // Bb
      Accidental.None, // B
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
      Accidental.Flat // Ab
    };

    private static readonly int[] s_intervals = new[]
    {
      0, // A
      1, // A#
      1, // Bb
      2, // B
      3, // C
      4, // C#
      4, // Db
      5, // D
      6, // D#
      6, // Eb
      7, // E
      8, // F
      9, // F#
      9, // Gb
      10, // G
      11, // G#
      11 // Ab
    };

    private static readonly Tone[] s_tones = new[]
    {
      Tone.A, Tone.ASharp, Tone.B, Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.G,
      Tone.GSharp
    };

    private static readonly string[] s_representations = new[]
    {
      "A", // A
      "A#", // A#
      "Bb", // Bb
      "B", // B
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
      "Ab" // Ab
    };

    #endregion

    #region Data Members

    private readonly byte _tone;
    private readonly byte _octave;

    #endregion

    #region Construction

    internal Note( Tone tone, int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.A, "tone");
      Contract.Requires<ArgumentOutOfRangeException>(tone <= Tone.AFlat, "tone");
      Contract.Requires<ArgumentOutOfRangeException>(octave > 0, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      _tone = (byte)tone;
      _octave = (byte)octave;
    }

    #endregion

    #region Factories

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

    internal Note Next( int interval, bool flat )
    {
      if( interval == 0 )
        return this;

      int index = Index + interval;
      if( index < MIN_INDEX )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes lower than A{0} are not supported", MIN_OCTAVE));

      if( index > MAX_INDEX )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes higher than G#{0} are not supported", MAX_OCTAVE));

      int octave = (index / INTERVAL_COUNT) + 1;
      var tone = (int)s_tones[index % INTERVAL_COUNT];

      if( flat && s_accidentals[tone] == Accidental.Sharp )
        tone = ((tone + 1) % TONE_COUNT);

      var note = new Note((Tone)tone, octave);
      return note;
    }

    internal Note Previous( int interval, bool flat )
    {
      return Next(-interval, flat);
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
