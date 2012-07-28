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
    public void NextTest()
    {
      Assert.AreEqual(Note.ASharp4, Note.A4.Next(1, false));
      Assert.AreEqual(Note.A5, Note.GSharp4.Next(1, false));
      Assert.AreEqual(Note.A5, Note.AFlat4.Next(1, true));

      Assert.AreEqual(Note.A5, Note.A4.Next(12, false));
      Assert.AreEqual(Note.ASharp5, Note.A4.Next(13, false));
      Assert.AreEqual(Note.BFlat5, Note.A4.Next(13, true));
    }

    [ TestMethod ]
    public void PreviousTest()
    {
      Assert.AreEqual(Note.GSharp3, Note.A4.Previous(1, false));
      Assert.AreEqual(Note.AFlat3, Note.A4.Previous(1, true));

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
