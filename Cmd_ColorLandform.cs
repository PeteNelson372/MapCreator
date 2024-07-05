using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ColorLandform : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private readonly Guid LandColorPathGuid = Guid.NewGuid();

        private readonly List<Tuple<SKPoint, int, SKShader>> StoredPoints = [];

        private SKPath ColorPath = new();

        public Cmd_ColorLandform(MapCreatorMap map)
        {
            Map = map;
        }

        public void AddCircle(float x, float y, int brushRadius, SKShader shader)
        {
            StoredPoints.Add(new Tuple<SKPoint, int, SKShader>(new SKPoint(x, y), brushRadius, shader));
            ColorPath.AddCircle(x, y, brushRadius);

            LandformType2Methods.LAND_COLOR_PAINT.Shader = shader;

            SKCanvas? landDrawingCanvas = MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);
            SKRectI recti = new(0, 0, Map.MapWidth, Map.MapHeight);

            if (landDrawingCanvas != null)
            {
                using SKRegion landDrawingRegion = new();
                landDrawingRegion.SetRect(recti);

                using SKRegion landPathRegion = new(landDrawingRegion);

                if (ColorPath.PointCount > 0)
                {
                    // if the outer path of the landform intersects the painted path,
                    // clip painting to the outer path of the landform
                    // LandformPath is the outer path of the landform

                    for (int i = 0; i < LandformType2Methods.LANDFORM_LIST.Count; i++)
                    {
                        SKPath landformOutlinePath = LandformType2Methods.LANDFORM_LIST[i].LandformPath;

                        using SKPath diffPath = landformOutlinePath.Op(ColorPath, SKPathOp.Difference);

                        if (diffPath != null && diffPath.PointCount > 0)
                        {
                            landPathRegion.SetPath(landformOutlinePath);

                            landDrawingCanvas.Save();
                            landDrawingCanvas.ClipRegion(landPathRegion);
                            landDrawingCanvas.DrawPath(ColorPath, LandformType2Methods.LAND_COLOR_PAINT);
                            landDrawingCanvas.Restore();
                        }
                    }
                }
            }
        }

        public void DoOperation()
        {
            MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER)?.Clear();
            LandformType2Methods.LAND_LAYER_COLOR_PATHS.Add(new Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>(LandColorPathGuid, StoredPoints));
            LandformType2Methods.ColorLandformPaths(Map);

            ColorPath.Reset();
        }

        public void UndoOperation()
        {
            for (int i = LandformType2Methods.LAND_LAYER_COLOR_PATHS.Count - 1; i >= 0; i--)
            {
                if (LandformType2Methods.LAND_LAYER_COLOR_PATHS[i].Item1.ToString() == LandColorPathGuid.ToString())
                {
                    LandformType2Methods.LAND_LAYER_COLOR_PATHS.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER)?.Clear();
            LandformType2Methods.ColorLandformPaths(Map);
        }
    }
}
