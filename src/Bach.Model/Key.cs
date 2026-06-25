using System.Collections.Generic;

namespace Bach.Model;

/// <summary>Represents a tonal key defined by a tonic, mode, and key signature.</summary>
public sealed class Key
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Key"/> class.
  /// </summary>
  /// <param name="tonic">The tonic pitch class.</param>
  /// <param name="mode">The mode of the key.</param>
  public Key(
    PitchClass tonic,
    ModeType mode )
  {
    Tonic = tonic;
    Mode = mode;
    Scale = new Scale( tonic, ResolveScaleFormula( mode ) );
  }

  /// <summary>Gets the tonic pitch class.</summary>
  public PitchClass Tonic { get; }

  /// <summary>Gets the mode for the key.</summary>
  public ModeType Mode { get; }

  /// <summary>Gets the key signature for the key.</summary>
  public KeySignature KeySignature => CalculateKeySignature( Tonic, Mode );

  /// <summary>Gets the scale implied by the key.</summary>
  public Scale Scale { get; }

  /// <inheritdoc />
  public override string ToString()
  {
    return $"{Tonic} {Mode}";
  }

  private static string ResolveScaleFormula( ModeType mode )
  {
    return mode switch
    {
      ModeType.Minor => "NaturalMinor",
      _ => "Major"
    };
  }

  private static readonly Dictionary<(PitchClass Tonic, ModeType Mode), KeySignature> s_keySignatureTable =
  new()
  {
    [ ( PitchClass.C, ModeType.Major ) ] = KeySignature.CMajor,
    [ ( PitchClass.G, ModeType.Major ) ] = KeySignature.GMajor,
    [ ( PitchClass.D, ModeType.Major ) ] = KeySignature.DMajor,
    [ ( PitchClass.A, ModeType.Major ) ] = KeySignature.AMajor,
    [ ( PitchClass.E, ModeType.Major ) ] = KeySignature.EMajor,
    [ ( PitchClass.B, ModeType.Major ) ] = KeySignature.BMajor,
    [ ( PitchClass.FSharp, ModeType.Major ) ] = KeySignature.FSharpMajor,
    [ ( PitchClass.F, ModeType.Major ) ] = KeySignature.FMajor,
    [ ( PitchClass.BFlat, ModeType.Major ) ] = KeySignature.BFlatMajor,
    [ ( PitchClass.EFlat, ModeType.Major ) ] = KeySignature.EFlatMajor,
    [ ( PitchClass.AFlat, ModeType.Major ) ] = KeySignature.AFlatMajor,
    [ ( PitchClass.DFlat, ModeType.Major ) ] = KeySignature.DFlatMajor,
    [ ( PitchClass.GFlat, ModeType.Major ) ] = KeySignature.GFlatMajor,
    [ ( PitchClass.B, ModeType.Major ) ] = KeySignature.CFlatMajor,
    [ ( PitchClass.A, ModeType.Minor ) ] = KeySignature.AMinor,
    [ ( PitchClass.E, ModeType.Minor ) ] = KeySignature.EMinor,
    [ ( PitchClass.B, ModeType.Minor ) ] = KeySignature.BMinor,
    [ ( PitchClass.FSharp, ModeType.Minor ) ] = KeySignature.FSharpMinor,
    [ ( PitchClass.CSharp, ModeType.Minor ) ] = KeySignature.CSharpMinor,
    [ ( PitchClass.GSharp, ModeType.Minor ) ] = KeySignature.GSharpMinor,
    [ ( PitchClass.DSharp, ModeType.Minor ) ] = KeySignature.DSharpMinor,
    [ ( PitchClass.D, ModeType.Minor ) ] = KeySignature.DMinor,
    [ ( PitchClass.G, ModeType.Minor ) ] = KeySignature.GMinor,
    [ ( PitchClass.C, ModeType.Minor ) ] = KeySignature.CMinor,
    [ ( PitchClass.F, ModeType.Minor ) ] = KeySignature.FMinor,
    [ ( PitchClass.BFlat, ModeType.Minor ) ] = KeySignature.BFlatMinor,
    [ ( PitchClass.EFlat, ModeType.Minor ) ] = KeySignature.EFlatMinor,
    [ ( PitchClass.AFlat, ModeType.Minor ) ] = KeySignature.AFlatMinor
  };

  private static KeySignature CalculateKeySignature(
    PitchClass tonic,
    ModeType mode )
  {
    return s_keySignatureTable.TryGetValue( (tonic, mode), out var signature )
      ? signature
      : KeySignature.Empty;
  }
}
