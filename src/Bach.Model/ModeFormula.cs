//  
// Module Name: ModeFormula.cs
// Project:     Bach.Model
// Copyright (c) 2013  Eddie Velasquez.
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
  using System;
  using System.Diagnostics.Contracts;

  public class ModeFormula: IEquatable<ModeFormula>
  {
    #region Constants

    private const int MIN_TONIC = 1;
    private const int MAX_TONIC = 7;

    #endregion

    #region Data Members

    public static readonly ModeFormula Ionian = new ModeFormula("Ionian", 1);
    public static readonly ModeFormula Dorian = new ModeFormula("Dorian", 2);
    public static readonly ModeFormula Phrygian = new ModeFormula("Phrygian", 3);
    public static readonly ModeFormula Lydian = new ModeFormula("Lydian", 4);
    public static readonly ModeFormula Mixolydian = new ModeFormula("Mixolydian", 5);
    public static readonly ModeFormula Aeolian = new ModeFormula("Aeolian", 6);
    public static readonly ModeFormula Locrian = new ModeFormula("Locrian", 7);

    #endregion

    #region Construction/Destruction

    public ModeFormula(string name, int tonic)
    {
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(tonic >= MIN_TONIC);
      Contract.Requires<ArgumentOutOfRangeException>(tonic <= MAX_TONIC);

      Name = name;
      Tonic = tonic;
    }

    #endregion

    #region Properties

    public string Name { get; }
    public int Tonic { get; }

    #endregion

    #region IEquatable<ModeFormula> Members

    public bool Equals(ModeFormula other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return Tonic == other.Tonic;
    }

    #endregion

    #region Public Methods

    public override bool Equals(object other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) || other.GetType() != GetType() )
      {
        return false;
      }

      return Equals((ModeFormula) other);
    }

    public override int GetHashCode()
    {
      return Tonic;
    }

    public override string ToString()
    {
      return Name;
    }

    #endregion
  }
}
