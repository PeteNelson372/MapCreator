using SkiaSharp;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapRegion : MapComponent, IXmlSerializable
    {
        public MapRegion(MapCreatorMap parentMap)
        {
            Map = parentMap;
        }

        public MapCreatorMap Map { get; set; }

        public List<SKPoint> RegionPoints { get; set; } = [];
        public Color RegionBorderColor { get; set; } = ColorTranslator.FromHtml("#0056b3");
        public int RegionBorderWidth { get; set; } = 10;
        public int RegionInnerOpacity { get; set; } = 64;
        public int RegionBorderSmoothing { get; set; } = 20;
        public bool SnapToCoastline { get; set; } = true;
        public PathTypeEnum RegionBorderType { get; set; } = PathTypeEnum.SolidLinePath;

        public bool IsSelected { get; set; } = false;

        public SKPaint RegionBorderPaint { get; set; } = new();
        public SKPaint RegionInnerPaint { get; set; } = new();

        public SKPath? BoundaryPath { get; set; } = null;

        public override void Render(SKCanvas canvas)
        {
            MapRegionMethods.DrawRegion(Map, this, canvas);
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
