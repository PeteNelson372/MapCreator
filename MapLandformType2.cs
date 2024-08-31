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
    public class MapLandformType2 : MapComponent, IXmlSerializable
    {
        private bool drawLandform = true;
        private MapCreatorMap? parentMap;
        private bool isSelected = false;
        private string landformName = string.Empty;
        private Guid landformGuid = Guid.NewGuid();
        private MapTexture? landformTexture;
        private Color landformOutlineColor = ColorTranslator.FromHtml("#3D3728");
        private int landformOutlineWidth = 2;
        private GradientDirectionEnum shorelineStyle = GradientDirectionEnum.None;
        private Color coastlineColor = ColorTranslator.FromHtml("#BB9CC3B7");
        private int coastlineEffectDistance = 16;
        private string coastlineStyleName = "Dash Pattern";
        private string? coastlineHatchPattern = string.Empty;
        private int coastlineHatchOpacity = 0;
        private int coastlineHatchScale = 0;
        private string? coastlineHatchBlendMode = string.Empty;
        private bool paintCoastlineGradient = true;

        private GeneratedMapData? generatedMapData = null;

        private List<SKPoint> landformContourPoints = [];

        public MapCreatorMap? ParentMap { get { return parentMap; } set { parentMap = value; } }

        public bool DrawLandform { get { return drawLandform; } set { drawLandform = value; } }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
            }
        }
        public string LandformName { get { return landformName; } set { landformName = value; } }
        public Guid LandformGuid { get { return landformGuid; } set { landformGuid = value; } }
        public MapTexture? LandformTexture
        {
            get { return landformTexture; }
            set
            {
                landformTexture = value;
                drawLandform = true;
            }
        }

        public Color LandformOutlineColor
        {
            get { return landformOutlineColor; }
            set
            {
                landformOutlineColor = value;
                drawLandform = true;
            }
        }

        public int LandformOutlineWidth
        {
            get { return landformOutlineWidth; }
            set
            {
                landformOutlineWidth = value;
                drawLandform = true;
            }
        }

        public GradientDirectionEnum ShorelineStyle
        {
            get { return shorelineStyle; }
            set
            {
                shorelineStyle = value;
                drawLandform = true;
            }
        }

        public Color CoastlineColor
        {
            get { return coastlineColor; }
            set
            {
                coastlineColor = value;
                drawLandform = true;
            }
        }

        public int CoastlineEffectDistance
        {
            get { return coastlineEffectDistance; }
            set
            {
                coastlineEffectDistance = value;
                drawLandform = true;
            }
        }

        public string CoastlineStyleName
        {
            get { return coastlineStyleName; }
            set
            {
                coastlineStyleName = value;
                drawLandform = true;
            }
        }

        public string? CoastlineHatchPattern
        {
            get { return coastlineHatchPattern; }
            set
            {
                coastlineHatchPattern = value;
                drawLandform = true;
            }
        }

        public int CoastlineHatchOpacity
        {
            get { return coastlineHatchOpacity; }
            set
            {
                coastlineHatchOpacity = value;
                drawLandform = true;
            }
        }

        public int CoastlineHatchScale
        {
            get { return coastlineHatchScale; }
            set
            {
                coastlineHatchScale = value;
                drawLandform = true;
            }
        }

        public string? CoastlineHatchBlendMode
        {
            get { return coastlineHatchBlendMode; }
            set
            {
                coastlineHatchBlendMode = value;
                drawLandform = true;
            }
        }

        public bool PaintCoastlineGradient
        {
            get { return paintCoastlineGradient; }
            set
            {
                paintCoastlineGradient = value;
                drawLandform = true;
            }
        }

        public SKPaint? LandformBackgroundPaint { get; set; } = null;

        public SKPath LandformPath { get; set; } = new SKPath()
        {
            FillType = SKPathFillType.Winding,
        };

        public SKPath LandformContourPath { get; set; } = new SKPath();

        public List<SKPoint> LandformContourPoints
        {
            get { return landformContourPoints; }
            set
            {
                landformContourPoints = value;
                drawLandform = true;
            }
        }

        public GeneratedMapData? GenMapData
        {
            get { return generatedMapData; }
            set { generatedMapData = value; }
        }

        // inner paths are used to paint the gradient shading around the inside of the landform
        public SKPath InnerPath1 { get; set; } = new SKPath();
        public SKPath InnerPath2 { get; set; } = new SKPath();
        public SKPath InnerPath3 { get; set; } = new SKPath();
        public SKPath InnerPath4 { get; set; } = new SKPath();
        public SKPath InnerPath5 { get; set; } = new SKPath();
        public SKPath InnerPath6 { get; set; } = new SKPath();
        public SKPath InnerPath7 { get; set; } = new SKPath();
        public SKPath InnerPath8 { get; set; } = new SKPath();

        // outer paths are used to paint the coastline effect around the outside of the landform
        public SKPath OuterPath1 { get; set; } = new SKPath();
        public SKPath OuterPath2 { get; set; } = new SKPath();
        public SKPath OuterPath3 { get; set; } = new SKPath();
        public SKPath OuterPath4 { get; set; } = new SKPath();
        public SKPath OuterPath5 { get; set; } = new SKPath();
        public SKPath OuterPath6 { get; set; } = new SKPath();
        public SKPath OuterPath7 { get; set; } = new SKPath();
        public SKPath OuterPath8 { get; set; } = new SKPath();

        //public List<SKPath> LandformColorPaths { get; set; } = [];

        public MapLandformType2()
        {
            RenderComponent = true;
            LandformGuid = Guid.NewGuid();
        }

        public override void Render(SKCanvas canvas)
        {
            if (ParentMap != null)
            {
                LandformType2Methods.DrawLandform(ParentMap, this);
            }
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
            XDocument mapLandformDoc = XDocument.Parse(content);

            IEnumerable<XElement?> nameElemEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformName"));
            if (nameElemEnum.First() != null)
            {
                string? name = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformName").Value).FirstOrDefault();
                LandformName = name;
            }

            IEnumerable<XElement?> guidElemEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformGuid"));
            if (guidElemEnum.First() != null)
            {
                string? mapGuid = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformGuid").Value).FirstOrDefault();
                LandformGuid = Guid.Parse(mapGuid);
            }

            IEnumerable<XElement?> landformTextureElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformTexture"));
            if (landformTextureElem.First() != null)
            {
                string? txName = null;
                string? txPath = null;

                if (landformTextureElem.First() is XElement txNameEl)
                {
                    txName = txNameEl.Value;
                }

                if (landformTextureElem.Last() is XElement txPathEl)
                {
                    txPath = txPathEl.Value;
                }

                if (txName != null && txPath != null)
                {
                    LandformTexture = new(txName, txPath);

                    Bitmap b = new(LandformTexture.TexturePath);
                    Bitmap resizedBitmap = new(b, MapBuilder.MAP_DEFAULT_WIDTH, MapBuilder.MAP_DEFAULT_HEIGHT);

                    // create and set a shader from the selected texture
                    SKShader s = SKShader.CreateBitmap(SkiaSharp.Views.Desktop.Extensions.ToSKBitmap(resizedBitmap),
                        SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                    SKPaint p = new()
                    {
                        Shader = s,
                        Style = SKPaintStyle.Fill,
                        IsAntialias = true,
                        BlendMode = SKBlendMode.DstATop
                    };

                    LandformBackgroundPaint = p;
                }
            }

            IEnumerable<XElement> colorElem = mapLandformDoc.Descendants(ns + "LandformOutlineColor");
            if (colorElem.First() != null)
            {
                string? color = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformOutlineColor").Value).FirstOrDefault();
                LandformOutlineColor = ColorTranslator.FromHtml(color);
            }

            IEnumerable<XElement?> widthElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformOutlineWidth"));
            if (widthElem.First() != null)
            {
                string? width = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformOutlineWidth").Value).FirstOrDefault();
                LandformOutlineWidth = int.Parse(width);
            }


            IEnumerable<XElement?> styleEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "ShorelineStyle"));
            if (nameElemEnum.First() != null)
            {
                string? styleName = mapLandformDoc.Descendants().Select(x => x.Element(ns + "ShorelineStyle").Value).FirstOrDefault();
                ShorelineStyle = Enum.Parse<GradientDirectionEnum>(styleName);
            }

            colorElem = mapLandformDoc.Descendants(ns + "CoastlineColor");
            if (colorElem.First() != null)
            {
                string? color = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineColor").Value).FirstOrDefault();
                CoastlineColor = ColorTranslator.FromHtml(color);
            }

            IEnumerable<XElement?> distanceElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineEffectDistance"));
            if (distanceElem.First() != null)
            {
                string? distance = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineEffectDistance").Value).FirstOrDefault();
                CoastlineEffectDistance = int.Parse(distance);
            }

            IEnumerable<XElement?> styleNameEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineStyleName"));
            if (styleNameEnum.First() != null)
            {
                string? styleName = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineStyleName").Value).FirstOrDefault();
                CoastlineStyleName = styleName;
            }

            IEnumerable<XElement?> patternNameEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchPattern"));
            if (patternNameEnum.First() != null)
            {
                string? patternName = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchPattern").Value).FirstOrDefault();
                CoastlineHatchPattern = patternName;
            }

            IEnumerable<XElement?> opacityEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchOpacity"));
            if (opacityEnum.First() != null)
            {
                string? opacity = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchOpacity").Value).FirstOrDefault();
                CoastlineHatchOpacity = int.Parse(opacity);
            }

            IEnumerable<XElement?> hatchScaleEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchScale"));
            if (hatchScaleEnum.First() != null)
            {
                string? hatchScale = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchScale").Value).FirstOrDefault();
                CoastlineHatchScale = int.Parse(hatchScale);
            }

            IEnumerable<XElement?> blendModeEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchBlendMode"));
            if (blendModeEnum.First() != null)
            {
                string? blendMode = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchBlendMode").Value).FirstOrDefault();
                CoastlineHatchBlendMode = blendMode;
            }

            IEnumerable<XElement?> boolElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "PaintCoastlineGradient"));
            if (boolElem.First() != null)
            {
                string? paintCoastlineGradient = mapLandformDoc.Descendants().Select(x => x.Element(ns + "PaintCoastlineGradient").Value).FirstOrDefault();
                PaintCoastlineGradient = bool.Parse(paintCoastlineGradient);
            }

            IEnumerable<XElement?> pathElemEnum = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformPath"));
            if (pathElemEnum.First() != null)
            {
                string? landformPath = mapLandformDoc.Descendants().Select(x => x.Element(ns + "LandformPath").Value).FirstOrDefault();
                LandformPath = SKPath.ParseSvgPathData(landformPath);
            }

            /*
            IEnumerable<XElement> colorPathElem = mapLandformDoc.Descendants(ns + "LandformColorPaths");
            if (colorPathElem != null && colorPathElem.Any() && colorPathElem.First() != null)
            {
                List<XElement> elemList = colorPathElem.Descendants().ToList();

                if (elemList != null)
                {
                    foreach (XElement elem in elemList)
                    {
                        if (elem.Name.LocalName.ToString() == "LandformColorPath")
                        {
                            SKPath skColorPath = SKPath.ParseSvgPathData(elem.Value);
                            LandformColorPaths.Add(skColorPath);
                        }
                    }
                }
            }
            */

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public void WriteXml(XmlWriter writer)
        {
            using MemoryStream ms = new();
            using SKManagedWStream wstream = new(ms);

            // landform name
            writer.WriteStartElement("LandformName");
            writer.WriteString(LandformName);
            writer.WriteEndElement();

            // landform GUID
            writer.WriteStartElement("LandformGuid");
            writer.WriteString(LandformGuid.ToString());
            writer.WriteEndElement();

            if (LandformTexture != null)
            {
                // landform texture
                writer.WriteStartElement("LandformTexture");
                writer.WriteStartElement("TextureName");
                writer.WriteString(LandformTexture.TextureName);
                writer.WriteEndElement();
                writer.WriteStartElement("TexturePath");
                writer.WriteString(LandformTexture.TexturePath);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }


            // landform outline color
            XmlColor outlinecolor = new(LandformOutlineColor);
            writer.WriteStartElement("LandformOutlineColor");
            outlinecolor.WriteXml(writer);
            writer.WriteEndElement();

            // landform outline width
            writer.WriteStartElement("LandformOutlineWidth");
            writer.WriteValue(LandformOutlineWidth);
            writer.WriteEndElement();

            // shoreline style
            writer.WriteStartElement("ShorelineStyle");
            writer.WriteString(ShorelineStyle.ToString());
            writer.WriteEndElement();

            // coastline color
            XmlColor coastcolor = new(CoastlineColor);
            writer.WriteStartElement("CoastlineColor");
            coastcolor.WriteXml(writer);
            writer.WriteEndElement();

            // coastline effect distance
            writer.WriteStartElement("CoastlineEffectDistance");
            writer.WriteValue(CoastlineEffectDistance);
            writer.WriteEndElement();


            if (CoastlineStyleName != null)
            {
                // coastline style
                writer.WriteStartElement("CoastlineStyleName");
                writer.WriteString(CoastlineStyleName);
                writer.WriteEndElement();
            }

            if (CoastlineHatchPattern != null)
            {
                // coastline hatch pattern
                writer.WriteStartElement("CoastlineHatchPattern");
                writer.WriteString(CoastlineHatchPattern);
                writer.WriteEndElement();
            }

            // coastline hatch opacity
            writer.WriteStartElement("CoastlineHatchOpacity");
            writer.WriteValue(CoastlineHatchOpacity);
            writer.WriteEndElement();

            // coastline hatch scale
            writer.WriteStartElement("CoastlineHatchScale");
            writer.WriteValue(CoastlineHatchScale);
            writer.WriteEndElement();

            // coastline hatch blend mode
            writer.WriteStartElement("CoastlineHatchBlendMode");
            writer.WriteString(CoastlineHatchBlendMode);
            writer.WriteEndElement();

            // paint coastline gradient
            writer.WriteStartElement("PaintCoastlineGradient");
            writer.WriteValue(PaintCoastlineGradient);
            writer.WriteEndElement();

            // landform path
            writer.WriteStartElement("LandformPath");
            string pathSvg = LandformPath.ToSvgPathData();
            writer.WriteValue(pathSvg);
            writer.WriteEndElement();

            /*
            // landform color paths
            writer.WriteStartElement("LandformColorPaths");

            for (int i = 0; i < LandformColorPaths.Count; i++)
            {
                SKPath path = LandformColorPaths[i];
                if (path != null && path.PointCount > 0)
                {
                    string colorPathSvg = path.ToSvgPathData();
                    writer.WriteElementString("LandformColorPath", colorPathSvg);
                }
            }

            writer.WriteEndElement();
            */
        }
    }
}
