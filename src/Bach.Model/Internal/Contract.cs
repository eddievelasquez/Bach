// Module Name: Contract.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Internal
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;

  internal static class Contract
  {
    #region Public Methods

    public static void Requires<TException>(bool condition,
                                            string message = null)
      where TException: Exception, new()
    {
      if( condition )
      {
        return;
      }

      //https://stackoverflow.com/questions/41397/asking-a-generic-method-to-throw-specific-exception-type-on-fail/41450#41450
      TException exception;

      try
      {
        message = message ?? "Unexpected Condition";
        exception = Activator.CreateInstance(typeof(TException), message) as TException;
      }
      catch( MissingMethodException )
      {
        exception = new TException();
      }

      Debug.Assert(exception != null, nameof(exception) + " != null");
      throw exception;
    }

    public static void RequiresNotNullOrEmpty<T>(ICollection<T> value,
                                                 string message = null)
    {
      Requires<ArgumentNullException>(value != null, message);
      Requires<ArgumentException>(value.Count > 0, message);
    }

    public static void RequiresNotNullOrEmpty(string value,
                                              string message = null)
    {
      Requires<ArgumentNullException>(value != null, message);
      Requires<ArgumentException>(value.Length > 0, message);
    }

    #endregion
  }
}
