namespace MapCreator
{
    internal class Cmd_DeleteRiver(MapCreatorMap map, MapRiver river) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapRiver River = river;

        public void DoOperation()
        {
            List<MapRiver> riverList = WaterFeatureMethods.MAP_RIVER_LIST;

            riverList.Remove(River);

            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

            for (int j = waterLayer.MapLayerComponents.Count - 1; j > 0; j--)
            {
                if (waterLayer.MapLayerComponents[j] is MapRiver mr && mr.MapRiverGuid.ToString() == River.MapRiverGuid.ToString())
                {
                    waterLayer.MapLayerComponents.RemoveAt(j);
                }
            }
        }

        public void UndoOperation()
        {
            River.IsSelected = false;

            WaterFeatureMethods.MAP_RIVER_LIST.Add(River);

            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

            waterLayer.MapLayerComponents.Add(River);
        }
    }
}
