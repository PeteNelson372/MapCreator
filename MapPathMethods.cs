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
    internal class MapPathMethods
    {
        private static List<MapPath> MAP_PATH_LIST = [];
        private static MapPath NEW_PATH = new();
        private static List<MapPathPoint> PATH_POINT_LIST = [];

        private static MapPathPoint? SELECTED_MAP_PATH_POINT = null;

        private static readonly SKPaint MAPPATH_SELECT_PAINT = new();
        private static readonly SKPaint MAPPATH_SELECT_ERASE_PAINT = new();
        private static readonly SKPaint MAPPATH_CONTROL_POINT_PAINT = new();
        private static readonly SKPaint MAPPATH_CONTROL_POINT_OUTLINE_PAINT = new();
        private static readonly SKPaint MAPPATH_SELECTED_CONTROL_POINT_PAINT = new();

        private static SKShader? PATH_TEXTURE_SHADER;

        public static List<MapTexture> PATH_TEXTURE_LIST { get; set; } = [];

        public static List<MapVector> PATH_VECTOR_LIST { get; set; } = [];

        internal static ref List<MapPath> GetMapPathList()
        {
            return ref MAP_PATH_LIST;
        }

        internal static void ConstructMapPathPaintObjects()
        {
            MAPPATH_SELECT_PAINT.Style = SKPaintStyle.Stroke;
            MAPPATH_SELECT_PAINT.IsAntialias = true;
            MAPPATH_SELECT_PAINT.Color = SKColors.BlueViolet;
            MAPPATH_SELECT_PAINT.StrokeWidth = 2;
            MAPPATH_SELECT_PAINT.PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F);

            MAPPATH_SELECT_ERASE_PAINT.Style = SKPaintStyle.Stroke;
            MAPPATH_SELECT_ERASE_PAINT.IsAntialias = true;
            MAPPATH_SELECT_ERASE_PAINT.Color = SKColors.Empty;
            MAPPATH_SELECT_ERASE_PAINT.StrokeWidth = 2;

            MAPPATH_CONTROL_POINT_PAINT.Style = SKPaintStyle.Fill;
            MAPPATH_CONTROL_POINT_PAINT.IsAntialias = true;
            MAPPATH_CONTROL_POINT_PAINT.StrokeWidth = 2;
            MAPPATH_CONTROL_POINT_PAINT.Color = SKColors.WhiteSmoke;

            MAPPATH_SELECTED_CONTROL_POINT_PAINT.Style = SKPaintStyle.Fill;
            MAPPATH_SELECTED_CONTROL_POINT_PAINT.IsAntialias = true;
            MAPPATH_SELECTED_CONTROL_POINT_PAINT.StrokeWidth = 2;
            MAPPATH_SELECTED_CONTROL_POINT_PAINT.Color = SKColors.BlueViolet;

            MAPPATH_CONTROL_POINT_OUTLINE_PAINT.Style = SKPaintStyle.Stroke;
            MAPPATH_CONTROL_POINT_OUTLINE_PAINT.IsAntialias = true;
            MAPPATH_CONTROL_POINT_OUTLINE_PAINT.StrokeWidth = 1;
            MAPPATH_CONTROL_POINT_OUTLINE_PAINT.Color = SKColors.Black;
        }


        public static void AddMapPath(MapPath mapPath)
        {
            bool found = false;
            List<MapPath> mapPathList = GetMapPathList();
            for (int i = 0; i < mapPathList.Count; i++)
            {
                if (mapPathList[i].MapPathGuid.Equals(mapPath.MapPathGuid))
                {
                    mapPathList[i] = mapPath;
                    found = true;
                }
            }

            if (!found)
            {
                mapPathList.Add(mapPath);
            }
        }

        public static void ConstructPathPaint(MapPath mapPath)
        {
            if (mapPath.PathPaint != null) return;

            float strokeWidth = mapPath.PathWidth;

            switch (mapPath.PathType)
            {
                case PathTypeEnum.ThickSolidLinePath:
                    strokeWidth = mapPath.PathWidth * 1.5F;
                    break;
            }

            SKPaint pathPaint = new()
            {
                Color = Extensions.ToSKColor(mapPath.PathColor),
                StrokeWidth = strokeWidth,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
            };

            SKPathEffect pathLineEffect = ConstructPathLineEffect(mapPath.PathType, mapPath.PathWidth * 2);

            if (pathLineEffect != null)
            {
                pathPaint.PathEffect = pathLineEffect;
            }

            switch (mapPath.PathType)
            {
                case PathTypeEnum.LineAndDashesPath:
                case PathTypeEnum.BorderedGradientPath:
                case PathTypeEnum.BorderedLightSolidPath:
                    pathPaint.StrokeCap = SKStrokeCap.Butt;
                    break;
                case PathTypeEnum.TexturedPath:
                    // construct a shader from the selected path texture
                    if (PATH_TEXTURE_SHADER != null)
                    {
                        pathPaint.Shader = PATH_TEXTURE_SHADER;
                        pathPaint.Style = SKPaintStyle.Stroke;
                    }
                    break;
                case PathTypeEnum.BorderAndTexturePath:
                    pathPaint.StrokeCap = SKStrokeCap.Butt;

                    // construct a shader from the selected path texture
                    if (PATH_TEXTURE_SHADER != null)
                    {
                        pathPaint.Shader = PATH_TEXTURE_SHADER;
                        pathPaint.Style = SKPaintStyle.Stroke;
                    }
                    break;
            }

            mapPath.PathPaint = pathPaint;
        }

        private static SKPathEffect ConstructPathLineEffect(PathTypeEnum pathType, float pathWidth)
        {
            SKPathEffect? pathLineEffect = null;

            switch (pathType)
            {
                case PathTypeEnum.DottedLinePath:
                    float[] intervals = [0, pathWidth];
                    pathLineEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashedLinePath:
                    intervals = [pathWidth, pathWidth];
                    pathLineEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashDotLinePath:
                    intervals = [pathWidth, pathWidth, 0, pathWidth];
                    pathLineEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashDotDotLinePath:
                    intervals = [pathWidth, pathWidth, 0, pathWidth, 0, pathWidth];
                    pathLineEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.ChevronLinePath:
                    string svgPath = "M 0 0"
                        + " L" + pathWidth.ToString() + " 0"
                        + " L" + (pathWidth * 1.5F).ToString() + " " + (pathWidth / 2.0F).ToString()
                        + " L" + pathWidth.ToString() + " " + pathWidth.ToString()
                        + " L0 " + pathWidth.ToString()
                        + " L" + (pathWidth / 2.0F).ToString() + " " + (pathWidth / 2.0F).ToString()
                        + " L0 0"
                        + " Z";

                    pathLineEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData(svgPath), pathWidth * 2, 0, SKPath1DPathEffectStyle.Rotate);
                    break;
                case PathTypeEnum.LineAndDashesPath:

                    float ldWidth = Math.Max(1, pathWidth / 2.0F);

                    svgPath = "M 0 0"
                        + " h" + (pathWidth).ToString()
                        + " v" + Math.Max(1, ldWidth / 2.0F).ToString()
                        + " h" + (-pathWidth).ToString()
                        + " M0" + "," + (pathWidth - 1.0F).ToString()
                        + " h" + (ldWidth).ToString()
                        + " v2"
                        + " h" + (-ldWidth).ToString();

                    pathLineEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData(svgPath),
                        pathWidth, 0, SKPath1DPathEffectStyle.Morph);
                    break;
                case PathTypeEnum.ShortIrregularDashPath:
                    svgPath = "m0 0"
                        + " v " + pathWidth.ToString()
                        + " h " + (pathWidth / 4.0F).ToString()
                        + " v " + (-pathWidth).ToString()
                        + " z"
                        + " m" + pathWidth.ToString() + " 0"
                        + " v " + pathWidth.ToString()
                        + " h " + (pathWidth / 4.0F).ToString()
                        + " v " + (-pathWidth).ToString()
                        + " z";

                    pathLineEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData(svgPath),
                        pathWidth * 2, 0, SKPath1DPathEffectStyle.Rotate);
                    break;
                case PathTypeEnum.BearTracksPath:
                    SKPath? bearTrackPath = GetPathFromSvg("Bear Tracks", pathWidth);

                    if (bearTrackPath != null)
                    {
                        pathLineEffect = SKPathEffect.Create1DPath(bearTrackPath,
                            pathWidth, 0, SKPath1DPathEffectStyle.Rotate);
                    }

                    break;
                case PathTypeEnum.BirdTracksPath:
                    SKPath? birdTrackPath = GetPathFromSvg("Bird Tracks", pathWidth);

                    if (birdTrackPath != null)
                    {
                        pathLineEffect = SKPathEffect.Create1DPath(birdTrackPath,
                            pathWidth, 0, SKPath1DPathEffectStyle.Rotate);
                    }
                    break;
                case PathTypeEnum.FootprintsPath:
                    SKPath? footprintsPath = GetPathFromSvg("Foot Prints", pathWidth);

                    if (footprintsPath != null)
                    {
                        pathLineEffect = SKPathEffect.Create1DPath(footprintsPath,
                            pathWidth, 0, SKPath1DPathEffectStyle.Rotate);
                    }
                    break;
                case PathTypeEnum.RailroadTracksPath:
                    // TODO: the railroad tracks path doesn't look great; improve it
                    svgPath = "M0,0"
                        + " h " + pathWidth.ToString()
                        + " v" + (pathWidth * 0.2F).ToString()
                        + " h " + (-pathWidth).ToString()
                        + " M" + (pathWidth / 3.33F).ToString() + ", " + (pathWidth * 0.2F).ToString()
                        + " v " + pathWidth.ToString()
                        + " h" + (pathWidth * 0.2F).ToString()
                        + " v " + (-pathWidth).ToString()
                        + " M0," + (pathWidth * 1.2F).ToString()
                        + " h " + pathWidth.ToString()
                        + " v" + (-pathWidth * 0.2F).ToString()
                        + " h " + (-pathWidth).ToString();

                    pathLineEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData(svgPath), pathWidth, 0, SKPath1DPathEffectStyle.Morph);
                    break;

            }

#pragma warning disable CS8603 // Possible null reference return.
            return pathLineEffect;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static void DrawMapPath(MapCreatorMap map, MapPath mapPath, SKCanvas c)
        {
            SKRectI recti = new(0, 0, map.MapWidth, map.MapHeight);

            using SKRegion pathDrawingRegion = new();
            pathDrawingRegion.SetRect(recti);

            using SKRegion pathRegion = new(pathDrawingRegion);

            List<MapPathPoint> distinctPathPoints = mapPath.PathPoints.Distinct(new MapPathPointComparer()).ToList();
            List<MapLandformType2> landformList = LandformType2Methods.LANDFORM_LIST;

            for (int i = 0; i < landformList.Count; i++)
            {
                SKPath landformOutlinePath = landformList[i].LandformPath;

                if (landformOutlinePath != null && landformOutlinePath.PointCount > 0 && mapPath.PathPoints.Count > 0 && mapPath.PathPaint != null)
                {
                    pathRegion.SetPath(landformOutlinePath);

                    c.Save();
                    c.ClipRegion(pathRegion);

                    switch (mapPath.PathType)
                    {
                        case PathTypeEnum.SolidBlackBorderPath:
                            {
                                using SKPaint blackBorderPaint = mapPath.PathPaint.Clone();
                                blackBorderPaint.StrokeWidth = mapPath.PathPaint.StrokeWidth * 1.2F;
                                blackBorderPaint.Color = SKColors.Black;

                                DrawBezierCurvesFromPoints(c, distinctPathPoints, blackBorderPaint);
                                DrawBezierCurvesFromPoints(c, distinctPathPoints, mapPath.PathPaint);
                            }
                            break;
                        case PathTypeEnum.BorderedGradientPath:
                            {
                                using SKPaint borderPaint = mapPath.PathPaint.Clone();
                                borderPaint.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                borderPaint.Color = SKColors.Black;

                                List<MapPathPoint> parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.2F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, borderPaint);

                                using SKPaint linePaint1 = mapPath.PathPaint.Clone();
                                linePaint1.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                Color clr = Color.FromArgb(154, mapPath.PathColor);
                                linePaint1.Color = Extensions.ToSKColor(clr);

                                parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.4F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, linePaint1);

                                using SKPaint linePaint2 = mapPath.PathPaint.Clone();
                                linePaint2.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                clr = Color.FromArgb(102, mapPath.PathColor);
                                linePaint2.Color = Extensions.ToSKColor(clr);

                                parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.6F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, linePaint2);

                                using SKPaint linePaint3 = mapPath.PathPaint.Clone();
                                linePaint3.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                clr = Color.FromArgb(51, mapPath.PathColor);
                                linePaint3.Color = Extensions.ToSKColor(clr);

                                parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.8F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, linePaint3);

                                using SKPaint linePaint4 = mapPath.PathPaint.Clone();
                                linePaint4.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                clr = Color.FromArgb(25, mapPath.PathColor);
                                linePaint4.Color = Extensions.ToSKColor(clr);

                                parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, linePaint4);
                            }
                            break;
                        case PathTypeEnum.BorderedLightSolidPath:
                            {
                                using SKPaint borderPaint = mapPath.PathPaint.Clone();
                                borderPaint.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;
                                borderPaint.Color = SKColors.Black;

                                DrawBezierCurvesFromPoints(c, distinctPathPoints, borderPaint);

                                using SKPaint linePaint1 = mapPath.PathPaint.Clone();
                                linePaint1.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.8F;
                                Color clr = Color.FromArgb(102, mapPath.PathColor);
                                linePaint1.Color = Extensions.ToSKColor(clr);

                                List<MapPathPoint> parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.2F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, linePaint1);
                            }
                            break;
                        case PathTypeEnum.DoubleSolidBorderPath:
                            {
                                using SKPaint borderPaint = mapPath.PathPaint.Clone();
                                borderPaint.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;

                                DrawBezierCurvesFromPoints(c, distinctPathPoints, borderPaint);

                                List<MapPathPoint> parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, borderPaint);

                            }
                            break;
                        case PathTypeEnum.BorderAndTexturePath:
                            {
                                DrawBezierCurvesFromPoints(c, distinctPathPoints, mapPath.PathPaint);

                                using SKPaint borderPaint = mapPath.PathPaint.Clone();
                                borderPaint.Shader = null;

                                borderPaint.StrokeWidth = mapPath.PathPaint.StrokeWidth * 0.2F;

                                List<MapPathPoint> parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.5F, ParallelEnum.Above);
                                DrawBezierCurvesFromPoints(c, parallelPoints, borderPaint);

                                parallelPoints = MapDrawingMethods.GetParallelPathPoints(distinctPathPoints, mapPath.PathPaint.StrokeWidth * 0.5F, ParallelEnum.Below);
                                DrawBezierCurvesFromPoints(c, parallelPoints, borderPaint);
                            }

                            break;
                        default:
                            DrawBezierCurvesFromPoints(c, distinctPathPoints, mapPath.PathPaint);
                            break;
                    }

                    c.Restore();
                }
            }

            if (mapPath.IsSelected)
            {
                if (mapPath.BoundaryPath != null)
                {
                    // draw an outline around the path to show that it is selected
                    mapPath.BoundaryPath.GetTightBounds(out SKRect boundRect);
                    using SKPath boundsPath = new();
                    boundsPath.AddRect(boundRect);

                    MapBuilder.GetLayerCanvas(map, MapBuilder.PATHUPPERLAYER)?.DrawPath(boundsPath, MAPPATH_SELECT_PAINT);
                }
            }

            if (mapPath.ShowPathPoints)
            {
                List<MapPathPoint> controlPoints = GetMapPathControlPoints(mapPath);

                foreach (MapPathPoint p in controlPoints)
                {
                    PaintControlPoint(map, p, 2.0F, MAPPATH_CONTROL_POINT_PAINT);
                }
            }
        }

        private static void PaintControlPoint(MapCreatorMap map, MapPathPoint pathPoint, float size, SKPaint paint)
        {
            if (pathPoint != null)
            {
                using SKPath controlPointPath = new();

                controlPointPath.AddCircle(pathPoint.MapPoint.X, pathPoint.MapPoint.Y, size);

                MapBuilder.GetLayerCanvas(map, MapBuilder.PATHUPPERLAYER)?.DrawPath(controlPointPath, paint);
                MapBuilder.GetLayerCanvas(map, MapBuilder.PATHUPPERLAYER)?.DrawPath(controlPointPath, MAPPATH_CONTROL_POINT_OUTLINE_PAINT);
            }
        }

        private static List<MapPathPoint> GetMapPathControlPoints(MapPath mapPath)
        {
            List<MapPathPoint> mapPathPoints = [];

            for (int i = 0; i < mapPath.PathPoints.Count - 10; i += 10)
            {
                mapPathPoints.Add(mapPath.PathPoints[i]);
            }

            mapPathPoints.Add(mapPath.PathPoints[mapPath.PathPoints.Count - 1]);

            return mapPathPoints;
        }

        public static MapPathPoint? FindMapPathPoint(MapPath mapPath, PointF point)
        {
            foreach (MapPathPoint p in mapPath.PathPoints)
            {
                if (p.MapPoint.X == point.X && p.MapPoint.Y == point.Y)
                {
                    return p;
                }
            }

            return null;
        }

        public static void DrawAllPaths(MapCreatorMap map)
        {
            foreach (MapPath p in MAP_PATH_LIST)
            {
                SKCanvas? c;
                if (p.DrawOverSymbols)
                {
                    c = MapBuilder.GetLayerCanvas(map, MapBuilder.PATHUPPERLAYER);
                }
                else
                {
                    c = MapBuilder.GetLayerCanvas(map, MapBuilder.PATHLOWERLAYER);
                }

#pragma warning disable CS8604 // Possible null reference argument.
                DrawMapPath(map, p, c);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        private static void DrawBezierCurvesFromPoints(SKCanvas canvas, List<MapPathPoint> curvePoints, SKPaint paint)
        {
            if (curvePoints.Count == 2)
            {
                canvas.DrawLine(curvePoints[0].MapPoint, curvePoints[1].MapPoint, paint);
            }
            else if (curvePoints.Count > 2)
            {
                using SKPath path = new();
                path.MoveTo(curvePoints[0].MapPoint);

                for (int j = 0; j < curvePoints.Count; j += 3)
                {
                    if (j < curvePoints.Count - 2)
                    {
                        path.CubicTo(curvePoints[j].MapPoint, curvePoints[j + 1].MapPoint, curvePoints[j + 2].MapPoint);
                    }
                }

                canvas.DrawPath(path, paint);
            }
        }

        private static SKPath? GetPathFromSvg(string vectorName, float pathWidth)
        {
            MapVector? pathVector = null;

            for (int i = 0; i < PATH_VECTOR_LIST.Count; i++)
            {
                if (PATH_VECTOR_LIST[i].VectorName == vectorName)
                {
                    pathVector = PATH_VECTOR_LIST[i];
                    break;
                }
            }

            if (pathVector != null && pathVector.VectorSkPath != null)
            {
                pathVector.ScaledPath = new(pathVector.VectorSkPath);

                float xSize = pathWidth;
                float ySize = pathWidth;

                if (pathVector.ViewBoxSizeWidth != 0 && pathVector.ViewBoxSizeHeight != 0)
                {
                    xSize = pathVector.ViewBoxSizeWidth;
                    ySize = pathVector.ViewBoxSizeHeight;
                }

                float xScale = pathWidth / xSize;
                float yScale = pathWidth / ySize;


                pathVector.ScaledPath.Transform(SKMatrix.CreateScale(xScale, yScale));
                return pathVector.ScaledPath;
            }

            return null;
        }

        internal static MapPath? SelectMapPathAtPoint(Point mapClickPoint)
        {
            MapPath? selectedMapPath = null;

            for (int i = 0; i < MAP_PATH_LIST.Count; i++)
            {
                MapPath mapPath = MAP_PATH_LIST[i];

                if (mapPath.BoundaryPath != null && mapPath.BoundaryPath.PointCount > 0)
                {
                    mapPath.BoundaryPath.GetTightBounds(out SKRect boundRect);
                    using SKPath boundsPath = new();
                    boundsPath.AddRect(boundRect);

                    if (boundsPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                    {
                        selectedMapPath = mapPath;
                        break;
                    }
                }
            }

            return selectedMapPath;
        }

        public static MapPathPoint? SelectMapPathPointAtPoint(MapPath mapPath, PointF mapClickPoint)
        {
            MapPathPoint? selectedPoint = null;

            for (int i = 0; i < mapPath.PathPoints.Count; i++)
            {
                using SKPath controlPointPath = new();
                controlPointPath.AddCircle(mapPath.PathPoints[i].MapPoint.X, mapPath.PathPoints[i].MapPoint.Y, mapPath.PathWidth);

                if (controlPointPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                {
                    selectedPoint = mapPath.PathPoints[i];
                    break;
                }
            }

            if (selectedPoint != null)
            {
                selectedPoint.IsSelected = true;
            }

            SELECTED_MAP_PATH_POINT = selectedPoint;
            return selectedPoint;
        }

        public static MapPath? GetSelectedPath()
        {
            MapPath? selectedMapPath = null;

            for (int i = 0; i < MAP_PATH_LIST.Count; i++)
            {
                if (MAP_PATH_LIST[i].IsSelected)
                {
                    selectedMapPath = MAP_PATH_LIST[i];
                    break;
                }
            }

            return selectedMapPath;
        }

        public static MapPathPoint? GetMapPathPointById(MapPath mapPath, Guid guid)
        {
            foreach (MapPathPoint point in mapPath.PathPoints)
            {
                if (point.PointGuid == guid)
                {
                    return point;
                }
            }

            return null;
        }

        public static int GetMapPathPointIndexById(MapPath mapPath, Guid guid)
        {
            for (int i = 0; i < mapPath.PathPoints.Count; i++)
            {
                if (mapPath.PathPoints[i].PointGuid == guid)
                {
                    return i;
                }
            }

            return -1;
        }

        public static SKPath GenerateMapPathBoundaryPath(List<MapPathPoint> points)
        {
            SKPath path = new();

            path.MoveTo(points[0].MapPoint);

            for (int j = 0; j < points.Count; j += 3)
            {
                if (j < points.Count - 2)
                {
                    path.CubicTo(points[j].MapPoint, points[j + 1].MapPoint, points[j + 2].MapPoint);
                }
            }

            return path;
        }

        internal static void AddPointToMapPath(MapPathPoint mapPathPoint)
        {
            PATH_POINT_LIST.Add(mapPathPoint);
        }

        internal static MapPath GetNewPath()
        {
            return NEW_PATH;
        }

        internal static void SetSelectedMapPathPoints(MapPath mapPath)
        {
            mapPath.PathPoints.Clear();
            mapPath.PathPoints = new(PATH_POINT_LIST);
        }

        internal static List<MapTexture> GetPathTextureList()
        {
            return PATH_TEXTURE_LIST;
        }

        internal static void UpdateMapPathTextureShader(Bitmap resizedBitmap)
        {
            PATH_TEXTURE_SHADER?.Dispose();

            // create and set the shader from the selected texture
            PATH_TEXTURE_SHADER = SKShader.CreateBitmap(Extensions.ToSKBitmap(resizedBitmap),
                SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);
        }

        internal static MapPathPoint? GetSelectedMapPathPoint()
        {
            return SELECTED_MAP_PATH_POINT;
        }

        internal static void CreateNewMapPath()
        {
            NEW_PATH = new MapPath();
        }

        internal static ref List<MapPathPoint> GetPathPointList()
        {
            return ref PATH_POINT_LIST;
        }

        internal static void ConstructNewMapPath(MapCreatorMap map)
        {
            SKPath path = GenerateMapPathBoundaryPath(NEW_PATH.PathPoints);

            NEW_PATH.BoundaryPath?.Dispose();
            NEW_PATH.BoundaryPath = new(path);

            path.Dispose();

            AddMapPath(NEW_PATH);

            MapLayer mapPathLowerLayer = MapBuilder.GetMapLayerByIndex(map, MapBuilder.PATHLOWERLAYER);
            MapLayer mapPathUpperLayer = MapBuilder.GetMapLayerByIndex(map, MapBuilder.PATHUPPERLAYER);

            if (NEW_PATH.DrawOverSymbols)
            {
                mapPathUpperLayer.MapLayerComponents.Add(NEW_PATH);
            }
            else
            {
                mapPathLowerLayer.MapLayerComponents.Add(NEW_PATH);
            }
        }

        internal static void ClearPathPointList()
        {
            PATH_POINT_LIST = [];
        }

        internal static void MoveSelectedMapPathPoint(SKPoint movePoint)
        {
            MapPathPoint? mapPathPoint = GetSelectedMapPathPoint();
            if (mapPathPoint != null)
            {
                MapPath? selectedMapPath = GetSelectedPath();

                if (selectedMapPath != null)
                {
                    int selectedIndex = GetMapPathPointIndexById(selectedMapPath, mapPathPoint.PointGuid);

                    if (selectedIndex > -1)
                    {
                        // move the selected mappath point and the 4 points before and after it
                        if (selectedIndex - 4 > 0)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.2F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.2F;
                            SKPoint newPoint = new(movePoint.X - xDelta, movePoint.Y - yDelta);
                            selectedMapPath.PathPoints[selectedIndex - 4].MapPoint = newPoint;
                        }

                        if (selectedIndex - 3 > 0)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.4F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.4F;
                            SKPoint newPoint = new(movePoint.X - xDelta, movePoint.Y - yDelta);
                            selectedMapPath.PathPoints[selectedIndex - 3].MapPoint = newPoint;
                        }

                        if (selectedIndex - 2 > 0)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.6F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.6F;
                            SKPoint newPoint = new(movePoint.X - xDelta, movePoint.Y - yDelta);
                            selectedMapPath.PathPoints[selectedIndex - 2].MapPoint = newPoint;
                        }

                        if (selectedIndex - 1 > 0)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.8F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.8F;
                            SKPoint newPoint = new(movePoint.X - xDelta, movePoint.Y - yDelta);
                            selectedMapPath.PathPoints[selectedIndex - 1].MapPoint = newPoint;
                        }

                        selectedMapPath.PathPoints[selectedIndex].MapPoint = movePoint;

                        if (selectedIndex + 1 < selectedMapPath.PathPoints.Count - 1)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.2F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.2F;
                            SKPoint newPoint = new(movePoint.X + xDelta, movePoint.Y + yDelta);
                            selectedMapPath.PathPoints[selectedIndex + 1].MapPoint = newPoint;
                        }

                        if (selectedIndex + 2 < selectedMapPath.PathPoints.Count - 1)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.4F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.4F;
                            SKPoint newPoint = new(movePoint.X + xDelta, movePoint.Y + yDelta);
                            selectedMapPath.PathPoints[selectedIndex + 2].MapPoint = newPoint;
                        }

                        if (selectedIndex + 3 < selectedMapPath.PathPoints.Count - 1)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.6F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.6F;
                            SKPoint newPoint = new(movePoint.X + xDelta, movePoint.Y + yDelta);
                            selectedMapPath.PathPoints[selectedIndex + 3].MapPoint = newPoint;
                        }

                        if (selectedIndex + 4 < selectedMapPath.PathPoints.Count - 1)
                        {
                            float xDelta = (movePoint.X - selectedMapPath.PathPoints[selectedIndex].MapPoint.X) * 0.8F;
                            float yDelta = (movePoint.Y - selectedMapPath.PathPoints[selectedIndex].MapPoint.Y) * 0.8F;
                            SKPoint newPoint = new(movePoint.X + xDelta, movePoint.Y + yDelta);
                            selectedMapPath.PathPoints[selectedIndex + 4].MapPoint = newPoint;
                        }

                    }

                    SKPath path = GenerateMapPathBoundaryPath(selectedMapPath.PathPoints);
                    selectedMapPath.BoundaryPath?.Dispose();
                    selectedMapPath.BoundaryPath = new(path);
                    path.Dispose();
                }
            }
        }
    }
}
