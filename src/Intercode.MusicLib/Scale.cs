namespace Intercode.MusicLib
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.Contracts;

  public class Scale
  {
    #region Constants

    public static readonly Scale Major = new Scale("Major", 2, 2, 1, 2, 2, 2, 1);
    public static readonly Scale NaturalMinor = new Scale("Natural Minor", 2, 1, 2, 2, 1, 2, 2);
    public static readonly Scale HarmonicMinor = new Scale("Harmonic Minor", 2, 1, 2, 2, 1, 3, 1);
    public static readonly Scale MelodicMinor = new Scale("Melodic Minor", 2, 1, 2, 2, 2, 2, 1);
    public static readonly Scale Diminished = new Scale("Diminished", 2, 1, 2, 1, 2, 1, 2, 1);
    public static readonly Scale Polytonal = new Scale("Polytonal", 1, 2, 1, 2, 1, 2, 1, 2);
    public static readonly Scale Pentatonic = new Scale("Pentatonic", 2, 2, 3, 2, 3);
    public static readonly Scale Blues = new Scale("Blues", 3, 2, 1, 1, 3, 2);
    public static readonly Scale Gospel = new Scale("Gospel", 2, 1, 1, 3, 2, 3);

    #endregion

    #region Construction

    public Scale( string name, params int[] intervals )
    {
      Contract.Requires<ArgumentNullException>(name != null, "name");
      Contract.Requires<ArgumentException>(name.Length > 0, "name");
      Contract.Requires<ArgumentException>(intervals.Length > 0, "intervals");

      Name = name;
      Intervals = intervals;
    }

    #endregion

    #region Properties

    public String Name { get; private set; }

    public Int32 NoteCount
    {
      get { return Intervals.Length; }
    }

    private Int32[] Intervals { get; set; }

    #endregion

    #region Public Methods

    public IEnumerable<Note> GetNotes( Note root )
    {
      int index = 0;
      Note current = root;

      while( true )
      {
        yield return current;

        Note previous = current;

        index %= Intervals.Length;
        int interval = Intervals[index];
        if( !current.TryNext(interval, out current) )
          break;

        if( current.ToneWithoutAccidental == previous.ToneWithoutAccidental )
          current = current.AsFlat();

        ++index;
      }
    }

    #endregion

    #region Object Members

    public override string ToString()
    {
      return Name;
    }

    #endregion
  }
}
