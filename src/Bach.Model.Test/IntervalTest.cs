// 
//   IntervalTest.cs: 
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
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    This is a test class for Interval and is intended
   ///    to contain all Interval Unit Tests
   /// </summary>
   [ TestClass ]
   public class IntervalTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      [ TestMethod ]
      public void StepTest()
      {
         Assert.AreEqual(Interval.Perfect1.Steps, 0);
         Assert.AreEqual(Interval.Augmented1.Steps, 1);
         Assert.AreEqual(Interval.Diminished2.Steps, 0);
         Assert.AreEqual(Interval.Minor2.Steps, 1);
         Assert.AreEqual(Interval.Major2.Steps, 2);
         Assert.AreEqual(Interval.Augmented2.Steps, 3);
         Assert.AreEqual(Interval.Diminished3.Steps, 2);
         Assert.AreEqual(Interval.Minor3.Steps, 3);
         Assert.AreEqual(Interval.Major3.Steps, 4);
         Assert.AreEqual(Interval.Augmented3.Steps, 5);
         Assert.AreEqual(Interval.Diminished4.Steps, 4);
         Assert.AreEqual(Interval.Perfect4.Steps, 5);
         Assert.AreEqual(Interval.Augmented4.Steps, 6);
         Assert.AreEqual(Interval.Diminished5.Steps, 6);
         Assert.AreEqual(Interval.Perfect5.Steps, 7);
         Assert.AreEqual(Interval.Augmented5.Steps, 8);
         Assert.AreEqual(Interval.Diminished6.Steps, 7);
         Assert.AreEqual(Interval.Minor6.Steps, 8);
         Assert.AreEqual(Interval.Major6.Steps, 9);
         Assert.AreEqual(Interval.Augmented6.Steps, 10);
         Assert.AreEqual(Interval.Diminished7.Steps, 9);
         Assert.AreEqual(Interval.Minor7.Steps, 10);
         Assert.AreEqual(Interval.Major7.Steps, 11);
         Assert.AreEqual(Interval.Augmented7.Steps, 12);
         Assert.AreEqual(Interval.Diminished8.Steps, 11);
         Assert.AreEqual(Interval.Perfect8.Steps, 12);
         Assert.AreEqual(Interval.Augmented8.Steps, 13);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Interval(5, IntervalQuality.Perfect);
         object y = new Interval(5, IntervalQuality.Perfect);
         object z = new Interval(5, IntervalQuality.Perfect);

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
         var x = new Interval(5, IntervalQuality.Perfect);
         var y = new Interval(5, IntervalQuality.Perfect);
         var z = new Interval(5, IntervalQuality.Perfect);

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
         object actual = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Interval(5, IntervalQuality.Perfect);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Interval(5, IntervalQuality.Perfect);
         var expected = new Interval(5, IntervalQuality.Perfect);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }

      [ TestMethod ]
      public void EqualitySucceedsWithTwoObjectsTest()
      {
         var lhs = new Interval(5, IntervalQuality.Perfect);
         var rhs = new Interval(5, IntervalQuality.Perfect);
         Assert.IsTrue(lhs == rhs);
      }

      [ TestMethod ]
      public void EqualitySucceedsWithSameObjectTest()
      {
#pragma warning disable 1718
         var lhs = new Interval(5, IntervalQuality.Perfect);
         Assert.IsTrue(lhs == lhs);
#pragma warning restore 1718
      }

      [ TestMethod ]
      public void EqualityFailsWithNullTest()
      {
         var lhs = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(lhs == null);
      }

      [ TestMethod ]
      public void InequalitySucceedsWithTwoObjectsTest()
      {
         var lhs = new Interval(5, IntervalQuality.Perfect);
         var rhs = new Interval(5, IntervalQuality.Augmented);
         Assert.IsTrue(lhs != rhs);
      }

      [ TestMethod ]
      public void InequalityFailsWithSameObjectTest()
      {
#pragma warning disable 1718
         var lhs = new Interval(5, IntervalQuality.Perfect);
         Assert.IsFalse(lhs != lhs);
#pragma warning restore 1718
      }

      [ TestMethod ]
      public void TryParseTest()
      {
         Interval actual;
         Assert.IsTrue(Interval.TryParse("P1", out actual));
         Assert.AreEqual(Interval.Perfect1, actual);
         Assert.IsTrue(Interval.TryParse("1", out actual));
         Assert.AreEqual(Interval.Perfect1, actual);
         Assert.IsTrue(Interval.TryParse("M2", out actual));
         Assert.AreEqual(Interval.Major2, actual);
         Assert.IsTrue(Interval.TryParse("2", out actual));
         Assert.AreEqual(Interval.Major2, actual);
         Assert.IsFalse(Interval.TryParse("M1", out actual));
         Assert.AreEqual(Interval.Invalid, actual);
         Assert.IsFalse(Interval.TryParse("P2", out actual));
         Assert.AreEqual(Interval.Invalid, actual);
      }
   }
}
