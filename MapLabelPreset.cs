using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    internal class MapLabelPreset : IXmlSerializable
    {
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
