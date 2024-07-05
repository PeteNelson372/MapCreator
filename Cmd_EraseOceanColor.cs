using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_EraseOceanColor : IMapOperation
    {
        private MapCreatorMap Map;
        private SKPath StoredErasePath = new();

        public Cmd_EraseOceanColor(MapCreatorMap map)
        {
            Map = map;

            OceanPaintMethods.OCEAN_LAYER_ERASER_PATH.Reset();
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            OceanPaintMethods.OCEAN_LAYER_ERASER_PATH.AddCircle(x, y, brushRadius);
        }

        public void DoOperation()
        {
            if (StoredErasePath.PointCount > 0)
            {
                OceanPaintMethods.OCEAN_LAYER_ERASER_PATH = new(StoredErasePath);
                StoredErasePath.Reset();
            }
        }

        public void UndoOperation()
        {
            StoredErasePath = new(OceanPaintMethods.OCEAN_LAYER_ERASER_PATH);
            OceanPaintMethods.OCEAN_LAYER_ERASER_PATH.Reset();
            OceanPaintMethods.ColorOceanPaths(Map);
        }
    }
}
