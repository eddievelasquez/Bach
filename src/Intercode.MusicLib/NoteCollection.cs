// 
//   NoteCollection.cs: 
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
   using System.Collections.ObjectModel;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class NoteCollection: Collection<Note>, IEquatable<IEnumerable<Note>>
   {
      #region Construction

      public NoteCollection()
      {
      }

      public NoteCollection(IList<Note> list)
         : base(list)
      {
      }

      #endregion

      #region Public Methods

      public static bool TryParse(string value, out NoteCollection notes, int defaultOctave = 4)
      {
         Contract.Requires<ArgumentNullException>(value != null, "value");
         Contract.Requires<ArgumentException>(value.Length > 0, "value");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave >= Note.MIN_OCTAVE, "defaultOctave");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave <= Note.MAX_OCTAVE, "defaultOctave");

         notes = new NoteCollection();

         foreach( var s in value.Split(',') )
         {
            Note note;
            if( !Note.TryParse(s, out note, defaultOctave) )
            {
               notes = null;
               return false;
            }

            notes.Add(note);
         }

         return true;
      }

      public static NoteCollection Parse(string value, int defaultOctave = 4)
      {
         Contract.Requires<ArgumentNullException>(value != null, "value");
         Contract.Requires<ArgumentException>(value.Length > 0, "value");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave >= Note.MIN_OCTAVE, "defaultOctave");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave <= Note.MAX_OCTAVE, "defaultOctave");

         NoteCollection notes;
         if( !TryParse(value, out notes, defaultOctave) )
            throw new FormatException(String.Format("{0} contains invalid notes", value));

         return notes;
      }

      public static string ToString(IEnumerable<Note> notes)
      {
         Contract.Requires<ArgumentNullException>(notes != null, "notes");

         var buf = new StringBuilder();
         bool needsComma = false;

         foreach( var note in notes )
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

      #region Overrides

      public override string ToString()
      {
         return ToString(this);
      }

      #endregion

      #region IEquatable<IEnumerable<Note>> Implementation

      public bool Equals(IEnumerable<Note> other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return this.SequenceEqual(other);
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((NoteCollection)other);
      }

      public override int GetHashCode()
      {
         const int MULTIPLIER = 89;

         var first = this.FirstOrDefault();
         var last = this.LastOrDefault();

         unchecked
         {
            int result = ((first.GetHashCode() + Count) * MULTIPLIER) + (last.GetHashCode() + Count);
            return result;
         }
      }

      #endregion
   }
}
