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
      TestScale(Note.C(4), Scale.Major, Note.Parse("C,D,E,F,G,A,B"));
      TestScale(Note.C(4), Scale.NaturalMinor, Note.Parse("C,D,Eb,F,G,Ab,Bb"));
      TestScale(Note.C(4), Scale.HarmonicMinor, Note.Parse("C,D,Eb,F,G,Ab,B"));
      TestScale(Note.C(4), Scale.MelodicMinor, Note.Parse("C,D,Eb,F,G,A,B"));
      TestScale(Note.C(4), Scale.Diminished, Note.Parse("C,D,Eb,F,Gb,G#,A,B"));
      TestScale(Note.C(4), Scale.Polytonal, Note.Parse("C,Db,Eb,Fb,F#,G,A,Bb"));
      TestScale(Note.C(4), Scale.Pentatonic, Note.Parse("C,D,E,G,A"));
      TestScale(Note.C(4), Scale.Blues, Note.Parse("C,Eb,F,Gb,G,Bb"));
      TestScale(Note.C(4), Scale.Gospel, Note.Parse("C,D,Eb,E,G,A"));
    }

    private static void TestScale( Note root, Scale scale, params Note[] expectedNotes )
    {
      Assert.IsNotNull(scale);

      Note[] notes = scale.GetNotes(root).Take(expectedNotes.Length).ToArray();
      Assert.AreEqual(expectedNotes.Length, notes.Length);

      for( int i = 0; i < expectedNotes.Length; i++ )
        Assert.AreEqual(expectedNotes[i], notes[i]);
    }
  }
}
