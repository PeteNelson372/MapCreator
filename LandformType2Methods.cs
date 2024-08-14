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
    internal class LandformType2Methods
    {
        public static List<MapLandformType2> LANDFORM_LIST { get; set; } = [];

        public static MapLandformType2 SELECTED_LANDFORM { get; set; } = new();

        public static SKPath LAND_LAYER_ERASER_PATH = new()
        {
            FillType = SKPathFillType.Winding,
        };

        public static int LAND_BRUSH_SIZE { get; set; } = 64;
        public static int LAND_ERASER_SIZE { get; set; } = 20;
        public static int LAND_COLOR_BRUSH_SIZE { get; set; } = 20;
        public static int LAND_COLOR_ERASER_SIZE { get; set; } = 20;

        public static List<Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>> LAND_LAYER_COLOR_PATHS { get; set; } = [];

        public static SKPath LAND_LAYER_COLOR_ERASER_PATH = new();

        public static readonly SKPaint LAND_BORDER_PAINT = new();
        public static readonly SKPaint LAND_FILL_PAINT = new();
        public static readonly SKPaint LAND_ERASER_PAINT = new();
        public static readonly SKPaint LAND_COLOR_PAINT = new();
        public static readonly SKPaint LAND_COLOR_ERASER_PAINT = new();

        public static readonly SKPaint LANDFORM_SELECT_PAINT = new();
        public static readonly SKPaint LANDFORM_SELECT_ERASE_PAINT = new();

        public static readonly SKPaint COAST_FILL_PAINT = new();
        public static readonly SKPaint COAST_ERASER_PAINT = new();

        private static SKShader? DASH_BITMAP_SHADER;
        private static SKShader? DASH_LINEAR_GRADIENT_SHADER;
        private static SKShader? DASH_COMBINED_SHADER;

        private static SKShader? LINEHATCH_BITMAP_SHADER;
        private static SKShader? LINEHATCH_LINEAR_GRADIENT_SHADER;
        private static SKShader? LINEHATCH_COMBINED_SHADER;

        private static SKShader? USERDEFINED_BITMAP_SHADER;
        private static SKShader? USERDEFINED_LINEAR_GRADIENT_SHADER;
        private static SKShader? USERDEFINED_COMBINED_SHADER;

        private static SKBlendMode USERDEFINED_BLENDMODE = SKBlendMode.DstOver;

        public static List<MapTexture> LAND_TEXTURE_LIST { get; set; } = [];
        public static List<MapTexture> HATCH_TEXTURE_LIST { get; set; } = [];


        internal static void ConstructLandPaintObjects()
        {
            LAND_BORDER_PAINT.Style = SKPaintStyle.Stroke;
            LAND_BORDER_PAINT.IsAntialias = true;
            LAND_BORDER_PAINT.StrokeWidth = 2;

            LAND_FILL_PAINT.Style = SKPaintStyle.Fill;
            LAND_FILL_PAINT.IsAntialias = true;

            LAND_ERASER_PAINT.Color = SKColor.Empty;
            LAND_ERASER_PAINT.Style = SKPaintStyle.Fill;
            LAND_ERASER_PAINT.BlendMode = SKBlendMode.Src;
            LAND_ERASER_PAINT.IsAntialias = true;

            LAND_COLOR_PAINT.Style = SKPaintStyle.Fill;
            LAND_COLOR_PAINT.IsAntialias = true;

            LAND_COLOR_ERASER_PAINT.Color = SKColor.Empty;
            LAND_COLOR_ERASER_PAINT.Style = SKPaintStyle.Fill;
            LAND_COLOR_ERASER_PAINT.BlendMode = SKBlendMode.Src;
            LAND_COLOR_ERASER_PAINT.IsAntialias = true;

            COAST_FILL_PAINT.Style = SKPaintStyle.Stroke;
            COAST_FILL_PAINT.IsAntialias = true;

            COAST_ERASER_PAINT.Color = SKColor.Empty;
            COAST_ERASER_PAINT.Style = SKPaintStyle.Fill;
            COAST_ERASER_PAINT.BlendMode = SKBlendMode.Src;
            COAST_ERASER_PAINT.IsAntialias = true;

            LANDFORM_SELECT_PAINT.Style = SKPaintStyle.Stroke;
            LANDFORM_SELECT_PAINT.IsAntialias = true;
            LANDFORM_SELECT_PAINT.Color = SKColors.Firebrick;
            LANDFORM_SELECT_PAINT.StrokeWidth = 2;
            LANDFORM_SELECT_PAINT.PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F);

            LANDFORM_SELECT_ERASE_PAINT.Style = SKPaintStyle.Stroke;
            LANDFORM_SELECT_ERASE_PAINT.IsAntialias = true;
            LANDFORM_SELECT_ERASE_PAINT.Color = SKColors.Transparent;
            LANDFORM_SELECT_ERASE_PAINT.StrokeWidth = 2;


            MapTexture? lineHatchTexture = HATCH_TEXTURE_LIST.Find(x => x.TextureName == "Line Hatch");

            if (lineHatchTexture != null)
            {
                lineHatchTexture.TextureBitmap ??= new Bitmap(lineHatchTexture.TexturePath);

                SKBitmap resizedSKBitmap = new(100, 100);

                Extensions.ToSKBitmap(lineHatchTexture.TextureBitmap).ScalePixels(resizedSKBitmap, SKFilterQuality.High);

                LINEHATCH_BITMAP_SHADER = SKShader.CreateBitmap(resizedSKBitmap, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                LINEHATCH_LINEAR_GRADIENT_SHADER = SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(lineHatchTexture.TextureBitmap.Width, 0), [SKColors.Transparent, SKColor.Parse("#44FFFFFF"), SKColors.Transparent], SKShaderTileMode.Clamp);
                LINEHATCH_COMBINED_SHADER = SKShader.CreateCompose(LINEHATCH_LINEAR_GRADIENT_SHADER, LINEHATCH_BITMAP_SHADER, SKBlendMode.Modulate);
            }
        }

        internal static void ConstructUserDefinedShaders(SKBitmap resizedSKBitmap, int hatchOpacity, int bitmapSize, SKBlendMode blendMode)
        {
            USERDEFINED_BITMAP_SHADER = SKShader.CreateBitmap(resizedSKBitmap, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);


            Color opacityColor = Color.FromArgb(hatchOpacity, Color.White);

            USERDEFINED_LINEAR_GRADIENT_SHADER = SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(resizedSKBitmap.Width, 0), [SKColors.Transparent, Extensions.ToSKColor(opacityColor), SKColors.Transparent], SKShaderTileMode.Clamp);
            USERDEFINED_COMBINED_SHADER = SKShader.CreateCompose(USERDEFINED_LINEAR_GRADIENT_SHADER, USERDEFINED_BITMAP_SHADER, SKBlendMode.Modulate);

            USERDEFINED_BLENDMODE = blendMode;
        }

        internal static void AddHatchTexture(MapTexture t)
        {
            HATCH_TEXTURE_LIST.Add(t);
        }

        internal static MapLandformType2 GetNewSelectedLandform(MapCreatorMap map)
        {
            SELECTED_LANDFORM = new()
            {
                ParentMap = map
            };

            return SELECTED_LANDFORM;
        }

        public static void ResetLandCanvases(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.Clear(SKColor.Empty);
            MapBuilder.GetLayerBitmap(map, MapBuilder.LANDFORMLAYER)?.Erase(SKColor.Empty);

            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.Clear(SKColor.Empty);
            MapBuilder.GetLayerBitmap(map, MapBuilder.LANDCOASTLINELAYER)?.Erase(SKColor.Empty);

            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDDRAWINGLAYER)?.Clear(SKColor.Empty);
            MapBuilder.GetLayerBitmap(map, MapBuilder.LANDDRAWINGLAYER)?.Erase(SKColor.Empty);
        }

        internal static void AddSelectedLandFormToLandformList()
        {
            SELECTED_LANDFORM.LandformName = "Landform " + LANDFORM_LIST.Count;
            LANDFORM_LIST.Add(SELECTED_LANDFORM);
        }

        public static void FillMapWithLandForm(MapCreatorMap map, MapLandformType2 landform)
        {
            Cmd_FillMapWithLandform cmd = new(map, landform);
            UndoManager.AddCommand(cmd);
            cmd.DoOperation();

            //DrawLandform(map, landform);
            //PaintLandForm(map);
        }

        internal static void MergeLandforms()
        {
            // merge overlapping landforms
            for (int i = 0; i < LANDFORM_LIST.Count; i++)
            {
                for (int j = 0; j < LANDFORM_LIST.Count; j++)
                {
                    if (i < LANDFORM_LIST.Count && j < LANDFORM_LIST.Count && i != j)
                    {
                        SKPath landformPath1 = LANDFORM_LIST[i].LandformPath;
                        SKPath landformPath2 = LANDFORM_LIST[j].LandformPath;

                        bool pathsMerged = MergeLandformPaths(landformPath2, ref landformPath1);

                        if (pathsMerged)
                        {
                            LANDFORM_LIST[i].LandformPath = new(landformPath1);
                            LANDFORM_LIST[i].DrawLandform = true;

                            // merge the other data from LANDFORM_LIST[j]
                            // with the data from LANDFORM_LIST[i]
                            MapLandformType2 dstLandform = LANDFORM_LIST[i];

                            MergeLandformData(ref dstLandform, LANDFORM_LIST[j]);

                            LANDFORM_LIST[i] = dstLandform;
                            LANDFORM_LIST.RemoveAt(j);
                        }
                    }
                }
            }
        }

        internal static bool MergeLandformPaths(SKPath landformPath1, ref SKPath landformPath2)
        {
            // merge paths from two landforms; if the paths overlap, then the second
            // set of paths is modified to include the first set (the second set becomes the union
            // of the two original sets)
            bool pathsMerged = false;

            if (landformPath1.PointCount > 0)
            {
                // get the intersection between the paths
                SKPath intersectionPath = landformPath2.Op(landformPath1, SKPathOp.Intersect);

                // if the intersection path isn't null or empty, then merge the paths
                if (intersectionPath != null && intersectionPath.PointCount > 0)
                {
                    // calculate the union between the land layer paths and the land layer draw paths
                    SKPath unionPath = landformPath2.Op(landformPath1, SKPathOp.Union);

                    if (unionPath != null && unionPath.PointCount > 0)
                    {
                        pathsMerged = true;
                        landformPath2.Dispose();
                        landformPath2 = new SKPath(unionPath)
                        {
                            FillType = SKPathFillType.Winding
                        };

                        unionPath.Dispose();
                    }
                }
            }
            

            return pathsMerged;
        }

        public static void MergeLandformData(ref MapLandformType2 mapLandform1, MapLandformType2 mapLandform2)
        {
            // merge the data from mapLandform2 into mapLandform1 as needed

            if (mapLandform1.LandformBackgroundPaint == null)
            {
                mapLandform1.LandformBackgroundPaint = mapLandform2.LandformBackgroundPaint;
            }

            mapLandform1.CoastlineColor = mapLandform2.CoastlineColor;

            mapLandform1.CoastlineColorOpacity = mapLandform2.CoastlineColorOpacity;

            mapLandform1.CoastlineEffectDistance = mapLandform2.CoastlineEffectDistance;

            if (mapLandform1.CoastlineHatchBlendMode == null)
            {
                mapLandform1.CoastlineHatchBlendMode = mapLandform2.CoastlineHatchBlendMode;
            }

            mapLandform1.CoastlineHatchOpacity = mapLandform2.CoastlineHatchOpacity;


            if (mapLandform1.CoastlineHatchPattern == null)
            {
                mapLandform1.CoastlineHatchPattern = mapLandform2.CoastlineHatchPattern;
            }

            mapLandform1.CoastlineHatchScale = mapLandform2.CoastlineHatchScale;

            if (mapLandform1.CoastlineStyleName == null)
            {
                mapLandform1.CoastlineStyleName = mapLandform2.CoastlineStyleName;
            }

            mapLandform1.LandformOutlineColor = mapLandform2.LandformOutlineColor;

            mapLandform1.LandformOutlineWidth = mapLandform2.LandformOutlineWidth;

            if (mapLandform1.LandformTexture == null)
            {
                mapLandform1.LandformTexture = mapLandform2.LandformTexture;
            }

            mapLandform1.PaintCoastlineGradient = mapLandform2.PaintCoastlineGradient;
        }

        internal static void ResetLandformsOnCanvas(MapCreatorMap map)
        {
            MapLayer landformLayer = MapBuilder.GetMapLayerByIndex(map, MapBuilder.LANDFORMLAYER);

            // remove all existing landforms from the landform layer
            for (int i = landformLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (landformLayer.MapLayerComponents[i] is MapLandformType2)
                {
                    landformLayer.MapLayerComponents.RemoveAt(i);
                }
            }

            // add current landforms to the landform layer
            foreach (MapLandformType2 f in LANDFORM_LIST)
            {
                landformLayer.MapLayerComponents.Add(f);
            }
        }

        internal static void ColorLandformPaths(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDDRAWINGLAYER)?.Clear();

            SKPath ColorPath = new();
            foreach (Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>> colorPoints in LAND_LAYER_COLOR_PATHS)
            {
                ColorPath.Reset();

                foreach (Tuple<SKPoint, int, SKShader> sp in colorPoints.Item2)
                {
                    ColorPath.AddCircle(sp.Item1.X, sp.Item1.Y, sp.Item2);

                    LAND_COLOR_PAINT.Shader = sp.Item3;

                    SKCanvas? landDrawingCanvas = MapBuilder.GetLayerCanvas(map, MapBuilder.LANDDRAWINGLAYER);
                    SKRectI recti = new(0, 0, map.MapWidth, map.MapHeight);

                    if (landDrawingCanvas != null)
                    {
                        using SKRegion landDrawingRegion = new();
                        landDrawingRegion.SetRect(recti);

                        using SKRegion landPathRegion = new(landDrawingRegion);

                        if (ColorPath.PointCount > 0)
                        {
                            // clip painting to the outer path of the landform
                            // LandformPath is the outer path of the landform

                            for (int i = 0; i < LANDFORM_LIST.Count; i++)
                            {
                                SKPath landformOutlinePath = LANDFORM_LIST[i].LandformPath;

                                landPathRegion.SetPath(landformOutlinePath);

                                landDrawingCanvas.Save();
                                landDrawingCanvas.ClipRegion(landPathRegion);
                                landDrawingCanvas.DrawPath(ColorPath, LAND_COLOR_PAINT);
                                landDrawingCanvas.Restore();
                            }
                        }
                    }
                }
            }
        }

        internal static void EraseLandformColor(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDDRAWINGLAYER)?.DrawPath(LAND_LAYER_COLOR_ERASER_PATH, LAND_COLOR_ERASER_PAINT);
        }

        internal static void EraseLandForm(MapCreatorMap map)
        {
            if (LAND_LAYER_ERASER_PATH.PointCount > 0)
            {
                for (int i = 0; i < LANDFORM_LIST.Count; i++)
                {
                    // calculate the difference between the land layer path and the land eraser path
                    using SKPath diffPath = LANDFORM_LIST[i].LandformPath.Op(LAND_LAYER_ERASER_PATH, SKPathOp.Difference);

                    if (diffPath != null)
                    {
                        LANDFORM_LIST[i].LandformPath.Dispose();
                        LANDFORM_LIST[i].LandformPath = new(diffPath);
                    }
                }

                LAND_LAYER_ERASER_PATH.Reset();
            }
        }

        internal static void CreateType2LandformPaths(MapCreatorMap map, MapLandformType2 landform)
        {
            landform.LandformContourPath = MapDrawingMethods.GetContourPathFromPath(landform.LandformPath, map.MapWidth, map.MapHeight, out List<SKPoint> contourPoints);
            landform.LandformContourPoints = contourPoints;

            if (contourPoints.Count > 0)
            {
                LAND_BORDER_PAINT.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

                landform.InnerPath1.Dispose();
                landform.InnerPath2.Dispose();
                landform.InnerPath3.Dispose();
                landform.InnerPath4.Dispose();
                landform.InnerPath5.Dispose();
                landform.InnerPath6.Dispose();
                landform.InnerPath7.Dispose();
                landform.InnerPath8.Dispose();

                landform.OuterPath1.Dispose();
                landform.OuterPath2.Dispose();
                landform.OuterPath3.Dispose();
                landform.OuterPath4.Dispose();
                landform.OuterPath5.Dispose();
                landform.OuterPath6.Dispose();
                landform.OuterPath7.Dispose();
                landform.OuterPath8.Dispose();

                landform.InnerPath1 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth, ParallelEnum.Below);

                landform.InnerPath2 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 2, ParallelEnum.Below);

                landform.InnerPath3 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 3, ParallelEnum.Below);

                landform.InnerPath4 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 4, ParallelEnum.Below);

                landform.InnerPath5 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 5, ParallelEnum.Below);

                landform.InnerPath6 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 6, ParallelEnum.Below);

                landform.InnerPath7 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 7, ParallelEnum.Below);

                landform.InnerPath8 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 8, ParallelEnum.Below);

                landform.OuterPath1 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 1, ParallelEnum.Above);

                landform.OuterPath2 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 2, ParallelEnum.Above);

                landform.OuterPath3 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 3, ParallelEnum.Above);

                landform.OuterPath4 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 4, ParallelEnum.Above);

                landform.OuterPath5 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 5, ParallelEnum.Above);

                landform.OuterPath6 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 6, ParallelEnum.Above);

                landform.OuterPath7 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 7, ParallelEnum.Above);

                landform.OuterPath8 =
                    MapDrawingMethods.GetInnerOrOuterPath(contourPoints,
                    LAND_BORDER_PAINT.StrokeWidth * 8, ParallelEnum.Above);
            }
        }

        /**************************************************************************************************************************
        *** LANDFORM DRAW METHODS
        ***************************************************************************************************************************/

        public static void DrawLandform(MapCreatorMap map, MapLandformType2 landform)
        {
            if (landform == null || string.IsNullOrEmpty(landform.CoastlineStyleName)) return;

            if (landform.IsSelected)
            {
                // draw an outline around the landform to show that it is selected
                landform.LandformPath.GetBounds(out SKRect boundRect);
                using SKPath boundsPath = new();
                boundsPath.AddRect(boundRect);

                MapBuilder.GetLayerCanvas(map, MapBuilder.SELECTIONLAYER)?.DrawPath(boundsPath, LANDFORM_SELECT_PAINT);
            }

            if (landform.DrawLandform)
            {
                if (landform.ShorelineStyle == GradientDirectionEnum.None)
                {
                    DrawNoShorelineEffectLandform(map, landform);
                }
                else
                {
                    DrawGradientLandforms(map, landform);
                }

                if (!string.IsNullOrEmpty(landform.CoastlineStyleName))
                {
                    switch (landform.CoastlineStyleName)
                    {
                        case "None":
                            break;
                        case "Uniform Band":
                            DrawUniformBandCoastlineEffect(map, landform);
                            break;
                        case "Uniform Blend":
                            DrawUniformBlendCoastlineEffect(map, landform);
                            break;
                        case "Uniform Outline":
                            DrawUniformOutlineCoastlineEffect(map, landform);
                            break;
                        case "Three-Tiered":
                            DrawThreeTieredCoastlineEffect(map, landform);
                            break;
                        case "Circular Pattern":
                            DrawRadialPatternCoastlineEffect(map, landform);
                            break;
                        case "Dash Pattern":
                            DrawDashPatternCoastlineEffect(map, landform);
                            break;
                        case "Hatch Pattern":
                            DrawHatchPatternCoastlineEffect(map, landform);
                            break;
                        case "User Defined":
                            DrawUserDefinedHatchEffect(map, landform);
                            break;
                    }
                }

                landform.DrawLandform = false;
            }
        }

        private static void DrawNoShorelineEffectLandform(MapCreatorMap map, MapLandformType2 landform)
        {
            // draw landforms with no shoreline effect
            LAND_BORDER_PAINT.BlendMode = SKBlendMode.Src;

            // if no texture selected, then fill the landform with the border color
            LAND_BORDER_PAINT.Style = SKPaintStyle.Stroke;

#pragma warning disable CS8629 // Nullable value type may be null.
            LAND_BORDER_PAINT.Color = Extensions.ToSKColor((Color)landform.LandformOutlineColor);
            LAND_BORDER_PAINT.StrokeWidth = (float)landform.LandformOutlineWidth;
#pragma warning restore CS8629 // Nullable value type may be null.

            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.LandformPath, LAND_BORDER_PAINT);

            if (landform.LandformBackgroundPaint == null)
            {
                LAND_FILL_PAINT.Shader = null;
                int a = 51;
                LAND_FILL_PAINT.Color = Extensions.ToSKColor(Color.FromArgb(a, (Color)landform.LandformOutlineColor));
                LAND_FILL_PAINT.BlendMode = SKBlendMode.Src;
                landform.LandformBackgroundPaint = LAND_FILL_PAINT;
            }

            // fill the landform base path with texture or color
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.LandformPath, landform.LandformBackgroundPaint);
        }

        private static void DrawGradientLandforms(MapCreatorMap map, MapLandformType2 landform)
        {
            //
            // draw landforms with gradient
            //
            // fill the landform base path with texture or color
            if (landform.LandformBackgroundPaint == null)
            {
                landform.LandformBackgroundPaint = LAND_FILL_PAINT;
            }

            landform.LandformBackgroundPaint.BlendMode = SKBlendMode.Src;

            // fill the landform base path with texture or color
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.LandformPath, landform.LandformBackgroundPaint);

#pragma warning disable CS8629 // Nullable value type may be null.
            Color backColor = landform.LandformOutlineColor;
#pragma warning restore CS8629 // Nullable value type may be null.

            LAND_BORDER_PAINT.BlendMode = SKBlendMode.Src;
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100, backColor.GetBrightness() * 100);
            LAND_BORDER_PAINT.Style = SKPaintStyle.Stroke;
            LAND_BORDER_PAINT.IsAntialias = true;

            LAND_BORDER_PAINT.StrokeWidth = 2;

            // LandformContourPath
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.LandformContourPath, LAND_BORDER_PAINT);

            LAND_BORDER_PAINT.StrokeWidth = 4;
            LAND_BORDER_PAINT.BlendMode = SKBlendMode.SrcATop;

            byte alpha = 90;  // set the initial transparency of the gradient paths

            // inner path 1
            float luminosity = Math.Min((backColor.GetBrightness() * 100F) + (2 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath1, LAND_BORDER_PAINT);


            // inner path 2
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (3 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath2, LAND_BORDER_PAINT);

            // inner path 3
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (4 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath3, LAND_BORDER_PAINT);

            // inner path 4
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (5 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath4, LAND_BORDER_PAINT);

            // inner path 5
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (6 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath5, LAND_BORDER_PAINT);

            // inner path 6
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (7 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath6, LAND_BORDER_PAINT);

            // inner path 7
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (8 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath7, LAND_BORDER_PAINT);

            // inner path 8
            luminosity = Math.Min((backColor.GetBrightness() * 100F) + (9 * 4F), 100F);

            // decrease alpha channel (increase transparency)
            alpha -= 10;

            // create the color from the calculated luminosity
            LAND_BORDER_PAINT.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminosity, alpha);
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDFORMLAYER)?.DrawPath(landform.InnerPath8, LAND_BORDER_PAINT);
        }


        /**************************************************************************************************************************
        *** COASTLINE DRAW METHODS
        ***************************************************************************************************************************/

        private static void DrawUniformBandCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);
            paint.Shader?.Dispose();

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            paint.Color = Extensions.ToSKColor(Color.FromArgb(landform.CoastlineColorOpacity, landform.CoastlineColor));

            //===========

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);

        }

        private static void DrawUniformBlendCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);
            paint.Shader?.Dispose();

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            int coastlineColorOpacityStep = (int)(landform.CoastlineColorOpacity / 8.0F);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb(landform.CoastlineColorOpacity, landform.CoastlineColor));

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.9F), landform.CoastlineColor));

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.8F), landform.CoastlineColor));

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.7F), landform.CoastlineColor));

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.6F), landform.CoastlineColor));

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.5F), landform.CoastlineColor));

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.4F), landform.CoastlineColor));

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.3F), landform.CoastlineColor));

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }

        private static void DrawUniformOutlineCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);
            paint.Shader?.Dispose();

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            int coastlineColorOpacityStep = (int)(landform.CoastlineColorOpacity / 8.0F);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.5F), landform.CoastlineColor));

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.5F), landform.CoastlineColor));

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.5F), landform.CoastlineColor));

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity * 0.5F), landform.CoastlineColor));

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb(landform.CoastlineColorOpacity, landform.CoastlineColor));

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);
        }

        private static void DrawThreeTieredCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);
            paint.Shader?.Dispose();

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 1.0F), landform.CoastlineColor));

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 1.0F), landform.CoastlineColor));

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 2.0F), landform.CoastlineColor));

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 2.0F), landform.CoastlineColor));

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 2.0F), landform.CoastlineColor));

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 4.0F), landform.CoastlineColor));

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 4.0F), landform.CoastlineColor));

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            paint.Color = Extensions.ToSKColor(Color.FromArgb((byte)(landform.CoastlineColorOpacity / 4.0F), landform.CoastlineColor));

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }

        private static void DrawRadialPatternCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);
            paint.Shader?.Dispose();

            Color coastlineBandColor = landform.CoastlineColor;

            float coastEffectPathWidth = landform.CoastlineEffectDistance / 8.0F;

            SKRect pathBounds = landform.LandformContourPath.Bounds;

            SKShader gradient = SKShader.CreateRadialGradient(new SKPoint(pathBounds.MidX, pathBounds.MidY), coastEffectPathWidth, [Extensions.ToSKColor(coastlineBandColor), SKColors.Empty], SKShaderTileMode.Mirror);

            //===========
            Color gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity, landform.CoastlineColor);

            // create the color from the calculated luminosity
            SKColor gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 2, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 3, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 4, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 5, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 6, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 7, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            gcolor = Color.FromArgb((byte)landform.CoastlineColorOpacity / 8, landform.CoastlineColor);

            // create the color from the calculated luminosity
            gradientColor = Extensions.ToSKColor(gcolor);
            paint.Shader?.Dispose();

            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(gradientColor),
                gradient,
                SKBlendMode.Plus);

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }

        private static void DrawHatchPatternCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            //===========
            Color gradientColor = Color.FromArgb(0, (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            gradientColor = Color.FromArgb((int)(8 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            gradientColor = Color.FromArgb((int)(8 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            gradientColor = Color.FromArgb((int)(6 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            gradientColor = Color.FromArgb((int)(5 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            gradientColor = Color.FromArgb((int)(4 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            gradientColor = Color.FromArgb((int)(3 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            gradientColor = Color.FromArgb((int)(2 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                LINEHATCH_COMBINED_SHADER,
                SKBlendMode.DstOver);

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }

        private static void DrawDashPatternCoastlineEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            MapTexture? dashTexture = HATCH_TEXTURE_LIST.Find(x => x.TextureName == "Watercolor Dashes");

            if (dashTexture != null)
            {
                dashTexture.TextureBitmap ??= new Bitmap(dashTexture.TexturePath);

                SKBitmap resizedSKBitmap = new SKBitmap(100, 100);

                Extensions.ToSKBitmap(dashTexture.TextureBitmap).ScalePixels(resizedSKBitmap, SKFilterQuality.High);

                DASH_BITMAP_SHADER = SKShader.CreateBitmap(resizedSKBitmap, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                Color gradientFadeColor = Color.FromArgb((byte)landform.CoastlineColorOpacity, Color.White);

                DASH_LINEAR_GRADIENT_SHADER = SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(dashTexture.TextureBitmap.Width, 0), [SKColors.Transparent, gradientFadeColor.ToSKColor(), SKColors.Transparent], SKShaderTileMode.Clamp);
                DASH_COMBINED_SHADER = SKShader.CreateCompose(DASH_LINEAR_GRADIENT_SHADER, DASH_BITMAP_SHADER, SKBlendMode.Modulate);
            }

            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            //===========
            Color gradientColor = Color.FromArgb(landform.CoastlineColorOpacity, landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            gradientColor = Color.FromArgb((int)(8 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            gradientColor = Color.FromArgb((int)(7 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            gradientColor = Color.FromArgb((int)(6 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            gradientColor = Color.FromArgb((int)(5 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            gradientColor = Color.FromArgb((int)(4 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            gradientColor = Color.FromArgb((int)(3 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            gradientColor = Color.FromArgb((int)(2 / 32F * landform.CoastlineColorOpacity), landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                DASH_BITMAP_SHADER,
                SKBlendMode.Modulate);

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }

        private static void DrawUserDefinedHatchEffect(MapCreatorMap map, MapLandformType2 landform)
        {
            if (USERDEFINED_COMBINED_SHADER == null) return;

            using SKPaint paint = MapPaintMethods.DeepCopyPaintObject(COAST_FILL_PAINT);

            paint.StrokeWidth = landform.CoastlineEffectDistance / 8.0F;

            //===========
            Color gradientColor = Color.FromArgb(0, (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath1
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath1, paint);

            //===========
            gradientColor = Color.FromArgb((int)(1 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath2
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath2, paint);

            //===========
            gradientColor = Color.FromArgb((int)(2 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath3
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath3, paint);

            //===========
            gradientColor = Color.FromArgb((int)(3 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath4
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath4, paint);

            //===========
            gradientColor = Color.FromArgb((int)(4 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath5
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath5, paint);

            //===========
            gradientColor = Color.FromArgb((int)(5 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath6
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath6, paint);

            //===========
            gradientColor = Color.FromArgb((int)(6 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath7
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath7, paint);

            //===========
            gradientColor = Color.FromArgb((int)(7 / 32F * landform.CoastlineColorOpacity), (Color)landform.CoastlineColor);

            paint.Shader?.Dispose();
            paint.Shader = SKShader.CreateCompose(
                SKShader.CreateColor(Extensions.ToSKColor(gradientColor)),
                USERDEFINED_COMBINED_SHADER,
                USERDEFINED_BLENDMODE);

            // draw gradient path OuterPath8
            MapBuilder.GetLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER)?.DrawPath(landform.OuterPath8, paint);
        }
    }
}
