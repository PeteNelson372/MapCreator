using System.Xml;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("LabelPreset", Namespace = "MapCreator", IsNullable = false)]
    public class LabelPreset
    {
        [XmlIgnore]
        public string PresetXmlFilePath { get; set; } = string.Empty;
        [XmlAttribute]
        public bool IsDefault { get; set; } = false;
        [XmlElement]
        public string LabelPresetName { get; set; } = string.Empty;
        [XmlElement]
        public XmlColor LabelColor { get; set; } = Color.Empty;
        [XmlElement]
        public byte LabelColorOpacity { get; set; } = 255;
        [XmlElement]
        public XmlColor LabelOutlineColor { get; set; } = Color.Empty;
        [XmlElement]
        public byte LabelOutlineColorOpacity { get; set; } = 255;
        [XmlElement]
        public int LabelOutlineWidth { get; set; } = 0;
        [XmlElement]
        public XmlColor LabelGlowColor { get; set; } = Color.Empty;
        [XmlElement]
        public byte LabelGlowColorOpacity { get; set; } = 255;
        [XmlElement]
        public int LabelGlowStrength { get; set; } = 0;
        [XmlElement]
        public string LabelFontString = string.Empty;
    }
}
