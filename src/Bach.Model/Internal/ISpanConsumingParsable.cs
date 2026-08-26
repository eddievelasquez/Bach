// Module Name: ISpanConsumingParsable.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2026  Eddie Velasquez.
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

using System.Diagnostics.CodeAnalysis;

namespace Bach.Model.Internal;

/// <summary>
///   Defines a contract for types that can parse an instance of <typeparamref name="TSelf"/>
///   from a <see cref="ReadOnlySpan{T}"/> of characters while also reporting the unconsumed
///   remainder of the input.
/// </summary>
/// <typeparam name="TSelf">
///   The type that implements this interface. This type must provide a static
///   <c>TryParse</c> method that consumes a portion of the input span and returns the
///   parsed value along with the remaining span.
/// </typeparam>
/// <remarks>
///   <para>
///     <see cref="ISpanConsumingParsable{TSelf}"/> extends <see cref="ISpanParsable{TSSelf}"/>
///     by enabling incremental or chained parsing scenarios. Implementations are expected
///     to parse a prefix of the supplied character span and return both the parsed result
///     and the portion of the span that was not consumed.
///   </para>
///   <para>
///     This interface is intended for high‑performance parsing workflows that operate
///     directly on spans without allocating intermediate strings. It is particularly
///     useful for tokenizers, interpreters, and other streaming or sequential parsing
///     components.
///   </para>
/// </remarks>
public interface ISpanConsumingParsable<TSelf>: ISpanParsable<TSelf>
  where TSelf: ISpanConsumingParsable<TSelf>?
{
  #region Public Methods

  /// <summary>
  ///   Attempts to parse a value of type <typeparamref name="TSelf"/> from the specified
  ///   character span, returning both the parsed result and the remaining unconsumed
  ///   portion of the span.
  /// </summary>
  /// <param name="s">
  ///   The span of characters to parse. The method attempts to interpret a prefix of
  ///   this span as a value of type <typeparamref name="TSelf"/>.
  /// </param>
  /// <param name="provider">
  ///   An object that supplies culture‑specific formatting information. This parameter
  ///   may be <see langword="null"/> if culture‑specific behavior is not required.
  /// </param>
  /// <param name="result">
  ///   When this method returns, contains the parsed value if the operation succeeded,
  ///   or an undefined value if the operation failed.
  /// </param>
  /// <param name="tail">
  ///   When this method returns, contains the remaining portion of <paramref name="s"/>
  ///   that was not consumed during parsing. If the method returns <see langword="false"/>,
  ///   the value of this parameter is unspecified.
  /// </param>
  /// <returns>
  ///   <see langword="true"/> if the input span was successfully parsed; otherwise,
  ///   <see langword="false"/>.
  /// </returns>
  /// <remarks>
  ///   <para>
  ///     Implementations should consume only the characters necessary to parse a valid
  ///     instance of <typeparamref name="TSelf"/>. The <paramref name="tail"/> parameter
  ///     enables callers to perform additional parsing operations on the remaining span.
  ///   </para>
  ///   <para>
  ///     This method must not throw exceptions for invalid input. Instead, it should
  ///     return <see langword="false"/> and leave <paramref name="result"/> and
  ///     <paramref name="tail"/> in an unspecified state.
  ///   </para>
  /// </remarks>
  static abstract bool TryParse(
    ReadOnlySpan<char> s,
    IFormatProvider? provider,
    [NotNullWhen( true )] out TSelf? result,
    out ReadOnlySpan<char> tail );

  #endregion
}
