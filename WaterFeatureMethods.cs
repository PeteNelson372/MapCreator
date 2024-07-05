using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows;

namespace MapCreator
{
    internal class WaterFeatureMethods
    {
        public static List<MapPaintedWaterFeature> PAINTED_WATERFEATURE_LIST { get; set; } = [];
        public static MapPaintedWaterFeature? NEW_WATERFEATURE { get; set; } = new();

        public static List<MapRiver> MAP_RIVER_LIST { get; set; } = [];
        public static MapRiver NEW_RIVER { get; set; } = new();
        public static List<MapRiverPoint> RIVER_POINT_LIST { get; set; } = [];

        private static SKPaint RIVER_SELECT_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.BlueViolet,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F)
        };

        private static SKPaint RIVER_SELECT_ERASE_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.Empty,
            StrokeWidth = 2
        };

        private static SKPaint RIVER_CONTROL_POINT_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            StrokeWidth = 2,
            Color = SKColors.WhiteSmoke
        };

        private static SKPaint RIVER_CONTROL_POINT_OUTLINE_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeWidth = 1,
            Color = SKColors.Black
        };

        private static SKPaint RIVER_SELECTED_CONTROL_POINT_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            StrokeWidth = 2,
            Color = SKColors.BlueViolet,
        };

        public static List<Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>> WATER_LAYER_COLOR_PATHS { get; set; } = [];

        public static SKPath WATER_LAYER_PATH { get; set; } = new()
        {
            FillType = SKPathFillType.Winding
        };

        public static SKPath WATER_LAYER_DRAW_PATH { get; set; } = new()
        {
            FillType = SKPathFillType.Winding
        };

        public static SKPath WATER_LAYER_ERASER_PATH { get; set; } = new();        
        public static SKPath WATER_LAYER_COLOR_ERASER_PATH { get; set; } = new();
        private static int WATER_BRUSH_DEFAULT_SIZE { get; } = 20;
        public static int WATER_BRUSH_SIZE { get; set; } = 20;
        public static int WATER_ERASER_SIZE { get; set; } = 20;
        public static int WATER_COLOR_BRUSH_SIZE { get; set; } = 20;
        public static int WATER_COLOR_ERASER_SIZE { get; set; } = 20;

        public static Color DEFAULT_WATER_OUTLINE_COLOR { get; } = ColorTranslator.FromHtml("#A19076");
        public static Color DEFAULT_WATER_COLOR { get; } = ColorTranslator.FromHtml("#658CBFC5");

        public static SKPaint WATER_COLOR_PAINT { get; set; } = new()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            BlendMode = SKBlendMode.SrcOver
        };

        private static SKPaint WATER_COLOR_ERASER_PAINT { get; } = new()
        {
            Color = SKColor.Empty,
            Style = SKPaintStyle.Fill,
            BlendMode = SKBlendMode.Src,
            IsAntialias = true
        };

        private static SKPaint WATERFEATURE_SELECT_PAINT { get; } = new()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.CadetBlue,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash([3F, 3F], 6F)
        };

        public static List<MapTexture> WATER_TEXTURE_LIST { get; set; } = [];

        public static SKPathEffect ROUND_EFFECT { get; set; } = SKPathEffect.CreateCorner(WATER_BRUSH_DEFAULT_SIZE);


        internal static void PaintWaterPath(MapCreatorMap map)
        {
            SKPath waterFeaturePath = CalculateWaterFeaturePath(WATER_LAYER_PATH, false);

            if (NEW_WATERFEATURE != null && waterFeaturePath.PointCount > 0)
            {
                WATER_LAYER_PATH.Dispose();
                WATER_LAYER_PATH = new(waterFeaturePath);
                NEW_WATERFEATURE.SetWaterFeaturePath(WATER_LAYER_PATH);

                DrawWaterFeature(map, NEW_WATERFEATURE);
            }
        }

        internal static void AddPaintedWaterFeature(MapPaintedWaterFeature waterFeature)
        {
            bool found = false;
            for (int i = 0; i < PAINTED_WATERFEATURE_LIST.Count; i++)
            {
                if (PAINTED_WATERFEATURE_LIST[i].WaterFeatureGuid.Equals(waterFeature.WaterFeatureGuid))
                {
                    PAINTED_WATERFEATURE_LIST[i] = waterFeature;
                    found = true;
                }
            }

            if (!found)
            {
                PAINTED_WATERFEATURE_LIST.Add(waterFeature);
            }
        }

        internal static void ResetAllWaterPaths()
        {
            WATER_LAYER_PATH.Reset();
            WATER_LAYER_DRAW_PATH.Reset();
            WATER_LAYER_ERASER_PATH.Reset();
            WATER_LAYER_COLOR_ERASER_PATH.Reset();
        }

        internal static SKPath CalculateWaterFeaturePath(SKPath waterLayerPath, bool erase)
        {
            SKPath waterFeaturePath = new();

            if (erase)
            {
                if (WATER_LAYER_ERASER_PATH.PointCount > 0)
                {
                    // calculate the difference between the water layer paths and the water eraser paths
                    using SKPath diffPath = waterLayerPath.Op(WATER_LAYER_ERASER_PATH, SKPathOp.Difference);

                    if (diffPath != null)
                    {
                        waterFeaturePath.Dispose();
                        waterFeaturePath = new SKPath(diffPath)
                        {
                            FillType = SKPathFillType.Winding
                        };
                    }
                }
            }
            else
            {
                if (WATER_LAYER_DRAW_PATH.PointCount > 0)
                {
                    // calculate the union between the water layer path and the water layer draw path
                    using SKPath diffPath = waterLayerPath.Op(WATER_LAYER_DRAW_PATH, SKPathOp.Union);

                    if (diffPath != null)
                    {
                        waterFeaturePath.Dispose();
                        waterFeaturePath = new SKPath(diffPath)
                        {
                            FillType = SKPathFillType.Winding
                        };
                    }
                }
            }

            return waterFeaturePath;
        }

        internal static MapPaintedWaterFeature? MergeWaterFeatures()
        {
            MapPaintedWaterFeature? dstWaterFeature = null;

            // merge overlapping water features
            for (int i = 0; i < PAINTED_WATERFEATURE_LIST.Count; i++)
            {
                for (int j = 0; j < PAINTED_WATERFEATURE_LIST.Count; j++)
                {
                    if (i < PAINTED_WATERFEATURE_LIST.Count && j < PAINTED_WATERFEATURE_LIST.Count && i != j)
                    {
                        if (!PAINTED_WATERFEATURE_LIST[i].IsRemoved && !PAINTED_WATERFEATURE_LIST[j].IsRemoved)
                        {
                            SKPath waterFeaturePath1 = PAINTED_WATERFEATURE_LIST[i].GetWaterFeaturePath();
                            SKPath waterFeaturePath2 = PAINTED_WATERFEATURE_LIST[j].GetWaterFeaturePath();

                            bool pathsMerged = MergeWaterFeaturePaths(waterFeaturePath1, ref waterFeaturePath2);

                            if (pathsMerged)
                            {
                                PAINTED_WATERFEATURE_LIST[i].SetWaterFeaturePath(waterFeaturePath2);

                                // merge the other data from WATERFEATURE_LIST[j]
                                // with the data from WATERFEATURE_LIST[i];
                                dstWaterFeature = PAINTED_WATERFEATURE_LIST[i];

                                MergeWaterFeatureData(ref dstWaterFeature, PAINTED_WATERFEATURE_LIST[j]);

                                PAINTED_WATERFEATURE_LIST[i] = dstWaterFeature;

                                PAINTED_WATERFEATURE_LIST[j].IsRemoved = true;
                            }
                        }
                    }
                }
            }

            for (int i = PAINTED_WATERFEATURE_LIST.Count - 1; i >= 0; i--)
            {
                if (PAINTED_WATERFEATURE_LIST[i].IsRemoved)
                {
                    PAINTED_WATERFEATURE_LIST.RemoveAt(i);
                }
            }

            return dstWaterFeature;
        }

        internal static void ConstructWaterFeaturePaintObjects(MapPaintedWaterFeature waterFeature)
        {
            // TODO: is this combined effect actually needed?
            SKPathEffect JitterEffect = SKPathEffect.CreateDiscrete(waterFeature.WaterFeaturePathSegmentLength,
                waterFeature.WaterFeaturePathVariance,
                waterFeature.WaterFeatureVarianceSeed);

            // the COMBINED_EFFECT combines jitter and rounding
            SKPathEffect CombinedEffect = SKPathEffect.CreateCompose(ROUND_EFFECT, JitterEffect);

#pragma warning disable CS8629 // Nullable value type may be null.
            SKShader colorShader = SKShader.CreateColor(Extensions.ToSKColor((Color)waterFeature.WaterFeatureColor));

            // TODO: should water have a texture applied?
            //SKBitmap shaderBitmap = Extensions.ToSKBitmap(MapDrawingMethods.GetNoisyBitmap(MapPaintMethods.WATER_BRUSH_SIZE, MapPaintMethods.WATER_BRUSH_SIZE));
            //SKShader bitmapShader = SKShader.CreateBitmap(shaderBitmap);
            //SKShader combinedShader = SKShader.CreateCompose(colorShader, bitmapShader, SKBlendMode.SrcOver);

            waterFeature.WaterFeatureBackgroundPaint = new()
            {
                Style = SKPaintStyle.Fill,
                Shader = colorShader,
                BlendMode = SKBlendMode.Src,
                Color = Extensions.ToSKColor(Color.FromArgb((int)waterFeature.WaterFeatureColorOpacity, (Color)waterFeature.WaterFeatureColor)),
                //PathEffect = CombinedEffect,
                IsAntialias = true,
            };

            waterFeature.WaterFeatureShorelinePaint = new()
            {
                Style = SKPaintStyle.Stroke,
                BlendMode = SKBlendMode.Src,
                Color = Extensions.ToSKColor(Color.FromArgb((int)waterFeature.WaterFeatureShorelineColorOpacity, (Color)waterFeature.WaterFeatureShorelineColor)),
                StrokeWidth = 3,
                IsAntialias = true,
                //PathEffect = CombinedEffect
            };

            waterFeature.ShallowWaterPaint = new()
            {
                Style = SKPaintStyle.Stroke,
                BlendMode = SKBlendMode.SrcATop,
                StrokeWidth = 2,
                IsAntialias = true
            };

#pragma warning restore CS8629 // Nullable value type may be null.
        }

        internal static void MergeWaterFeatureData(ref MapPaintedWaterFeature mapWaterFeature1, MapPaintedWaterFeature mapWaterFeature2)
        {
            // attributes from mapWaterFeature2 overwrite the mapWaterFeature attributes
            if (mapWaterFeature2.WaterFeatureType != mapWaterFeature1.WaterFeatureType)
            {
                // if the water features aren't the same type, then set the type to Other
                mapWaterFeature1.WaterFeatureType = WaterFeatureTypeEnum.Other;
            }

            mapWaterFeature1.WaterFeatureColor = mapWaterFeature2.WaterFeatureColor;
            mapWaterFeature1.WaterFeatureColorOpacity = mapWaterFeature2.WaterFeatureColorOpacity;
            mapWaterFeature1.WaterFeaturePathSegmentLength = mapWaterFeature2.WaterFeaturePathSegmentLength;
            mapWaterFeature1.WaterFeaturePathVariance = mapWaterFeature2.WaterFeaturePathVariance;
            mapWaterFeature1.WaterFeatureVarianceSeed = mapWaterFeature2.WaterFeatureVarianceSeed;

            mapWaterFeature1.WaterFeatureBackgroundPaint = mapWaterFeature2.WaterFeatureBackgroundPaint;
            mapWaterFeature1.WaterFeatureShorelinePaint = mapWaterFeature2.WaterFeatureShorelinePaint;
            mapWaterFeature1.ShallowWaterPaint = mapWaterFeature2.ShallowWaterPaint;

            // merge the color paths from Water Feature 2 into Water Feature 1
            foreach (SKPath p in mapWaterFeature2.GetWaterFeatureColorPathList())
            {
                mapWaterFeature1.AddWaterFeatureColorPath(p);
            }
        }

        internal static bool MergeWaterFeaturePaths(SKPath waterFeaturePath1, ref SKPath waterFeaturePath2)
        {
            if (waterFeaturePath1 == null || waterFeaturePath2 == null) { return false; }

            // merge two sets of paths from two water features; if the paths overlap, then the second
            // set of paths is modified to include the first set (the second set becomes the union
            // of the two original sets)
            bool pathsMerged = false;

            if (waterFeaturePath1 != null && waterFeaturePath2 != null && waterFeaturePath2.PointCount > 0)
            {
                // get the intersection between the paths
                SKPath intersectionPath = waterFeaturePath2.Op(waterFeaturePath1, SKPathOp.Intersect);

                // if the intersection path isn't null or empty, then merge the paths
                if (intersectionPath != null && intersectionPath.PointCount > 0)
                {
                    // calculate the union between the water feature paths
                    SKPath unionPath = waterFeaturePath2.Op(waterFeaturePath1, SKPathOp.Union);

                    if (unionPath != null && unionPath.PointCount > 0)
                    {
                        pathsMerged = true;
                        waterFeaturePath2.Dispose();
                        waterFeaturePath2 = new SKPath(unionPath)
                        {
                            FillType = SKPathFillType.Winding
                        };

                        unionPath.Dispose();
                    }
                }
            }

            return pathsMerged;
        }

        internal static void DrawAllWaterFeatures(MapCreatorMap map)
        {
            // DO NOT CLEAR THE CANVAS BEFORE DRAWING WATER FEATURES!!!
            for (int i = 0; i < PAINTED_WATERFEATURE_LIST.Count; i++)
            {
                DrawWaterFeature(map, PAINTED_WATERFEATURE_LIST[i]);
            }
        }

        internal static void DrawWaterFeature(MapCreatorMap map, MapPaintedWaterFeature waterFeature)
        {
            if (waterFeature == null) return;

            SKCanvas? waterDrawingCanvas = MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER);
            SKRectI recti = new(0, 0, map.MapWidth, map.MapHeight);

            if (waterDrawingCanvas != null)
            {
                using SKRegion waterDrawingRegion = new();
                waterDrawingRegion.SetRect(recti);

                using SKRegion waterPathRegion = new(waterDrawingRegion);

                // if the outer path of the landform intersects the water layer path,
                // clip the water feature drawing to the outer path of the landform
                // MapBuilder.LANDLAYER_OUTLINE_PATH is the outer path of the landform
                // waterFeature.GetWaterFeaturePath() returns the path of the water feature

                List<MapLandformType2> landformList = LandformType2Methods.LANDFORM_LIST;

                for (int i = 0; i < landformList.Count; i++)
                {
                    SKPath landformOutlinePath = landformList[i].LandformPath;
                    SKPath waterFeaturePath = waterFeature.GetWaterFeaturePath();

                    if (landformOutlinePath != null && landformOutlinePath.PointCount > 0 && waterFeaturePath != null && waterFeaturePath.PointCount > 0)
                    {
                        SKPath diffPath = landformOutlinePath.Op(waterFeaturePath, SKPathOp.Intersect);

                        if (diffPath != null && diffPath.PointCount > 0)
                        {
                            diffPath.Dispose();

                            waterPathRegion.SetPath(landformOutlinePath);

                            waterDrawingCanvas.Save();
                            waterDrawingCanvas.ClipRegion(waterPathRegion);

                            DrawWaterFeatureWithGradient(map, waterFeature);

                            waterDrawingCanvas.Restore();
                        }
                    }
                }
            }

            if (waterFeature.IsSelected)
            {
                // draw an outline around the water feature to show that it is selected
                waterFeature.GetWaterFeaturePath().GetTightBounds(out SKRect boundRect);
                using SKPath boundsPath = new();
                boundsPath.AddRect(boundRect);

                MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER)?.DrawPath(boundsPath, WATERFEATURE_SELECT_PAINT);
            }
        }

        internal static void DrawWaterFeatureWithGradient(MapCreatorMap map, MapPaintedWaterFeature waterFeature)
        {
            //
            // draw waterFeature with gradient
            //
            SKCanvas? waterCanvas = MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER);

            if (waterCanvas == null) { return; }

            if (waterFeature.GetWaterFeaturePath().PointCount == 0)
            {
                return;
            }

#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

            Color backColor = (Color)waterFeature.WaterFeatureColor;

            // fill the water feature base path with color
            waterCanvas?.DrawPath(waterFeature.GetWaterFeaturePath(), waterFeature.WaterFeatureBackgroundPaint);

            // draw the water feature border
            waterCanvas?.DrawPath(waterFeature.GetWaterFeaturePath(), waterFeature.WaterFeatureShorelinePaint);

            using SKPath innerGradientPath = new();
            // create the first inner gradient path

            waterFeature.WaterFeatureShorelinePaint.GetFillPath(waterFeature.GetWaterFeaturePath(), innerGradientPath);

            // draw the shallow water gradients
            byte alpha = 100;
            float luminance = backColor.GetBrightness();
            luminance = Math.Min(1.5F * luminance, 1.0F);

            // create the color from the background color, but with 150% luminosity and reduced alpha
            waterFeature.ShallowWaterPaint.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminance * 100F, alpha);

            // draw the 1st water gradient
            waterCanvas?.DrawPath(innerGradientPath, waterFeature.ShallowWaterPaint);

            using (SKPath innerGradientPath2 = new SKPath())
            {
                // create the 2nd inner gradient
                waterFeature.WaterFeatureShorelinePaint.GetFillPath(innerGradientPath, innerGradientPath2);

                // draw the 2nd shallow water gradient
                alpha = 50;

                luminance = backColor.GetBrightness();
                luminance = Math.Min(1.25F * luminance, 1.0F);

                // create the color from the background color, but with 125% luminosity and reduced alpha
                waterFeature.ShallowWaterPaint.Color = SKColor.FromHsl(backColor.GetHue(), backColor.GetSaturation() * 100F, luminance * 100F, alpha);
                waterCanvas?.DrawPath(innerGradientPath2, waterFeature.ShallowWaterPaint);
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        internal static void EraseWaterPath(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER)?.Clear();

            for (int i = 0; i < PAINTED_WATERFEATURE_LIST.Count; i++)
            {
                SKPath waterFeaturePath = PAINTED_WATERFEATURE_LIST[i].WaterFeaturePath;

                SKPath newPath = CalculateWaterFeaturePath(waterFeaturePath, true);

                PAINTED_WATERFEATURE_LIST[i].WaterFeaturePath = new(newPath);
            }

            DrawAllWaterFeatures(map);

            // when erasing, have to wait until all water features have been painted
            // to reset the eraser path
            WATER_LAYER_ERASER_PATH.Reset();
        }

        internal static void PaintLake(MapCreatorMap map)
        {
            SKPath newPath = CalculateWaterFeaturePath(WATER_LAYER_PATH, false);

            if (NEW_WATERFEATURE != null && newPath.PointCount > 0)
            {
                NEW_WATERFEATURE.WaterFeaturePath = new SKPath(newPath);
                DrawWaterFeature(map, NEW_WATERFEATURE);
            }
        }

        internal static void ColorWaterFeaturePaths(MapCreatorMap map)
        {
            SKCanvas? waterDrawingCanvas = MapBuilder.GetLayerCanvas(map, MapBuilder.WATERDRAWINGLAYER);

            SKPath ColorPath = new();
            foreach (Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>> colorPoints in WATER_LAYER_COLOR_PATHS)
            {
                ColorPath.Reset();

                foreach (Tuple<SKPoint, int, SKShader> sp in colorPoints.Item2)
                {
                    ColorPath.AddCircle(sp.Item1.X, sp.Item1.Y, sp.Item2);

                    WATER_COLOR_PAINT.Shader = sp.Item3;

                    SKRectI recti = new(0, 0, map.MapWidth, map.MapHeight);

                    if (waterDrawingCanvas != null)
                    {
                        using SKRegion waterDrawingRegion = new();
                        waterDrawingRegion.SetRect(recti);

                        using SKRegion waterPathRegion = new(waterDrawingRegion);

                        if (ColorPath.PointCount > 0)
                        {
                            // if the outer path of the water feature intersects the painted path,
                            // clip painting to the outer path of the water feature
                            for (int i = 0; i < PAINTED_WATERFEATURE_LIST.Count; i++)
                            {
                                SKPath waterOutlinePath = PAINTED_WATERFEATURE_LIST[i].GetWaterFeaturePath();

                                SKPath diffPath = waterOutlinePath.Op(ColorPath, SKPathOp.Difference);

                                if (diffPath != null && diffPath.PointCount > 0)
                                {
                                    diffPath.Dispose();

                                    waterPathRegion.SetPath(waterOutlinePath);

                                    waterDrawingCanvas.Save();
                                    waterDrawingCanvas.ClipRegion(waterPathRegion);
                                    waterDrawingCanvas.DrawPath(ColorPath, WATER_COLOR_PAINT);
                                    waterDrawingCanvas.Restore();

                                    PAINTED_WATERFEATURE_LIST[i].AddWaterFeatureColorPath(new(ColorPath));
                                }
                            }

                            // TODO: clipping to the river boundary path may not work; a contour path may need to be calculated for rivers
                            // (woks well enough for now?)
                            for (int i = 0; i < MAP_RIVER_LIST.Count; i++)
                            {
                                SKPath? riverOutlinePath = MAP_RIVER_LIST[i].RiverBoundaryPath;

                                if (riverOutlinePath != null && riverOutlinePath.PointCount > 0)
                                {
                                    SKPath diffPath = riverOutlinePath.Op(ColorPath, SKPathOp.Difference);

                                    if (diffPath != null && diffPath.PointCount > 0)
                                    {
                                        diffPath.Dispose();

                                        waterPathRegion.SetPath(riverOutlinePath);

                                        waterDrawingCanvas.Save();
                                        waterDrawingCanvas.ClipRegion(waterPathRegion);
                                        waterDrawingCanvas.DrawPath(ColorPath, WATER_COLOR_PAINT);
                                        waterDrawingCanvas.Restore();

                                        MAP_RIVER_LIST[i].RiverColorPaths.Add(new(ColorPath));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void EraseWaterFeatureColor(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.WATERDRAWINGLAYER)?.DrawPath(WATER_LAYER_COLOR_ERASER_PATH, WATER_COLOR_ERASER_PAINT);
        }

        internal static MapTexture? GetWaterTextureByName(string name)
        {
            return WATER_TEXTURE_LIST.Find(x => x.TextureName == name);
        }

        internal static MapPaintedWaterFeature GetNewWaterFeature(MapCreatorMap map)
        {
            NEW_WATERFEATURE = new()
            {
                ParentMap = map,
                WaterFeatureType = WaterFeatureTypeEnum.Other,
            };

            return NEW_WATERFEATURE;
        }

        internal static void AddNewPaintedWaterFeatureToWaterFeatureList()
        {
            if (NEW_WATERFEATURE != null)
            {
                NEW_WATERFEATURE.WaterFeatureName = "Water Feature " + PAINTED_WATERFEATURE_LIST.Count;
                AddPaintedWaterFeature(NEW_WATERFEATURE);
            }
        }

        internal static void ResetWaterFeaturesOnCanvas(MapCreatorMap map)
        {
            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(map, MapBuilder.WATERLAYER);

            // remove all existing water features from the water layer
            for (int i = waterLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (waterLayer.MapLayerComponents[i] is MapPaintedWaterFeature)
                {
                    waterLayer.MapLayerComponents.RemoveAt(i);
                }
            }

            // add current water features to the water layer
            foreach (MapPaintedWaterFeature f in PAINTED_WATERFEATURE_LIST)
            {
                if (!f.IsRemoved)
                {
                    waterLayer.MapLayerComponents.Add(f);
                }
            }

            foreach (MapRiver r in MAP_RIVER_LIST)
            {
                if (!r.IsRemoved)
                {
                    waterLayer.MapLayerComponents.Add(r);
                }
            }
        }

        internal static void AddCircleToWaterColorErasePath(float x, float y, int brushRadius)
        {
            WATER_LAYER_COLOR_ERASER_PATH.AddCircle(x, y, brushRadius);
        }

        internal static void ConstructRiverPaintObjects(MapRiver mapRiver)
        {
            if (mapRiver.RiverPaint != null) return;

            float strokeWidth = mapRiver.RiverWidth;

            SKShader colorShader = SKShader.CreateColor(Extensions.ToSKColor(mapRiver.RiverColor));

            MapTexture? riverTexture = GetWaterTextureByName("Gray Texture");
            
            SKShader combinedShader;

            if (riverTexture != null)
            {
                if (riverTexture.TextureBitmap == null)
                {
                    riverTexture.TextureBitmap = System.Drawing.Image.FromFile(riverTexture.TexturePath) as Bitmap;
                }

                SKBitmap bitmap = Extensions.ToSKBitmap(riverTexture.TextureBitmap);
                SKBitmap resizedSKBitmap = new((int)mapRiver.RiverWidth, (int)mapRiver.RiverWidth);
                
                bitmap.ScalePixels(resizedSKBitmap, SKFilterQuality.High);

                SKShader bitmapShader = SKShader.CreateBitmap(resizedSKBitmap, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                combinedShader = SKShader.CreateCompose(colorShader, bitmapShader, SKBlendMode.Modulate);
            }
            else
            {
                combinedShader = colorShader;
            }

            mapRiver.RiverPaint = new()
            {
                Color = Extensions.ToSKColor(mapRiver.RiverColor),
                StrokeWidth = strokeWidth,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
                Shader = combinedShader
            };

            mapRiver.RiverShorelinePaint = new()
            {
                StrokeWidth = strokeWidth,
                Style = SKPaintStyle.Stroke,
                BlendMode = SKBlendMode.Src,
                Color = Extensions.ToSKColor(Color.FromArgb(mapRiver.RiverShorelineColorOpacity, mapRiver.RiverShorelineColor)),
                IsAntialias = true,
            };

            // shallow water is a lighter shade of river color
            SKColor shallowWaterColor = Extensions.ToSKColor(mapRiver.RiverColor);

            shallowWaterColor.ToHsl(out float hue, out float saturation, out float luminance);
            luminance *= 1.1F;

            shallowWaterColor = SKColor.FromHsl(hue, saturation, luminance);

            mapRiver.RiverShallowWaterPaint = new()
            {
                Color = shallowWaterColor,
                StrokeWidth = strokeWidth / 1.5F,
                Style = SKPaintStyle.Stroke,
                BlendMode = SKBlendMode.SrcATop,
                IsAntialias = true
            };
        }

        internal static void SetSelectedRiverPoints(MapRiver mapRiver)
        {
            mapRiver.RiverPoints.Clear();
            mapRiver.RiverPoints = new(RIVER_POINT_LIST);
        }

        internal static void DrawRiver(MapCreatorMap map, MapRiver mapRiver, SKCanvas c)
        {
            SKRectI recti = new(0, 0, map.MapWidth, map.MapHeight);

            using SKRegion pathDrawingRegion = new();
            pathDrawingRegion.SetRect(recti);

            using SKRegion pathRegion = new(pathDrawingRegion);

            List<MapRiverPoint> distinctRiverPoints = mapRiver.RiverPoints.Distinct(new MapRiverPointComparer()).ToList();
            List<MapLandformType2> landformList = LandformType2Methods.LANDFORM_LIST;

            for (int i = 0; i < landformList.Count; i++)
            {
                SKPath landformOutlinePath = landformList[i].LandformPath;

                if (landformOutlinePath != null && landformOutlinePath.PointCount > 0 && mapRiver.RiverPoints.Count > 0 && mapRiver.RiverPaint != null)
                {
                    pathRegion.SetPath(landformOutlinePath);

                    c.Save();
                    c.ClipRegion(pathRegion);

                    // use multiple paths and multiple sets of parallel points and paint objects
                    // to draw lines as gradients to shade rivers

                    // shoreline
                    using SKPath shorelinePath = new();

                    List<MapRiverPoint> parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth * 1.1F, ParallelEnum.Below, mapRiver.RiverSourceFadeIn);

                    if (parallelPoints.Count > 2)
                    {
                        shorelinePath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                shorelinePath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    parallelPoints.Clear();
                    parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth * 1.1F, ParallelEnum.Above, mapRiver.RiverSourceFadeIn);

                    if (parallelPoints.Count > 2)
                    {
                        shorelinePath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                shorelinePath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    c.DrawPath(shorelinePath, mapRiver.RiverShorelinePaint);
                    mapRiver.RiverBoundaryPath?.Dispose();
                    mapRiver.RiverBoundaryPath = new(shorelinePath)
                    {
                        FillType = SKPathFillType.Winding,
                    };

                    parallelPoints.Clear();
                    parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth, ParallelEnum.Above, mapRiver.RiverSourceFadeIn);

                    // shallow water
                    using SKPath shallowWaterPath = new();

                    if (parallelPoints.Count > 2)
                    {
                        shallowWaterPath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                shallowWaterPath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    parallelPoints.Clear();
                    parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth, ParallelEnum.Below, mapRiver.RiverSourceFadeIn);

                    if (parallelPoints.Count > 2)
                    {
                        shallowWaterPath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                shallowWaterPath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    c.DrawPath(shallowWaterPath, mapRiver.RiverShallowWaterPaint);

                    // river
                    using SKPath riverPath = new();

                    parallelPoints.Clear();
                    parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth / 2.0F, ParallelEnum.Above, mapRiver.RiverSourceFadeIn);

                    if (parallelPoints.Count > 2)
                    {
                        riverPath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                riverPath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    parallelPoints.Clear();
                    parallelPoints = MapDrawingMethods.GetParallelRiverPoints(distinctRiverPoints, mapRiver.RiverPaint.StrokeWidth / 2.0F, ParallelEnum.Below, mapRiver.RiverSourceFadeIn);

                    if (parallelPoints.Count > 2)
                    {
                        riverPath.MoveTo(parallelPoints[0].RiverPoint);

                        for (int j = 0; j < parallelPoints.Count; j += 3)
                        {
                            if (j < parallelPoints.Count - 2)
                            {
                                riverPath.CubicTo(parallelPoints[j].RiverPoint, parallelPoints[j + 1].RiverPoint, parallelPoints[j + 2].RiverPoint);
                            }
                        }
                    }

                    c.DrawPath(riverPath, mapRiver.RiverPaint);

                    c.Restore();
                }
            }

            if (mapRiver.IsSelected)
            {
                if (mapRiver.RiverBoundaryPath != null)
                {
                    // draw an outline around the path to show that it is selected
                    mapRiver.RiverBoundaryPath.GetTightBounds(out SKRect boundRect);
                    using SKPath boundsPath = new();
                    boundsPath.AddRect(boundRect);

                    MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER)?.DrawPath(boundsPath, RIVER_SELECT_PAINT);
                }
            }

            if (mapRiver.ShowRiverPoints)
            {
                List<MapRiverPoint> controlPoints = GetRiverControlPoints(mapRiver);

                foreach (MapRiverPoint p in controlPoints)
                {
                    PaintControlPoint(map, p, 2.0F, RIVER_CONTROL_POINT_PAINT);
                }
            }
        }

        private static List<MapRiverPoint> GetRiverControlPoints(MapRiver mapRiver)
        {
            List<MapRiverPoint> mapRiverPoints = [];

            for (int i = 0; i < mapRiver.RiverPoints.Count - 10; i += 10)
            {
                mapRiverPoints.Add(mapRiver.RiverPoints[i]);
            }

            mapRiverPoints.Add(mapRiver.RiverPoints[mapRiver.RiverPoints.Count - 1]);

            return mapRiverPoints;
        }

        private static void PaintControlPoint(MapCreatorMap map, MapRiverPoint mapRiverPoint, float size, SKPaint paint)
        {
            if (mapRiverPoint != null)
            {
                using SKPath controlPointPath = new();

                controlPointPath.AddCircle(mapRiverPoint.RiverPoint.X, mapRiverPoint.RiverPoint.Y, size);

                MapBuilder.GetLayerCanvas(map, MapBuilder.WATERDRAWINGLAYER)?.DrawPath(controlPointPath, paint);
                MapBuilder.GetLayerCanvas(map, MapBuilder.WATERDRAWINGLAYER)?.DrawPath(controlPointPath, RIVER_CONTROL_POINT_OUTLINE_PAINT);
            }
        }

        internal static MapRiver ConstructNewRiver(MapCreatorMap map)
        {
            SKPath path = GenerateRiverBoundaryPath(NEW_RIVER.RiverPoints);

            NEW_RIVER.RiverBoundaryPath?.Dispose();
            NEW_RIVER.RiverBoundaryPath = new(path);

            path.Dispose();

            AddRiver(NEW_RIVER);

            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(map, MapBuilder.WATERLAYER);

            waterLayer.MapLayerComponents.Add(NEW_RIVER);

            return NEW_RIVER;
        }

        public static SKPath GenerateRiverBoundaryPath(List<MapRiverPoint> points)
        {
            SKPath path = new();

            path.MoveTo(points[0].RiverPoint);

            for (int j = 0; j < points.Count; j += 3)
            {
                if (j < points.Count - 2)
                {
                    path.CubicTo(points[j].RiverPoint, points[j + 1].RiverPoint, points[j + 2].RiverPoint);
                }
            }

            return path;
        }

        public static void AddRiver(MapRiver mapRiver)
        {
            bool found = false;
            for (int i = 0; i < MAP_RIVER_LIST.Count; i++)
            {
                if (MAP_RIVER_LIST[i].MapRiverGuid.ToString().Equals(mapRiver.MapRiverGuid.ToString()))
                {
                    MAP_RIVER_LIST[i] = mapRiver;
                    found = true;
                }
            }

            if (!found)
            {
                MAP_RIVER_LIST.Add(mapRiver);
            }
        }

        internal static void DrawAllRivers(MapCreatorMap map)
        {
            foreach (MapRiver r in MAP_RIVER_LIST)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                DrawRiver(map, r, MapBuilder.GetLayerCanvas(map, MapBuilder.WATERLAYER));
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
    }
}
