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

            map.MapAreaWidth = (float)MapAreaWidthUpDown.Value;
            map.MapAreaHeight = float.Parse(MapAreaHeightLabel.Text);

            if (MapUnitsUpDown.SelectedItem != null)
            {
                map.MapAreaUnits = (string)MapUnitsUpDown.SelectedItem;
            }

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

        private void WorldMapButton_CheckedChanged(object sender, EventArgs e)
        {

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
    }
}
