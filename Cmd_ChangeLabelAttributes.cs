
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
