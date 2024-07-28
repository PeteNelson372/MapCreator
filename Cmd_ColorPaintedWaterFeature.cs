using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ColorPaintedWaterFeature : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private readonly Guid WaterColorPathGuid = Guid.NewGuid();

        private readonly List<Tuple<SKPoint, int, SKShader>> StoredPoints = [];

        private SKPath ColorPath = new();

        public Cmd_ColorPaintedWaterFeature(MapCreatorMap map)
        {
            Map = map;
        }

        public void AddCircle(float x, float y, int brushRadius, SKShader shader)
        {
            StoredPoints.Add(new Tuple<SKPoint, int, SKShader>(new SKPoint(x, y), brushRadius, shader));
            ColorPath.AddCircle(x, y, brushRadius);

            WaterFeatureMethods.WATER_COLOR_PAINT.Shader = shader;

            SKCanvas? waterDrawingCanvas = MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER);
            SKRectI wrecti = new(0, 0, Map.MapWidth, Map.MapHeight);

            if (waterDrawingCanvas != null)
            {
                using SKRegion waterDrawingRegion = new();
                waterDrawingRegion.SetRect(wrecti);

                using SKRegion waterPathRegion = new(waterDrawingRegion);

                if (ColorPath.PointCount > 0)
                {
                    // if the outer path of the water feature intersects the painted path,
                    // clip painting to the outer path of the water feature
                    for (int i = 0; i < WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Count; i++)
                    {
                        SKPath waterOutlinePath = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST[i].GetWaterFeaturePath();

                        waterPathRegion.SetPath(waterOutlinePath);

                        waterDrawingCanvas.Save();
                        waterDrawingCanvas.ClipRegion(waterPathRegion);
                        waterDrawingCanvas.DrawPath(ColorPath, WaterFeatureMethods.WATER_COLOR_PAINT);
                        waterDrawingCanvas.Restore();

                        WaterFeatureMethods.PAINTED_WATERFEATURE_LIST[i].AddWaterFeatureColorPath(new(ColorPath));
                    }

                    for (int i = 0; i < WaterFeatureMethods.MAP_RIVER_LIST.Count; i++)
                    {
                        SKPath? riverOutlinePath = WaterFeatureMethods.MAP_RIVER_LIST[i].RiverBoundaryPath;

                        if (riverOutlinePath != null && riverOutlinePath.PointCount > 0)
                        {
                            waterPathRegion.SetPath(riverOutlinePath);

                            waterDrawingCanvas.Save();
                            waterDrawingCanvas.ClipRegion(waterPathRegion);
                            waterDrawingCanvas.DrawPath(ColorPath, WaterFeatureMethods.WATER_COLOR_PAINT);
                            waterDrawingCanvas.Restore();

                            WaterFeatureMethods.MAP_RIVER_LIST[i].RiverColorPaths.Add(new(ColorPath));

                        }
                    }
                }
            }


        }

        public void DoOperation()
        {
            MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER).Clear();
            WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.Add(new Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>(WaterColorPathGuid, StoredPoints));
            WaterFeatureMethods.ColorWaterFeaturePaths(Map);

            ColorPath.Reset();
        }

        public void UndoOperation()
        {
            for (int i = WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.Count - 1; i >= 0; i--)
            {
                if (WaterFeatureMethods.WATER_LAYER_COLOR_PATHS[i].Item1.ToString() == WaterColorPathGuid.ToString())
                {
                    WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER)?.Clear();
            WaterFeatureMethods.ColorWaterFeaturePaths(Map);
        }
    }
}
