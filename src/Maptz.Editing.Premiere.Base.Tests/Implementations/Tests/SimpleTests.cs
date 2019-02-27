using System.IO;
using System.Linq;
using Xunit;

namespace Maptz.Editing.Premiere.Base.Tests
{

    public class SimpleTests
    {

        public string ComplexTestFilePath { get; private set; }
        public string SimpleTestFilePath { get; private set; }

        public SimpleTests()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            this.SimpleTestFilePath = Path.Combine(currentDirectory, "TestFiles", "Simple.prproj");
            this.ComplexTestFilePath = Path.Combine(currentDirectory, "TestFiles", "Complex.prproj");
        }

        [Fact]
        public void GetTitles_Returns_TitleContent()
        {


            /* #region Arrange */
            var framerate = SmpteFrameRate.Smpte25;
            var project = new ProjectFile(this.SimpleTestFilePath);
            var sequences = project.GetSequences();
            var seq = sequences.First();
            var root = project.XDoc.Root;
            var tracks = seq.GetVideoTracks(project.XDoc.Root);
            /* #endregion*/

            /* #region Act */
            var titles = tracks.SelectMany(track => track.GetTitles(root, framerate)).ToArray();
            /* #endregion*/

            /* #region Assert */
            Assert.True(sequences.Count() == 1);
            Assert.True(titles.Count() == 1);
            Assert.True(titles.First().Content == "hello from steve");
            /* #endregion*/

        }
    }
}