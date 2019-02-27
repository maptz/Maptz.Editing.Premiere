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
    public static class SequenceExtensions
    {
        public static IEnumerable<Track> GetVideoTracks(this Sequence seq, XElement root)
        {
            var trackGroupIds = seq.XElement.Descendants(XName.Get("TrackGroup"))
             .Select(p => p.Descendants(XName.Get("Second")).FirstOrDefault()?.Attribute(XName.Get("ObjectRef"))?.Value);

            var trackGroups = root.Elements(XName.Get("VideoTrackGroup")).Where(p=>trackGroupIds.Any(q=> p.Attribute(XName.Get("ObjectID"))?.Value == q));

            List<Track> retval = new List<Track>();
            foreach(var trackGroup in trackGroups)
            {
                var fr = trackGroup.Descendants(XName.Get("FrameRate")).FirstOrDefault()?.Value;
                var framerate = PremiereHelpers.ParseFramerate(fr);
                var trackIds = trackGroup.Descendants(XName.Get("Track")).Select(p => p.Attribute(XName.Get("ObjectURef"))?.Value);
                //AllVideoClipTracks
                var allVideoClipTracks = root.Elements(XName.Get("VideoClipTrack"));

                var myTracks = allVideoClipTracks.Where(p => trackIds.Any(q => q == p.Attribute(XName.Get("ObjectUID"))?.Value)).Select(p => new Track(p, framerate));
                retval.AddRange(myTracks);
            }
            //Video Track Group has a framerate: 10160640000
            ////10594584000 is 23.97 fps, 10160640000 is 25fps, and 8475667200 is 29.9 fps.
            return retval;
        }
    }
}