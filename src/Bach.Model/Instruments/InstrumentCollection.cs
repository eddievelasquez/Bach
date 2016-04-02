// 
// InstrumentCollection.cs: 
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

   public class InstrumentCollection: KeyedCollection<string, Instrument>
   {
      #region Construction

      public InstrumentCollection()
         : base(StringComparer.CurrentCultureIgnoreCase)
      {
      }

      #endregion

      #region Overrides

      protected override string GetKeyForItem(Instrument item)
      {
         return item.Name;
      }

      #endregion
   }
}
