using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapRegion : MapComponent, IXmlSerializable
    {
        public MapCreatorMap? Map { get; set; }

        public string RegionName { get; set; } = string.Empty;
        public Guid RegionGuid { get; set; } = Guid.NewGuid();

        public List<MapRegionPoint> MapRegionPoints { get; set; } = [];
        public Color RegionBorderColor { get; set; } = ColorTranslator.FromHtml("#0056B3");
        public int RegionBorderWidth { get; set; } = 10;
        public int RegionInnerOpacity { get; set; } = 64;
        public int RegionBorderSmoothing { get; set; } = 20;
        public PathTypeEnum RegionBorderType { get; set; } = PathTypeEnum.SolidLinePath;
        public bool IsSelected { get; set; } = false;

        public SKPaint RegionBorderPaint { get; set; } = new();

        public SKPaint RegionInnerPaint { get; set; } = new();

        public SKPath? BoundaryPath { get; set; } = null;

        public MapRegion(MapCreatorMap parentMap)
        {
            Map = parentMap;
        }

        public MapRegion() { }

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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.

            XNamespace ns = "MapCreator";
            string content = reader.ReadOuterXml();
            XDocument mapRegionDoc = XDocument.Parse(content);

            IEnumerable<XElement?> nameElemEnum = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionName"));
            if (nameElemEnum.First() != null)
            {
                string? regionName = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionName").Value).FirstOrDefault();
                RegionName = regionName;
            }

            IEnumerable<XElement?> guidElemEnum = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionGuid"));
            if (guidElemEnum.First() != null)
            {
                string? regionGuid = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionGuid").Value).FirstOrDefault();
                RegionGuid = Guid.Parse(regionGuid);
            }

            IEnumerable<XElement> regionPointElem = mapRegionDoc.Descendants(ns + "MapRegionPoint");
            if (regionPointElem.First() != null)
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                foreach (XElement elem in regionPointElem)
                {
                    string regionPointString = elem.ToString();

                    using (XmlReader pointReader = XmlReader.Create(new StringReader(regionPointString), settings))
                    {
                        pointReader.Read();
                        MapRegionPoint mrp = new();
                        mrp.ReadXml(pointReader);

                        MapRegionPoints.Add(mrp);
                    }
                }
            }

            IEnumerable<XElement?> regionBorderColorElem = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderColor"));
            if (regionBorderColorElem.First() != null)
            {
                string? regionBorderColor = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderColor").Value).FirstOrDefault();
                RegionBorderColor = Color.FromArgb(int.Parse(regionBorderColor));
            }

            IEnumerable<XElement?> regionBorderWidthElem = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderWidth"));
            if (regionBorderWidthElem.First() != null)
            {
                string? regionBorderWidth = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderWidth").Value).FirstOrDefault();
                RegionBorderWidth = int.Parse(regionBorderWidth);
            }

            IEnumerable<XElement?> regionInnerOpacityElem = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionInnerOpacity"));
            if (regionInnerOpacityElem.First() != null)
            {
                string? regionInnerOpacity = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionInnerOpacity").Value).FirstOrDefault();
                RegionInnerOpacity = int.Parse(regionInnerOpacity);
            }

            IEnumerable<XElement?> regionBorderSmoothingElem = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderSmoothing"));
            if (regionBorderSmoothingElem.First() != null)
            {
                string? regionBorderSmoothing = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderSmoothing").Value).FirstOrDefault();
                RegionBorderSmoothing = int.Parse(regionBorderSmoothing);
            }

            IEnumerable<XElement?> regionBorderTypeElem = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderType"));
            if (regionBorderTypeElem.First() != null)
            {
                string? regionBorderType = mapRegionDoc.Descendants().Select(x => x.Element(ns + "RegionBorderType").Value).FirstOrDefault();
                RegionBorderType = Enum.Parse<PathTypeEnum>(regionBorderType);
            }

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("RegionName");
            writer.WriteString(RegionName);
            writer.WriteEndElement();

            writer.WriteStartElement("RegionGuid");
            writer.WriteString(RegionGuid.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("MapRegionPoints");
            foreach (MapRegionPoint point in MapRegionPoints)
            {
                writer.WriteStartElement("MapRegionPoint");
                point.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("RegionBorderColor");
            writer.WriteValue(RegionBorderColor.ToArgb());
            writer.WriteEndElement();

            writer.WriteStartElement("RegionBorderWidth");
            writer.WriteValue(RegionBorderWidth);
            writer.WriteEndElement();

            writer.WriteStartElement("RegionInnerOpacity");
            writer.WriteValue(RegionInnerOpacity);
            writer.WriteEndElement();

            writer.WriteStartElement("RegionBorderSmoothing");
            writer.WriteValue(RegionBorderSmoothing);
            writer.WriteEndElement();

            writer.WriteStartElement("RegionBorderType");
            writer.WriteValue(RegionBorderType.ToString());
            writer.WriteEndElement();
        }
    }
}
