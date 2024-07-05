using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapRiver : WaterFeature, IXmlSerializable
    {
        public Guid MapRiverGuid { get; set; } = Guid.NewGuid();

        public string MapRiverName { get; set; } = "";

        public List<MapRiverPoint> RiverPoints { get; set; } = [];

        public Color RiverColor { get; set; } = ColorTranslator.FromHtml("#839690");

        public int RiverColorOpacity { get; set; } = 255;
        
        public float RiverWidth { get; set; } = 4;

        public bool RiverSourceFadeIn { get; set; } = false;

        public Color RiverShorelineColor { get; set; } = ColorTranslator.FromHtml("#A19076");

        public int RiverShorelineColorOpacity { get; set; } = 255;

        [XmlIgnore]
        public bool ShowRiverPoints { get; set; } = false;

        [XmlIgnore]
        public bool IsSelected { get; set; } = false;

        [XmlIgnore]
        public bool IsRemoved { get; set; } = false;

        [XmlIgnore]
        public SKPaint? RiverPaint { get; set; }

        [XmlIgnore]
        public SKPaint? RiverShorelinePaint { get; set; } = null;
        
        [XmlIgnore]
        public SKPaint? RiverShallowWaterPaint { get; set; } = null;


        [XmlIgnore]
        public SKPath? RiverBoundaryPath { get; set; } = null;

        [XmlIgnore]
        public List<SKPath> RiverColorPaths = [];

        public override void Render(SKCanvas canvas)
        {
            // no op, paths are rendered
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
            XDocument mapRiverDoc = XDocument.Parse(content);

            IEnumerable<XElement?> nameElemEnum = mapRiverDoc.Descendants().Select(x => x.Element(ns + "MapRiverName"));
            if (nameElemEnum.First() != null)
            {
                string? mapRiverName = mapRiverDoc.Descendants().Select(x => x.Element(ns + "MapRiverName").Value).FirstOrDefault();
                MapRiverName = mapRiverName;
            }

            IEnumerable<XElement?> guidElemEnum = mapRiverDoc.Descendants().Select(x => x.Element(ns + "MapRiverGuid"));
            if (guidElemEnum.First() != null)
            {
                string? mapGuid = mapRiverDoc.Descendants().Select(x => x.Element(ns + "MapRiverGuid").Value).FirstOrDefault();
                MapRiverGuid = Guid.Parse(mapGuid);
            }

            IEnumerable<XElement> riverColorElem = mapRiverDoc.Descendants(ns + "RiverColor");
            if (riverColorElem.First() != null)
            {
                string? riverColor = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverColor").Value).FirstOrDefault();
                RiverColor = ColorTranslator.FromHtml(riverColor);
            }

            IEnumerable<XElement?> riverColorOpacityElem = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverColorOpacity"));
            if (riverColorOpacityElem.First() != null)
            {
                string? riverColorOpacity = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverColorOpacity").Value).FirstOrDefault();
                RiverColorOpacity = int.Parse(riverColorOpacity);
            }

            IEnumerable<XElement?> riverWidthElem = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverWidth"));
            if (riverWidthElem.First() != null)
            {
                string? riverWidth = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverWidth").Value).FirstOrDefault();
                RiverWidth = int.Parse(riverWidth);
            }

            IEnumerable<XElement?> riverSourceFadeInElem = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverSourceFadeIn"));
            if (riverSourceFadeInElem.First() != null)
            {
                string? riverSourceFadeIn = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverSourceFadeIn").Value).FirstOrDefault();
                RiverSourceFadeIn = bool.Parse(riverSourceFadeIn);
            }

            IEnumerable<XElement> riverShorelineColorElem = mapRiverDoc.Descendants(ns + "RiverShorelineColor");
            if (riverShorelineColorElem.First() != null)
            {
                string? riverShorelineColor = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverShorelineColor").Value).FirstOrDefault();
                RiverShorelineColor = ColorTranslator.FromHtml(riverShorelineColor);
            }

            IEnumerable<XElement?> riverShorelineColorOpacityElem = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverShorelineColorOpacity"));
            if (riverShorelineColorOpacityElem.First() != null)
            {
                string? riverShorelineColorOpacity = mapRiverDoc.Descendants().Select(x => x.Element(ns + "RiverShorelineColorOpacity").Value).FirstOrDefault();
                RiverShorelineColorOpacity = int.Parse(riverShorelineColorOpacity);
            }

            IEnumerable<XElement> mapPointElem = mapRiverDoc.Descendants(ns + "MapRiverPoint");
            if (mapPointElem.First() != null)
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                foreach (XElement elem in mapPointElem)
                {
                    string mapPointString = elem.ToString();

                    using (XmlReader pointReader = XmlReader.Create(new StringReader(mapPointString), settings))
                    {
                        pointReader.Read();
                        MapRiverPoint mpp = new();
                        mpp.ReadXml(pointReader);

                        RiverPoints.Add(mpp);
                    }
                }
            }

            IEnumerable<XElement> colorPathElem = mapRiverDoc.Descendants(ns + "RiverColorPaths");
            if (colorPathElem != null && colorPathElem.Any() && colorPathElem.First() != null)
            {
                List<XElement> elemList = colorPathElem.Descendants().ToList();

                if (elemList != null)
                {
                    foreach (XElement elem in elemList)
                    {
                        if (elem.Name.LocalName.ToString() == "RiverColorPath")
                        {
                            SKPath skColorPath = SKPath.ParseSvgPathData(elem.Value);
                            RiverColorPaths.Add(skColorPath);
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
            using MemoryStream ms = new();
            using SKManagedWStream wstream = new(ms);

            // water feature name
            writer.WriteStartElement("MapRiverName");
            writer.WriteString(MapRiverName);
            writer.WriteEndElement();

            // water feature GUID
            writer.WriteStartElement("MapRiverGuid");
            writer.WriteString(MapRiverGuid.ToString());
            writer.WriteEndElement();

            // water feature color
            XmlColor riverColor = new(RiverColor);
            writer.WriteStartElement("RiverColor");
            riverColor.WriteXml(writer);
            writer.WriteEndElement();

            // water feature color opacity
            writer.WriteStartElement("RiverColorOpacity");
            writer.WriteValue(RiverColorOpacity);
            writer.WriteEndElement();

            writer.WriteStartElement("RiverWidth");
            writer.WriteValue(RiverWidth.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("RiverSourceFadeIn");
            writer.WriteValue(RiverSourceFadeIn.ToString());
            writer.WriteEndElement();

            XmlColor riverShorelineColor = new(RiverShorelineColor);
            writer.WriteStartElement("RiverShorelineColor");
            riverShorelineColor.WriteXml(writer);
            writer.WriteEndElement();

            // water feature color opacity
            writer.WriteStartElement("RiverShorelineColorOpacity");
            writer.WriteValue(RiverShorelineColorOpacity);
            writer.WriteEndElement();

            writer.WriteStartElement("MapRiverPoints");
            foreach (MapRiverPoint point in RiverPoints)
            {
                writer.WriteStartElement("MapRiverPoint");
                point.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // water feature color paths
            writer.WriteStartElement("RiverColorPaths");

            for (int i = 0; i < RiverColorPaths.Count; i++)
            {
                SKPath path = RiverColorPaths[i];
                if (path != null && path.PointCount > 0)
                {
                    string colorPathSvg = path.ToSvgPathData();
                    writer.WriteElementString("RiverColorPath", colorPathSvg);
                }
            }

            writer.WriteEndElement();
        }
    }
}
