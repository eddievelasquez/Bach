// 
// Guitar.cs: 
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
   public class Guitar: StringedInstrument
   {
      #region Construction

      public Guitar()
         : base("Guitar", 6)
      {
         Tunings.Add(new Tuning(this, "Standard", NoteCollection.Parse("E4,B3,G3,D3,A2,E2")));
      }

      #endregion
   }
}
