//  
// Module Name: GuitarDefinition.cs
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

namespace Bach.Model.Instruments
{
  public class GuitarDefinition: StringedInstrumentDefinition
  {
    #region Construction/Destruction

    public GuitarDefinition()
      : base("Guitar", 6)
    {
      Tunings.Add(new Tuning(this, "Standard", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,E2")));
      Tunings.Add(new Tuning(this, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
      Tunings.Add(new Tuning(this, "Open D", AbsoluteNoteCollection.Parse("D4,A3,F#3,D3,A2,D2")));
      Tunings.Add(new Tuning(this, "Open G", AbsoluteNoteCollection.Parse("D4,B3,G2,D2,G2,D2")));
      Tunings.Add(new Tuning(this, "Open A", AbsoluteNoteCollection.Parse("E4,C#4,A3,E3,A2,E2")));
    }

    #endregion
  }
}
