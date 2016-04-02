// 
// Tuning.cs: 
// 
// Author: evelasquez
// 
// Copyright (c) 2014  Intercode Consulting, LLC.  All Rights Reserved.
// 
// Unauthorized use, duplication or distribution of this software, 
// or any portion of it, is prohibited.  
// 
// http://www.intercodeconsulting.com

namespace Bach.Model.Instruments
{
   using System;
   using System.Diagnostics.Contracts;
   using System.Linq;

   public class Tuning: IEquatable<Tuning>
   {
      private static readonly StringComparer s_nameComparer = StringComparer.CurrentCultureIgnoreCase;

      #region Construction

      public Tuning(StringedInstrument instrument, string name, params Note[] notes)
      {
         Contract.Requires<ArgumentNullException>(instrument != null, "instrument");
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentOutOfRangeException>(notes.Length == instrument.StringCount, "notes.Length");

         Instrument = instrument;
         Name = name;
         Notes = notes;
      }

      public Tuning(StringedInstrument instrument, string name, NoteCollection notes)
      {
         Contract.Requires<ArgumentNullException>(instrument != null, "instrument");
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentOutOfRangeException>(notes.Count == instrument.StringCount, "notes.Length");

         Instrument = instrument;
         Name = name;
         Notes = notes.ToArray();
      }

      #endregion

      #region Properties

      public StringedInstrument Instrument { get; private set; }
      public string Name { get; private set; }
      public Note[] Notes { get; private set; }

      #endregion

      #region IEquatable<Tuning> Members

      public bool Equals(Tuning other)
      {
         if( ReferenceEquals(null, other) )
            return false;

         if( ReferenceEquals(this, other) )
            return true;

         return Instrument.Equals(other.Instrument) && s_nameComparer.Equals(Name, other.Name)
                && Notes.SequenceEqual(other.Notes);
      }

      #endregion

      #region Overrides

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(null, obj) )
            return false;

         if( ReferenceEquals(this, obj) )
            return true;

         return obj.GetType() == GetType() && Equals((Tuning)obj);
      }

      public override int GetHashCode()
      {
         int hash = 17;
         hash = hash * 23 + Instrument.GetHashCode();
         hash = hash * 23 + s_nameComparer.GetHashCode(Name);
         return hash;
      }

      #endregion
   }
}
