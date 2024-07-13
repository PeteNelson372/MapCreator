using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class MapLabelMethods
    {
        public static List<LabelPreset> LABEL_PRESETS = [];

        public static List<MapLabel> MAP_LABELS = [];

        public static List<PlacedMapBox> MAP_BOXES = [];

        public static List<MapBox> MAP_BOX_TEXTURES = [];

        public static bool CreatingLabel { get; set; } = false;

        public static Font SELECTED_FONT = new("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);

        public static List<SKPoint> LABEL_PATH_POINTS = [];
        public static SKPath LABEL_PATH = new();

        private static MapBox? SelectedMapBox = null;

        internal static LabelTextAlignEnum SelectedLabelAlignment { get; set; } = LabelTextAlignEnum.AlignLeft;

        public static SKPaint LABEL_SELECT_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.Coral,
            StrokeWidth = 1,
            PathEffect = SKPathEffect.CreateDash([4F, 2F], 6F),
            TextAlign = SKTextAlign.Center,
        };

        public static SKPaint LABEL_PATH_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.Gray,
            StrokeWidth = 1,
            PathEffect = SKPathEffect.CreateDash([2F, 2F], 4F)
        };

        internal static SKPaint CreateLabelPaint(Font labelFont, Color labelColor)
        {
            SKPaint paint = new()
            {
                Color = Extensions.ToSKColor(labelColor),
                TextSize = (labelFont.Size * 4.0F) / 3.0F
            };

            switch (SelectedLabelAlignment)
            {
                case LabelTextAlignEnum.AlignLeft:
                    paint.TextAlign = SKTextAlign.Left;
                    break;
                case LabelTextAlignEnum.AlignCenter:
                    paint.TextAlign = SKTextAlign.Center;
                    break;
                case LabelTextAlignEnum.AlignRight:
                    paint.TextAlign = SKTextAlign.Right;
                    break;
            }

            SKFontStyle fs = SKFontStyle.Normal;

            if (labelFont.Bold && labelFont.Italic)
            {
                fs = SKFontStyle.BoldItalic;
            }
            else if (labelFont.Bold)
            {
                fs = SKFontStyle.Bold;
            }
            else if (labelFont.Italic)
            {
                fs = SKFontStyle.Italic;
            }

            paint.Typeface = SKFontManager.Default.MatchFamily(labelFont.Name, fs);
            paint.IsAntialias = true;

            return paint;
        }

        internal static MapLabel? SelectLabelAtPoint(Point mapClickPoint)
        {
            foreach (MapLabel label in MAP_LABELS)
            {
                Rectangle labelRect = new((int)label.X, (int)label.Y - (int)label.Height, (int)label.Width, (int)label.Height * 2);
                if (labelRect.Contains(mapClickPoint))
                {
                    return label;
                }
            }

            return null;
        }

        internal static void ConstructPathFromPoints()
        {
            LABEL_PATH.Dispose();
            LABEL_PATH = new();

            if (LABEL_PATH_POINTS.Count > 2)
            {
                LABEL_PATH.MoveTo(LABEL_PATH_POINTS[0]);

                for (int j = 0; j < LABEL_PATH_POINTS.Count; j += 3)
                {
                    if (j < LABEL_PATH_POINTS.Count - 2)
                    {
                        LABEL_PATH.CubicTo(LABEL_PATH_POINTS[j], LABEL_PATH_POINTS[j + 1], LABEL_PATH_POINTS[j + 2]);
                    }
                }
            }
        }

        internal static void DrawLabelPath(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.WORKLAYER)?.DrawPath(LABEL_PATH, LABEL_PATH_PAINT);
        }

        internal static void SetSelectedMapBox(MapBox b)
        {
            SelectedMapBox = b;
        }
        
        internal static MapBox? GetSelectedMapBox()
        {
            return SelectedMapBox;
        }

        internal static void ClearSelectedMapBox()
        {
            SelectedMapBox = null;
        }

        internal static PlacedMapBox? SelectMapBoxAtPoint(Point mapClickPoint)
        {
            foreach (PlacedMapBox box in MAP_BOXES)
            {
                Rectangle labelRect = new((int)box.X, (int)box.Y, (int)box.Width, (int)box.Height);
                if (labelRect.Contains(mapClickPoint))
                {
                    return box;
                }
            }

            return null;
        }
    }
}
