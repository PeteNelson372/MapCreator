

using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ClearAllLandforms : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = null;
        public List<MapLandformType2> STORED_LANDFORM_LIST { get; set; } = [];
        public SKPath? STORED_LAND_LAYER_ERASER_PATH { get; set; } = null;
        public SKBitmap? STORED_LANDFORMLAYER_BITMAP { get; set; } = null;
        public SKBitmap? STORED_LANDCOASTLINELAYER_BITMAP { get; set; } = null;
        public SKBitmap? STORED_LANDDRAWINGLAYER_BITMAP { get; set; } = null;

        public void DoOperation()
        {
            if (Map != null)
            {
                LandformType2Methods.LANDFORM_LIST.Clear();
                LandformType2Methods.LAND_LAYER_ERASER_PATH.Reset();

                LandformType2Methods.ResetLandCanvases(Map);
            }
        }

        public void UndoOperation()
        {
            if (Map != null && STORED_LANDFORMLAYER_BITMAP != null && STORED_LANDCOASTLINELAYER_BITMAP != null && STORED_LANDDRAWINGLAYER_BITMAP != null)
            {
                LandformType2Methods.LANDFORM_LIST = new(STORED_LANDFORM_LIST);
                LandformType2Methods.LAND_LAYER_ERASER_PATH = new(STORED_LAND_LAYER_ERASER_PATH);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDFORMLAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDFORMLAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDFORMLAYER, STORED_LANDFORMLAYER_BITMAP);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDCOASTLINELAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDCOASTLINELAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDCOASTLINELAYER, STORED_LANDCOASTLINELAYER_BITMAP);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDDRAWINGLAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDDRAWINGLAYER, STORED_LANDDRAWINGLAYER_BITMAP);
            }
        }
    }
}
