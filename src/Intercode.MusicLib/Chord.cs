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

namespace Bach.Model
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class Chord: IEquatable<Chord>, IEnumerable<Note>
   {
      #region Data Members

      private readonly NoteCollection _notes;

      #endregion

      #region Construction

      private Chord(Note root, ChordFormula formula, string name, IList<Note> notes)
      {
         Contract.Requires<ArgumentException>(root.IsValid, "root.IsValid");
         Contract.Requires<ArgumentNullException>(formula != null, "formula");
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentNullException>(notes != null, "notes");
         Contract.Requires<ArgumentException>(notes.Count > 0, "notes");

         Root = root;
         Formula = formula;
         Name = name;
         _notes = new NoteCollection(notes);
      }

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

         _notes = new NoteCollection(Formula.Generate(Root).ToArray());
      }

      #endregion

      #region Properties

      public Note Root { get; private set; }
      public string Name { get; private set; }
      public ChordFormula Formula { get; private set; }

      public ReadOnlyCollection<Note> Notes
      {
         get { return new ReadOnlyCollection<Note>(_notes); }
      }

      #endregion

      #region Public Methods

      public Chord Invert(int inversion = 1)
      {
         Contract.Requires<ArgumentOutOfRangeException>(inversion > 0, "inversion");

         var notes = Notes.ToList();
         while( inversion > 0 )
         {
            Note bass = notes[0] + Note.INTERVALS_PER_OCTAVE;
            notes.RemoveAt(0);
            notes.Add(bass);

            --inversion;
         }

         var inv = new Chord(Root, Formula, Name, notes);
         return inv;
      }

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
