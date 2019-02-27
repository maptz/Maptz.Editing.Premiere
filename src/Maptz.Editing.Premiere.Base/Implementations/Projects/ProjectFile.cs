using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maptz.Editing.Premiere
{

    public class ProjectFile
    {



        public ProjectFile(string filePath)
        {
            this.FilePath = filePath;
            var fileInfo = new FileInfo(this.FilePath);
            var xmlContent = DecompressGZip(fileInfo);

            this.Xml = xmlContent;
            var xdoc = XDocument.Parse(this.Xml);
            this.XDoc = xdoc;
        }

        public string FilePath { get; private set; }
        public XDocument XDoc { get; private set; }
        public string Xml { get; private set; }

        public IEnumerable<Sequence> GetSequences()
        {
            var retval = new List<Sequence>();
            foreach (var sequenceNode in this.XDoc.Root.Elements(XName.Get("Sequence"))) //Descendants would get deeper descs!
            {

                var parent = sequenceNode.Parent;
                var sequence = new Sequence(sequenceNode);
                retval.Add(sequence);
            }
            return retval;
        }

        public static string DecompressGZip(FileInfo fileToDecompress)
        {
            var retval = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        using (var streamReader = new StreamReader(memoryStream))

                        {
                            retval = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return retval;
        }
    }
}
