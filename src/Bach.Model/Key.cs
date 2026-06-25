namespace Bach.Model;

/// <summary>Represents a tonal key defined by a tonic, mode, and key signature.</summary>
public sealed class Key
{
  /// <summary>Initializes a new instance of the <see cref="Key"/> class.</summary>
  /// <param name="tonic">The tonic pitch class for the key.</param>
  /// <param name="mode">The mode name, such as Major or Minor.</param>
  /// <param name="keySignature">The number of sharps or flats in the key signature.</param>
  public Key(
    PitchClass tonic,
    string mode,
    int keySignature )
  {
    ArgumentNullException.ThrowIfNull( mode );
    ArgumentException.ThrowIfNullOrWhiteSpace( mode );

    Tonic = tonic;
    Mode = NormalizeMode( mode );
    KeySignature = keySignature;
    Scale = new Scale( tonic, ResolveScaleFormula( Mode ) );
  }

  /// <summary>Gets the tonic pitch class.</summary>
  public PitchClass Tonic { get; }

  /// <summary>Gets the mode name.</summary>
  public string Mode { get; }

  /// <summary>Gets the number of sharps or flats in the key signature.</summary>
  public int KeySignature { get; }

  /// <summary>Gets the scale implied by the key.</summary>
  public Scale Scale { get; }

  /// <inheritdoc />
  public override string ToString()
  {
    return $"{Tonic} {Mode}";
  }

  private static string NormalizeMode( string mode )
  {
    return mode.Trim() switch
    {
      "major" => "Major",
      "minor" => "Minor",
      _ => mode.Trim()
    };
  }

  private static string ResolveScaleFormula( string mode )
  {
    return mode switch
    {
      "Minor" => "NaturalMinor",
      _ => mode
    };
  }
}
