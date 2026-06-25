namespace Bach.Model;

using System.Collections.Generic;
using System.Linq;

/// <summary>Represents a sequence of chords forming a progression.</summary>
public sealed class ChordProgression
{
  /// <summary>Initializes a new instance of the <see cref="ChordProgression"/> class.</summary>
  /// <param name="chords">The chords in the progression.</param>
  public ChordProgression( IReadOnlyList<Chord> chords )
  {
    ArgumentNullException.ThrowIfNull( chords );

    Chords = chords.ToList();
  }

  /// <summary>Gets the chords in the progression.</summary>
  public IReadOnlyList<Chord> Chords { get; }

  /// <inheritdoc />
  public override string ToString()
  {
    return string.Join( ", ", Chords.Select( chord => chord.Name ) );
  }
}
