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

    public class PremiereHelpers
    {
        public static long GetFramerateDivisor(SmpteFrameRate framerate)
        {
            switch (framerate)
            {
                case SmpteFrameRate.Smpte2398:
                    return 10594584000;
                case SmpteFrameRate.Smpte24:
                    throw new NotImplementedException();
                case SmpteFrameRate.Smpte25:
                    return 10160640000;
                case SmpteFrameRate.Smpte2997Drop:
                    return 8475667200;
                default:
                    throw new NotImplementedException();

                    //////10594584000 is 23.97 fps, 10160640000 is 25fps, and 8475667200 is 29.9 fps.
            }
        }

        public static IEnumerable<string> DecodeCompressedTitleContent(byte[] data)
        {
            //Remove the first few bytes.
            var compressedArray = new byte[data.Length - 32];
            Array.Copy(data, 32, compressedArray, 0, data.Length - 32);

            throw new NotImplementedException("Migration requires looking into this. The original version used Ionic. Can we just use System.IO.Compression?");
            //var decompressed = Ionic.Zlib.ZlibStream.UncompressBuffer(compressedArray);
            //var str2 = Encoding.Unicode.GetString(decompressed);
            //try
            //{
            //    //NOTE Some strange first char!!
            //    str2 = str2.Substring(1);
            //    return DecodeTitleContent(str2);
            //}
            //catch (Exception e)
            //{
            //    return new string[0];
            //}
        }

        public static IEnumerable<string> DecodeTitleContent(string str)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(str);
            }
            catch (Exception)
            {
                return new string[0];
            }
            //
            var titles = xdoc.Root.Descendants(XName.Get("Adobe_Title"));
            if (titles.Any())
            {
                var trstrings = xdoc.Descendants(XName.Get("TRString")).Select(p => p.Value);
                return trstrings;
            }
            else
            {
                return new string[0];
            }
        }

        public static SmpteFrameRate ParseFramerate(string fr)
        {
            switch (fr)
            {
                case "10594584000":
                    return SmpteFrameRate.Smpte2398;
                case "10160640000":
                    return SmpteFrameRate.Smpte25;
                case "8475667200":
                    return SmpteFrameRate.Smpte2997Drop;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}