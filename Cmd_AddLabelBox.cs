namespace MapCreator
{
    internal class Cmd_AddLabelBox(MapCreatorMap map, PlacedMapBox mapBox) : IMapOperation
    {
        private MapCreatorMap Map = map;
        private PlacedMapBox MapBox = mapBox;

        public void DoOperation()
        {
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER).MapLayerComponents.Add(MapBox);
            MapLabelMethods.MAP_BOXES.Add(MapBox);
        }

        public void UndoOperation()
        {
            MapLabelMethods.MAP_BOXES.Remove(MapBox);
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER).MapLayerComponents.Remove(MapBox);

            MapBuilder.ClearLayerCanvas(Map, MapBuilder.BOXLAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.BOXLAYER);
        }
    }
}
