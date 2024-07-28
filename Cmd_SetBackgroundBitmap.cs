using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_SetBackgroundBitmap(MapCreatorMap map, SKBitmap bitmap) : IMapOperation
    {
        public MapCreatorMap Map { get; set; } = map;
        SKBitmap LayerBitmap { get; set; } = bitmap;
        MapBitmap? backgroundBitmap { get; set; }

        public void DoOperation()
        {
            MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BASELAYER);

            if (backgroundLayer.MapLayerComponents.Count() <= 1)
            {
                backgroundBitmap = new()
                {
                    MBitmap = LayerBitmap.Copy()
                };
                backgroundLayer.MapLayerComponents.Add(backgroundBitmap);
            }
        }

        public void UndoOperation()
        {
            MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BASELAYER);

            if (backgroundBitmap != null)
            {
                backgroundLayer.MapLayerComponents.Remove(backgroundBitmap);
            }

            // base layer is cleared to WHITE, not transparent or empty
            MapBuilder.GetLayerCanvas(Map, MapBuilder.BASELAYER).Clear(SKColors.White);
        }
    }
}
