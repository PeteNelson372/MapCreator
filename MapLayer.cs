using SkiaSharp;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlType("MapLayer")]
    public class MapLayer : MapComponent
    {
        private ushort mapLayerOrder = 0;

        [XmlAttribute]
        public Guid MapLayerGuid { get; set; } = Guid.NewGuid();

        [XmlAttribute]
        public string MapLayerName { get; set; } = "";

        [XmlAttribute]
        public ushort MapLayerOrder { get => mapLayerOrder; set => mapLayerOrder = value; }

        [XmlIgnore]
        public SKSurface? LayerSurface { get; set; }

        [XmlIgnore]
        public bool ShowLayer { get; set; } = true;

        public override void Render(SKCanvas canvas)
        {
            if (ShowLayer)
            {
                if (MapLayerComponents != null)
                {
                    //LayerSurface.Canvas.Clear();

                    foreach (var component in MapLayerComponents)
                    {
                        if (component.RenderComponent && LayerSurface != null)
                        {
                            component.Render(canvas);
                            //canvas.DrawSurface(LayerSurface, 0, 0);
                            //component.Render(canvas);
                        }
                    }
                }
            }
        }
    }
}
