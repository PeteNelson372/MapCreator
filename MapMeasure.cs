using SkiaSharp;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    internal class MapMeasure : MapComponent, IXmlSerializable
    {
        public override void Render(SKCanvas canvas)
        {
            throw new NotImplementedException();
        }

        public XmlSchema? GetSchema()
        {
            throw new NotImplementedException();
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
