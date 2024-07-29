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

namespace MapCreator
{
    internal class Cmd_ChangeLabelAttributes : IMapOperation
    {
        private MapLabel Label;
        private Color LabelColor;
        private Color OutlineColor;
        private int OutlineWidth;
        private Color GlowColor;
        private int GlowStrength;
        private Font SelectedFont;

        private Color StoredLabelColor;
        private Color StoredOutlineColor;
        private int StoredOutlineWidth;
        private Color StoredGlowColor;
        private int StoredGlowStrength;
        private Font StoredSelectedFont;

        public Cmd_ChangeLabelAttributes(MapLabel label, Color labelColor, Color outlineColor, int outlineWidth, Color glowColor, int glowStrength, Font selectedFont)
        {
            Label = label;
            LabelColor = labelColor;
            OutlineColor = outlineColor;
            OutlineWidth = outlineWidth;
            GlowColor = glowColor;
            GlowStrength = glowStrength;
            SelectedFont = selectedFont;

            StoredLabelColor = Label.LabelColor;
            StoredOutlineColor = Label.LabelOutlineColor;
            StoredOutlineWidth = Label.LabelOutlineWidth;
            StoredGlowColor = Label.LabelGlowColor;
            StoredGlowStrength = Label.LabelGlowStrength;
            StoredSelectedFont = Label.LabelFont;
        }

        public void DoOperation()
        {
            Label.LabelColor = LabelColor;
            Label.LabelOutlineColor = OutlineColor;
            Label.LabelOutlineWidth = OutlineWidth;
            Label.LabelGlowColor = GlowColor;
            Label.LabelGlowStrength = GlowStrength;
            Label.LabelFont = SelectedFont;

            SKPaint paint = MapLabelMethods.CreateLabelPaint(Label.LabelFont, LabelColor);

            Label.LabelPaint = paint;
            Label.Width = (int)paint.MeasureText(Label.LabelText);
        }

        public void UndoOperation()
        {
            Label.LabelColor = StoredLabelColor;
            Label.LabelOutlineColor = StoredOutlineColor;
            Label.LabelOutlineWidth = StoredOutlineWidth;
            Label.LabelGlowColor = StoredGlowColor;
            Label.LabelGlowStrength = StoredGlowStrength;
            Label.LabelFont = StoredSelectedFont;

            SKPaint paint = MapLabelMethods.CreateLabelPaint(Label.LabelFont, LabelColor);

            Label.LabelPaint = paint;
            Label.Width = (int)paint.MeasureText(Label.LabelText);
        }
    }
}
