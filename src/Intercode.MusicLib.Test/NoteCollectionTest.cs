// 
//   NoteCollectionTest.cs: 
// 
//   Author: Eddie Velasquez
// 
//   Copyright (c) 2013  Intercode Consulting, LLC.  All Rights Reserved.
// 
//      Unauthorized use, duplication or distribution of this software, 
//      or any portion of it, is prohibited.  
// 
//   http://www.intercodeconsulting.com
// 

namespace Bach.Model.Test
{
   using System.Linq;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class NoteCollectionTest
   {
      [ TestMethod ]
      public void ParseTest()
      {
         var expected = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         var actual = NoteCollection.Parse("C4,C5");
         Assert.IsTrue(actual.SequenceEqual(expected));
      }

      [ TestMethod ]
      public void ToStringTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.AreEqual("C4,C5", actual.ToString());
      }

   }
}
