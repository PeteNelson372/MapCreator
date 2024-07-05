using SkiaSharp;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlInclude(typeof(MapBitmap))]
    [XmlInclude(typeof(MapLayer))]
    public abstract class MapComponent : IMapComponent
    {
        [XmlIgnore]
        public bool RenderComponent { get; set; } = true;

        [XmlArray("MapLayerComponents")]
        [XmlArrayItem("MapBitmap", Type = typeof(MapBitmap))]
        [XmlArrayItem("MapLandform", Type = typeof(MapLandformType2))]
        [XmlArrayItem("MapPaintedWaterFeature", Type = typeof(MapPaintedWaterFeature))]
        [XmlArrayItem("MapPath", Type = typeof(MapPath))]
        [XmlArrayItem("MapSymbol", Type = typeof(MapSymbol))]
        [XmlArrayItem("MapRiver", Type = typeof(MapRiver))]
        [XmlArrayItem("MapLabel", Type = typeof(MapLabel))]
        [XmlArrayItem("PlacedMapBox", Type = typeof(PlacedMapBox))]
        [XmlArrayItem("PlacedMapFrame", Type = typeof(PlacedMapFrame))]
        [XmlArrayItem("MapGrid", Type = typeof(MapGrid))]
        public List<MapComponent> MapLayerComponents { get; } = new List<MapComponent>(500);

        [XmlAttribute]
        public uint X { get; set; }

        [XmlAttribute]
        public uint Y { get; set; }

        [XmlAttribute]
        public uint Width { get; set; }

        [XmlAttribute]
        public uint Height { get; set; }

        public abstract void Render(SKCanvas canvas);
    }
}
