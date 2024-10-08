﻿/**************************************************************************************************************************
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
    public class MapRiver : WaterFeature, IXmlSerializable
    {
        public Guid MapRiverGuid { get; set; } = Guid.NewGuid();

        public string MapRiverName { get; set; } = "";

        public List<MapRiverPoint> RiverPoints { get; set; } = [];

        public Color RiverColor { get; set; } = ColorTranslator.FromHtml("#839690");
     
        public float RiverWidth { get; set; } = 4;

        public bool RiverSourceFadeIn { get; set; } = false;

        public Color RiverShorelineColor { get; set; } = ColorTranslator.FromHtml("#A19076");

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
