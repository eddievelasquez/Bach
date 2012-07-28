namespace Intercode.MusicLib.Test
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [ TestClass ]
  public class ToneTests
  {
    [ TestMethod ]
    public void ToStringTests()
    {
      Assert.AreEqual("A", Tone.A.ToString());
      Assert.AreEqual("A#", Tone.ASharp.ToString());
      Assert.AreEqual("Bb", Tone.BFlat.ToString());
      Assert.AreEqual("C", Tone.C.ToString());
      Assert.AreEqual("C#", Tone.CSharp.ToString());
      Assert.AreEqual("Db", Tone.DFlat.ToString());
      Assert.AreEqual("D", Tone.D.ToString());
      Assert.AreEqual("D#", Tone.DSharp.ToString());
      Assert.AreEqual("Eb", Tone.EFlat.ToString());
      Assert.AreEqual("E", Tone.E.ToString());
      Assert.AreEqual("F", Tone.F.ToString());
      Assert.AreEqual("F#", Tone.FSharp.ToString());
      Assert.AreEqual("Gb", Tone.GFlat.ToString());
      Assert.AreEqual("G", Tone.G.ToString());
      Assert.AreEqual("G#", Tone.GSharp.ToString());
      Assert.AreEqual("Ab", Tone.AFlat.ToString());
    }

    [ TestMethod ]
    public void NextTests()
    {
      // Test no interval
      Assert.AreEqual(Tone.A, Tone.A.Next(0, false));

      // Test next for natural
      Assert.AreEqual(Tone.ASharp, Tone.A.Next(1, false));
      Assert.AreEqual(Tone.BFlat, Tone.A.Next(1, true));

      // Test next for # and b
      Assert.AreEqual(Tone.B, Tone.ASharp.Next(1, false));
      Assert.AreEqual(Tone.B, Tone.BFlat.Next(1, true));

      // Test wrap-around
      Assert.AreEqual(Tone.A, Tone.GSharp.Next(1, false));
      Assert.AreEqual(Tone.A, Tone.AFlat.Next(1, true));

      // Test over an octave intervals
      Assert.AreEqual(Tone.A, Tone.A.Next(12, false));
      Assert.AreEqual(Tone.ASharp, Tone.A.Next(13, false));
      Assert.AreEqual(Tone.BFlat, Tone.A.Next(13, true));
    }

    [ TestMethod ]
    public void PreviousTests()
    {
      // Test no interval
      Assert.AreEqual(Tone.A, Tone.A.Previous(0, false));

      // Test previous for natural
      Assert.AreEqual(Tone.A, Tone.ASharp.Previous(1, false));
      Assert.AreEqual(Tone.A, Tone.BFlat.Previous(1, true));

      // Test wrap-around
      Assert.AreEqual(Tone.GSharp, Tone.A.Previous(1, false));
      Assert.AreEqual(Tone.AFlat, Tone.A.Previous(1, true));

      // Test over an octave intervals
      Assert.AreEqual(Tone.A, Tone.A.Previous(12, false));
      Assert.AreEqual(Tone.GSharp, Tone.A.Previous(13, false));
      Assert.AreEqual(Tone.AFlat, Tone.A.Previous(13, true));
    }

    [ TestMethod ]
    public void AsSharpTest()
    {
      Assert.AreEqual(Tone.A, Tone.A.AsSharp());
      Assert.AreEqual(Tone.ASharp, Tone.ASharp.AsSharp());
      Assert.AreEqual(Tone.ASharp, Tone.BFlat.AsSharp());
    }

    [ TestMethod ]
    public void AsFlatTest()
    {
      Assert.AreEqual(Tone.A, Tone.A.AsFlat());
      Assert.AreEqual(Tone.AFlat, Tone.AFlat.AsSharp());
      Assert.AreEqual(Tone.AFlat, Tone.GSharp.AsSharp());
    }
  }
}
