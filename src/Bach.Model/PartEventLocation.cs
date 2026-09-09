namespace Bach.Model;

/// <summary>
///   Identifies an event by its zero-based measure and event indexes.
/// </summary>
/// <param name="MeasureIndex">The zero-based measure index.</param>
/// <param name="EventIndex">The zero-based event index within the measure.</param>
public readonly record struct PartEventLocation(
  int MeasureIndex,
  int EventIndex );
