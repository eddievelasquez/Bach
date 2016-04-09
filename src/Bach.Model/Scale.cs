//  
// Module Name: Scale.cs
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
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics.Contracts;
  using System.Linq;
  using System.Text;

  public class Scale: IEquatable<Scale>,
                      IEnumerable<Note>
  {
    #region Construction/Destruction

    public Scale(Note root, Formula formula)
    {
      Contract.Requires<ArgumentNullException>(root != null);
      Contract.Requires<ArgumentNullException>(formula != null);

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

    public Note Root { get; }
    public string Name { get; }
    public Formula Formula { get; }

    #endregion

    #region IEnumerable<Note> Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<Note> GetEnumerator()
    {
      return Formula.Generate(Root).GetEnumerator();
    }

    #endregion

    #region IEquatable<Scale> Members

    public bool Equals(Scale other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return Root.Equals(other.Root) && Formula.Equals(other.Formula);
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

      return Equals((Scale) other);
    }

    public override int GetHashCode()
    {
      int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
      return hashCode;
    }

    public override string ToString()
    {
      return NoteCollection.ToString(this.Take(Formula.Count));
    }

    #endregion
  }
}
