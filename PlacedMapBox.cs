using SkiaSharp;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Extensions = SkiaSharp.Views.Desktop.Extensions;

namespace MapCreator
{
    public class PlacedMapBox : MapComponent, IXmlSerializable
    {
        public Guid BoxGuid = Guid.NewGuid();

        private Bitmap? BoxBitmap { get; set; } = null;

        public Color BoxTint {  get; set; } = Color.White;

        public SKPaint? BoxPaint { get; set; } = null;

        public bool IsSelected { get; set; } = false;

        public float BoxCenterLeft { get; set; } = 0;
        public float BoxCenterTop { get; set; } = 0;
        public float BoxCenterRight { get; set; } = 0;
        public float BoxCenterBottom { get; set; } = 0;

        public void SetBoxBitmap(Bitmap b)
        {
            BoxBitmap = b;
        }

        public override void Render(SKCanvas canvas)
        {
            if (BoxBitmap != null)
            {
                using SKPaint boxPaint = new()
                {
                    Style = SKPaintStyle.Fill,
                    ColorFilter = SKColorFilter.CreateBlendMode(
                        Extensions.ToSKColor(BoxTint),
                        SKBlendMode.Modulate) // combine the tint with the bitmap color
                };

                canvas.DrawBitmap(Extensions.ToSKBitmap(BoxBitmap), X, Y, boxPaint);

                if (IsSelected)
                {
                    // draw box around label to show it is selected
                    SKRect selectRect = new(X, Y, X + Width, Y + Height);
                    canvas.DrawRect(selectRect, MapLabelMethods.LABEL_SELECT_PAINT);
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
            XDocument mapBoxDoc = XDocument.Parse(content);

            XAttribute? xAttr = mapBoxDoc.Root.Attribute("X");
            if (xAttr != null)
            {
                X = int.Parse(xAttr.Value);
            }

            XAttribute? yAttr = mapBoxDoc.Root.Attribute("Y");
            if (yAttr != null)
            {
                Y = int.Parse(yAttr.Value);
            }

            XAttribute? wAttr = mapBoxDoc.Root.Attribute("Width");
            if (wAttr != null)
            {
                Width = int.Parse(wAttr.Value);
            }

            XAttribute? hAttr = mapBoxDoc.Root.Attribute("Height");
            if (hAttr != null)
            {
                Height = int.Parse(hAttr.Value);
            }

            IEnumerable<XElement?> guidElemEnum = mapBoxDoc.Descendants().Select(x => x.Element(ns + "BoxGuid"));
            if (guidElemEnum.First() != null)
            {
                string? boxGuid = mapBoxDoc.Descendants().Select(x => x.Element(ns + "BoxGuid").Value).FirstOrDefault();
                BoxGuid = Guid.Parse(boxGuid);
            }

            IEnumerable<XElement?> boxBitmapEnum = mapBoxDoc.Descendants().Select(x => x.Element(ns + "BoxBitmap"));
            if (boxBitmapEnum.First() != null)
            {
                string? boxBitmapBase64String = mapBoxDoc.Descendants().Select(x => x.Element(ns + "BoxBitmap").Value).FirstOrDefault();

                byte[] imageBytes = Convert.FromBase64String(boxBitmapBase64String);

                // Create an image from the byte array
                using (MemoryStream ms = new(imageBytes))
                {
                    BoxBitmap = Extensions.ToBitmap(SKBitmap.Decode(ms));
                }
            }

            IEnumerable<XElement> boxTintElem = mapBoxDoc.Descendants(ns + "BoxTint");
            if (boxTintElem.First() != null)
            {
                string? boxTint = mapBoxDoc.Descendants().Select(x => x.Element(ns + "BoxTint").Value).FirstOrDefault();
                BoxTint = ColorTranslator.FromHtml(boxTint);
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

            writer.WriteStartElement("BoxGuid");
            writer.WriteString(BoxGuid.ToString());
            writer.WriteEndElement();

            using MemoryStream ms = new();
            using SKManagedWStream wstream = new(ms);
            Extensions.ToSKBitmap(BoxBitmap).Encode(wstream, SKEncodedImageFormat.Png, 100);
            byte[] bitmapData = ms.ToArray();
            writer.WriteStartElement("BoxBitmap");
            writer.WriteBase64(bitmapData, 0, bitmapData.Length);
            writer.WriteEndElement();

            XmlColor boxTint = new(BoxTint);
            writer.WriteStartElement("BoxTint");
            boxTint.WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}
