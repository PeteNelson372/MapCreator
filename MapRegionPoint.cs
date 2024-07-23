using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapRegionPoint : IXmlSerializable
    {
        public Guid PointGuid { get; set; } = Guid.NewGuid();
        public SKPoint RegionPoint { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; } = false;

        public MapRegionPoint() { }

        public MapRegionPoint(SKPoint point)
        {
            RegionPoint = point;
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.

            XNamespace ns = "MapCreator";
            string content = reader.ReadOuterXml();
            XDocument mapRegionPointDoc = XDocument.Parse(content);


            IEnumerable<XElement?> guidElemEnum = mapRegionPointDoc.Descendants().Select(x => x.Element(ns + "PointGuid"));
            if (guidElemEnum.First() != null)
            {
                string? mapGuid = mapRegionPointDoc.Descendants().Select(x => x.Element(ns + "PointGuid").Value).FirstOrDefault();
                PointGuid = Guid.Parse(mapGuid);
            }

            IEnumerable<XElement?> xyElemEnum = mapRegionPointDoc.Descendants().Select(x => x.Element(ns + "RegionPoint"));
            if (xyElemEnum.First() != null)
            {
                List<XElement> elemList = xyElemEnum.Descendants().ToList();

                if (elemList != null)
                {
                    float x = 0;
                    float y = 0;

                    foreach (XElement elem in elemList)
                    {
                        if (elem.Name.LocalName.ToString() == "X")
                        {
                            x = float.Parse(elem.Value);
                        }

                        if (elem.Name.LocalName.ToString() == "Y")
                        {
                            y = float.Parse(elem.Value);
                            RegionPoint = new SKPoint(x, y);
                        }
                    }
                }
            }
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("PointGuid");
            writer.WriteString(PointGuid.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("RegionPoint");
            writer.WriteStartElement("X");
            writer.WriteValue(RegionPoint.X);
            writer.WriteEndElement();
            writer.WriteStartElement("Y");
            writer.WriteValue(RegionPoint.Y);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}

