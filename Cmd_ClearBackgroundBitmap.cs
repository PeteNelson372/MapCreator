using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ClearBackgroundBitmap(MapCreatorMap map, SKBitmap bitmap) : IMapOperation
    {
        public MapCreatorMap Map { get; set; } = map;
        SKBitmap LayerBitmap { get; set; } = bitmap;

        public void DoOperation()
        {
            // base layer is cleared to WHITE, not transparent or empty
            MapBuilder.GetLayerCanvas(Map, MapBuilder.BASELAYER).Clear(SKColors.White);
        }

        public void UndoOperation()
        {
            SKCanvas layerCanvas = MapBuilder.GetLayerCanvas(Map, MapBuilder.BASELAYER);
            layerCanvas.DrawBitmap(LayerBitmap, 0, 0);
        }
    }
}
