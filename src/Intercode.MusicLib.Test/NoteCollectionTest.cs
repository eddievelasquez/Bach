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
   using System;
   using System.Linq;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class NoteCollectionTest
   {
      [ TestMethod ]
      public void ParseWithNotesTest()
      {
         var expected = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         var actual = NoteCollection.Parse("C4,C5");
         Assert.IsTrue(actual.SequenceEqual(expected));
      }

      [ TestMethod ]
      public void ParseWithMidiTest()
      {
         var expected = new NoteCollection(new[] { Note.FromMidi(60), Note.FromMidi(70) });
         var actual = NoteCollection.Parse("60,70");
         Assert.IsTrue(actual.SequenceEqual(expected));
      }

      [ TestMethod ]
      public void ToStringTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.AreEqual("C4,C5", actual.ToString());
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         object y = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         object z = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });

         Assert.IsTrue(x.Equals(x)); // Reflexive
         Assert.IsTrue(x.Equals(y)); // Symetric
         Assert.IsTrue(y.Equals(x));
         Assert.IsTrue(y.Equals(z)); // Transitive
         Assert.IsTrue(x.Equals(z));
         Assert.IsFalse(x.Equals(null)); // Never equal to null
      }

      [ TestMethod ]
      public void TypeSafeEqualsContractTest()
      {
         var x = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         var y = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         var z = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });

         Assert.IsTrue(x.Equals(x)); // Reflexive
         Assert.IsTrue(x.Equals(y)); // Symetric
         Assert.IsTrue(y.Equals(x));
         Assert.IsTrue(y.Equals(z)); // Transitive
         Assert.IsTrue(x.Equals(z));
         Assert.IsFalse(x.Equals(null)); // Never equal to null
      }

      [ TestMethod ]
      public void EqualsFailsWithDifferentTypeTest()
      {
         object actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         var expected = new NoteCollection(new[] { Note.Parse("C4"), Note.Parse("C5") });
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
