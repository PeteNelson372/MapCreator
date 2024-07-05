using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ColorOcean : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private readonly Guid OceanPathGuid = Guid.NewGuid();

        private readonly List<Tuple<SKPoint, int, SKShader>> StoredPoints = [];

        private SKPath ColorPath = new();

        public Cmd_ColorOcean(MapCreatorMap map)
        {
            Map = map;
        }

        public void AddCircle(float x, float y, int brushRadius, SKShader shader)
        {
            StoredPoints.Add(new Tuple<SKPoint, int, SKShader>(new SKPoint(x, y), brushRadius, shader));
            ColorPath.AddCircle(x, y, brushRadius);

            OceanPaintMethods.OCEAN_PAINT.Shader = shader;

            MapBuilder.GetLayerCanvas(Map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(ColorPath, OceanPaintMethods.OCEAN_PAINT);
        }

        public void DoOperation()
        {
            ColorPath.Reset();
            foreach (Tuple<SKPoint, int, SKShader> sp in StoredPoints)
            {
                ColorPath.AddCircle(sp.Item1.X, sp.Item1.Y, sp.Item2);

                OceanPaintMethods.OCEAN_PAINT.Shader = sp.Item3;

                MapBuilder.GetLayerCanvas(Map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(ColorPath, OceanPaintMethods.OCEAN_PAINT);
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.OCEANDRAWINGLAYER)?.Clear();
            OceanPaintMethods.OCEAN_COLOR_PATHS.Add(new Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>(OceanPathGuid, StoredPoints));
            OceanPaintMethods.ColorOceanPaths(Map);
        }

        public void UndoOperation()
        {
            for (int i = OceanPaintMethods.OCEAN_COLOR_PATHS.Count - 1; i >= 0; i--)
            {
                if (OceanPaintMethods.OCEAN_COLOR_PATHS[i].Item1.ToString() == OceanPathGuid.ToString())
                {
                    OceanPaintMethods.OCEAN_COLOR_PATHS.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.OCEANDRAWINGLAYER)?.Clear();
            OceanPaintMethods.ColorOceanPaths(Map);
        }
    }
}
