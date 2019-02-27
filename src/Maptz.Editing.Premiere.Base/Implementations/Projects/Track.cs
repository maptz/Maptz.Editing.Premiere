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

    public class Track
    {
        public Track(XElement xElement, SmpteFrameRate framerate)
        {
            this.XElement = xElement;
            this.Framerate = framerate;
        }

        public XElement XElement { get; private set; }
        public SmpteFrameRate Framerate { get; }
    }
}