using SkiaSharp;
using System.Xml;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapVignette : MapComponent
    {
        [XmlIgnore]
        public MapCreatorMap? Map { get; set; }

        [XmlElement]
        public int VignetteStrength { get; set; } = 64;

        [XmlElement]
        public XmlColor VignetteColor { get; set; } = ColorTranslator.FromHtml("#C9977B");

        public MapVignette(MapCreatorMap parentMap)
        {
            Map = parentMap;
        }

        public MapVignette() {}

        public override void Render(SKCanvas canvas)
        {
            if (Map != null)
            {
                // vignette is painted only to vignette layer canvas
                SKRect bounds = new(0, 0, Map.MapWidth, Map.MapHeight);
                MapPaintMethods.PaintVignette(MapBuilder.GetLayerCanvas(Map, MapBuilder.VIGNETTELAYER), bounds, VignetteColor, VignetteStrength);
            }
        }
    }
}
