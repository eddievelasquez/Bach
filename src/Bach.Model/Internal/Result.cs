// Module Name: ${File.FileName}
// Project:     ${File.ProjectName}
// Copyright (c) 2012, ${CurrentDate.Year}  Eddie Velasquez.
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

namespace Bach.Model.Internal;

internal sealed class Result<T>
{
  private Result(T value, string? error, bool isSuccess)
  {
    Value = value;
    Error = error;
    IsSuccess = isSuccess;
  }

  public T Value
  {
    get => IsSuccess ? field : throw new InvalidOperationException( "Cannot access Value when Result is a failure." );
    init;
  }

  public string? Error { get; }
  public bool IsSuccess { get; }

  public static Result<T> Ok(T value)
  {
    return new Result<T>(value, null, true);
  }

  public static Result<T> Fail(
    string error )
  {
    return new Result<T>(default!, error, false);
  }
}
