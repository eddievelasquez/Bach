// 
// StringedInstrument.cs: 
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

   public abstract class StringedInstrument: Instrument, IEquatable<StringedInstrument>
   {
      #region Construction

      protected StringedInstrument(string name, int stringCount)
         : base(name)
      {
         Contract.Requires<ArgumentOutOfRangeException>(stringCount > 0, "stringCount");

         StringCount = stringCount;
         Tunings = new TuningCollection(this);
      }

      #endregion

      #region Properties

      public int StringCount { get; private set; }
      public TuningCollection Tunings { get; private set; }

      #endregion

      #region IEquatable<StringedInstrument> Members

      public bool Equals(StringedInstrument other)
      {
         if( ReferenceEquals(null, other) )
            return false;

         if( ReferenceEquals(this, other) )
            return true;

         return StringCount == other.StringCount && base.Equals(other);
      }

      #endregion

      #region Overrides

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(null, obj) )
            return false;

         if( ReferenceEquals(this, obj) )
            return true;

         return obj.GetType() == GetType() && Equals((StringedInstrument)obj);
      }

      public override int GetHashCode()
      {
         int hash = 17;
         hash = hash * 23 + base.GetHashCode();
         hash = hash * 23 + StringCount;
         return hash;
      }

      #endregion
   }
}
