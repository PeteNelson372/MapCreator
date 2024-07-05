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

        [XmlAttribute]
        public ushort MapWidth { get => mapWidth; set => mapWidth = value; }

        [XmlAttribute]
        public ushort MapHeight { get => mapHeight; set => mapHeight = value; }

        [XmlAttribute]
        public float MapAreaWidth { get => mapAreaWidth; set => mapAreaWidth = value; }

        [XmlAttribute]
        public float MapAreaHeight { get => mapAreaHeight; set => mapAreaHeight = value; }

        [XmlAttribute]
        public string MapAreaUnits { get; set; } = string.Empty;

        [XmlArray("MapLayers")]
        public List<MapLayer> MapLayers = new(MapBuilder.MAP_LAYER_COUNT);

        [XmlIgnore]
        public bool IsSaved { get => isSaved; set => isSaved = value; }

        [XmlIgnore]
        public float MapPixelWidth { get; set; } = 0F;

        [XmlIgnore]
        public float MapPixelHeight { get; set; } = 0F;

        public void Render(SKCanvas canvas)
        {
            if (MapLayers != null)
            {
                foreach (var mapLayer in MapLayers)
                {
                    mapLayer?.Render(canvas);
                }
            };
        }
    }
}
