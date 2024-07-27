namespace MapCreator
{
    internal class Cmd_DeleteMapRegion(MapCreatorMap map, MapRegion mapRegion) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapRegion NewMapRegion = mapRegion;

        public void DoOperation()
        {
            MapRegionMethods.MAP_REGION_LIST.Remove(NewMapRegion);

            for (int i = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.REGIONLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(Map, MapBuilder.REGIONLAYER).MapLayerComponents[i] is MapRegion r)
                {
                    if (r.RegionGuid.ToString() == NewMapRegion.RegionGuid.ToString())
                    {
                        MapBuilder.GetMapLayerByIndex(Map, MapBuilder.REGIONLAYER).MapLayerComponents.RemoveAt(i);
                        break;
                    }
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.REGIONLAYER).Clear();
        }

        public void UndoOperation()
        {
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.REGIONLAYER).MapLayerComponents.Add(NewMapRegion);
            MapRegionMethods.MAP_REGION_LIST.Add(NewMapRegion);
        }
    }
}
