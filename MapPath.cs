/**************************************************************************************************************************
* Copyright 2024, Peter R. Nelson
*
* This file is part of the MapCreator application. The MapCreator application is intended
* for creating fantasy maps for gaming and world building.
*
* MapCreator is free software: you can redistribute it and/or modify it under the terms
* of the GNU General Public License as published by the Free Software Foundation,
* either version 3 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program.
* The text of the GNU General Public License (GPL) is found in the LICENSE file.
* If the LICENSE file is not present or the text of the GNU GPL is not present in the LICENSE file,
* see https://www.gnu.org/licenses/.
*
* For questions about the MapCreator application or about licensing, please email
* contact@brookmonte.com
*
***************************************************************************************************************************/
using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapPath : MapComponent, IXmlSerializable
    {
        public Guid MapPathGuid { get; set; } = Guid.NewGuid();
        public string MapPathName { get; set; } = "";
        public List<MapPathPoint> PathPoints { get; set; } = [];
        public PathTypeEnum PathType { get; set; } = PathTypeEnum.SolidLinePath;
        public Color PathColor {  get; set; } = ColorTranslator.FromHtml("#4B311A");
        public float PathWidth { get; set; } = 4;
        public bool DrawOverSymbols { get; set; } = false;


        public bool ShowPathPoints { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public SKPaint? PathPaint { get; set; }
        public SKPath? BoundaryPath { get; set; } = null;

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
            XDocument mapPathDoc = XDocument.Parse(content);

            IEnumerable<XElement?> nameElemEnum = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathName"));
            if (nameElemEnum.First() != null)
            {
                string? mapPathName = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathName").Value).FirstOrDefault();
                MapPathName = mapPathName;
            }

            IEnumerable<XElement?> guidElemEnum = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathGuid"));
            if (guidElemEnum.First() != null)
            {
                string? mapGuid = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathGuid").Value).FirstOrDefault();
                MapPathGuid = Guid.Parse(mapGuid);
            }

            IEnumerable<XElement?> typeElemEnum = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathType"));
            if (typeElemEnum.First() != null)
            {
                string? pathType = mapPathDoc.Descendants().Select(x => x.Element(ns + "MapPathType").Value).FirstOrDefault();
                PathType = Enum.Parse<PathTypeEnum>(pathType);
            }

            IEnumerable<XElement> pathColorElem = mapPathDoc.Descendants(ns + "PathColor");
            if (pathColorElem.First() != null)
            {
                string? pathColor = mapPathDoc.Descendants().Select(x => x.Element(ns + "PathColor").Value).FirstOrDefault();
                PathColor = ColorTranslator.FromHtml(pathColor);
            }

            IEnumerable<XElement?> pathWidthElem = mapPathDoc.Descendants().Select(x => x.Element(ns + "PathWidth"));
            if (pathWidthElem.First() != null)
            {
                string? pathWidth = mapPathDoc.Descendants().Select(x => x.Element(ns + "PathWidth").Value).FirstOrDefault();
                PathWidth = int.Parse(pathWidth);
            }

            IEnumerable<XElement?> drawOverSymbolsElem = mapPathDoc.Descendants().Select(x => x.Element(ns + "DrawOverSymbols"));
            if (drawOverSymbolsElem.First() != null)
            {
                string? drawOverSymbols = mapPathDoc.Descendants().Select(x => x.Element(ns + "DrawOverSymbols").Value).FirstOrDefault();
                DrawOverSymbols = bool.Parse(drawOverSymbols);
            }

            IEnumerable<XElement> pathPointElem = mapPathDoc.Descendants(ns + "PathPoint");
            if (pathPointElem.First() != null)
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                foreach (XElement elem in pathPointElem)
                {
                    string pathPointString = elem.ToString();

                    using (XmlReader pointReader = XmlReader.Create(new StringReader(pathPointString), settings))
                    {
                        pointReader.Read();
                        MapPathPoint mpp = new();
                        mpp.ReadXml(pointReader);

                        PathPoints.Add(mpp);
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

            // map path name
            writer.WriteStartElement("MapPathName");
            writer.WriteString(MapPathName);
            writer.WriteEndElement();

            // map path GUID
            writer.WriteStartElement("MapPathGuid");
            writer.WriteString(MapPathGuid.ToString());
            writer.WriteEndElement();

            // map path points
            writer.WriteStartElement("MapPathPoints");
            foreach (MapPathPoint point in PathPoints)
            {
                writer.WriteStartElement("PathPoint");
                point.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("MapPathType");
            writer.WriteString(PathType.ToString());
            writer.WriteEndElement();

            XmlColor pathcolor = new XmlColor(PathColor);
            writer.WriteStartElement("PathColor");
            pathcolor.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("PathWidth");
            writer.WriteValue(PathWidth);
            writer.WriteEndElement();

            writer.WriteStartElement("DrawOverSymbols");
            writer.WriteValue(DrawOverSymbols);
            writer.WriteEndElement();
        }

        public override void Render(SKCanvas canvas)
        {
            // no op - points are rendered
        }
    }
}
