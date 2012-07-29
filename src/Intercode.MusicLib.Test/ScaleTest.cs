namespace Intercode.MusicLib.Test
{
  using System.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [ TestClass ]
  public class ScaleTest
  {
    [ TestMethod ]
    public void ConstructorTest()
    {
      var major = new Scale("Major", 2, 2, 1, 2, 2, 2);
      Assert.AreEqual("Major", major.Name);
    }

    [ TestMethod ]
    public void MajorGetNotesTest()
    {
      TestScale(Scale.Major, Note.C1, Note.C1, Note.D1, Note.E1, Note.F1, Note.G1, Note.A2, Note.B2);
    }

    private static void TestScale( Scale scale, Note root, params Note[] expectedNotes )
    {
      Note[] notes = scale.GetNotes(root).ToArray();
      Assert.AreEqual(expectedNotes.Length, notes.Length);

      for( int i = 0; i < expectedNotes.Length; i++ )
        Assert.AreEqual(expectedNotes[i], notes[i]);
    }
  }
}
