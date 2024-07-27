using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class MapMeasure : MapComponent
    {
        private readonly MapCreatorMap Map;

        public MapMeasure(MapCreatorMap map)
        {
            Map = map;
            MeasureLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                Color = MeasureLineColor.ToSKColor()
            };

            MeasureAreaPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 1,
                Color = MeasureLineColor.ToSKColor()
            };

            MeasureValuePaint = new()
            {
                Color = SKColors.White,
                TextSize = (MeasureValueFont.Size * 4.0F) / 3.0F,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true
            };

            MeasureValueOutlinePaint = new()
            {
                Color = SKColors.Black,
                TextSize = (MeasureValueFont.Size * 4.0F) / 3.0F,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                ImageFilter = SKImageFilter.CreateDilate(1, 1)
            };
        }

        public Color MeasureLineColor { get; set; } = Color.FromArgb(191, 138, 26, 0);

        public Font MeasureValueFont { get; set; } = new Font("Tahoma", 8.0F, FontStyle.Regular, GraphicsUnit.Point, 0);

        public bool UseMapUnits { get; set; } = false;

        public bool MeasureArea { get; set; } = false;

        public List<SKPoint> MeasurePoints { get; set; } = [];

        public SKPaint MeasureLinePaint { get; set; }

        public SKPaint MeasureAreaPaint { get; set; }

        public SKPaint MeasureValuePaint { get; set; }

        public SKPaint MeasureValueOutlinePaint { get; set; }

        public float TotalMeasureLength { get; set; } = 0;

        public bool RenderValue = false;

        public override void Render(SKCanvas canvas)
        {
            if (MeasurePoints.Count >= 2)
            {
                if (MeasureArea && MeasurePoints.Count > 2)
                {
                    SKPath path = new();

                    path.MoveTo(MeasurePoints.First());

                    for (int i = 1; i < MeasurePoints.Count; i++)
                    {
                        path.LineTo(MeasurePoints[i]);
                    }

                    path.Close();

                    canvas.DrawPath(path, MeasureAreaPaint);
                }
                else
                {
                    for (int i = 0; i < MeasurePoints.Count - 1; i++)
                    {
                        canvas.DrawLine(MeasurePoints[i], MeasurePoints[i + 1], MeasureLinePaint);
                    }
                }

                if (RenderValue)
                {
                    // render measure value and units
                    SKPoint measureValuePoint = new(MeasurePoints.Last().X + 30, MeasurePoints.Last().Y + 20);
                    RenderDistanceLabel(canvas, measureValuePoint, TotalMeasureLength);

                    if (MeasureArea)
                    {
                        float area = MapDrawingMethods.CalculatePolygonArea(MeasurePoints);

                        SKPoint measureAreaPoint = new(MeasurePoints.Last().X + 30, MeasurePoints.Last().Y + 40);

                        RenderAreaLabel(canvas, measureAreaPoint, area);
                    }
                }
            }
        }

        public void RenderDistanceLabel(SKCanvas canvas, SKPoint labelPoint, float distance)
        {
            if (UseMapUnits && !string.IsNullOrEmpty(Map.MapAreaUnits))
            {
                canvas.DrawText(string.Format("{0} {1}", (int)(distance * Map.MapPixelWidth), Map.MapAreaUnits), labelPoint, MeasureValueOutlinePaint);
                canvas.DrawText(string.Format("{0} {1}", (int)(distance * Map.MapPixelWidth), Map.MapAreaUnits), labelPoint, MeasureValuePaint);
            }
            else
            {
                canvas.DrawText(string.Format("{0} {1}", (int)distance, "pixels"), labelPoint, MeasureValueOutlinePaint);
                canvas.DrawText(string.Format("{0} {1}", (int)distance, "pixels"), labelPoint, MeasureValuePaint);
            }
        }

        public void RenderAreaLabel(SKCanvas canvas, SKPoint labelPoint, float measuredArea)
        {
            if (UseMapUnits && !string.IsNullOrEmpty(Map.MapAreaUnits))
            {
                string labelString = string.Format("{0} {1}", (int)(measuredArea * (Map.MapPixelWidth * Map.MapPixelWidth)), Map.MapAreaUnits + "\xB2");

                canvas.DrawText(labelString, labelPoint, MeasureValueOutlinePaint);
                canvas.DrawText(labelString, labelPoint, MeasureValuePaint);
            }
            else
            {
                canvas.DrawText(string.Format("{0} {1}", (int)measuredArea, "pixels\xB2"), labelPoint, MeasureValueOutlinePaint);
                canvas.DrawText(string.Format("{0} {1}", (int)measuredArea, "pixels\xB2"), labelPoint, MeasureValuePaint);
            }
        }
    }
}
