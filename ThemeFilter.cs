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
namespace MapCreator
{
    public class ThemeFilter
    {
        public bool ApplyBackgroundSettings { get; set; } = true;
        public bool ApplyOceanSettings { get; set; } = true;
        public bool ApplyOceanColorPaletteSettings { get; set; } = true;
        public bool ApplyLandSettings { get; set; } = true;
        public bool ApplyLandformColorPaletteSettings { get; set; } = true;
        public bool ApplyFreshwaterSettings { get; set; } = true;
        public bool ApplyFreshwaterColorPaletteSettings { get; set; } = true;
        public bool ApplyPathSetSettings { get; set; } = true;
        public bool ApplySymbolSettings {  get; set; } = true;
        public bool ApplyLabelSettings { get; set; } = true;
        public bool ApplyLabelPresetSettings { get; set; } = true;
    }
}
