using SkiaSharp;

namespace MapCreator
{
    public partial class MapScaleCreator : Form
    {
        MapCreatorMap map;
        SKCanvas canvas;
        Font selectedFont = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

        public MapScaleCreator(MapCreatorMap mapCreatorMap, SKCanvas mapcanvas)
        {
            InitializeComponent();
            map = mapCreatorMap;
            canvas = mapcanvas;

            ScaleNumbersDisplayCheckList.SetItemChecked(3, true);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ResetScaleColorsButton_Click(object sender, EventArgs e)
        {
            ScaleColor1Label.BackColor = Color.Black;
            ScaleColor2Label.BackColor = Color.White;
            ScaleColor3Label.BackColor = Color.Black;
        }

        private void ScaleColor1Label_Click(object sender, EventArgs e)
        {
            TopMost = true;
            Color color = MapPaintMethods.SelectColorFromDialog();
            TopMost = false;

            if (color.ToArgb() != Color.Empty.ToArgb())
            {
                ScaleColor1Label.BackColor = color;
            }
        }

        private void ScaleColor2Label_Click(object sender, EventArgs e)
        {
            TopMost = true;
            Color color = MapPaintMethods.SelectColorFromDialog();
            TopMost = false;

            if (color.ToArgb() != Color.Empty.ToArgb())
            {
                ScaleColor2Label.BackColor = color;
            }
        }

        private void ScaleColor3Label_Click(object sender, EventArgs e)
        {
            TopMost = true;
            Color color = MapPaintMethods.SelectColorFromDialog();
            TopMost = false;

            if (color.ToArgb() != Color.Empty.ToArgb())
            {
                ScaleColor3Label.BackColor = color;
            }
        }

        private void SelectScaleFontButton_Click(object sender, EventArgs e)
        {
            FontDialog fd = new()
            {
                FontMustExist = true
            };

            DialogResult result = fd.ShowDialog();

            if (result == DialogResult.OK)
            {
                SelectScaleFontButton.Font = new Font(fd.Font.FontFamily, 14);

            }
        }

        private void ScaleFontColorSelectLabel_Click(object sender, EventArgs e)
        {
            TopMost = true;
            Color color = MapPaintMethods.SelectColorFromDialog();
            TopMost = false;

            if (color.ToArgb() != Color.Empty.ToArgb())
            {
                ScaleFontColorSelectLabel.BackColor = Color.FromArgb(ScaleFontColorOpacityTrack.Value, color);
            }
        }

        private void ScaleFontColorOpacityTrack_Scroll(object sender, EventArgs e)
        {
            ScaleFontColorOpacityLabel.Text = ScaleFontColorOpacityTrack.Value.ToString();
            ScaleFontColorSelectLabel.BackColor = Color.FromArgb(ScaleFontColorOpacityTrack.Value,
                ScaleFontColorSelectLabel.BackColor.R, ScaleFontColorSelectLabel.BackColor.G, ScaleFontColorSelectLabel.BackColor.B);

        }

        private void ScaleOutlineColorSelectLabel_Click(object sender, EventArgs e)
        {
            TopMost = true;
            Color color = MapPaintMethods.SelectColorFromDialog();
            TopMost = false;

            if (color.ToArgb() != Color.Empty.ToArgb())
            {
                ScaleOutlineColorSelectLabel.BackColor = Color.FromArgb(ScaleOutlineColorOpacityTrack.Value, color);
            }
        }

        private void ScaleOutlineColorOpacityTrack_Scroll(object sender, EventArgs e)
        {
            ScaleOutlineColorOpacityLabel.Text = ScaleOutlineColorOpacityTrack.Value.ToString();
            ScaleOutlineColorSelectLabel.BackColor = Color.FromArgb(ScaleOutlineColorOpacityTrack.Value,
                ScaleOutlineColorSelectLabel.BackColor.R, ScaleOutlineColorSelectLabel.BackColor.G, ScaleOutlineColorSelectLabel.BackColor.B);
        }

        private void CreateScaleButton_Click(object sender, EventArgs e)
        {
            MapScale mapScale = new()
            {
                Width = (int)ScaleWidthUpDown.Value,
                Height = (int)ScaleHeightUpDown.Value,
                ScaleSegmentCount = (int)ScaleSegmentCountUpDown.Value,
                ScaleLineWidth = (int)ScaleLineWidthUpDown.Value,
                ScaleColor1 = ScaleColor1Label.BackColor,
                ScaleColor2 = ScaleColor2Label.BackColor,
                ScaleColor3 = ScaleColor3Label.BackColor,
                ScaleDistance = (float)ScaleSegmentDistanceUpDown.Value,
                ScaleDistanceUnit = ScaleUnitsTextBox.Text,
                ScaleFontColor = ScaleFontColorSelectLabel.BackColor,
                ScaleFontColorOpacity = ScaleFontColorOpacityTrack.Value,
                ScaleOutlineWidth = (int)ScaleOutlineWidthUpDown.Value,
                ScaleOutlineColor = ScaleOutlineColorSelectLabel.BackColor,
                ScaleOutlineColorOpacity = ScaleOutlineColorOpacityTrack.Value,
                ScaleFont = selectedFont,
            };

            for (int i = 0; i < ScaleNumbersDisplayCheckList.CheckedItems.Count; i++)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                switch (ScaleNumbersDisplayCheckList.CheckedItems[i].ToString())
                {
                    case "None":
                        mapScale.ScaleNumbersDisplayType = ScaleNumbersDisplayEnum.None;
                        break;
                    case "Ends":
                        mapScale.ScaleNumbersDisplayType = ScaleNumbersDisplayEnum.Ends;
                        break;
                    case "Every Other":
                        mapScale.ScaleNumbersDisplayType = ScaleNumbersDisplayEnum.EveryOther;
                        break;
                    case "All":
                        mapScale.ScaleNumbersDisplayType = ScaleNumbersDisplayEnum.All;
                        break;
                    default:
                        mapScale.ScaleNumbersDisplayType = ScaleNumbersDisplayEnum.None;
                        break;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            
            mapScale.X = (map.MapWidth / 2) - (mapScale.Width / 2);
            mapScale.Y = (map.MapHeight / 2) - (mapScale.Height / 2);

            // make sure there is only one scale - remove any existing scale
            for (int i = MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is MapScale)
                {
                    MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.RemoveAt(i);
                }
            }

            MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Add(mapScale);

            if (Owner != null)
            {
                ((MainForm)Owner).RenderDrawingPanel();
            }
        }

        private void DeleteScaleButton_Click(object sender, EventArgs e)
        {
            // remove any existing scale
            for (int i = MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is MapScale)
                {
                    MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.RemoveAt(i);
                }
            }

            if (Owner != null)
            {
                ((MainForm)Owner).RenderDrawingPanel();
            }
        }
    }
}
