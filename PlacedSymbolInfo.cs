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
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    public partial class PlacedSymbolInfo : Form
    {
        private readonly MapSymbol selectedSymbol;
        private readonly MainForm mainform;

        public PlacedSymbolInfo(MapSymbol selectedSymbol, MainForm mainForm)
        {
            InitializeComponent();
            this.selectedSymbol = selectedSymbol;
            mainform = mainForm;

            SymbolNameLabel.Text = this.selectedSymbol.GetSymbolName();
            CollectionNameLabel.Text = this.selectedSymbol.GetCollectionName();
            SymbolPathLabel.Text = this.selectedSymbol.GetSymbolFilePath();
            SymbolGuidLabel.Text = this.selectedSymbol.GetSymbolGuid().ToString();
            SymbolFormatLabel.Text = this.selectedSymbol.GetSymbolFormat().ToString();

            switch (this.selectedSymbol.GetSymbolType())
            {
                case SymbolTypeEnum.Terrain:
                    TerrainRadioButton.Checked = true;
                    break;
                case SymbolTypeEnum.Structure:
                    StructureRadioButton.Checked = true;
                    break;
                case SymbolTypeEnum.Vegetation:
                    VegetationRadioButton.Checked = true;
                    break;
            }

            if (this.selectedSymbol.GetIsGrayScale())
            {
                GrayScaleSymbolRadio.Checked = true;
            }

            if (this.selectedSymbol.GetUseCustomColors())
            {
                UseCustomColorsRadio.Checked = true;
            }

            foreach (string tag in SymbolMethods.GetSymbolTags())
            {
                // add the tags in the symbol to the tag list box
                CheckedTagsListBox.Items.Add(tag);
            }

            AddTagsToListBox(this.selectedSymbol.GetSymbolTags());

            foreach (string tag in this.selectedSymbol.GetSymbolTags())
            {
                // check the symbol tags
                CheckTagInTagList(tag, CheckState.Checked);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddTagsToListBox(List<string> tags)
        {
            foreach (string tag in tags)
            {
                // add the tags in the symbol to the tag list box
                AddTagToTagList(tag);
            }
        }

        private void AddTagToTagList(string tag)
        {
            tag = tag.Trim([' ', ',']).ToLower();

            if (!CheckedTagsListBox.Items.Contains(tag))
            {
                CheckedTagsListBox.Items.Add(tag);
            }
        }

        private void CheckTagInTagList(string tag, CheckState checkState)
        {
            int tagIndex = CheckedTagsListBox.Items.IndexOf(tag);
            if (tagIndex > -1)
            {
                CheckedTagsListBox.SetItemCheckState(tagIndex, checkState);
            }
        }

        private void ColorSymbolsButton_Click(object sender, EventArgs e)
        {
            if (selectedSymbol.GetUseCustomColors())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                selectedSymbol.SetColorMappedBitmap(selectedSymbol.GetSymbolBitmap());
#pragma warning restore CS8604 // Possible null reference argument.

                Bitmap colorMappedBitmap = Extensions.ToBitmap(selectedSymbol.GetColorMappedBitmap());

                MapDrawingMethods.MapCustomColorsToColorableBitmap(ref colorMappedBitmap);

                selectedSymbol.SetColorMappedBitmap(Extensions.ToSKBitmap(colorMappedBitmap));
            }

            SKColor? paintColor = Extensions.ToSKColor(SymbolColor1Label.BackColor);
            if (selectedSymbol.GetIsGrayScale() && paintColor != null)
            {
                // if a symbol has been selected and is grayscale, then color it with the
                // selected custom color
                selectedSymbol.SetSymbolCustomColorAtIndex((SKColor)paintColor, 0);
                SKPaint paint = new()
                {
                    ColorFilter = SKColorFilter.CreateBlendMode((SKColor)paintColor,
                        SKBlendMode.Modulate) // combine the selected color with the bitmap colors
                };

                selectedSymbol.SetSymbolPaint(paint);
            }

            mainform.MapImageBox.Refresh();
            mainform.RenderDrawingPanel();
            mainform.Refresh();
        }

        private void SymbolColor1Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor1Label.BackColor);
            SymbolColor1Label.BackColor = c;
        }

        private void SymbolColor2Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor2Label.BackColor);
            SymbolColor2Label.BackColor = c;
        }

        private void SymbolColor3Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor3Label.BackColor);
            SymbolColor3Label.BackColor = c;
        }

        private void SymbolColor4Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor4Label.BackColor);
            SymbolColor4Label.BackColor = c;
        }

        private void ResetSymbolColorsButton_Click(object sender, EventArgs e)
        {
            SymbolColor1Label.BackColor = Color.FromArgb(255, 85, 44, 36);
            SymbolColor2Label.BackColor = Color.FromArgb(255, 53, 45, 32);
            SymbolColor3Label.BackColor = Color.FromArgb(161, 214, 202, 171);
            SymbolColor4Label.BackColor = Color.White;
        }
    }
}
