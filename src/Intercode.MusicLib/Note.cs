namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public class Note : IEquatable<Note>
  {
    #region Constants

    public static readonly Note A1 = new Note(Tone.A, 1);
    public static readonly Note ASharp1 = new Note(Tone.ASharp, 1);
    public static readonly Note BFlat1 = new Note(Tone.BFlat, 1);
    public static readonly Note B1 = new Note(Tone.B, 1);
    public static readonly Note C1 = new Note(Tone.C, 1);
    public static readonly Note CSharp1 = new Note(Tone.CSharp, 1);
    public static readonly Note DFlat1 = new Note(Tone.DFlat, 1);
    public static readonly Note D1 = new Note(Tone.D, 1);
    public static readonly Note DSharp1 = new Note(Tone.DSharp, 1);
    public static readonly Note EFlat1 = new Note(Tone.EFlat, 1);
    public static readonly Note E1 = new Note(Tone.E, 1);
    public static readonly Note F1 = new Note(Tone.F, 1);
    public static readonly Note FSharp1 = new Note(Tone.FSharp, 1);
    public static readonly Note GFlat1 = new Note(Tone.GFlat, 1);
    public static readonly Note G1 = new Note(Tone.G, 1);
    public static readonly Note GSharp1 = new Note(Tone.GSharp, 1);
    public static readonly Note AFlat1 = new Note(Tone.AFlat, 1);

    public static readonly Note A2 = new Note(Tone.A, 2);
    public static readonly Note ASharp2 = new Note(Tone.ASharp, 2);
    public static readonly Note BFlat2 = new Note(Tone.BFlat, 2);
    public static readonly Note B2 = new Note(Tone.B, 2);
    public static readonly Note C2 = new Note(Tone.C, 2);
    public static readonly Note CSharp2 = new Note(Tone.CSharp, 2);
    public static readonly Note DFlat2 = new Note(Tone.DFlat, 2);
    public static readonly Note D2 = new Note(Tone.D, 2);
    public static readonly Note DSharp2 = new Note(Tone.DSharp, 2);
    public static readonly Note EFlat2 = new Note(Tone.EFlat, 2);
    public static readonly Note E2 = new Note(Tone.E, 2);
    public static readonly Note F2 = new Note(Tone.F, 2);
    public static readonly Note FSharp2 = new Note(Tone.FSharp, 2);
    public static readonly Note GFlat2 = new Note(Tone.GFlat, 2);
    public static readonly Note G2 = new Note(Tone.G, 2);
    public static readonly Note GSharp2 = new Note(Tone.GSharp, 2);
    public static readonly Note AFlat2 = new Note(Tone.AFlat, 2);

    public static readonly Note A3 = new Note(Tone.A, 3);
    public static readonly Note ASharp3 = new Note(Tone.ASharp, 3);
    public static readonly Note BFlat3 = new Note(Tone.BFlat, 3);
    public static readonly Note B3 = new Note(Tone.B, 3);
    public static readonly Note C3 = new Note(Tone.C, 3);
    public static readonly Note CSharp3 = new Note(Tone.CSharp, 3);
    public static readonly Note DFlat3 = new Note(Tone.DFlat, 3);
    public static readonly Note D3 = new Note(Tone.D, 3);
    public static readonly Note DSharp3 = new Note(Tone.DSharp, 3);
    public static readonly Note EFlat3 = new Note(Tone.EFlat, 3);
    public static readonly Note E3 = new Note(Tone.E, 3);
    public static readonly Note F3 = new Note(Tone.F, 3);
    public static readonly Note FSharp3 = new Note(Tone.FSharp, 3);
    public static readonly Note GFlat3 = new Note(Tone.GFlat, 3);
    public static readonly Note G3 = new Note(Tone.G, 3);
    public static readonly Note GSharp3 = new Note(Tone.GSharp, 3);
    public static readonly Note AFlat3 = new Note(Tone.AFlat, 3);

    public static readonly Note A4 = new Note(Tone.A, 4);
    public static readonly Note ASharp4 = new Note(Tone.ASharp, 4);
    public static readonly Note BFlat4 = new Note(Tone.BFlat, 4);
    public static readonly Note B4 = new Note(Tone.B, 4);
    public static readonly Note C4 = new Note(Tone.C, 4);
    public static readonly Note CSharp4 = new Note(Tone.CSharp, 4);
    public static readonly Note DFlat4 = new Note(Tone.DFlat, 4);
    public static readonly Note D4 = new Note(Tone.D, 4);
    public static readonly Note DSharp4 = new Note(Tone.DSharp, 4);
    public static readonly Note EFlat4 = new Note(Tone.EFlat, 4);
    public static readonly Note E4 = new Note(Tone.E, 4);
    public static readonly Note F4 = new Note(Tone.F, 4);
    public static readonly Note FSharp4 = new Note(Tone.FSharp, 4);
    public static readonly Note GFlat4 = new Note(Tone.GFlat, 4);
    public static readonly Note G4 = new Note(Tone.G, 4);
    public static readonly Note GSharp4 = new Note(Tone.GSharp, 4);
    public static readonly Note AFlat4 = new Note(Tone.AFlat, 4);

    public static readonly Note A5 = new Note(Tone.A, 5);
    public static readonly Note ASharp5 = new Note(Tone.ASharp, 5);
    public static readonly Note BFlat5 = new Note(Tone.BFlat, 5);
    public static readonly Note B5 = new Note(Tone.B, 5);
    public static readonly Note C5 = new Note(Tone.C, 5);
    public static readonly Note CSharp5 = new Note(Tone.CSharp, 5);
    public static readonly Note DFlat5 = new Note(Tone.DFlat, 5);
    public static readonly Note D5 = new Note(Tone.D, 5);
    public static readonly Note DSharp5 = new Note(Tone.DSharp, 5);
    public static readonly Note EFlat5 = new Note(Tone.EFlat, 5);
    public static readonly Note E5 = new Note(Tone.E, 5);
    public static readonly Note F5 = new Note(Tone.F, 5);
    public static readonly Note FSharp5 = new Note(Tone.FSharp, 5);
    public static readonly Note GFlat5 = new Note(Tone.GFlat, 5);
    public static readonly Note G5 = new Note(Tone.G, 5);
    public static readonly Note GSharp5 = new Note(Tone.GSharp, 5);
    public static readonly Note AFlat5 = new Note(Tone.AFlat, 5);

    public static readonly Note A6 = new Note(Tone.A, 6);
    public static readonly Note ASharp6 = new Note(Tone.ASharp, 6);
    public static readonly Note BFlat6 = new Note(Tone.BFlat, 6);
    public static readonly Note B6 = new Note(Tone.B, 6);
    public static readonly Note C6 = new Note(Tone.C, 6);
    public static readonly Note CSharp6 = new Note(Tone.CSharp, 6);
    public static readonly Note DFlat6 = new Note(Tone.DFlat, 6);
    public static readonly Note D6 = new Note(Tone.D, 6);
    public static readonly Note DSharp6 = new Note(Tone.DSharp, 6);
    public static readonly Note EFlat6 = new Note(Tone.EFlat, 6);
    public static readonly Note E6 = new Note(Tone.E, 6);
    public static readonly Note F6 = new Note(Tone.F, 6);
    public static readonly Note FSharp6 = new Note(Tone.FSharp, 6);
    public static readonly Note GFlat6 = new Note(Tone.GFlat, 6);
    public static readonly Note G6 = new Note(Tone.G, 6);
    public static readonly Note GSharp6 = new Note(Tone.GSharp, 6);
    public static readonly Note AFlat6 = new Note(Tone.AFlat, 6);

    public static readonly Note A7 = new Note(Tone.A, 7);
    public static readonly Note ASharp7 = new Note(Tone.ASharp, 7);
    public static readonly Note BFlat7 = new Note(Tone.BFlat, 7);
    public static readonly Note B7 = new Note(Tone.B, 7);
    public static readonly Note C7 = new Note(Tone.C, 7);
    public static readonly Note CSharp7 = new Note(Tone.CSharp, 7);
    public static readonly Note DFlat7 = new Note(Tone.DFlat, 7);
    public static readonly Note D7 = new Note(Tone.D, 7);
    public static readonly Note DSharp7 = new Note(Tone.DSharp, 7);
    public static readonly Note EFlat7 = new Note(Tone.EFlat, 7);
    public static readonly Note E7 = new Note(Tone.E, 7);
    public static readonly Note F7 = new Note(Tone.F, 7);
    public static readonly Note FSharp7 = new Note(Tone.FSharp, 7);
    public static readonly Note GFlat7 = new Note(Tone.GFlat, 7);
    public static readonly Note G7 = new Note(Tone.G, 7);
    public static readonly Note GSharp7 = new Note(Tone.GSharp, 7);
    public static readonly Note AFlat7 = new Note(Tone.AFlat, 7);

    public static readonly Note A8 = new Note(Tone.A, 8);
    public static readonly Note ASharp8 = new Note(Tone.ASharp, 8);
    public static readonly Note BFlat8 = new Note(Tone.BFlat, 8);
    public static readonly Note B8 = new Note(Tone.B, 8);
    public static readonly Note C8 = new Note(Tone.C, 8);
    public static readonly Note CSharp8 = new Note(Tone.CSharp, 8);
    public static readonly Note DFlat8 = new Note(Tone.DFlat, 8);
    public static readonly Note D8 = new Note(Tone.D, 8);
    public static readonly Note DSharp8 = new Note(Tone.DSharp, 8);
    public static readonly Note EFlat8 = new Note(Tone.EFlat, 8);
    public static readonly Note E8 = new Note(Tone.E, 8);
    public static readonly Note F8 = new Note(Tone.F, 8);
    public static readonly Note FSharp8 = new Note(Tone.FSharp, 8);
    public static readonly Note GFlat8 = new Note(Tone.GFlat, 8);
    public static readonly Note G8 = new Note(Tone.G, 8);
    public static readonly Note GSharp8 = new Note(Tone.GSharp, 8);
    public static readonly Note AFlat8 = new Note(Tone.AFlat, 8);

    #endregion

    #region Construction

    private Note( Tone tone, int octave )
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave >= 1, "octave");
      Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

      Tone = tone;
      Octave = octave;
    }

    #endregion

    #region Properties

    public Tone Tone { get; private set; }
    public Int32 Octave { get; private set; }

    #endregion

    #region Publuc Methods

    public Note AsSharp()
    {
      var note = new Note(Tone.AsSharp(), Octave);
      return note;
    }

    public Note AsFlat()
    {
      var note = new Note(Tone.AsSharp(), Octave);
      return note;
    }

    #endregion

    #region Internal Methods

    internal Note Next( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int octave = Octave + ((Tone.Index + interval) / Tone.INTERVAL_COUNT);
      var newNote = new Note(Tone.Next(interval, flat), octave);
      return newNote;
    }

    internal Note Previous( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int octave = Octave;
      if( Tone.Index - interval < 0 )
        octave -= (Tone.Index + ((Tone.INTERVAL_COUNT + interval) - 1)) / Tone.INTERVAL_COUNT;

      var newNote = new Note(Tone.Previous(interval, flat), octave);
      return newNote;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals( Note other )
    {
      if( ReferenceEquals(null, other) )
        return false;

      if( ReferenceEquals(this, other) )
        return true;

      return Tone.Equals(other.Tone) && Octave == other.Octave;
    }

    #endregion

    #region Overrides

    public override bool Equals( object obj )
    {
      if( ReferenceEquals(null, obj) )
        return false;

      if( ReferenceEquals(this, obj) )
        return true;

      if( obj.GetType() != GetType() )
        return false;

      return Equals((Note) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (Tone.GetHashCode() * 397) ^ Octave;
      }
    }

    public override string ToString()
    {
      return Tone + Octave.ToString();
    }

    #endregion

    public static bool operator ==( Note left, Note right )
    {
      return Equals(left, right);
    }

    public static bool operator !=( Note left, Note right )
    {
      return !Equals(left, right);
    }
  }
}
