// 
//   ScaleFormula.cs: 
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
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;

   public class ScaleFormula
   {
      #region Constants

      public static readonly ScaleFormula Major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 1);
      public static readonly ScaleFormula NaturalMinor = new ScaleFormula("Natural Minor", 2, 1, 2, 2, 1, 2, 2);
      public static readonly ScaleFormula HarmonicMinor = new ScaleFormula("Harmonic Minor", 2, 1, 2, 2, 1, 3, 1);
      public static readonly ScaleFormula MelodicMinor = new ScaleFormula("Melodic Minor", 2, 1, 2, 2, 2, 2, 1);
      public static readonly ScaleFormula Diminished = new ScaleFormula("Diminished", 2, 1, 2, 1, 2, 1, 2, 1);
      public static readonly ScaleFormula Polytonal = new ScaleFormula("Polytonal", 1, 2, 1, 2, 1, 2, 1, 2);
      public static readonly ScaleFormula Pentatonic = new ScaleFormula("Pentatonic", 2, 2, 3, 2, 3);
      public static readonly ScaleFormula Blues = new ScaleFormula("Blues", 3, 2, 1, 1, 3, 2);
      public static readonly ScaleFormula Gospel = new ScaleFormula("Gospel", 2, 1, 1, 3, 2, 3);

      #endregion

      #region Construction

      public ScaleFormula(string name, params int[] intervals)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentException>(intervals.Length > 0, "intervals");

         Name = name;
         Intervals = intervals;
      }

      #endregion

      #region Properties

      public String Name { get; private set; }

      public Int32 Count
      {
         get { return Intervals.Length; }
      }

      public Int32[] Intervals { get; private set; }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
      }

      #endregion

      #region Public Methods

      public IEnumerable<Note> GetNotes(Note root, AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
      {
         int index = 0;
         Note current = root;

         while( true )
         {
            yield return current;

            Note previous = current;

            index %= Intervals.Length;
            int interval = Intervals[index];
            current = current.Add(interval, accidentalMode);
            //if( !current.TryNext(interval, out current) )
            //   break;

            //if( current.ToneWithoutAccidental == previous.ToneWithoutAccidental )
            //   current = current.AsFlat();

            ++index;
         }
      }

      #endregion
   }
}
