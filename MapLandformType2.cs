using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MapCreator
{
    public class MapLandformType2 : MapComponent, IXmlSerializable
    {
        public MapCreatorMap? ParentMap { get; set; } = null;

        public bool IsSelected { get; set; } = false;
        public string LandformName { get; set; } = "";
        public Guid LandformGuid { get; set; } = new Guid();
        public MapTexture? LandformTexture { get; set; }
        public Color LandformOutlineColor { get; set; } = ColorTranslator.FromHtml("#3D3728");
        public int LandformOutlineWidth { get; set; } = 2;
        public GradientDirectionEnum? ShorelineStyle { get; set; }
        public Color CoastlineColor { get; set; } = ColorTranslator.FromHtml("#9CC3B7");
        public int CoastlineColorOpacity { get; set; } = 187;
        public int CoastlineEffectDistance { get; set; } = 16;
        public string CoastlineStyleName { get; set; } = "Dash Pattern";
        public string? CoastlineHatchPattern { get; set; }
        public int? CoastlineHatchOpacity { get; set; }
        public int? CoastlineHatchScale { get; set; }
        public string? CoastlineHatchBlendMode { get; set; }
        public bool? PaintCoastlineGradient { get; set; }

        public SKPaint? LandformBackgroundPaint { get; set; } = null;

        public SKPath LandformPath { get; set; } = new SKPath()
        {
            FillType = SKPathFillType.Winding,
        };

        public SKPath LandformContourPath { get; set; } = new SKPath();

        public List<SKPoint> LandformContourPoints { get; set; } = [];

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
            RenderComponent = false;
            LandformGuid = Guid.NewGuid();
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

            IEnumerable<XElement?> opacityElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineColorOpacity"));
            if (opacityElem.First() != null)
            {
                string? opacity = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineColorOpacity").Value).FirstOrDefault();
                CoastlineColorOpacity = int.Parse(opacity);
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

            opacityElem = mapLandformDoc.Descendants().Select(x => x.Element(ns + "CoastlineHatchOpacity"));
            if (opacityElem.First() != null)
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

            // coastline color opacity
            writer.WriteStartElement("CoastlineColorOpacity");
            writer.WriteValue(CoastlineColorOpacity);
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

            if (CoastlineHatchOpacity != null)
            {
                // coastline hatch opacity
                writer.WriteStartElement("CoastlineHatchOpacity");
                writer.WriteValue(CoastlineHatchOpacity);
                writer.WriteEndElement();
            }

            if (CoastlineHatchScale != null)
            {
                // coastline hatch scale
                writer.WriteStartElement("CoastlineHatchScale");
                writer.WriteValue(CoastlineHatchScale);
                writer.WriteEndElement();
            }


            // coastline hatch blend mode
            writer.WriteStartElement("CoastlineHatchBlendMode");
            writer.WriteString(CoastlineHatchBlendMode);
            writer.WriteEndElement();

            if (PaintCoastlineGradient != null)
            {
                // paint coastline gradient
                writer.WriteStartElement("PaintCoastlineGradient");
                writer.WriteValue(PaintCoastlineGradient);
                writer.WriteEndElement();
            }


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

        public override void Render(SKCanvas canvas)
        {
            // no op - paths are rendered
        }
    }
}
