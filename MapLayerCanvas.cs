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
    public class MapLayerCanvas : MapComponent, IXmlSerializable
    {
        public bool Show { get; set; } = true;

        public SKBitmap? LayerCanvasBitmap { get; set; }

        public SKCanvas? LayerCanvas { get; set; }

        public bool RenderCanvas { get; set; } = false;

        public MapLayerCanvas() { }

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
            XDocument mapBitmapDoc = XDocument.Parse(content);

            string? base64String = mapBitmapDoc.Descendants().Select(x => x.Element(ns + "LayerCanvasBitmap").Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(base64String))
            {
                // Convert Base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Create an image from the byte array
                using (MemoryStream ms = new(imageBytes))
                {
                    LayerCanvasBitmap = SKBitmap.Decode(ms);

                    Width = LayerCanvasBitmap.Width;
                    Height = LayerCanvasBitmap.Height;
                }
            }

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public void WriteXml(XmlWriter writer)
        {
            if (LayerCanvasBitmap != null)
            {
                using MemoryStream ms = new();
                using SKManagedWStream wstream = new(ms);
                LayerCanvasBitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
                byte[] bitmapData = ms.ToArray();
                writer.WriteStartElement("LayerCanvasBitmap");
                writer.WriteBase64(bitmapData, 0, bitmapData.Length);
                writer.WriteEndElement();
            }
        }

        public override void Render(SKCanvas canvas)
        {
            if (LayerCanvasBitmap != null && Show && RenderCanvas)
            {
                canvas.DrawBitmap(LayerCanvasBitmap, X, Y);
                RenderCanvas = false;
            }
        }
    }
}
