namespace Intercode.MusicLib.Test
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  ///     Summary description for NoteTests
  /// </summary>
  [ TestClass ]
  public class NoteTests
  {
    [ TestMethod ]
    public void ToStringTests()
    {
      Assert.AreEqual("A1", Note.A1.ToString());
      Assert.AreEqual("A#1", Note.ASharp1.ToString());
      Assert.AreEqual("Bb1", Note.BFlat1.ToString());
      Assert.AreEqual("C2", Note.C2.ToString());
      Assert.AreEqual("C#2", Note.CSharp2.ToString());
      Assert.AreEqual("Db2", Note.DFlat2.ToString());
      Assert.AreEqual("D3", Note.D3.ToString());
      Assert.AreEqual("D#3", Note.DSharp3.ToString());
      Assert.AreEqual("Eb3", Note.EFlat3.ToString());
      Assert.AreEqual("E4", Note.E4.ToString());
      Assert.AreEqual("F4", Note.F4.ToString());
      Assert.AreEqual("F#5", Note.FSharp5.ToString());
      Assert.AreEqual("Gb5", Note.GFlat5.ToString());
      Assert.AreEqual("G6", Note.G6.ToString());
      Assert.AreEqual("G#7", Note.GSharp7.ToString());
      Assert.AreEqual("Ab8", Note.AFlat8.ToString());
    }

    [ TestMethod ]
    public void NextTest()
    {
      Assert.AreEqual(Note.A1, Note.A1.Next(0, false));
      Assert.AreEqual(Note.A1, Note.A1.Next(0, true));

      Assert.AreEqual(Note.ASharp1, Note.A1.Next(1, false));
      Assert.AreEqual(Note.BFlat1, Note.A1.Next(1, true));

      Assert.AreEqual(Note.A2, Note.GSharp1.Next(1, false));
      Assert.AreEqual(Note.A3, Note.GSharp2.Next(1, false));
      Assert.AreEqual(Note.A4, Note.GSharp3.Next(1, false));
      Assert.AreEqual(Note.A5, Note.GSharp4.Next(1, false));
      Assert.AreEqual(Note.A6, Note.GSharp5.Next(1, false));
      Assert.AreEqual(Note.A7, Note.GSharp6.Next(1, false));
      Assert.AreEqual(Note.A8, Note.GSharp7.Next(1, false));

      Assert.AreEqual(Note.A2, Note.A1.Next(12, false));
      Assert.AreEqual(Note.ASharp2, Note.A1.Next(13, false));
      Assert.AreEqual(Note.BFlat2, Note.A1.Next(13, true));
    }

    [ TestMethod ]
    public void PreviousTest()
    {
      Assert.AreEqual(Note.A1, Note.A1.Previous(0, false));
      Assert.AreEqual(Note.A1, Note.A1.Previous(0, true));

      Assert.AreEqual(Note.A1, Note.ASharp1.Previous(1, false));
      Assert.AreEqual(Note.A1, Note.BFlat1.Previous(1, true));

      Assert.AreEqual(Note.GSharp7, Note.A8.Previous(1, false));
      Assert.AreEqual(Note.AFlat7, Note.A8.Previous(1, true));

      Assert.AreEqual(Note.GSharp6, Note.A7.Previous(1, false));
      Assert.AreEqual(Note.AFlat6, Note.A7.Previous(1, true));

      Assert.AreEqual(Note.GSharp5, Note.A6.Previous(1, false));
      Assert.AreEqual(Note.AFlat5, Note.A6.Previous(1, true));

      Assert.AreEqual(Note.GSharp4, Note.A5.Previous(1, false));
      Assert.AreEqual(Note.AFlat4, Note.A5.Previous(1, true));

      Assert.AreEqual(Note.GSharp3, Note.A4.Previous(1, false));
      Assert.AreEqual(Note.AFlat3, Note.A4.Previous(1, true));

      Assert.AreEqual(Note.GSharp2, Note.A3.Previous(1, false));
      Assert.AreEqual(Note.AFlat2, Note.A3.Previous(1, true));

      Assert.AreEqual(Note.GSharp1, Note.A2.Previous(1, false));
      Assert.AreEqual(Note.AFlat1, Note.A2.Previous(1, true));

      Assert.AreEqual(Note.A3, Note.A4.Previous(12, false));
      Assert.AreEqual(Note.GSharp2, Note.A4.Previous(13, false));
      Assert.AreEqual(Note.AFlat2, Note.A4.Previous(13, true));
    }

    [ TestMethod ]
    public void AsSharpTest()
    {
      Assert.AreEqual(Note.A4, Note.A4.AsSharp());
      Assert.AreEqual(Note.ASharp4, Note.ASharp4.AsSharp());
      Assert.AreEqual(Note.ASharp4, Note.BFlat4.AsSharp());
    }

    [ TestMethod ]
    public void AsFlatTest()
    {
      Assert.AreEqual(Note.A4, Note.A4.AsFlat());
      Assert.AreEqual(Note.AFlat4, Note.AFlat4.AsSharp());
      Assert.AreEqual(Note.AFlat4, Note.GSharp4.AsSharp());
    }
  }
}
