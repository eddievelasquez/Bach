// 
// Bass.cs: 
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
   public class Bass: StringedInstrument
   {
      #region Construction

      public Bass()
         : base("Bass", 4)
      {
         Tunings.Add(new Tuning(this, "Standard", NoteCollection.Parse("G2,D2,A1,E1")));
      }

      #endregion
   }
}
