namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public enum Accidental
  {
    None,
    Sharp,
    Flat
  }

  public class Note: IEquatable<Note>
  {
    #region Constants

    public static readonly Note A1 = new Note(0, Accidental.None);
    public static readonly Note ASharp1 = new Note(1, Accidental.Sharp);
    public static readonly Note BFlat1 = new Note(1, Accidental.Flat);
    public static readonly Note B1 = new Note(2, Accidental.None);
    public static readonly Note C1 = new Note(3, Accidental.None);
    public static readonly Note CSharp1 = new Note(4, Accidental.Sharp);
    public static readonly Note DFlat1 = new Note(4, Accidental.Flat);
    public static readonly Note D1 = new Note(5, Accidental.None);
    public static readonly Note DSharp1 = new Note(6, Accidental.Sharp);
    public static readonly Note EFlat1 = new Note(6, Accidental.Flat);
    public static readonly Note E1 = new Note(7, Accidental.None);
    public static readonly Note F1 = new Note(8, Accidental.None);
    public static readonly Note FSharp1 = new Note(9, Accidental.Sharp);
    public static readonly Note GFlat1 = new Note(9, Accidental.Flat);
    public static readonly Note G1 = new Note(10, Accidental.None);
    public static readonly Note GSharp1 = new Note(11, Accidental.Sharp);
    public static readonly Note AFlat1 = new Note(11, Accidental.Flat);

    public static readonly Note A2 = new Note(12, Accidental.None);
    public static readonly Note ASharp2 = new Note(13, Accidental.Sharp);
    public static readonly Note BFlat2 = new Note(13, Accidental.Flat);
    public static readonly Note B2 = new Note(14, Accidental.None);
    public static readonly Note C2 = new Note(15, Accidental.None);
    public static readonly Note CSharp2 = new Note(16, Accidental.Sharp);
    public static readonly Note DFlat2 = new Note(16, Accidental.Flat);
    public static readonly Note D2 = new Note(17, Accidental.None);
    public static readonly Note DSharp2 = new Note(18, Accidental.Sharp);
    public static readonly Note EFlat2 = new Note(18, Accidental.Flat);
    public static readonly Note E2 = new Note(19, Accidental.None);
    public static readonly Note F2 = new Note(20, Accidental.None);
    public static readonly Note FSharp2 = new Note(21, Accidental.Sharp);
    public static readonly Note GFlat2 = new Note(21, Accidental.Flat);
    public static readonly Note G2 = new Note(22, Accidental.None);
    public static readonly Note GSharp2 = new Note(23, Accidental.Sharp);
    public static readonly Note AFlat2 = new Note(23, Accidental.Flat);

    public static readonly Note A3 = new Note(24, Accidental.None);
    public static readonly Note ASharp3 = new Note(25, Accidental.Sharp);
    public static readonly Note BFlat3 = new Note(25, Accidental.Flat);
    public static readonly Note B3 = new Note(26, Accidental.None);
    public static readonly Note C3 = new Note(27, Accidental.None);
    public static readonly Note CSharp3 = new Note(28, Accidental.Sharp);
    public static readonly Note DFlat3 = new Note(28, Accidental.Flat);
    public static readonly Note D3 = new Note(29, Accidental.None);
    public static readonly Note DSharp3 = new Note(30, Accidental.Sharp);
    public static readonly Note EFlat3 = new Note(30, Accidental.Flat);
    public static readonly Note E3 = new Note(31, Accidental.None);
    public static readonly Note F3 = new Note(32, Accidental.None);
    public static readonly Note FSharp3 = new Note(33, Accidental.Sharp);
    public static readonly Note GFlat3 = new Note(33, Accidental.Flat);
    public static readonly Note G3 = new Note(34, Accidental.None);
    public static readonly Note GSharp3 = new Note(35, Accidental.Sharp);
    public static readonly Note AFlat3 = new Note(35, Accidental.Flat);

    public static readonly Note A4 = new Note(36, Accidental.None);
    public static readonly Note ASharp4 = new Note(37, Accidental.Sharp);
    public static readonly Note BFlat4 = new Note(37, Accidental.Flat);
    public static readonly Note B4 = new Note(38, Accidental.None);
    public static readonly Note C4 = new Note(39, Accidental.None);
    public static readonly Note CSharp4 = new Note(40, Accidental.Sharp);
    public static readonly Note DFlat4 = new Note(40, Accidental.Flat);
    public static readonly Note D4 = new Note(41, Accidental.None);
    public static readonly Note DSharp4 = new Note(42, Accidental.Sharp);
    public static readonly Note EFlat4 = new Note(42, Accidental.Flat);
    public static readonly Note E4 = new Note(43, Accidental.None);
    public static readonly Note F4 = new Note(44, Accidental.None);
    public static readonly Note FSharp4 = new Note(45, Accidental.Sharp);
    public static readonly Note GFlat4 = new Note(45, Accidental.Flat);
    public static readonly Note G4 = new Note(46, Accidental.None);
    public static readonly Note GSharp4 = new Note(47, Accidental.Sharp);
    public static readonly Note AFlat4 = new Note(47, Accidental.Flat);

    public static readonly Note A5 = new Note(48, Accidental.None);
    public static readonly Note ASharp5 = new Note(49, Accidental.Sharp);
    public static readonly Note BFlat5 = new Note(49, Accidental.Flat);
    public static readonly Note B5 = new Note(50, Accidental.None);
    public static readonly Note C5 = new Note(51, Accidental.None);
    public static readonly Note CSharp5 = new Note(52, Accidental.Sharp);
    public static readonly Note DFlat5 = new Note(52, Accidental.Flat);
    public static readonly Note D5 = new Note(53, Accidental.None);
    public static readonly Note DSharp5 = new Note(54, Accidental.Sharp);
    public static readonly Note EFlat5 = new Note(54, Accidental.Flat);
    public static readonly Note E5 = new Note(55, Accidental.None);
    public static readonly Note F5 = new Note(56, Accidental.None);
    public static readonly Note FSharp5 = new Note(57, Accidental.Sharp);
    public static readonly Note GFlat5 = new Note(57, Accidental.Flat);
    public static readonly Note G5 = new Note(58, Accidental.None);
    public static readonly Note GSharp5 = new Note(59, Accidental.Sharp);
    public static readonly Note AFlat5 = new Note(59, Accidental.Flat);

    public static readonly Note A6 = new Note(60, Accidental.None);
    public static readonly Note ASharp6 = new Note(61, Accidental.Sharp);
    public static readonly Note BFlat6 = new Note(61, Accidental.Flat);
    public static readonly Note B6 = new Note(62, Accidental.None);
    public static readonly Note C6 = new Note(63, Accidental.None);
    public static readonly Note CSharp6 = new Note(64, Accidental.Sharp);
    public static readonly Note DFlat6 = new Note(64, Accidental.Flat);
    public static readonly Note D6 = new Note(65, Accidental.None);
    public static readonly Note DSharp6 = new Note(66, Accidental.Sharp);
    public static readonly Note EFlat6 = new Note(66, Accidental.Flat);
    public static readonly Note E6 = new Note(67, Accidental.None);
    public static readonly Note F6 = new Note(68, Accidental.None);
    public static readonly Note FSharp6 = new Note(69, Accidental.Sharp);
    public static readonly Note GFlat6 = new Note(69, Accidental.Flat);
    public static readonly Note G6 = new Note(70, Accidental.None);
    public static readonly Note GSharp6 = new Note(71, Accidental.Sharp);
    public static readonly Note AFlat6 = new Note(71, Accidental.Flat);

    public static readonly Note A7 = new Note(72, Accidental.None);
    public static readonly Note ASharp7 = new Note(73, Accidental.Sharp);
    public static readonly Note BFlat7 = new Note(73, Accidental.Flat);
    public static readonly Note B7 = new Note(74, Accidental.None);
    public static readonly Note C7 = new Note(75, Accidental.None);
    public static readonly Note CSharp7 = new Note(76, Accidental.Sharp);
    public static readonly Note DFlat7 = new Note(76, Accidental.Flat);
    public static readonly Note D7 = new Note(77, Accidental.None);
    public static readonly Note DSharp7 = new Note(78, Accidental.Sharp);
    public static readonly Note EFlat7 = new Note(78, Accidental.Flat);
    public static readonly Note E7 = new Note(79, Accidental.None);
    public static readonly Note F7 = new Note(80, Accidental.None);
    public static readonly Note FSharp7 = new Note(81, Accidental.Sharp);
    public static readonly Note GFlat7 = new Note(81, Accidental.Flat);
    public static readonly Note G7 = new Note(82, Accidental.None);
    public static readonly Note GSharp7 = new Note(83, Accidental.Sharp);
    public static readonly Note AFlat7 = new Note(83, Accidental.Flat);

    public static readonly Note A8 = new Note(84, Accidental.None);
    public static readonly Note ASharp8 = new Note(85, Accidental.Sharp);
    public static readonly Note BFlat8 = new Note(85, Accidental.Flat);
    public static readonly Note B8 = new Note(86, Accidental.None);
    public static readonly Note C8 = new Note(87, Accidental.None);
    public static readonly Note CSharp8 = new Note(88, Accidental.Sharp);
    public static readonly Note DFlat8 = new Note(88, Accidental.Flat);
    public static readonly Note D8 = new Note(89, Accidental.None);
    public static readonly Note DSharp8 = new Note(90, Accidental.Sharp);
    public static readonly Note EFlat8 = new Note(90, Accidental.Flat);
    public static readonly Note E8 = new Note(91, Accidental.None);
    public static readonly Note F8 = new Note(92, Accidental.None);
    public static readonly Note FSharp8 = new Note(93, Accidental.Sharp);
    public static readonly Note GFlat8 = new Note(93, Accidental.Flat);
    public static readonly Note G8 = new Note(94, Accidental.None);
    public static readonly Note GSharp8 = new Note(95, Accidental.Sharp);
    public static readonly Note AFlat8 = new Note(95, Accidental.Flat);

    private const int INTERVAL_COUNT = 12;

    #endregion

    #region Data Members

    private static readonly string[] s_sharpRepresentations = new[]
    { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

    private static readonly string[] s_flatRepresentations = new[]
    { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };

    #endregion

    #region Construction

    private Note( byte index, Accidental accidental )
    {
      Index = index;
      Accidental = accidental;
    }

    #endregion

    #region Properties

    private byte Index { get; set; }

    public Int32 Octave
    {
      get { return (Index / INTERVAL_COUNT) + 1; }
    }

    public Accidental Accidental { get; private set; }

    #endregion

    #region Public Methods

    public Note AsSharp()
    {
      if( Accidental == Accidental.None )
        return this;

      var note = new Note(Index, Accidental.Sharp);
      return note;
    }

    public Note AsFlat()
    {
      if( Accidental == Accidental.None )
        return this;

      var note = new Note(Index, Accidental.Flat);
      return note;
    }

    #endregion

    #region Internal Methods

    internal Note Next( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int newIndex = Index + interval;
      if( newIndex > GSharp8.Index )
        throw new ArgumentOutOfRangeException("interval",
                                              String.Format("Notes higher than {0} are not supported", GSharp8));

      var newAccidental = CalcAccidental(newIndex, flat);
      var note = new Note((byte)newIndex, newAccidental);
      return note;
    }

    internal Note Previous( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>(interval >= 0, "interval");

      if( interval == 0 )
        return this;

      int newIndex = Index - interval;
      if( newIndex < A1.Index )
        throw new ArgumentOutOfRangeException("interval", String.Format("Notes lower than {0} are not supported", A1));

      var newAccidental = CalcAccidental(newIndex, flat);
      var note = new Note((byte)newIndex, newAccidental);
      return note;
    }

    #endregion

    #region Overrides

    #region Equality members

    public bool Equals( Note other )
    {
      if( ReferenceEquals(null, other) )
        return false;

      if( ReferenceEquals(this, other) )
        return true;

      return Index == other.Index;
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals(null, obj) )
        return false;

      if( ReferenceEquals(this, obj) )
        return true;

      if( obj.GetType() != GetType() )
        return false;

      return Equals((Note)obj);
    }

    public override int GetHashCode()
    {
      return Index.GetHashCode();
    }

    public static bool operator ==( Note left, Note right )
    {
      return Equals(left, right);
    }

    public static bool operator !=( Note left, Note right )
    {
      return !Equals(left, right);
    }

    #endregion

    public override string ToString()
    {
      int pos = Index % INTERVAL_COUNT;
      if( Accidental == Accidental.Flat )
        return s_flatRepresentations[pos] + Octave;

      return s_sharpRepresentations[pos] + Octave;
    }

    #endregion

    #region Implementation

    private static Accidental CalcAccidental( int index, bool flat )
    {
      var newAccidental = Accidental.None;
      int pos = index % INTERVAL_COUNT;
      if( s_sharpRepresentations[pos].Length > 1 )
        newAccidental = flat ? Accidental.Flat : Accidental.Sharp;

      return newAccidental;
    }

    #endregion
  }
}
