using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Bach.Model.Library;

/// <summary>
///   Provides read-only iteration and lookup by object identifier or name.
/// </summary>
/// <typeparam name="T">The named object type.</typeparam>
public interface INamedObjectCollection<T>: IReadOnlyCollection<T>
  where T: INamedObject
{
  /// <summary>
  ///   Gets an item by its identifier or name.
  /// </summary>
  /// <param name="idOrName">The item identifier or name.</param>
  /// <returns>The matching item.</returns>
  /// <exception cref="KeyNotFoundException">The identifier or name is not found.</exception>
  T this[
    string idOrName ] { get; }

  /// <summary>
  ///   Tries to get an item by its identifier or name.
  /// </summary>
  /// <param name="idOrName">The item identifier or name.</param>
  /// <param name="item">The matching item, or the default value if no item is found.</param>
  /// <returns><see langword="true"/> if an item is found; otherwise, <see langword="false"/>.</returns>
  bool TryGetValue(
    string idOrName,
    [MaybeNullWhen( false )] out T item );

  /// <summary>
  ///   Tries to get an item by its identifier or name.
  /// </summary>
  /// <param name="idOrName">The item identifier or name.</param>
  /// <param name="item">The matching item, or the default value if no item is found.</param>
  /// <returns><see langword="true"/> if an item is found; otherwise, <see langword="false"/>.</returns>
  bool TryGetValue(
    ReadOnlySpan<char> idOrName,
    [MaybeNullWhen( false )] out T item );
}
