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
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("maptheme", Namespace = "MapCreator", IsNullable = false)]
    public class MapTheme
    {
        public string? ThemeName { get; set; }
        public string? ThemePath { get; set; }
        public bool IsDefaultTheme { get; set; } = false;
        public bool IsSystemTheme { get; set; } = false;
        public MapTexture? BackgroundTexture { get; set; }
        public MapTexture? OceanTexture { get; set; }
        public int? OceanTextureOpacity { get; set; }
        public XmlColor? OceanColor { get; set; } = Color.Empty;
        public int? OceanColorOpacity { get; set; }
        public List<XmlColor> OceanColorPalette { get; set; } = [];
        public XmlColor? LandformOutlineColor { get; set; } = Color.Empty;
        public int? LandformOutlineWidth { get; set; }
        public MapTexture? LandformTexture { get; set; }
        public string? LandShorelineStyle { get; set; }
        public XmlColor? LandformCoastlineColor { get; set; } = Color.Empty;
        public int? LandformCoastlineColorOpacity { get; set; }
        public string? LandformCoastlineStyle { get; set; }
        public int? LandformCoastlineEffectDistance { get; set; }
        public List<XmlColor> LandformColorPalette { get; set; } = [];
        public XmlColor? FreshwaterColor { get; set; } = Color.Empty;
        public int? FreshwaterColorOpacity { get; set; }
        public XmlColor? FreshwaterShorelineColor { get; set; } = Color.Empty;
        public int? FreshwaterShorelineColorOpacity { get; set; }
        public int? RiverWidth { get; set; }
        public bool? RiverSourceFadeIn { get; set; }
        public List<XmlColor> FreshwaterColorPalette { get; set; } = [];
        public XmlColor? PathColor { get; set; } = Color.Empty;
        public int? PathColorOpacity { get; set; }
        public int? PathWidth { get; set; }
        public string? PathStyle { get; set; }
        public XmlColor? VignetteColor { get; set; } = ColorTranslator.FromHtml("#C9977B");
        public int? VignetteStrength { get; set; }
        public XmlColor[] SymbolCustomColors { get; set; } = [Color.Empty, Color.Empty, Color.Empty, Color.Empty];
    }
}
