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



    public static class TrackExtensions
    {


        public static IEnumerable<Title> GetTitles(this Track track, XElement root, SmpteFrameRate framerate)
        {
            var retval = new List<Title>();
            var trackItemIds = track.XElement.Descendants(XName.Get("TrackItem")).Select(p => p.Attribute(XName.Get("ObjectRef"))?.Value);
            var trackItems = root.Elements(XName.Get("VideoClipTrackItem")).Where(
                p => trackItemIds.Any(q => p.Attribute(XName.Get("ObjectID"))?.Value == q));

            foreach (var trackItem in trackItems)
            {
                TimeCode startTC, endTC;
                string name;
                IEnumerable<string> titleLines = new string[0];
                /* #region Get titles */
                {
                    var start = trackItem.Descendants(XName.Get("Start")).First().Value;
                    var startFrames = long.Parse(start) / PremiereHelpers.GetFramerateDivisor(framerate);
                    startTC = TimeCode.FromFrames(startFrames, framerate);
                    var end = trackItem.Descendants(XName.Get("End")).First().Value;
                    var endFrames = long.Parse(end) / PremiereHelpers.GetFramerateDivisor(framerate);
                    endTC = TimeCode.FromFrames(endFrames, framerate);

                    var subclipId = trackItem.Descendants(XName.Get("SubClip")).FirstOrDefault()?.Attribute(XName.Get("ObjectRef"))?.Value;
                    var subclips = root.Elements(XName.Get("SubClip"));
                    var subclip = root.Elements(XName.Get("SubClip")).FirstOrDefault(p => p.Attribute(XName.Get("ObjectID"))?.Value == subclipId);

                    name = subclip.Element(XName.Get("Name"))?.Value;

                    var clipId = subclip.Elements(XName.Get("Clip"))?.Attributes(XName.Get("ObjectRef")).FirstOrDefault()?.Value;
                    var videoClips = root.Elements(XName.Get("VideoClip"));
                    var videoClip = videoClips.FirstOrDefault(p => p.Attribute(XName.Get("ObjectID"))?.Value == clipId);

                    var sourceId = videoClip.Element(XName.Get("Clip")).Element(XName.Get("Source"))?.Attribute(XName.Get("ObjectRef"))?.Value;

                    var videoMediaSources = root.Elements(XName.Get("VideoMediaSource"));
                    var videoMediaSource = videoMediaSources.FirstOrDefault(p => p.Attribute(XName.Get("ObjectID"))?.Value == sourceId);

                    var mediaUID = videoMediaSource.Element(XName.Get("MediaSource")).Element(XName.Get("Media"))?.Attribute(XName.Get("ObjectURef"))?.Value;

                    var mediaNodes = root.Elements(XName.Get("Media"));
                    var media = mediaNodes.FirstOrDefault(p => p.Attribute(XName.Get("ObjectUID"))?.Value == mediaUID);

                    var encoding = media.Element(XName.Get("ImporterPrefs"))?.Value;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        continue;
                    }

                    byte[] data = Convert.FromBase64String(encoding);
                    string decodedString = Encoding.UTF8.GetString(data);
                    if (decodedString.Contains("Compressed"))
                    {
                        titleLines = PremiereHelpers.DecodeCompressedTitleContent(data);
                    }
                    else
                    {
                        titleLines = PremiereHelpers.DecodeTitleContent(decodedString);
                    }
                }
                /* #endregion*/

                var title = new Title()
                {
                    Start = startTC,
                    End = endTC,
                    Content = string.Join(Environment.NewLine, titleLines)
                };
                retval.Add(title);

            }
            return retval;
        }
    }
}