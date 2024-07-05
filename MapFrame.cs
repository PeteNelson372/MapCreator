using SkiaSharp;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("mapframe", Namespace = "MapCreator", IsNullable = false)]
    public class MapFrame
    {
        public string? FrameName { get; set; }

        [XmlIgnore]
        public SKBitmap? FrameBitmap { get; set; }

        public string? FrameBitmapPath { get; set; }

        public string? FrameXmlFilePath { get; set; }

        public float FrameCenterLeft { get; set; } = 0;
        public float FrameCenterTop { get; set;} = 0;
        public float FrameCenterRight { get; set;} = 0;
        public float FrameCenterBottom { get; set; } = 0;
    }
}
