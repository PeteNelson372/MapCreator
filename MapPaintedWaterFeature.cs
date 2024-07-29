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
    public class MapPaintedWaterFeature : WaterFeature, IXmlSerializable
    {
        [XmlIgnore]
        public MapCreatorMap? ParentMap { get; set; } = null;

        [XmlIgnore]
        public bool IsSelected { get; set; } = false;
        
        [XmlIgnore]
        public bool IsRemoved { get; set; } = false;

        public string WaterFeatureName { get; set; } = "";
        public Guid WaterFeatureGuid { get; set; }
        public WaterFeatureTypeEnum WaterFeatureType { get; set; } = WaterFeatureTypeEnum.NotSet;
        public Color? WaterFeatureColor { get; set; }
        public int? WaterFeatureColorOpacity { get; set; }
        public Color? WaterFeatureShorelineColor { get; set; } = ColorTranslator.FromHtml("#A19076");
        public int? WaterFeatureShorelineColorOpacity { get; set; } = 255;
        public float WaterFeaturePathSegmentLength { get; set; }
        public float WaterFeaturePathVariance { get; set; }
        public uint WaterFeatureVarianceSeed { get; set; }

        [XmlIgnore]
        public SKPaint? WaterFeatureBackgroundPaint { get; set; } = null;
        [XmlIgnore]
        public SKPaint? WaterFeatureShorelinePaint { get; set; } = null;
        [XmlIgnore]
        public SKPaint? ShallowWaterPaint { get; set; } = null;

        [XmlIgnore]
        public SKPath WaterFeaturePath { get; set; } = new()
        {
            FillType = SKPathFillType.Winding
        };

        [XmlIgnore]
        public List<SKPath> WaterFeatureColorPaths { get; set; } = [];

        public MapPaintedWaterFeature()
        {
            RenderComponent = false;
            WaterFeatureGuid = Guid.NewGuid();
        }

        public MapPaintedWaterFeature(MapPaintedWaterFeature original)
        {

        }

        public SKPath GetWaterFeaturePath()
        {
            return WaterFeaturePath;
        }

        public void SetWaterFeaturePath(SKPath path)
        {
            WaterFeaturePath = new(path);
        }

        public void ResetWaterFeaturePath()
        {
            WaterFeaturePath.Reset();
        }

        public List<SKPath> GetWaterFeatureColorPathList()
        {
            return WaterFeatureColorPaths;
        }

        public void AddWaterFeatureColorPath(SKPath path)
        {
            WaterFeatureColorPaths.Add(path);
        }

        public SKPath GetWaterFeatureColorPathAtIndex(int index)
        {
            return WaterFeatureColorPaths[index];
        }

        public void RemoveWaterFeatureColorPathAtIndex(int index)
        {
            WaterFeatureColorPaths.RemoveAt(index);
        }

        public void ClearWaterFeatureColorPaths()
        {
            foreach (SKPath p in WaterFeatureColorPaths)
            {
                p.Reset();
            }

            WaterFeatureColorPaths.Clear();
        }

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
            XDocument mapWaterFeatureDoc = XDocument.Parse(content);

            IEnumerable<XNode> nodes = mapWaterFeatureDoc.Descendants();

            if (!(nodes.Count() > 1))
            {
                IsRemoved = true;
                return;
            }

            IEnumerable<XElement?> nameElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureName"));
            if (nameElemEnum.First() != null)
            {
                string? mapWaterFeatureName = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureName").Value).FirstOrDefault();
                WaterFeatureName = mapWaterFeatureName;
            }

            IEnumerable<XElement?> guidElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureGuid"));
            if (guidElemEnum.First() != null)
            {
                string? mapGuid = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureGuid").Value).FirstOrDefault();
                WaterFeatureGuid = Guid.Parse(mapGuid);
            }

            IEnumerable<XElement?> typeElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureType"));
            if (typeElemEnum.First() != null)
            {
                string? waterFeatureType = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureType").Value).FirstOrDefault();
                WaterFeatureType = Enum.Parse<WaterFeatureTypeEnum>(waterFeatureType);
            }

            IEnumerable<XElement?> colorElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureColor"));
            if (colorElemEnum.First() != null)
            {
                string? waterFeatureColor = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureColor").Value).FirstOrDefault();
                WaterFeatureColor = ColorTranslator.FromHtml(waterFeatureColor);
            }

            IEnumerable<XElement?> colorOpacityElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureColorOpacity"));
            if (colorOpacityElemEnum.First() != null)
            {
                string? waterFeatureColorOpacity = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureColorOpacity").Value).FirstOrDefault();
                WaterFeatureColorOpacity = Convert.ToInt32(waterFeatureColorOpacity);
            }

            IEnumerable<XElement?> shoreColorElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureShorelineColor"));
            if (shoreColorElemEnum.First() != null)
            {
                string? waterFeatureShorelineColor = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureShorelineColor").Value).FirstOrDefault();
                WaterFeatureShorelineColor = ColorTranslator.FromHtml(waterFeatureShorelineColor);
            }

            IEnumerable<XElement?> shoreColorOpacityElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureShorelineColorOpacity"));
            if (shoreColorOpacityElemEnum.First() != null)
            {
                string? waterFeatureShorelineColorOpacity = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureShorelineColorOpacity").Value).FirstOrDefault();
                WaterFeatureShorelineColorOpacity = Convert.ToInt32(waterFeatureShorelineColorOpacity);
            }

            IEnumerable<XElement?> segLengthElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePathSegmentLength"));
            if (segLengthElemEnum.First() != null)
            {
                string? waterFeaturePathSegmentLength = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePathSegmentLength").Value).FirstOrDefault();
                WaterFeaturePathSegmentLength = Convert.ToSingle(waterFeaturePathSegmentLength);
            }

            IEnumerable<XElement?> pathVarElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePathVariance"));
            if (pathVarElemEnum.First() != null)
            {
                string? waterFeaturePathVariance = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePathVariance").Value).FirstOrDefault();
                WaterFeaturePathVariance = Convert.ToSingle(waterFeaturePathVariance);
            }

            IEnumerable<XElement?> varSeedElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureVarianceSeed"));
            if (varSeedElemEnum.First() != null)
            {
                string? waterFeatureVarianceSeed = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeatureVarianceSeed").Value).FirstOrDefault();
                WaterFeatureVarianceSeed = Convert.ToUInt32(waterFeatureVarianceSeed);
            }

            IEnumerable<XElement?> pathElemEnum = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePath"));
            if (pathElemEnum.First() != null)
            {
                string? waterFeaturePath = mapWaterFeatureDoc.Descendants().Select(x => x.Element(ns + "WaterFeaturePath").Value).FirstOrDefault();
                WaterFeaturePath = SKPath.ParseSvgPathData(waterFeaturePath);
            }

            IEnumerable<XElement> colorPathElem = mapWaterFeatureDoc.Descendants(ns + "WaterFeatureColorPaths");
            if (colorPathElem != null && colorPathElem.Any() && colorPathElem.First() != null)
            {
                List<XElement> elemList = colorPathElem.Descendants().ToList();

                if (elemList != null)
                {
                    foreach (XElement elem in elemList)
                    {
                        if (elem.Name.LocalName.ToString() == "WaterFeatureColorPath")
                        {
                            SKPath skColorPath = SKPath.ParseSvgPathData(elem.Value);
                            WaterFeatureColorPaths.Add(skColorPath);
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
            if (IsRemoved) return;

            using MemoryStream ms = new();
            using SKManagedWStream wstream = new(ms);

            // water feature name
            writer.WriteStartElement("WaterFeatureName");
            writer.WriteString(WaterFeatureName);
            writer.WriteEndElement();

            // water feature GUID
            writer.WriteStartElement("WaterFeatureGuid");
            writer.WriteString(WaterFeatureGuid.ToString());
            writer.WriteEndElement();

            // water feature type
            writer.WriteStartElement("WaterFeatureType");
            writer.WriteString(WaterFeatureType.ToString());
            writer.WriteEndElement();

            if (WaterFeatureColor != null)
            {
                // water feature color
                XmlColor waterfeaturecolor = new XmlColor((Color)WaterFeatureColor);
                writer.WriteStartElement("WaterFeatureColor");
                waterfeaturecolor.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (WaterFeatureColorOpacity != null)
            {
                // water feature color opacity
                writer.WriteStartElement("WaterFeatureColorOpacity");
                writer.WriteValue(WaterFeatureColorOpacity);
                writer.WriteEndElement();
            }

            // path segment length
            writer.WriteStartElement("WaterFeaturePathSegmentLength");
            writer.WriteValue(WaterFeaturePathSegmentLength);
            writer.WriteEndElement();

            // path variance
            writer.WriteStartElement("WaterFeaturePathVariance");
            writer.WriteValue(WaterFeaturePathVariance);
            writer.WriteEndElement();

            // path variance seed
            writer.WriteStartElement("WaterFeatureVarianceSeed");
            writer.WriteValue(WaterFeatureVarianceSeed);
            writer.WriteEndElement();

            // water feature path
            string pathSvg = WaterFeaturePath.ToSvgPathData();
            writer.WriteElementString("WaterFeaturePath", pathSvg);

            // water feature color paths
            writer.WriteStartElement("WaterFeatureColorPaths");

            for (int i = 0; i < WaterFeatureColorPaths.Count; i++)
            {
                SKPath path = WaterFeatureColorPaths[i];
                if (path != null && path.PointCount > 0)
                {
                    string colorPathSvg = path.ToSvgPathData();
                    writer.WriteElementString("WaterFeatureColorPath", colorPathSvg);
                }
            }

            writer.WriteEndElement();
        }
    }
}
