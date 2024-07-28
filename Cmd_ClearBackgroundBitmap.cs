using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ClearBackgroundBitmap(MapCreatorMap map, MapBitmap bitmap) : IMapOperation
    {
        public MapCreatorMap Map { get; set; } = map;
        MapBitmap LayerBitmap { get; set; } = bitmap;

        public void DoOperation()
        {
            MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BASELAYER);

            if (LayerBitmap != null)
            {
                backgroundLayer.MapLayerComponents.Remove(LayerBitmap);
            }

            // base layer is cleared to WHITE, not transparent or empty
            MapBuilder.GetLayerCanvas(Map, MapBuilder.BASELAYER).Clear(SKColors.White);
        }

        public void UndoOperation()
        {
            MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BASELAYER);

            if (backgroundLayer.MapLayerComponents.Count() <= 1)
            {
                backgroundLayer.MapLayerComponents.Add(LayerBitmap);
            }
        }
    }
}
