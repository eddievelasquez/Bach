//  
// Module Name: ScaleFormula.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
// 
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
//  portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  public class ScaleFormula: Formula
  {
    #region Data Members

    public static readonly ScaleFormula Major = new ScaleFormula("Major", "Major", "1,2,3,4,5,6,7");

    public static readonly ScaleFormula NaturalMinor = new ScaleFormula("NaturalMinor", "Natural Minor",
                                                                        "1,2,m3,4,5,m6,m7");

    public static readonly ScaleFormula HarmonicMinor = new ScaleFormula("HarmonicMinor", "Harmonic Minor",
                                                                         "1,2,m3,4,5,m6,7");

    public static readonly ScaleFormula MelodicMinor = new ScaleFormula("MelodicMinor", "Melodic Minor",
                                                                        "1,2,m3,4,5,6,7");

    public static readonly ScaleFormula Diminished = new ScaleFormula("Diminished", "Diminished", "1,2,m3,4,d5,A5,6,7");
    public static readonly ScaleFormula Polytonal = new ScaleFormula("Polytonal", "Polytonal", "1,m2,m3,d4,A4,5,6,m7");
    public static readonly ScaleFormula WholeTone = new ScaleFormula("WholeTone", "Whole Tone", "1,2,3,A4,A5,A6");
    public static readonly ScaleFormula Pentatonic = new ScaleFormula("Pentatonic", "Pentatonic", "1,2,3,5,6");

    public static readonly ScaleFormula MinorPentatonic = new ScaleFormula("MinorPentatonic", "Minor Pentatonic",
                                                                           "1,m3,4,5,m7");

    public static readonly ScaleFormula Blues = new ScaleFormula("Blues", "Blues", "1,m3,4,d5,5,m7");
    public static readonly ScaleFormula Gospel = new ScaleFormula("Gospel", "Gospel", "1,2,m3,3,5,6");

    #endregion

    #region Construction/Destruction

    public ScaleFormula(string key, string name, Interval[] intervals)
      : base(key, name, intervals)
    {
    }

    public ScaleFormula(string key, string name, string formula)
      : base(key, name, formula)
    {
    }

    #endregion
  }
}
