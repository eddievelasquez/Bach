namespace Intercode.MusicLib
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.Contracts;

  public class Scale
  {
    #region Constants

    public static readonly Scale Major = new Scale("Major", 2, 2, 1, 2, 2, 2);

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
    private Int32[] Intervals { get; set; }

    #endregion

    public IEnumerable<Note> GetNotes( Note root )
    {
      yield return root;

      Note current = root;
      foreach( var interval in Intervals )
      {
        current = current.Next(interval, false);
        yield return current;
      }
    }
  }
}
