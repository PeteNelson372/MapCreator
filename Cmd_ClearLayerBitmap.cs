using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ClearLayerBitmap : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = null;
        public int LayerIndex { get; set; } = -1;
        SKBitmap? LayerBitmap { get; set; } = null;

        public Cmd_ClearLayerBitmap(MapCreatorMap map, int layerIndex, SKBitmap bitmap)
        {
            Map = map;
            LayerIndex = layerIndex;
            LayerBitmap = bitmap;
        }

        public void DoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT)
            {
                MapBuilder.ClearLayerBitmap(Map, LayerIndex);
            }
        }

        public void UndoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT && LayerBitmap != null)
            {
                MapBuilder.SetLayerBitMap(Map, LayerIndex, LayerBitmap);
            }
        }
    }
}
