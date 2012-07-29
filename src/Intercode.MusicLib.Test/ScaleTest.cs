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
      TestScale(Scale.Major, Note.C(1), Note.C(1), Note.D(1), Note.E(1), Note.F(1), Note.G(1), Note.A(2), Note.B(2));
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
