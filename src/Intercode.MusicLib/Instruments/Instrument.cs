// 
// Instrument.cs: 
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

   public abstract class Instrument: IEquatable<Instrument>
   {
      private static readonly StringComparer s_nameComparer = StringComparer.CurrentCultureIgnoreCase;
      private static readonly InstrumentCollection s_instruments;

      #region Construction

      static Instrument()
      {
         s_instruments = new InstrumentCollection { new Guitar(), new Bass() };
      }

      protected Instrument(string name)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");

         Name = name;
      }

      #endregion

      #region Properties

      public string Name { get; private set; }

      public static InstrumentCollection Instruments
      {
         get { return s_instruments; }
      }

      #endregion

      #region IEquatable<Instrument> Members

      public bool Equals(Instrument other)
      {
         if( ReferenceEquals(null, other) )
            return false;

         if( ReferenceEquals(this, other) )
            return true;

         return s_nameComparer.Equals(Name, other.Name);
      }

      #endregion

      #region Overrides

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(null, obj) )
            return false;

         if( ReferenceEquals(this, obj) )
            return true;

         if( obj.GetType() != GetType() )
            return false;

         return Equals((Instrument)obj);
      }

      public override int GetHashCode()
      {
         return s_nameComparer.GetHashCode(Name);
      }

      #endregion
   }
}
