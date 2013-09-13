// 
//   Chord.cs: 
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

namespace Intercode.MusicLib
{
   using System;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class Chord: IEquatable<Chord>
   {
      #region Construction

      public Chord(Note root, ChordFormula formula)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");
         Contract.Requires<ArgumentNullException>(formula != null, "formula");

         Root = root;
         Formula = formula;

         var buf = new StringBuilder();
         buf.Append(root.Tone);
         buf.Append(root.Accidental.ToSymbol());
         buf.Append(formula.Symbol);

         Name = buf.ToString();
         Notes = formula.Formula.Generate(root).ToArray();
      }

      #endregion

      #region Properties

      public Note Root { get; private set; }
      public string Name { get; private set; }
      public ChordFormula Formula { get; private set; }
      public Note[] Notes { get; private set; }

      #endregion

      #region IEquatable<Chord> Implementation

      public bool Equals(Chord other)
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

         return Equals((Chord)other);
      }

      public override int GetHashCode()
      {
         int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
         return hashCode;
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         var buf = new StringBuilder();
         bool needsComma = false;

         foreach( var note in Notes )
         {
            if( needsComma )
               buf.Append(',');
            else
               needsComma = true;

            buf.Append(note);
         }

         return buf.ToString();
      }

      #endregion
   }
}
