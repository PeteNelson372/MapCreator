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
    internal class MapRegionMethods
    {
        public static List<MapRegion> MAP_REGION_LIST { get; set; }  = [];
        public static MapRegion? NEW_REGION { get; set; }
        public static MapPathPoint? SELECTED_REGION_POINT { get; set; } = null;

        public static int POINT_CIRCLE_RADIUS = 5;

        private static readonly SKPaint REGION_SELECT_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.BlueViolet,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F),
        };

        public static readonly SKPaint REGION_POINT_FILL_PAINT = new()
        {
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            Color = SKColors.White,
            StrokeWidth = 1,
        };

        public static readonly SKPaint REGION_POINT_SELECTED_FILL_PAINT = new()
        {
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            Color = SKColors.Blue,
            StrokeWidth = 1,
        };

        public static readonly SKPaint REGION_NEW_POINT_FILL_PAINT = new()
        {
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
            Color = SKColors.Yellow,
            StrokeWidth = 1,
        };

        public static readonly SKPaint REGION_POINT_OUTLINE_PAINT = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.Black,
            StrokeWidth = 1,
        };

        public static SKPath GetLinePathFromRegionPoints(List<MapRegionPoint> points)
        {
            SKPath path = new();

            path.MoveTo(points.First().RegionPoint);

            for (int i = 1; i < points.Count; i++)
            {
                path.LineTo(points[i].RegionPoint);
            }

            path.LineTo(points.First().RegionPoint);

            path.Close();

            return path;
        }

        public static SKPath GetLinePathFromSKPoints(List<SKPoint> points)
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

        public static void DrawRegion(MapCreatorMap? map, MapRegion region, SKCanvas c)
        {
            if (map == null) return;

            SKPath path = new();

            path.MoveTo(region.MapRegionPoints.First().RegionPoint);

            for (int i = 1; i < region.MapRegionPoints.Count; i++)
            {
                path.LineTo(region.MapRegionPoints[i].RegionPoint);
            }

            path.LineTo(region.MapRegionPoints.First().RegionPoint);

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

                        List<MapRegionPoint> parallelPoints = MapDrawingMethods.GetParallelRegionPoints(region.MapRegionPoints, region.RegionBorderPaint.StrokeWidth * 0.4F, ParallelEnum.Below);
                        using SKPath p1 = GetLinePathFromRegionPoints(parallelPoints);
                        c.DrawPath(p1, gradientPaint);

                        clr = Color.FromArgb(102, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelRegionPoints(region.MapRegionPoints, region.RegionBorderPaint.StrokeWidth * 0.6F, ParallelEnum.Below);
                        using SKPath p2 = GetLinePathFromRegionPoints(parallelPoints);
                        c.DrawPath(p2, gradientPaint);

                        clr = Color.FromArgb(51, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelRegionPoints(region.MapRegionPoints, region.RegionBorderPaint.StrokeWidth * 0.8F, ParallelEnum.Below);
                        using SKPath p3 = GetLinePathFromRegionPoints(parallelPoints);
                        c.DrawPath(p3, gradientPaint);

                        clr = Color.FromArgb(25, region.RegionBorderPaint.Color.ToDrawingColor());
                        gradientPaint.Color = Extensions.ToSKColor(clr);

                        parallelPoints = MapDrawingMethods.GetParallelRegionPoints(region.MapRegionPoints, region.RegionBorderPaint.StrokeWidth, ParallelEnum.Below);
                        using SKPath p4 = GetLinePathFromRegionPoints(parallelPoints);
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

                        List<MapRegionPoint> parallelPoints = MapDrawingMethods.GetParallelRegionPoints(region.MapRegionPoints, region.RegionBorderPaint.StrokeWidth * 0.2F, ParallelEnum.Below);
                        using SKPath p1 = GetLinePathFromRegionPoints(parallelPoints);
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
                        using SKPath p1 = GetLinePathFromSKPoints(parallelPoints);
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

                    MapBuilder.GetLayerCanvas(map, MapBuilder.REGIONLAYER).DrawPath(boundsPath, REGION_SELECT_PAINT);

                    // draw dots on region vertices

                    foreach (MapRegionPoint p in region.MapRegionPoints)
                    {
                        p.Render(MapBuilder.GetLayerCanvas(map, MapBuilder.REGIONLAYER));
                    }
                }
            }
        }

        internal static MapRegion? SelectRegionAtPoint(Point mapClickPoint)
        {
            MapRegion? selectedRegion = null;
            List<MapRegion> regions = MAP_REGION_LIST;

            for (int i = 0; i < regions.Count; i++)
            {
                MapRegion region = regions[i];
                SKPath? boundaryPath = region.BoundaryPath;

                if (boundaryPath != null && boundaryPath.PointCount > 0)
                {
                    if (boundaryPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                    {
                        region.IsSelected = !region.IsSelected;

                        if (region.IsSelected)
                        {
                            selectedRegion = region;
                        }
                        break;
                    }
                }
            }

#pragma warning disable CS8604 // Possible null reference argument.
            MapPaintMethods.DeselectAllMapComponents(selectedRegion);
#pragma warning restore CS8604 // Possible null reference argument.

            return selectedRegion;
        }
    }
}
