// 
// TuningCollection.cs: 
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
   using System.Collections.ObjectModel;
   using System.Diagnostics.Contracts;

   public class TuningCollection: KeyedCollection<String, Tuning>
   {
      #region Construction

      public TuningCollection(StringedInstrument instrument)
      {
         Contract.Requires<ArgumentNullException>(instrument != null, "instrument");
         Instrument = instrument;
      }

      #endregion

      #region Properties

      public StringedInstrument Instrument { get; private set; }

      #endregion

      #region Overrides

      protected override void InsertItem(int index, Tuning item)
      {
         VerifyInstrument(item);
         base.InsertItem(index, item);
      }

      protected override void SetItem(int index, Tuning item)
      {
         VerifyInstrument(item);
         base.SetItem(index, item);
      }

      protected override string GetKeyForItem(Tuning item)
      {
         return item.Name;
      }

      #endregion

      #region Implementation

      private void VerifyInstrument(Tuning tuning)
      {
         if( !Instrument.Equals(tuning.Instrument) )
            throw new ArgumentException(String.Format("\"{0}\" is not a \"{1}\" tuning", tuning.Name, Instrument.Name));
      }

      #endregion
   }
}
