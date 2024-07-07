namespace MapCreator
{
    internal class Cmd_AddWindrose(MapCreatorMap map, MapWindrose windrose) : IMapOperation
    {
        private MapCreatorMap Map = map;
        private MapWindrose Windrose = windrose;

        public void DoOperation()
        {
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WINDROSELAYER).MapLayerComponents.Add(Windrose);
        }

        public void UndoOperation()
        {
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WINDROSELAYER).MapLayerComponents.Remove(Windrose);

            MapBuilder.ClearLayerCanvas(Map, MapBuilder.WINDROSELAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.WINDROSELAYER);
        }
    }
}
