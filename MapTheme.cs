using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("maptheme", Namespace = "MapCreator", IsNullable = false)]
    public class MapTheme
    {
        public string? ThemeName { get; set; }
        public string? ThemePath { get; set; }
        public bool IsDefaultTheme { get; set; } = false;
        public MapTexture? BackgroundTexture { get; set; }
        public int? BackGroundTextureOpacity { get; set; }
        public MapTexture? OceanTexture { get; set; }
        public int? OceanTextureOpacity { get; set; }
        public XmlColor? OceanColor { get; set; } = Color.Empty;
        public int? OceanColorOpacity { get; set; }
        public XmlColor? LandformOutlineColor { get; set; } = Color.Empty;
        public int? LandformOutlineWidth { get; set; }
        public MapTexture? LandformTexture { get; set; }
        public string? LandShorelineStyle { get; set; }
        public XmlColor? LandformCoastlineColor { get; set; } = Color.Empty;
        public int? LandformCoastlineColorOpacity { get; set; }
        public string? LandformCoastlineStyle { get; set; }
        public int? LandformCoastlineEffectDistance { get; set; }
        public XmlColor? FreshwaterColor { get; set; } = Color.Empty;
        public int? FreshwaterColorOpacity { get; set; }
        public XmlColor? FreshwaterShorelineColor { get; set; } = Color.Empty;
        public int? FreshwaterShorelineColorOpacity { get; set; }
        public int? FreshwaterSegmentSize { get; set; }
        public int? FreshwaterVariance { get; set; }
        public int? RiverWidth { get; set; }
        public bool? RiverSourceFadeIn { get; set; }
        public XmlColor? PathColor { get; set; } = Color.Empty;
        public int? PathColorOpacity { get; set; }
        public int? PathWidth { get; set; }
        public string? PathStyle { get; set; }
        public int? VignetteStrength { get; set; }
        public string? MapFrameStyle { get; set; }
        public XmlColor[] SymbolCustomColors { get; set; } = [Color.Empty, Color.Empty, Color.Empty, Color.Empty];
    }
}
