using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class MapRegionMethods
    {
        public static List<MapRegion> MAP_REGION_LIST { get; set; }  = [];
        public static MapRegion? NEW_REGION { get; set; }
        public static MapPathPoint? SELECTED_REGION_POINT { get; set; } = null;

        private static readonly SKPaint REGION_SELECT_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.BlueViolet,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F),
        };

        public static SKPath GetLinePathFromPoints(List<SKPoint> points)
        {
            SKPath path = new();

            path.MoveTo(points.First());

            for (int i = 1; i < points.Count; i++)
            {
                path.LineTo(points[i]);
            }

            path.LineTo(points.First());

            path.Close();

            return path;
        }

        public static void DrawRegion(MapCreatorMap map, MapRegion region, SKCanvas c)
        {
            SKPath path = new();

            path.MoveTo(region.RegionPoints.First());

            for (int i = 1; i < region.RegionPoints.Count; i++)
            {
                path.LineTo(region.RegionPoints[i]);
            }

            path.LineTo(region.RegionPoints.First());

            path.Close();

            region.BoundaryPath = new(path);

            // handle drawing of borders with gradients and other forms
            // that are not handled by a PathEffect in the RegionBorderPaint

            switch (region.RegionBorderType)
            {
                case PathTypeEnum.BorderedGradientPath:
                    {
                        using SKPaint gradientPaint = new SKPaint()
                        {
                            StrokeWidth = region.RegionBorderWidth,
                            Color = region.RegionBorderColor.ToSKColor(),
                            Style = SKPaintStyle.Stroke,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeJoin = SKStrokeJoin.Round,
                            IsAntialias = true,
                            PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
                        };

                        c.DrawPath(path, region.RegionInnerPaint);

                        gradientPaint.StrokeWidth = region.RegionBorderPaint.StrokeWidth * 0.4F;
                        gradientPaint.Color = SKColors.Black;
                        c.DrawPath(path, gradientPaint);

                        Color clr = Color.FromArgb(154, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        List<SKPoint> parallelPoints = MapDrawingMethods.GetParallelSKPoints(region.RegionPoints, region.RegionBorderPaint.StrokeWidth * 0.4F, ParallelEnum.Below);
                        using SKPath p1 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p1, gradientPaint);

                        clr = Color.FromArgb(102, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelSKPoints(region.RegionPoints, region.RegionBorderPaint.StrokeWidth * 0.6F, ParallelEnum.Below);
                        using SKPath p2 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p2, gradientPaint);

                        clr = Color.FromArgb(51, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelSKPoints(region.RegionPoints, region.RegionBorderPaint.StrokeWidth * 0.8F, ParallelEnum.Below);
                        using SKPath p3 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p3, gradientPaint);

                        clr = Color.FromArgb(25, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelSKPoints(region.RegionPoints, region.RegionBorderPaint.StrokeWidth, ParallelEnum.Below);
                        using SKPath p4 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p4, gradientPaint);
                    }
                    break;
                case PathTypeEnum.BorderedLightSolidPath:
                    {
                        c.DrawPath(path, region.RegionInnerPaint);

                        using SKPaint borderPaint = new()
                        {
                            Color = SKColors.Black,
                            StrokeWidth = region.RegionBorderWidth * 0.2F,
                            Style = SKPaintStyle.Stroke,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeJoin = SKStrokeJoin.Round,
                            IsAntialias = true,
                            PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
                        };

                        c.DrawPath(path, borderPaint);

                        using SKPaint linePaint1 = new()
                        {
                            StrokeWidth = region.RegionBorderWidth * 0.8F,
                            Style = SKPaintStyle.Stroke,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeJoin = SKStrokeJoin.Round,
                            IsAntialias = true,
                            PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
                        };

                        Color clr = Color.FromArgb(102, region.RegionBorderPaint.Color.ToDrawingColor());
                        linePaint1.Color = Extensions.ToSKColor(clr);

                        List<SKPoint> parallelPoints = MapDrawingMethods.GetParallelSKPoints(region.RegionPoints, region.RegionBorderPaint.StrokeWidth * 0.2F, ParallelEnum.Below);
                        using SKPath p1 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p1, linePaint1);
                    }
                    break;
                case PathTypeEnum.DoubleSolidBorderPath:
                    {
                        c.DrawPath(path, region.RegionInnerPaint);

                        using SKPaint borderPaint = new()
                        {
                            Color = SKColors.Black,
                            StrokeWidth = region.RegionBorderWidth * 0.2F,
                            Style = SKPaintStyle.Stroke,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeJoin = SKStrokeJoin.Round,
                            IsAntialias = true,
                            PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
                        };

                        c.DrawPath(path, borderPaint);

                        List<SKPoint> points = [.. path.Points];
                        List<SKPoint> parallelPoints = MapDrawingMethods.GetParallelSKPoints(points, region.RegionBorderPaint.StrokeWidth, ParallelEnum.Below);
                        using SKPath p1 = GetLinePathFromPoints(parallelPoints);
                        c.DrawPath(p1, borderPaint);
                    }
                    break;

                default:
                    c.DrawPath(path, region.RegionInnerPaint);
                    c.DrawPath(path, region.RegionBorderPaint);
                    break;
            }


            if (region.IsSelected)
            {
                if (region.BoundaryPath != null)
                {
                    // draw an outline around the region to show that it is selected
                    region.BoundaryPath.GetTightBounds(out SKRect boundRect);
                    using SKPath boundsPath = new();
                    boundsPath.AddRect(boundRect);

                    MapBuilder.GetLayerCanvas(map, MapBuilder.WORKLAYER)?.DrawPath(boundsPath, REGION_SELECT_PAINT);

                    // draw dots on region vertices
                }
            }
        }
    }
}
