// 
//   AccidentalExtensionsTest.cs: 
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

namespace Intercode.MusicLib.Test
{
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    This is a test class for AccidentalExtensionsTest and is intended
   ///    to contain all AccidentalExtensionsTest Unit Tests
   /// </summary>
   [ TestClass() ]
   public class AccidentalExtensionsTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      /// <summary>
      ///    A test for IsPrefix
      /// </summary>
      [ TestMethod ]
      public void IsPrefixTest()
      {
         Assert.IsTrue(Accidental.DoubleFlat.IsPrefix());
         Assert.IsTrue(Accidental.Flat.IsPrefix());
         Assert.IsFalse(Accidental.Natural.IsPrefix());
         Assert.IsFalse(Accidental.Sharp.IsPrefix());
         Assert.IsFalse(Accidental.DoubleSharp.IsPrefix());
      }

      /// <summary>
      ///    A test for ToSymbol
      /// </summary>
      [ TestMethod ]
      public void ToSymbolTest()
      {
         Assert.AreEqual(Accidental.DoubleFlat.ToSymbol(), "bb");
         Assert.AreEqual(Accidental.Flat.ToSymbol(), "b");
         Assert.AreEqual(Accidental.Natural.ToSymbol(), "");
         Assert.AreEqual(Accidental.Sharp.ToSymbol(), "#");
         Assert.AreEqual(Accidental.DoubleSharp.ToSymbol(), "##");
      }
   }
}
