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
using Extensions = SkiaSharp.Views.Desktop.Extensions;

namespace MapCreator
{
    public class MapSymbol : MapComponent, IXmlSerializable
    {
        // TODO: refactor to use public attributes with { get; set; } and remove accessor methods
        public int XLocation { get; set; }

        public int YLocation { get; set; }

        public int SymbolWidth { get; set; }

        public int SymbolHeight { get; set; }

        private bool IsSelected = false;

        private SymbolFormatEnum SymbolFormat = SymbolFormatEnum.NotSet;

        private SymbolTypeEnum SymbolType = SymbolTypeEnum.NotSet;

        private readonly List<string> SymbolTags = [];

        private Guid SymbolGuid = Guid.NewGuid();

        private string SymbolName = string.Empty;

        private string CollectionName = string.Empty;

        private string CollectionPath = string.Empty;

        private string SymbolFilePath = string.Empty;

        private bool IsGrayscale = false;

        private bool UseCustomColors = false;

        private readonly SKColor[] CustomSymbolColors = new SKColor[4];

        private SKPaint? SymbolPaint = null;

        private SKBitmap? SymbolBitmap = null;

        private SKBitmap? ColorMappedBitmap = null;

        private SKBitmap? PlacedBitmap = null;

        private string SymbolSVG = string.Empty;

        public MapSymbol() { }

        public MapSymbol(MapSymbol original)
        {
            IsGrayscale = original.IsGrayscale;
            UseCustomColors = original.UseCustomColors;

            for(int i = 0; i < CustomSymbolColors.Length; i++)
            {
                CustomSymbolColors[i] = original.CustomSymbolColors[i];
            }

            IsSelected = original.IsSelected;
            SymbolFormat = original.SymbolFormat;
            CollectionName = original.CollectionName;
            PlacedBitmap = original.PlacedBitmap?.Copy();
            SymbolBitmap = original.SymbolBitmap?.Copy();
            SymbolFilePath = original.SymbolFilePath;
            SymbolName = original.SymbolName;
            SymbolSVG = original.SymbolSVG;
            SymbolType = original.SymbolType;
            SymbolTags = [.. original.SymbolTags];
            Width = original.Width;
            Height = original.Height;
            X = original.X;
            Y = original.Y;
        }

        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
        }

        public bool GetIsSelected()
        {
            return IsSelected;
        }

        public SymbolFormatEnum GetSymbolFormat()
        {
            return SymbolFormat;
        }

        public void SetSymbolFormat(SymbolFormatEnum format)
        {
            SymbolFormat = format;        
        }

        public SymbolTypeEnum GetSymbolType()
        {
            return SymbolType;
        }

        public void SetSymbolType(SymbolTypeEnum st)
        {
            SymbolType = st;
        }

        public string GetSymbolName()
        {
            return SymbolName;
        }

        public void SetSymbolName(string name)
        {
            SymbolName = name;
        }

        public string GetCollectionName()
        {
            return CollectionName;
        }

        public void SetCollectionName(string name)
        {
            CollectionName = name;
        }

        public string GetCollectionPath()
        {
            return CollectionPath;
        }

        public void SetCollectionPath(string path)
        {
            CollectionPath = path;
        }

        public Guid GetSymbolGuid()
        {
            return SymbolGuid;
        }

        public void ClearSymbolTags()
        {
            SymbolTags.Clear();
        }

        public List<string> GetSymbolTags()
        {
            return SymbolTags;
        }

        public void AddSymbolTag(string tag)
        {
            if (SymbolTags.Contains(tag)) return;
            SymbolTags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            return SymbolTags.Contains(tag);
        }

        public int GetSymbolTagCount()
        {
            return SymbolTags.Count;
        }

        public bool HasAnySymbolTags()
        {
            return SymbolTags.Any();
        }

        public bool GetIsGrayScale()
        {
            return IsGrayscale;
        }

        public void SetIsGrayScale(bool isGrayScale)
        {
            IsGrayscale = isGrayScale;
        }

        public bool GetUseCustomColors()
        {
            return UseCustomColors;
        }

        public void SetUseCustomColors(bool useCustomColors)
        {
            UseCustomColors = useCustomColors;
        }

        public void SetSymbolCustomColorAtIndex(SKColor color, int index)
        {
            if (index < 0 || index > CustomSymbolColors.Length - 1) return;
            CustomSymbolColors[index] = color;
        }

        public SKColor? GetSymbolCustomColorAtIndex(int index)
        {
            if (index < 0 || index > CustomSymbolColors.Length - 1) return null;
            return CustomSymbolColors[index];
        }

        public void SetSymboFilePath(string filePath)
        {
            SymbolFilePath = filePath;
        }

        public string GetSymbolFilePath()
        {
            return SymbolFilePath;
        }
        
        public SKBitmap? GetSymbolBitmap()
        {
            return SymbolBitmap;
        }

        public void SetSymbolBitmap(SKBitmap? bitmap)
        {
            SymbolBitmap = bitmap;
        }

        public void SetSymbolBitmapFromPath(string path)
        {
            if (File.Exists(path))
            {
                SymbolBitmap?.Dispose();
                SymbolBitmap = SKBitmap.Decode(path);
            }
        }

        public SKBitmap? GetPlacedBitmap()
        {
            return PlacedBitmap;
        }

        public void SetPlacedBitmap(SKBitmap placedBitmap)
        {
            PlacedBitmap = placedBitmap;
            Width = PlacedBitmap.Width;
            Height = PlacedBitmap.Height;
        }

        public SKBitmap? GetColorMappedBitmap()
        {
            return ColorMappedBitmap;
        }

        public void SetColorMappedBitmap(SKBitmap colorMappedBitmap)
        {
            ColorMappedBitmap = colorMappedBitmap;
        }

        public void SetSymbolSVG(string svg)
        {
            SymbolSVG = svg;
        }

        public string GetSymbolSVG()
        {
            return SymbolSVG;
        }

        public void SetSymbolPaint(SKPaint? paint)
        {
            SymbolPaint = paint;
        }

        public void ClearSymbolPaint()
        {
            SymbolPaint = null;
        }

        public override void Render(SKCanvas canvas)
        {
            if (PlacedBitmap != null)
            {
                SKPoint point = new(X, Y);

                if (IsGrayscale)
                {
                    canvas.DrawBitmap(PlacedBitmap, point, SymbolPaint);
                }
                else
                {
                    canvas.DrawBitmap(PlacedBitmap, point, null);
                }

                if (IsSelected)
                {
                    // draw line around bitmap to show it is selected
                    SKPaint SymbolSelectPaint = SymbolMethods.GetSymbolSelectPaint();
                    SKRect selectRect = new(X, Y, X + Width, Y + Height);
                    canvas.DrawRect(selectRect, SymbolSelectPaint);
                }
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
            XDocument mapSymbolDoc = XDocument.Parse(content);

            XAttribute? xAttr = mapSymbolDoc.Root.Attribute("X");
            if (xAttr != null)
            {
                X = int.Parse(xAttr.Value);
            }

            XAttribute? yAttr = mapSymbolDoc.Root.Attribute("Y");
            if (yAttr != null)
            {
                Y = int.Parse(yAttr.Value);
            }

            XAttribute? wAttr = mapSymbolDoc.Root.Attribute("Width");
            if (wAttr != null)
            {
                Width = int.Parse(wAttr.Value);
            }

            XAttribute? hAttr = mapSymbolDoc.Root.Attribute("Height");
            if (hAttr != null)
            {
                Height = int.Parse(hAttr.Value);
            }

            IEnumerable<XElement?> xElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "XLocation"));
            if (xElemEnum.First() != null)
            {
                string? xLocation = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "XLocation").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(xLocation))
                {
                    XLocation = int.Parse(xLocation);
                }
            }

            IEnumerable<XElement?> yElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "YLocation"));
            if (yElemEnum.First() != null)
            {
                string? yLocation = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "YLocation").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(yLocation))
                {
                    YLocation = int.Parse(yLocation);
                }
            }

            IEnumerable<XElement?> wElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolWidth"));
            if (wElemEnum.First() != null)
            {
                string? symboWidth = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolWidth").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(symboWidth))
                {
                    SymbolWidth = int.Parse(symboWidth);
                }
            }

            IEnumerable<XElement?> hElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolHeight"));
            if (hElemEnum.First() != null)
            {
                string? symbolHeight = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolHeight").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(symbolHeight))
                {
                    SymbolHeight = int.Parse(symbolHeight);
                }
            }

            string? symbolFormat = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolFormat").Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(symbolFormat))
            {
                SymbolFormat = Enum.Parse<SymbolFormatEnum>(symbolFormat);
            }

            string? symbolType = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolType").Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(symbolFormat))
            {
                SymbolType = Enum.Parse<SymbolTypeEnum>(symbolType);
            }

            IEnumerable<XElement> tagElem = mapSymbolDoc.Descendants(ns + "Tag");
            foreach (XElement tag in tagElem)
            {
                string tagValue = tag.Value;
                if (!string.IsNullOrEmpty(tagValue))
                {
                    SymbolTags.Add(tagValue);
                }
            }

            string? symbolGuid = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolGuid").Value).FirstOrDefault();
            SymbolGuid = Guid.Parse(symbolGuid);

            string? symbolName = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolName").Value).FirstOrDefault();
            SymbolName = symbolName;

            string? collectionName = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "CollectionName").Value).FirstOrDefault();
            CollectionName = collectionName;

            IEnumerable<XElement?> cpElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "CollectionPath"));
            if (cpElemEnum.First() != null)
            {
                string? collectionPath = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "CollectionPath").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(CollectionPath))
                {
                    CollectionPath = collectionPath;
                }
            }

            string? symbolFilePath = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolFilePath").Value).FirstOrDefault();
            SymbolFilePath = symbolFilePath;

            string? isGrayscale = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "IsGrayscale").Value).FirstOrDefault();
            IsGrayscale = bool.Parse(isGrayscale);

            string? useCustomColors = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "UseCustomColors").Value).FirstOrDefault();
            UseCustomColors = bool.Parse(useCustomColors);

            IEnumerable<XElement> customColorElem = mapSymbolDoc.Descendants(ns + "CustomColors");
            if (customColorElem.First() != null)
            {
                List<XElement> elemList = customColorElem.Descendants().ToList();

                if (elemList != null && elemList.Count == 4)
                {
                    XElement colorELem = elemList[0];
                    string colorString = colorELem.Value;
                    CustomSymbolColors[0] = Extensions.ToSKColor(ColorTranslator.FromHtml(colorString));

                    colorELem = elemList[1];
                    colorString = colorELem.Value;
                    CustomSymbolColors[1] = Extensions.ToSKColor(ColorTranslator.FromHtml(colorString));

                    colorELem = elemList[2];
                    colorString = colorELem.Value;
                    CustomSymbolColors[2] = Extensions.ToSKColor(ColorTranslator.FromHtml(colorString));

                    colorELem = elemList[3];
                    colorString = colorELem.Value;
                    CustomSymbolColors[3] = Extensions.ToSKColor(ColorTranslator.FromHtml(colorString));
                }
            }

            IEnumerable<XElement?> sbmElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolBitmap"));
            if (sbmElemEnum.First() != null)
            {
                string? symbolBitmapBase64String = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "SymbolBitmap").Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(symbolBitmapBase64String))
                {
                    // Convert Base64 string to byte array
                    byte[] imageBytes = Convert.FromBase64String(symbolBitmapBase64String);

                    // Create an image from the byte array
                    using MemoryStream ms = new(imageBytes);
                    SymbolBitmap = SKBitmap.Decode(ms);
                }
            }

            IEnumerable<XElement?> cmbmElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "ColorMappedBitmap"));
            if (cmbmElemEnum.First() != null)
            {
                string? colorMappedBitmapBase64String = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "ColorMappedBitmap").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(colorMappedBitmapBase64String))
                {
                    // Convert Base64 string to byte array
                    byte[] imageBytes = Convert.FromBase64String(colorMappedBitmapBase64String);

                    // Create an image from the byte array
                    using MemoryStream ms = new(imageBytes);
                    ColorMappedBitmap = SKBitmap.Decode(ms);
                }
            }

            IEnumerable<XElement?> plbmElemEnum = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "PlacedBitmap"));
            if (plbmElemEnum.First() != null)
            {
                string? placedBitmapBase64String = mapSymbolDoc.Descendants().Select(x => x.Element(ns + "PlacedBitmap").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(placedBitmapBase64String))
                {
                    // Convert Base64 string to byte array
                    byte[] imageBytes = Convert.FromBase64String(placedBitmapBase64String);

                    // Create an image from the byte array
                    using MemoryStream ms = new(imageBytes);
                    PlacedBitmap = SKBitmap.Decode(ms);
                }
            }

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("X", X.ToString());
            writer.WriteAttributeString("Y", Y.ToString());
            writer.WriteAttributeString("Width", Width.ToString());
            writer.WriteAttributeString("Height", Height.ToString());

            writer.WriteStartElement("XLocation");
            writer.WriteValue(XLocation);
            writer.WriteEndElement();

            writer.WriteStartElement("YLocation");
            writer.WriteValue(YLocation);
            writer.WriteEndElement();

            writer.WriteStartElement("SymbolWidth");
            writer.WriteValue(SymbolWidth);
            writer.WriteEndElement();

            writer.WriteStartElement("SymbolHeight");
            writer.WriteValue(SymbolHeight);
            writer.WriteEndElement();

            // symbol format
            writer.WriteStartElement("SymbolFormat");
            writer.WriteString(SymbolFormat.ToString());
            writer.WriteEndElement();

            // symbol type
            writer.WriteStartElement("SymbolType");
            writer.WriteString(SymbolType.ToString());
            writer.WriteEndElement();

            // map path points
            writer.WriteStartElement("SymbolTags");
            foreach (string tag in SymbolTags)
            {
                writer.WriteStartElement("Tag");
                writer.WriteString($"{tag}");
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("SymbolGuid");
            writer.WriteString(SymbolGuid.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("SymbolName");
            writer.WriteString(SymbolName);
            writer.WriteEndElement();

            writer.WriteStartElement("CollectionName");
            writer.WriteString(CollectionName);
            writer.WriteEndElement();

            writer.WriteStartElement("CollectionPath");
            writer.WriteString(CollectionPath);
            writer.WriteEndElement();

            writer.WriteStartElement("SymbolFilePath");
            writer.WriteString(SymbolFilePath);
            writer.WriteEndElement();

            writer.WriteStartElement("IsGrayscale");
            writer.WriteValue(IsGrayscale);
            writer.WriteEndElement();

            writer.WriteStartElement("UseCustomColors");
            writer.WriteValue(UseCustomColors);
            writer.WriteEndElement();

            writer.WriteStartElement("CustomColors");
            for (int i = 0; i < CustomSymbolColors.Length; i++)
            {
                XmlColor color = new XmlColor(Extensions.ToDrawingColor(CustomSymbolColors[i]));
                writer.WriteStartElement("CustomColor" + i.ToString());
                color.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            if (SymbolBitmap != null)
            {
                using MemoryStream ms = new();
                using SKManagedWStream wstream = new(ms);
                SymbolBitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
                byte[] symbolBitmapData = ms.ToArray();
                writer.WriteStartElement("SymbolBitmap");
                writer.WriteBase64(symbolBitmapData, 0, symbolBitmapData.Length);
                writer.WriteEndElement();
            }

            if (ColorMappedBitmap != null)
            {
                using MemoryStream ms = new();
                using SKManagedWStream wstream = new(ms);
                ColorMappedBitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
                byte[] colorMappedBitmapData = ms.ToArray();
                writer.WriteStartElement("ColorMappedBitmap");
                writer.WriteBase64(colorMappedBitmapData, 0, colorMappedBitmapData.Length);
                writer.WriteEndElement();
            }

            if (PlacedBitmap != null)
            {
                using MemoryStream ms = new();
                using SKManagedWStream wstream = new(ms);
                PlacedBitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
                byte[] placedBitmapData = ms.ToArray();
                writer.WriteStartElement("PlacedBitmap");
                writer.WriteBase64(placedBitmapData, 0, placedBitmapData.Length);
                writer.WriteEndElement();
            }
        }
    }
}
