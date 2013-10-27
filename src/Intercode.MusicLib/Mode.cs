// 
//   Mode.cs: 
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
   using System.Linq;
   using System.Text;

   public class Mode: IEquatable<Mode>, IEnumerable<Note>
   {
      #region Data Members

      private readonly NoteCollection _notes;

      #endregion

      #region Construction

      public Mode(Note root, ModeFormula formula)
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

         var major = Model.Formula.Major;
         _notes = new NoteCollection(new Scale(Root, major).Skip(Formula.Tonic - 1).Take(major.Count).ToArray());
      }

      #endregion

      #region Properties

      public Note Root { get; private set; }
      public string Name { get; private set; }
      public ModeFormula Formula { get; private set; }

      #endregion

      #region IEquatable<Mode> Implementation

      public bool Equals(Mode other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return Root.Equals(other.Root) && Formula.Equals(other.Formula);
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((Mode)other);
      }

      public override int GetHashCode()
      {
         int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
         return hashCode;
      }

      #endregion

      #region IEnumerable<Note> Implementation

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public IEnumerator<Note> GetEnumerator()
      {
         return _notes.GetEnumerator();
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return _notes.ToString();
      }

      #endregion
   }
}
