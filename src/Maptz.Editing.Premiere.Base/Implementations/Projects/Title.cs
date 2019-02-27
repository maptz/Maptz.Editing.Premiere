using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Maptz.Editing.Premiere
{

    [DebuggerDisplay("{Start}-{End}")]
    public class Title
    {
        public TimeCode Start { get; set; }
        public TimeCode End { get; set; }
        public string Content { get; set; }
    }
}