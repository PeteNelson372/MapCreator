using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_FillMapWithLandform(MapCreatorMap map, MapLandformType2 filledLandform) : IMapOperation
    {
        MapCreatorMap Map = map;
        MapLandformType2 FilledLandform = filledLandform;

        public void DoOperation()
        {
            SKRect r = new(2, 2, Map.MapWidth - 2, Map.MapHeight - 2);
            FilledLandform.LandformPath.AddRect(r);

            LandformType2Methods.CreateType2LandformPaths(Map, FilledLandform);

            // add the landform to the LANDFORM_LIST
            LandformType2Methods.LANDFORM_LIST.Add(FilledLandform);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
        }

        public void UndoOperation()
        {
            LandformType2Methods.LANDFORM_LIST.Clear();

            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDDRAWINGLAYER);

            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
        }
    }
}
