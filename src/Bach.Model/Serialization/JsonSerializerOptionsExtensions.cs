using System.Text.Json;

namespace Bach.Model.Serialization;

/// <summary>
/// Provides extension methods for configuring JSON serialization for model events.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
  /// <summary>
  /// Adds the converters for serializing and deserializing part events.
  /// </summary>
  /// <param name="options">The options to configure.</param>
  /// <returns>The configured options.</returns>
  public static JsonSerializerOptions AddConverters(
    this JsonSerializerOptions options )
  {
    ArgumentNullException.ThrowIfNull( options );

    options.Converters.Add( new Internal.PartEventJsonConverter() );
    return options;
  }
}
