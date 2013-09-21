// 
//   IFormula.cs: 
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

namespace Bach.Model
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;

   [ ContractClass(typeof( ContractForIFormula )) ]
   public interface IFormula
   {
      int Count { get; }
      IEnumerable<Note> Generate(Note root);
   }

   [ ContractClassFor(typeof( IFormula )) ]
   internal abstract class ContractForIFormula: IFormula
   {
      public int Count { get; private set; }

      public IEnumerable<Note> Generate(Note root)
      {
         Contract.Requires<ArgumentException>(root.IsValid, "root");
         return default(IEnumerable<Note>);
      }
   }
}
