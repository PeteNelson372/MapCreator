using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_SetLayerBitmap(MapCreatorMap map, int layerIndex, SKBitmap bitmap) : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = map;
        public int LayerIndex { get; set; } = layerIndex;
        SKBitmap? LayerBitmap { get; set; } = bitmap;

        public void DoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT && LayerBitmap != null)
            {
                MapBuilder.SetLayerBitMap(Map, LayerIndex, LayerBitmap);
            }
        }

        public void UndoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT)
            {
                MapBuilder.ClearLayerBitmap(Map, LayerIndex);
            }
        }
    }
}
