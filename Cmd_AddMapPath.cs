using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_AddMapPath(MapCreatorMap map, SKPoint mapPathPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly SKPoint MapPathPoint = mapPathPoint;

        private MapPath? newMapPath = null;
        private int pathLayerIndex = -1;

        public void DoOperation()
        {
            if (newMapPath != null && (pathLayerIndex == MapBuilder.PATHLOWERLAYER || pathLayerIndex == MapBuilder.PATHUPPERLAYER))
            {
                newMapPath.IsSelected = false;

                MapPathMethods.GetMapPathList().Add(newMapPath);

                MapLayer pathLayer = MapBuilder.GetMapLayerByIndex(Map, pathLayerIndex);

                pathLayer.MapLayerComponents.Add(newMapPath);
            }
            else
            {
                MapPathMethods.GetPathPointList().Add(new MapPathPoint(MapPathPoint));
                if (MapPathMethods.GetPathPointList().Count > 0)
                {
                    MapPathMethods.SetSelectedMapPathPoints(MapPathMethods.GetNewPath());
                }

                MapPathMethods.ConstructNewMapPath(Map);

                newMapPath = MapPathMethods.GetNewPath();
            }
        }

        public void UndoOperation()
        {
            if (newMapPath != null)
            {
                List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
                mapPathList.Remove(newMapPath);

                // remove the selected path from the lower pathLayer
                MapLayer pathLayerLower = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHLOWERLAYER);

                for (int j = pathLayerLower.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (pathLayerLower.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == newMapPath.MapPathGuid.ToString())
                    {
                        pathLayerLower.MapLayerComponents.RemoveAt(j);
                        pathLayerIndex = MapBuilder.PATHLOWERLAYER;
                        break;
                    }
                }

                // remove the selected path from the upper pathLayer
                MapLayer pathLayerUpper = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHUPPERLAYER);

                for (int j = pathLayerUpper.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (pathLayerUpper.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == newMapPath.MapPathGuid.ToString())
                    {
                        pathLayerUpper.MapLayerComponents.RemoveAt(j);
                        pathLayerIndex = MapBuilder.PATHUPPERLAYER;
                        break;
                    }
                }
            }

        }
    }
}
