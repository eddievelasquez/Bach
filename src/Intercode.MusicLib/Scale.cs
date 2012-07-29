namespace Intercode.MusicLib
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Diagnostics.Contracts;

  public class Scale
  {
    #region Constants

    public static readonly Scale Major = new Scale("Major", 2, 2, 1, 2, 2, 2, 1);
    public static readonly Scale NaturalMinor = new Scale("Natural Minor", 2, 1, 2, 2, 1, 2, 2);

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

        if ( current.ToneWithoutAccidental == previous.ToneWithoutAccidental)
          current = current.AsFlat();

        ++index;
      }
    }

    #endregion
  }
}
