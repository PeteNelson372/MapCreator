namespace MapCreator
{
    internal class Cmd_AddLandform(MapCreatorMap map) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private List<MapLandformType2> STORED_LANDFORM_LIST { get; set; } = [];

        private MapLandformType2? STORED_LANDFORM { get; set; } = null;

        public void DoOperation()
        {
            if (STORED_LANDFORM != null)
            {
                LandformType2Methods.SELECTED_LANDFORM = STORED_LANDFORM;
            }

            STORED_LANDFORM_LIST = new(LandformType2Methods.LANDFORM_LIST);
            
            LandformType2Methods.AddSelectedLandFormToLandformList();
            LandformType2Methods.MergeLandforms();

            LandformType2Methods.SELECTED_LANDFORM = LandformType2Methods.LANDFORM_LIST.Last();

            STORED_LANDFORM = LandformType2Methods.SELECTED_LANDFORM;

            LandformType2Methods.CreateType2LandformPaths(Map, LandformType2Methods.SELECTED_LANDFORM);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
        }

        public void UndoOperation()
        {
            // have to create a new selected landform to completely clear the one being undone
            LandformType2Methods.GetNewSelectedLandform(Map);

            LandformType2Methods.LANDFORM_LIST.Clear();
            LandformType2Methods.LANDFORM_LIST = new(STORED_LANDFORM_LIST);

            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDDRAWINGLAYER);

            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
            STORED_LANDFORM_LIST.Clear();
        }
    }
}
