using SkiaSharp;

namespace MapCreator
{
    internal class OceanPaintMethods
    {
        public static List<Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>> OCEAN_COLOR_PATHS { get; set; } = [];

        public static SKPath OCEAN_LAYER_ERASER_PATH { get; set; } = new();

        public static int OCEAN_BRUSH_SIZE { get; set; } = 20;
        public static int OCEAN_ERASER_SIZE { get; set; } = 20;

        public static SKPaint OCEAN_ERASER_PAINT { get; set; } = new();
        public static SKPaint OCEAN_PAINT { get; set; } = new();

        internal static void ColorOceanPaths(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.Clear();

            SKPath ColorPath = new();
            foreach(Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>> colorPoints in OCEAN_COLOR_PATHS)
            {
                ColorPath.Reset();

                foreach (Tuple<SKPoint, int, SKShader> sp in colorPoints.Item2)
                {
                    ColorPath.AddCircle(sp.Item1.X, sp.Item1.Y, sp.Item2);

                    OCEAN_PAINT.Shader = sp.Item3;

                    MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(ColorPath, OCEAN_PAINT);
                }
            }
        }

        internal static void EraseOceanPath(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(OCEAN_LAYER_ERASER_PATH, OCEAN_ERASER_PAINT);
        }

        internal static void ConstructOceanPaintObjects()
        {
            OCEAN_ERASER_PAINT.Color = SKColor.Empty;
            OCEAN_ERASER_PAINT.Style = SKPaintStyle.Fill;
            OCEAN_ERASER_PAINT.BlendMode = SKBlendMode.Src;

            OCEAN_PAINT.Style = SKPaintStyle.Fill;
            OCEAN_PAINT.IsAntialias = true;
            OCEAN_PAINT.BlendMode = SKBlendMode.SrcOver;
        }
    }
}
