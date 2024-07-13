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

                    Width = (uint)LayerCanvasBitmap.Width;
                    Height = (uint)LayerCanvasBitmap.Height;
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
