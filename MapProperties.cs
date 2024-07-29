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
    public partial class MapProperties : Form
    {
        private readonly MapCreatorMap map;
        private float MapAspectRatio = 1.0F;

        public MapProperties(MapCreatorMap mapCreatorMap)
        {
            InitializeComponent();
            map = mapCreatorMap;
            MapFilePath.Text = map.MapPath;
            mapName.Text = map.MapName;
            MapHeight.Value = (map.MapHeight < MapHeight.Minimum) ? MapHeight.Minimum : map.MapHeight;
            MapWidth.Value = (map.MapWidth < MapWidth.Minimum) ? MapWidth.Minimum : map.MapWidth;

            MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
            MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            map.MapPath = MapFilePath.Text;

            if (map.MapName != mapName.Text)
            {
                map.MapName = mapName.Text;
                map.IsSaved = false;
            }

            // MapHeight and MapWidth are the size of the map in pixels (e.g. 1200 x 800)
            if (map.MapHeight != (ushort)MapHeight.Value)
            {
                map.MapHeight = (ushort)MapHeight.Value;
                map.IsSaved = false;
            }

            if (map.MapWidth != (ushort)MapWidth.Value)
            {
                map.MapWidth = (ushort)MapWidth.Value;
                map.IsSaved = false;
            }

            // MapAreaWidth and MapAreaHeight are the size of the map in MapAreaUnits (e.g. 1000 miles x 500 miles)
            map.MapAreaWidth = (float)MapAreaWidthUpDown.Value;
            map.MapAreaHeight = float.Parse(MapAreaHeightLabel.Text);

            if (MapUnitsCombo.SelectedItem != null)
            {
                map.MapAreaUnits = (string)MapUnitsCombo.SelectedItem;
            }
            else
            {
                map.MapAreaUnits = MapUnitsCombo.Text;
            }

            // MapPixelWidth and MapPixelHeight are the size of one pixel in MapAreaUnits
            map.MapPixelWidth = map.MapAreaWidth / map.MapWidth;
            map.MapPixelHeight = map.MapAreaHeight / map.MapHeight;

            Close();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void iconButton1_Click(object sender, EventArgs e)
        {
            decimal temp = MapWidth.Value;
            MapWidth.Value = MapHeight.Value;
            MapHeight.Value = temp;
        }

        private void MapWidth_ValueChanged(object sender, EventArgs e)
        {
            MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
            MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
        }

        private void MapHeight_ValueChanged(object sender, EventArgs e)
        {
            MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
            MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
        }

        private void WH1024x768Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH1024x768Radio.Checked)
            {
                MapWidth.Value = 1024;
                MapHeight.Value = 768;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH1280x1024Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH1280x1024Radio.Checked)
            {
                MapWidth.Value = 1280;
                MapHeight.Value = 1024;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH1600x1200Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH1600x1200Radio.Checked)
            {
                MapWidth.Value = 1600;
                MapHeight.Value = 1200;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH1920x1080Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH1920x1080Radio.Checked)
            {
                MapWidth.Value = 1920;
                MapHeight.Value = 1080;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH2560x1080Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH2560x1080Radio.Checked)
            {
                MapWidth.Value = 2560;
                MapHeight.Value = 1080;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH3840x2160Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH3840x2160Radio.Checked)
            {
                MapWidth.Value = 3840;
                MapHeight.Value = 2160;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH4096x2048Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH4096x2048Radio.Checked)
            {
                MapWidth.Value = 4096;
                MapHeight.Value = 2048;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH4691x7016Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH4691x7016Radio.Checked)
            {
                MapWidth.Value = 4691;
                MapHeight.Value = 7016;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH5102x6591Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH5102x6591Radio.Checked)
            {
                MapWidth.Value = 5102;
                MapHeight.Value = 6591;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH5102x8409Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH5102x8409Radio.Checked)
            {
                MapWidth.Value = 5102;
                MapHeight.Value = 8409;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void WH7016x9921Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (WH7016x9921Radio.Checked)
            {
                MapWidth.Value = 7106;
                MapHeight.Value = 9921;
                MapAspectRatio = (float)(MapWidth.Value / MapHeight.Value);
                MapAspectRatioLabel.Text = MapAspectRatio.ToString("F2");
            }
        }

        private void MapAreaWidthUpDown_ValueChanged(object sender, EventArgs e)
        {
            MapAreaHeightLabel.Text = ((float)MapAreaWidthUpDown.Value / MapAspectRatio).ToString("F2");
        }

        private void WorldMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RegionMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CityMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TownMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void InteriorMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DungeonMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StarMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ShipMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void OtherMapButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
