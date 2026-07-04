namespace Bach.Model.Internal;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides extension methods for <see cref="ArgumentException" /> to enhance argument validation.
/// </summary>
public static class ArgumentExceptionExtensions
{
  #region Nested Types

  extension(
    ArgumentException )
  {
    #region Implementation

    /// <summary>
    /// Throws an <see cref="ArgumentNullException" /> if the specified source is null, or an
    /// <see cref="ArgumentException" /> if the source is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to check.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the source is empty.</exception>
    public static void ThrowIfNullOrEmpty<T>(
      IEnumerable<T>? source,
      string? paramName = null )
    {
      if( source is null )
      {
        throw new ArgumentNullException( paramName );
      }

      if( !source.TryGetNonEnumeratedCount( out var count ) )
      {
        if( !source.Any() )
        {
          throw new ArgumentException( "Sequence must not be empty.", paramName );
        }
      }
      else if( count == 0 )
      {
        throw new ArgumentException( "Sequence must not be empty.", paramName );
      }
    }

    #endregion
  }

  #endregion
}
