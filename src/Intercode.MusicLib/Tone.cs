namespace Intercode.MusicLib
{
  using System;
  using System.Diagnostics.Contracts;

  public struct Tone : IEquatable<Tone>
  {
    public static readonly Tone A;
    public static readonly Tone ASharp;
    public static readonly Tone BFlat;
    public static readonly Tone B;
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

    private const int INTERVAL_COUNT = 12;

    private static readonly string[] s_sharpRepresentations;
    private static readonly string[] s_flatRepresentations;

    #region Construction

    static Tone()
    {
      Contract.Ensures( s_sharpRepresentations.Length == INTERVAL_COUNT );
      Contract.Ensures( s_flatRepresentations.Length == INTERVAL_COUNT );

      s_sharpRepresentations = new[] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
      s_flatRepresentations = new[] { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };

      A = new Tone( 0, false );
      ASharp = new Tone( 1, false );
      BFlat = new Tone( 1, true );
      B = new Tone( 2, false );
      C = new Tone( 3, false );
      CSharp = new Tone( 4, false );
      DFlat = new Tone( 4, true );
      D = new Tone( 5, false );
      DSharp = new Tone( 6, false );
      EFlat = new Tone( 6, true );
      E = new Tone( 7, false );
      F = new Tone( 8, false );
      FSharp = new Tone( 9, false );
      GFlat = new Tone( 9, true );
      G = new Tone( 10, false );
      GSharp = new Tone( 11, false );
      AFlat = new Tone( 11, true );
    }

    private Tone( byte index, bool isFlat )
      : this()
    {
      Contract.Requires<ArgumentOutOfRangeException>( index < s_sharpRepresentations.Length, "index" );

      Index = index;
      IsFlat = isFlat;
    }

    #endregion

    #region Properties

    private byte Index { get; set; }
    public bool IsFlat { get; private set; }

    #endregion

    #region Public Methods

    [ Pure ]
    public Tone AsSharp()
    {
      var tone = new Tone( Index, false );
      return tone;
    }

    [ Pure ]
    public Tone AsFlat()
    {
      var tone = new Tone( Index, true );
      return tone;
    }

    #endregion

    #region Internal Methods

    [ Pure ]
    internal Tone Next( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>( interval > 0, "interval" );
      interval %= INTERVAL_COUNT;

      int nextIndex = (Index + interval) % INTERVAL_COUNT;

      var tone = new Tone( (byte) nextIndex, flat );
      return tone;
    }

    [ Pure ]
    internal Tone Previous( int interval, bool flat )
    {
      Contract.Requires<ArgumentOutOfRangeException>( interval > 0, "interval" );
      interval %= INTERVAL_COUNT;

      int prevIndex = (Index + (INTERVAL_COUNT - interval)) % INTERVAL_COUNT;

      var tone = new Tone( (byte) prevIndex, flat );
      return tone;
    }

    #endregion

    #region IEquatable<Tone> Members

    public bool Equals( Tone other )
    {
      return Index == other.Index;
    }

    #endregion

    #region Overrides

    public override bool Equals( object obj )
    {
      if ( ReferenceEquals( null, obj ) )
        return false;

      return obj is Tone && Equals( (Tone) obj );
    }

    public override int GetHashCode()
    {
      return Index.GetHashCode();
    }

    public override string ToString()
    {
      return IsFlat ? s_flatRepresentations[Index] : s_sharpRepresentations[Index];
    }

    #endregion

    #region Operators

    public static bool operator ==( Tone left, Tone right )
    {
      return left.Equals( right );
    }

    public static bool operator !=( Tone left, Tone right )
    {
      return !left.Equals( right );
    }

    #endregion
  }
}
