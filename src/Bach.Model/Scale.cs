// 
//   Scale.cs: 
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
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Text;

   public class Scale: IEquatable<Scale>, IEnumerable<Note>
   {
      #region Construction

      public Scale(Note root, Formula formula)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");
         Contract.Requires<ArgumentNullException>(formula != null, "formula");

         Root = root;
         Formula = formula;

         var buf = new StringBuilder();
         buf.Append(root.Tone);
         buf.Append(root.Accidental.ToSymbol());
         buf.Append(' ');
         buf.Append(formula.Name);

         Name = buf.ToString();
      }

      #endregion

      #region Properties

      public Note Root { get; private set; }
      public string Name { get; private set; }
      public Formula Formula { get; private set; }

      #endregion

      #region IEquatable<Scale> Members

      public bool Equals(Scale other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return Root.Equals(other.Root) && Formula.Equals(other.Formula);
      }

      #endregion

      #region Overrides

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((Scale)other);
      }

      public override int GetHashCode()
      {
         int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
         return hashCode;
      }

      public override string ToString()
      {
         return NoteCollection.ToString(this);
      }

      #endregion

      #region IEnumerable<Note> Implementation

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public IEnumerator<Note> GetEnumerator()
      {
         return Formula.Generate(Root).GetEnumerator();
      }

      #endregion
   }
}
