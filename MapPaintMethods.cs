using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Point = System.Drawing.Point;

namespace MapCreator
{
    public class MapPaintMethods
    {
        private static MapBrush? SOFT_CIRCLE_BRUSH;
        private static MapBrush? HARD_CIRCLE_BRUSH;

        private static SKBitmap? CURSOR_OVERLAY_BITMAP = null;
        private static readonly SKPaint CURSOR_CIRCLE_PAINT = new();

        private static ColorPaintBrush SELECTED_BRUSH_TYPE = ColorPaintBrush.SoftBrush;

        public static Cursor? EYEDROPPER_CURSOR;

        public static readonly List<MapTexture> BACKGROUND_TEXTURE_LIST = [];

        public static readonly List<ApplicationIcon> APPLICATION_ICON_LIST = [];

        public static readonly List<MapBrush> BRUSH_LIST = [];
        public static readonly List<MapTheme> THEME_LIST = [];

        public static MapTheme? CURRENT_THEME = null;

        public static void PaintMap(ref MapCreatorMap map, DrawingModeEnum drawingMode)
        {
            //MapBuilder.ClearLayerCanvas(map, MapBuilder.LANDFORMLAYER);
            //MapBuilder.ClearLayerCanvas(map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerCanvas(map, MapBuilder.WATERLAYER);
            MapBuilder.ClearLayerCanvas(map, MapBuilder.PATHLOWERLAYER);
            MapBuilder.ClearLayerCanvas(map, MapBuilder.PATHUPPERLAYER);
            MapBuilder.ClearLayerCanvas(map, MapBuilder.SYMBOLLAYER);

            switch (drawingMode)
            {
                case DrawingModeEnum.OceanErase:
                    OceanPaintMethods.EraseOceanPath(map);
                    break;
                case DrawingModeEnum.LandPaint:
                    LandformType2Methods.PaintLandForm(map);
                    break;
                case DrawingModeEnum.LandErase:
                    LandformType2Methods.PaintLandForm(map);
                    break;
                case DrawingModeEnum.LandColorErase:
                    LandformType2Methods.EraseLandformColor(map);
                    break;
                case DrawingModeEnum.WaterPaint:
                    WaterFeatureMethods.PaintWaterPath(map);
                    break;
                case DrawingModeEnum.LakePaint:
                    WaterFeatureMethods.PaintLake(map);
                    break;
                case DrawingModeEnum.WaterColorErase:
                    WaterFeatureMethods.EraseWaterFeatureColor(map);
                    break;
            }

            LandformType2Methods.DrawAllType2Landforms(map);

            WaterFeatureMethods.MergeWaterFeatures();

            WaterFeatureMethods.DrawAllWaterFeatures(map);

            WaterFeatureMethods.DrawAllRivers(map);

            MapPathMethods.DrawAllPaths(map);

            SymbolMethods.DrawAllSymbols(map);
        }

        /**************************************************************************************************************************
        * UTILITY METHODS
        * ************************************************************************************************************************/

        internal static SKShader ConstructColorPaintShader(ColorPaintBrush colorPaintBrush, Color gradientColor, int brushOpacity, float brushRadius, float x, float y)
        {
            SKShader oceanShader = SKShader.CreateColor(Extensions.ToSKColor(gradientColor).WithAlpha((byte)brushOpacity));

            if (colorPaintBrush == ColorPaintBrush.SoftBrush)
            {
                oceanShader.Dispose();
                SKPoint gradientCenter = new(x, y);
                oceanShader = SKShader.CreateRadialGradient(gradientCenter, brushRadius, [Extensions.ToSKColor(gradientColor).WithAlpha((byte)brushOpacity), Extensions.ToSKColor(gradientColor).WithAlpha(0)], SKShaderTileMode.Clamp);
            }

            return oceanShader;
        }

        internal static SKBitmap? GetCursorOverlayBitmap()
        {
            return CURSOR_OVERLAY_BITMAP;
        }

        internal static SKBitmap SetCursorOverlayBitmap(SKBitmap skBitmap)
        {
            CURSOR_OVERLAY_BITMAP = skBitmap;
            return CURSOR_OVERLAY_BITMAP;
        }

        internal static SKPaint GetCursorCirclePaint()
        {
            return CURSOR_CIRCLE_PAINT;
        }

        internal static ColorPaintBrush GetSelectedColorBrushType()
        {
            return SELECTED_BRUSH_TYPE;
        }

        internal static void SetSelectedColorBrushType(ColorPaintBrush brush)
        {
            SELECTED_BRUSH_TYPE = brush;
        }

        public static SKPaint DeepCopyPaintObject(SKPaint p)
        {
            SKPaint newPaint = new()
            {
                BlendMode = p.BlendMode,
                Color = p.Color,
                ColorFilter = p.ColorFilter,
                FakeBoldText = p.FakeBoldText,
                FilterQuality = p.FilterQuality,
                HintingLevel = p.HintingLevel,
                ImageFilter = p.ImageFilter,
                IsAntialias = p.IsAntialias,
                IsAutohinted = p.IsAutohinted,
                IsDither = p.IsDither,
                IsEmbeddedBitmapText = p.IsEmbeddedBitmapText,
                IsLinearText = p.IsLinearText,
                IsStroke = p.IsStroke,
                LcdRenderText = p.LcdRenderText,
                MaskFilter = p.MaskFilter,
                PathEffect = p.PathEffect,
                Shader = p.Shader,
                StrokeCap = p.StrokeCap,
                StrokeJoin = p.StrokeJoin,
                StrokeMiter = p.StrokeMiter,
                StrokeWidth = p.StrokeWidth,
                Style = p.Style,
                SubpixelText = p.SubpixelText,
                TextAlign = p.TextAlign,
                TextEncoding = p.TextEncoding,
                TextScaleX = p.TextScaleX,
                TextSize = p.TextSize,
                TextSkewX = p.TextSkewX,
                Typeface = p.Typeface
            };

            return newPaint;
        }

        public static Color SelectColorFromDialog()
        {
            using ColorDialog colorDlg = new()
            {
                FullOpen = true,
                AllowFullOpen = true,
                AnyColor = true,
                SolidColorOnly = false
            };

            if (colorDlg.ShowDialog(new Form() { TopMost = true }) == DialogResult.OK)
            {
                return colorDlg.Color;
            }
            else
            {
                return Color.Empty;
            }
        }

        public static void DeselectAllMapComponents(MapComponent selectedComponent)
        {
            foreach(MapLandformType2 l in LandformType2Methods.LANDFORM_LIST)
            {
                if (selectedComponent != null && selectedComponent is MapLandformType2 landform && landform == l) continue;
                l.IsSelected = false;
            }

            foreach (MapPaintedWaterFeature w in WaterFeatureMethods.PAINTED_WATERFEATURE_LIST)
            {
                if (selectedComponent != null && selectedComponent is MapPaintedWaterFeature waterFeature && waterFeature == w) continue;
                w.IsSelected = false;
            }

            foreach (MapRiver r in WaterFeatureMethods.MAP_RIVER_LIST)
            {
                if (selectedComponent != null && selectedComponent is MapRiver river && river == r) continue;
                r.IsSelected = false;
            }

            foreach (MapPath p in MapPathMethods.GetMapPathList())
            {
                if (selectedComponent != null && selectedComponent is MapPath mapPath && mapPath == p) continue;
                p.IsSelected = false;
            }

            foreach (MapSymbol s in SymbolMethods.PlacedSymbolList)
            {
                if (selectedComponent != null && selectedComponent is MapSymbol mapSymbol && mapSymbol == s) continue;
                s.SetIsSelected(false);
            }

            foreach (MapLabel l in MapLabelMethods.MAP_LABELS)
            {
                if (selectedComponent != null && selectedComponent is MapLabel mapLabel && mapLabel == l) continue;
                l.IsSelected = false;
            }

            foreach (PlacedMapBox b in MapLabelMethods.MAP_BOXES)
            {
                if (selectedComponent != null && selectedComponent is PlacedMapBox mapBox && mapBox == b) continue;
                b.IsSelected = false;
            }

            foreach (MapRegion r in MapRegionMethods.MAP_REGION_LIST)
            {
                if (selectedComponent != null && selectedComponent is MapRegion region && region == r) continue;
                r.IsSelected = false;
            }
        }

        /**************************************************************************************************************************
        * MAP INITIALIZATION METHODS
        * ************************************************************************************************************************/

        public static void ConstructPaintObjectsAndShaders()
        {
            OceanPaintMethods.ConstructOceanPaintObjects();

            LandformType2Methods.ConstructLandPaintObjects();

            MapPathMethods.ConstructMapPathPaintObjects();
            SymbolMethods.ConstructMapSymbolPaintObjects();

            CURSOR_CIRCLE_PAINT.Color = SKColors.Black;
            CURSOR_CIRCLE_PAINT.StrokeWidth = 1;
            CURSOR_CIRCLE_PAINT.Style = SKPaintStyle.Stroke;
            CURSOR_CIRCLE_PAINT.PathEffect = SKPathEffect.CreateDash([5F, 5F], 10F);

            SOFT_CIRCLE_BRUSH = BRUSH_LIST.Find(x => x.BrushName == "White Circle Gradient");

            if (SOFT_CIRCLE_BRUSH != null)
            {
                // load the bitmap from the path
                SOFT_CIRCLE_BRUSH.BrushBitmap ??= new Bitmap(SOFT_CIRCLE_BRUSH.BrushPath);
            }

            HARD_CIRCLE_BRUSH = BRUSH_LIST.Find(x => x.BrushName == "White Hard Circle");

            if (HARD_CIRCLE_BRUSH != null)
            {
                HARD_CIRCLE_BRUSH.BrushBitmap ??= new Bitmap(HARD_CIRCLE_BRUSH.BrushPath);
            }
        }

        /**************************************************************************************************************************
        * BACKGROUND METHODS
        * ************************************************************************************************************************/
        internal static void AddBackgroundTexture(MapTexture t)
        {
            BACKGROUND_TEXTURE_LIST.Add(t);
        }

        internal static List<MapTexture> GetBackgroundTextureList()
        {
            return BACKGROUND_TEXTURE_LIST;
        }

        /**************************************************************************************************************************
        * LANDFORM METHODS
        * ************************************************************************************************************************/

        internal static MapLandformType2? SelectLandformAtPoint(Point mapClickPoint)
        {
            MapLandformType2? selectedLandform = null;
            List<MapLandformType2> landforms = LandformType2Methods.LANDFORM_LIST;

            for (int i = 0; i < landforms.Count; i++)
            {
                MapLandformType2 mapLandform = landforms[i];
                SKPath boundaryPath = mapLandform.LandformPath;

                if (boundaryPath != null && boundaryPath.PointCount > 0)
                {
                    if (boundaryPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                    {
                        mapLandform.IsSelected = !mapLandform.IsSelected;

                        if (mapLandform.IsSelected)
                        {
                            selectedLandform = mapLandform;
                        }
                        break;
                    }
                }
            }

#pragma warning disable CS8604 // Possible null reference argument.
            DeselectAllMapComponents(selectedLandform);
#pragma warning restore CS8604 // Possible null reference argument.

            return selectedLandform;
        }

        /**************************************************************************************************************************
        * WATER FEATURE METHODS
        * ************************************************************************************************************************/
        internal static WaterFeature? SelectWaterFeatureAtPoint(Point mapClickPoint)
        {
            MapPaintedWaterFeature? selectedPaintedWaterFeature = null;

            List<MapPaintedWaterFeature> waterFeatures = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST;
            for (int i = 0; i < waterFeatures.Count; i++)
            {
                MapPaintedWaterFeature mapWaterFeature = waterFeatures[i];
                SKPath boundaryPath = mapWaterFeature.GetWaterFeaturePath();

                if (boundaryPath != null && boundaryPath.PointCount > 0)
                {
                    if (boundaryPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                    {
                        mapWaterFeature.IsSelected = !mapWaterFeature.IsSelected;

                        if (mapWaterFeature.IsSelected)
                        {
                            selectedPaintedWaterFeature = mapWaterFeature;
                        }
                        break;
                    }
                }
            }

            if (selectedPaintedWaterFeature != null)
            {
                DeselectAllMapComponents(selectedPaintedWaterFeature);
                return selectedPaintedWaterFeature;
            }
            else
            {
                MapRiver? selectedRiver = null;

                List<MapRiver> rivers = WaterFeatureMethods.MAP_RIVER_LIST;
                for (int i = 0; i < rivers.Count; i++)
                {
                    MapRiver river = rivers[i];
                    SKPath? boundaryPath = river.RiverBoundaryPath;

                    if (boundaryPath != null && boundaryPath.PointCount > 0)
                    {
                        if (boundaryPath.Contains(mapClickPoint.X, mapClickPoint.Y))
                        {
                            river.IsSelected = !river.IsSelected;

                            if (river.IsSelected)
                            {
                                selectedRiver = river;
                            }
                            break;
                        }
                    }
                }

                if (selectedRiver != null)
                {
                    DeselectAllMapComponents(selectedRiver);
                    return selectedRiver;
                }
            }

            return null;
        }


        /**************************************************************************************************************************
        * PATH METHODS
        * ************************************************************************************************************************/


        /**************************************************************************************************************************
        * THEME METHODS
        * ************************************************************************************************************************/

        /**************************************************************************************************************************
        * OVERLAY METHODS
        * ************************************************************************************************************************/

        public static void PaintVignette(SKCanvas canvas, SKRect bounds, Color mapVignetteColor, int vignetteStrength)
        {
            SKColor gradientColor = (mapVignetteColor).ToSKColor();

            int tenthLeftRight = (int)(bounds.Width / 5);
            int tenthTopBottom = (int)(bounds.Height / 5);

            using SKShader linGradLR = SKShader.CreateLinearGradient(new SKPoint(0, bounds.Height / 2), new SKPoint(tenthLeftRight / 2, bounds.Height / 2), [gradientColor.WithAlpha((byte)vignetteStrength), SKColors.Transparent], SKShaderTileMode.Clamp);
            using SKShader linGradTB = SKShader.CreateLinearGradient(new SKPoint(bounds.Width / 2, 0), new SKPoint(bounds.Width / 2, tenthTopBottom), [gradientColor.WithAlpha((byte)vignetteStrength), SKColors.Transparent], SKShaderTileMode.Clamp);
            using SKShader linGradRL = SKShader.CreateLinearGradient(new SKPoint(bounds.Width, bounds.Height / 2), new SKPoint(bounds.Width - tenthLeftRight, bounds.Height / 2), [gradientColor.WithAlpha((byte)vignetteStrength), SKColors.Transparent], SKShaderTileMode.Clamp);
            using SKShader linGradBT = SKShader.CreateLinearGradient(new SKPoint(bounds.Width / 2, bounds.Height), new SKPoint(bounds.Width / 2, bounds.Height - tenthTopBottom), [gradientColor.WithAlpha((byte)vignetteStrength), SKColors.Transparent], SKShaderTileMode.Clamp);

            using SKPaint paint = new()
            {
                Shader = linGradLR,
                IsAntialias = true,
                Color = gradientColor,
            };
         
            canvas.Clear();
            SKRect rect = new(0, 0, tenthLeftRight, bounds.Height);
            canvas.DrawRect(rect, paint);

            paint.Shader = linGradTB;
            rect = new(0, 0, bounds.Width, tenthTopBottom);
            canvas.DrawRect(rect, paint);

            paint.Shader = linGradRL;
            rect = new(bounds.Width, 0, bounds.Width - tenthLeftRight, bounds.Height);
            canvas.DrawRect(rect, paint);

            paint.Shader = linGradBT;
            rect = new(0, bounds.Height - tenthTopBottom, bounds.Width, bounds.Height);
            canvas.DrawRect(rect, paint);
            
        }
    }
}
