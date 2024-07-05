using SkiaSharp;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapWindrose : MapComponent, IXmlSerializable
    {
        public Guid WindroseGuid { get; set; } = Guid.NewGuid();
        public Color WindroseColor { get; set; } = ColorTranslator.FromHtml("#7F3D3728");
        public int DirectionCount { get; set; } = 16;
        public int LineWidth { get; set; } = 3;
        public int InnerRadius { get; set; } = 0;
        public int OuterRadius { get; set; } = 1000;
        public int InnerCircles { get; set; } = 0;
        public bool FadeOut { get; set; } = false;

        public override void Render(SKCanvas canvas)
        {
            throw new NotImplementedException();
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
