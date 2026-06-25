namespace Bach.Model;

using System.Linq;

/// <summary>Represents a scale degree and resolves it within a key.</summary>
public readonly struct ScaleDegree
{
  /// <summary>Represents the tonic scale degree.</summary>
  public static readonly ScaleDegree Tonic = new( 1, "I" );

  /// <summary>Represents the supertonic scale degree.</summary>
  public static readonly ScaleDegree Supertonic = new( 2, "II" );

  /// <summary>Represents the mediant scale degree.</summary>
  public static readonly ScaleDegree Mediant = new( 3, "III" );

  /// <summary>Represents the subdominant scale degree.</summary>
  public static readonly ScaleDegree Subdominant = new( 4, "IV" );

  /// <summary>Represents the dominant scale degree.</summary>
  public static readonly ScaleDegree Dominant = new( 5, "V" );

  /// <summary>Represents the dominant scale degree using the historical spelling.</summary>
  public static readonly ScaleDegree Dominate = Dominant;

  /// <summary>Represents the submediant scale degree.</summary>
  public static readonly ScaleDegree Submediant = new( 6, "VI" );

  /// <summary>Represents the leading-tone scale degree.</summary>
  public static readonly ScaleDegree LeadingTone = new( 7, "VII" );

  private ScaleDegree( int degree, string symbol )
  {
    Degree = degree;
    Symbol = symbol;
  }

  /// <summary>Gets the numeric degree.</summary>
  public int Degree { get; }

  /// <summary>Gets the roman numeral symbol.</summary>
  public string Symbol { get; }

  /// <summary>Resolves the degree to a pitch class in the supplied key.</summary>
  /// <param name="key">The key to resolve against.</param>
  /// <returns>The pitch class for the degree.</returns>
  public PitchClass Resolve( Key key )
  {
    ArgumentNullException.ThrowIfNull( key );

    var scale = key.Scale.GetAscending()
                   .ToArray();

    var index = Degree - 1;
    return scale[index % scale.Length];
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Symbol;
  }
}
