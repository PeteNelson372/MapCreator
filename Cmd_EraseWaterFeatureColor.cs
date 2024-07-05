using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_EraseWaterFeatureColor : IMapOperation
    {
        private MapCreatorMap Map;
        private SKPath StoredErasePath = new();

        public Cmd_EraseWaterFeatureColor(MapCreatorMap map)
        {
            Map = map;

            WaterFeatureMethods.WATER_LAYER_COLOR_ERASER_PATH.Reset();
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            WaterFeatureMethods.WATER_LAYER_COLOR_ERASER_PATH.AddCircle(x, y, brushRadius);
            WaterFeatureMethods.EraseWaterFeatureColor(Map);
        }

        public void DoOperation()
        {
            if (StoredErasePath.PointCount > 0)
            {
                WaterFeatureMethods.WATER_LAYER_COLOR_ERASER_PATH = new(StoredErasePath);
                StoredErasePath.Reset();
            }
        }

        public void UndoOperation()
        {
            StoredErasePath = new(WaterFeatureMethods.WATER_LAYER_COLOR_ERASER_PATH);
            WaterFeatureMethods.WATER_LAYER_COLOR_ERASER_PATH.Reset();
            WaterFeatureMethods.ColorWaterFeaturePaths(Map);
        }
    }
}
