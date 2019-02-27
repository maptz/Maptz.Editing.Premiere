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


    public class Sequence
    {
        public Sequence(XElement xElement)
        {
            this.XElement = xElement;
        }

        public XElement XElement { get; private set; }

        public string Name
        {
            get
            {
                return this.XElement.Elements(XName.Get("Name")).FirstOrDefault()?.Value;
            }
        }
    }
}