using SkiaSharp;
using System.Xml;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("map", Namespace = "MapCreator", IsNullable = false)]
    [XmlInclude(typeof(MapComponent))]
    [XmlInclude(typeof(MapBitmap))]
    [XmlInclude(typeof(MapLayer))]
    public class MapCreatorMap
    {
        [XmlIgnore]
        private ushort mapWidth = 0;

        [XmlIgnore]
        private ushort mapHeight = 0;

        [XmlIgnore]
        private float mapAreaWidth = 0;

        [XmlIgnore]
        private float mapAreaHeight = 0;

        [XmlIgnore]
        private bool isSaved = false;

        [XmlIgnore]
        public SKBitmap? MapBackingBitmap { get; set; } = null;

        [XmlAttribute]
        public string MapName { get; set; } = "";

        [XmlAttribute]
        public string MapPath { get; set; } = "";

        // MapHeight and MapWidth are the size of the map in pixels (e.g. 1200 x 800)
        [XmlAttribute]
        public ushort MapWidth { get => mapWidth; set => mapWidth = value; }

        [XmlAttribute]
        public ushort MapHeight { get => mapHeight; set => mapHeight = value; }

        // MapAreaWidth and MapAreaHeight are the size of the map in MapUnits (e.g. 1000 miles x 500 miles)
        [XmlAttribute]
        public float MapAreaWidth { get => mapAreaWidth; set => mapAreaWidth = value; }

        [XmlAttribute]
        public float MapAreaHeight { get => mapAreaHeight; set => mapAreaHeight = value; }

        [XmlAttribute]
        public string MapAreaUnits { get; set; } = string.Empty;

        [XmlAttribute]
        public int MapVignetteStrength { get; set; } = 64;

        [XmlAttribute]
        public XmlColor MapVignetteColor { get; set; } = new XmlColor(ColorTranslator.FromHtml("#C9977B"));

        [XmlArray("MapLayers")]
        public List<MapLayer> MapLayers = new(MapBuilder.MAP_LAYER_COUNT);

        [XmlIgnore]
        public bool IsSaved { get => isSaved; set => isSaved = value; }

        // MapPixelWidth and MapPixelHeight are the size of one pixel in MapAreaUnits
        [XmlIgnore]
        public float MapPixelWidth { get; set; } = 0F;

        [XmlIgnore]
        public float MapPixelHeight { get; set; } = 0F;

        [XmlIgnore]
        public List<int> RenderOnlyLayers { get; set; } = [];

        public void Render(SKCanvas canvas)
        {
            if (MapLayers != null)
            {
                foreach (var mapLayer in MapLayers)
                {
                    if (RenderOnlyLayers.Count == 0 || RenderOnlyLayers.Contains(mapLayer.MapLayerOrder))
                    {
                        mapLayer?.Render(canvas);
                    }
                }
            };
        }
    }
}
