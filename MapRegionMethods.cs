using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;

namespace MapCreator
{
    internal class MapRegionMethods
    {
        public static List<MapRegion> MAP_REGION_LIST { get; set; }  = [];
        public static MapRegion? NEW_REGION { get; set; }
        public static List<MapRegionPoint> REGION_POINT_LIST { get; set; } = [];
        public static MapPathPoint? SELECTED_REGION_POINT { get; set; } = null;

        private static readonly SKPaint REGION_SELECT_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.BlueViolet,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F),
        };

        public static SKPath GenerateRegionBoundaryPath(List<MapRegionPoint> points)
        {
            SKPath path = new();

            path.MoveTo(points[0].RegionPoint);

            for (int j = 0; j < points.Count; j += 3)
            {
                if (j < points.Count - 2)
                {
                    path.CubicTo(points[j].RegionPoint, points[j + 1].RegionPoint, points[j + 2].RegionPoint);
                }
            }

            path.Close();

            return path;
        }

        public static void DrawLinePathFromPoints(List<SKPoint> points, SKCanvas c, SKPaint paint)
        {
            using SKPath path = new();

            path.MoveTo(points.First());

            for (int i = 1; i < points.Count; i++)
            {
                path.LineTo(points[i]);
            }

            path.Close();

            c.DrawPath(path, paint);
        }

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
                            PathEffect = SKPathEffect.CreateCorner(20)  // TODO: add smoothing trackbar to region UI for corner size
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

                    MapBuilder.GetLayerCanvas(map, MapBuilder.REGIONLAYER)?.DrawPath(boundsPath, REGION_SELECT_PAINT);
                }
            }
        }

        private static void DrawBezierCurvesFromPoints(SKCanvas canvas, List<MapRegionPoint> curvePoints, SKPaint paint)
        {
            if (curvePoints.Count == 2)
            {
                canvas.DrawLine(curvePoints[0].RegionPoint, curvePoints[1].RegionPoint, paint);
            }
            else if (curvePoints.Count > 2)
            {
                using SKPath path = new();
                path.MoveTo(curvePoints[0].RegionPoint);

                for (int j = 0; j < curvePoints.Count; j += 3)
                {
                    if (j < curvePoints.Count - 2)
                    {
                        path.CubicTo(curvePoints[j].RegionPoint, curvePoints[j + 1].RegionPoint, curvePoints[j + 2].RegionPoint);
                    }
                }

                canvas.DrawPath(path, paint);
            }
        }

        internal static List<MapRegionPoint> GetParallelRegionPoints(List<MapRegionPoint> points, float distance, ParallelEnum location)
        {
            List<MapRegionPoint> parallelPoints = [];

            for (int i = 0; i < points.Count - 1; i += 2)
            {
                float lineAngle = MapDrawingMethods.CalculateLineAngle(points[i].RegionPoint, points[i + 1].RegionPoint);

                float angle = (location == ParallelEnum.Below) ? 90 : -90;

                SKPoint p1 = MapDrawingMethods.PointOnCircle(distance, lineAngle + angle, points[i].RegionPoint);
                SKPoint p2 = MapDrawingMethods.PointOnCircle(distance, lineAngle + angle, points[i + 1].RegionPoint);

                parallelPoints.Add(new MapRegionPoint(p1));
                parallelPoints.Add(new MapRegionPoint(p2));
            }

            return parallelPoints;
        }

        public static void DrawAllRegions(MapCreatorMap map)
        {
            foreach (MapRegion r in MAP_REGION_LIST)
            {
                //SKCanvas c = MapBuilder.GetLayerCanvas(map, MapBuilder.REGIONLAYER);
                //DrawRegion(map, r, c);
            }
        }
    }
}
