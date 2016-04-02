// 
// GuitarTest.cs: 
// 
// Author: evelasquez
// 
// Copyright (c) 2014  Intercode Consulting, LLC.  All Rights Reserved.
// 
// Unauthorized use, duplication or distribution of this software, 
// or any portion of it, is prohibited.  
// 
// http://www.intercodeconsulting.com

namespace Bach.Model.Test.Instruments
{
   using System;
   using Bach.Model.Instruments;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class GuitarTest
   {
      [ TestMethod ]
      public void TestConstructor()
      {
         var guitar = new Guitar();
         Assert.AreEqual(guitar.Name, "Guitar");
         Assert.AreEqual(guitar.StringCount, 6);
         Assert.IsNotNull(guitar.Tunings);
         Assert.AreNotEqual(guitar.Tunings.Count, 0);
      }

      [ TestMethod ]
      public void AddTuningTest()
      {
         var guitar = new Guitar();
         int tuningCount = guitar.Tunings.Count;
         guitar.Tunings.Add(new Tuning(guitar, "Drop D Test", NoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
         Assert.AreEqual(tuningCount + 1, guitar.Tunings.Count);
      }

      [ TestMethod ]
      [ ExpectedException(typeof( ArgumentException )) ]
      public void AddTuningFailsWithDifferentInstrumentTuningTest()
      {
         var guitar = new Guitar();
         guitar.Tunings.Add(new Tuning(new Bass(), "Drop D", NoteCollection.Parse("G2,D2,A1,D1")));
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Guitar();
         object y = new Guitar();
         object z = new Guitar();

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
         var x = new Guitar();
         var y = new Guitar();
         var z = new Guitar();

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
         object a = new Guitar();
         object b = new Bass();
         Assert.IsFalse(a.Equals(b));
         Assert.IsFalse(b.Equals(a));
         Assert.IsFalse(Equals(a, b));
         Assert.IsFalse(Equals(b, a));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var a = new Guitar();
         var b = new Bass();
         Assert.IsFalse(a.Equals(b));
         Assert.IsFalse(b.Equals(a));
         Assert.IsFalse(Equals(a, b));
         Assert.IsFalse(Equals(b, a));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Guitar();
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Guitar();
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Guitar();
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Guitar();
         var expected = new Guitar();
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
