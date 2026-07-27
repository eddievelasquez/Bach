namespace Bach.Model;

/// <summary>Marks a musical event that may be stored in a <see cref="Part" />.</summary>
public interface IPartEvent
{
  /// <summary>
  ///  Gets the pitch classes contained in the event.
  /// </summary>
  PitchClass[] PitchClasses { get; }
}
