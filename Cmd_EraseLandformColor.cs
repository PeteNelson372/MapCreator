using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_EraseLandformColor : IMapOperation
    {
        private MapCreatorMap Map;
        private SKPath StoredErasePath = new();

        public Cmd_EraseLandformColor(MapCreatorMap map)
        {
            Map = map;

            LandformType2Methods.LAND_LAYER_COLOR_ERASER_PATH.Reset();
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            LandformType2Methods.LAND_LAYER_COLOR_ERASER_PATH.AddCircle(x, y, brushRadius);
            LandformType2Methods.EraseLandformColor(Map);
        }

        public void DoOperation()
        {
            if (StoredErasePath.PointCount > 0)
            {
                LandformType2Methods.LAND_LAYER_COLOR_ERASER_PATH = new(StoredErasePath);
                StoredErasePath.Reset();
            }
        }

        public void UndoOperation()
        {
            StoredErasePath = new(LandformType2Methods.LAND_LAYER_COLOR_ERASER_PATH);
            LandformType2Methods.LAND_LAYER_COLOR_ERASER_PATH.Reset();
            LandformType2Methods.ColorLandformPaths(Map);
        }
    }
}
