using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_AddMapRegion(MapCreatorMap map, MapRegion mapRegion) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapRegion NewMapRegion = mapRegion;

        public void DoOperation()
        {
            MapBuilder.GetMapLayerByIndex(Map, MapBuilder.REGIONLAYER).MapLayerComponents.Add(NewMapRegion);
            MapRegionMethods.MAP_REGION_LIST.Add(NewMapRegion);
        }

        public void UndoOperation()
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
    }
}
