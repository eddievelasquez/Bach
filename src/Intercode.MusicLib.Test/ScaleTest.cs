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
      var major = new Scale("Major", 2, 2, 1, 2, 2, 2, 2);
      Assert.AreEqual("Major", major.Name);
      Assert.AreEqual(7, major.NoteCount);
    }

    [ TestMethod ]
    public void GetNotesTest()
    {
      TestScale(Note.C(1), Scale.Major, Note.C(1), Note.D(1), Note.E(1), Note.F(1), Note.G(1), Note.A(2), Note.B(2), Note.C(2));
      TestScale(Note.C(1), Scale.NaturalMinor, Note.C(1), Note.D(1), Note.EFlat(1), Note.F(1), Note.G(1), Note.AFlat(1), Note.BFlat(2), Note.C(2));
    }

    private static void TestScale( Note root, Scale scale, params Note[] expectedNotes )
    {
      Note[] notes = scale.GetNotes(root).Take(expectedNotes.Length).ToArray();
      Assert.AreEqual(expectedNotes.Length, notes.Length);

      for( int i = 0; i < expectedNotes.Length; i++ )
        Assert.AreEqual(expectedNotes[i], notes[i]);
    }
  }
}
