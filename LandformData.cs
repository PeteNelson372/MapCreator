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
    public partial class LandformData : Form
    {
        private MapLandformType2? landform;

        public LandformData()
        {
            InitializeComponent();
        }

        public void SetMapLandform(MapLandformType2 mapLandform)
        {
            landform = mapLandform;

            LandformGuidLabel.Text = landform.LandformGuid.ToString();
            LandformNameText.Text = landform.LandformName;
            LandformTextureText.Text = landform.LandformTexture?.TextureName;
#pragma warning disable CS8629 // Nullable value type may be null.
            LandformOutlineColorText.Text = ColorTranslator.ToHtml((Color)(landform?.LandformOutlineColor));
#pragma warning restore CS8629 // Nullable value type may be null.
            LandformOutlineWidthText.Text = landform.LandformOutlineWidth.ToString();
            LandformShorelineStyleText.Text = landform.ShorelineStyle.ToString();
#pragma warning disable CS8629 // Nullable value type may be null.
            LandformCoastlineColorText.Text = ColorTranslator.ToHtml((Color)(landform?.CoastlineColor));
#pragma warning restore CS8629 // Nullable value type may be null.
            CoastlineColorOpacityText.Text = landform?.CoastlineColorOpacity.ToString();
            CoastlineEffectDistanceText.Text = landform?.CoastlineEffectDistance.ToString();
            CoastlineStyleNameText.Text = landform?.CoastlineStyleName;
            CoastlineHatchPatternText.Text = landform?.CoastlineHatchPattern;
            CoastlineHatchOpacityText.Text = landform?.CoastlineHatchOpacity.ToString();
            CoastlineHatchScaleText.Text = landform?.CoastlineHatchScale.ToString();
            CoastlineHatchBlendModeText.Text = landform?.CoastlineHatchBlendMode;

#pragma warning disable CS8629 // Nullable value type may be null.
            PaintCoastlineGradientCheck.Checked = (bool)(landform?.PaintCoastlineGradient);
#pragma warning restore CS8629 // Nullable value type may be null.

            //ColorPathCountLabel.Text = landform?.LandformColorPaths.Count.ToString();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
