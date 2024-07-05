namespace MapCreator
{
    internal class Cmd_DeleteMapPath(MapCreatorMap map, MapPath mapPath) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapPath SelectedMapPath = mapPath;
        private int pathLayerIndex = -1;

        public void DoOperation()
        {
            List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
            mapPathList.Remove(SelectedMapPath);

            // remove the selected path from the lower pathLayer
            MapLayer pathLayerLower = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHLOWERLAYER);

            for (int j = pathLayerLower.MapLayerComponents.Count - 1; j > 0; j--)
            {
                if (pathLayerLower.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == SelectedMapPath.MapPathGuid.ToString())
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
                if (pathLayerUpper.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == SelectedMapPath.MapPathGuid.ToString())
                {
                    pathLayerUpper.MapLayerComponents.RemoveAt(j);
                    pathLayerIndex = MapBuilder.PATHUPPERLAYER;
                    break;
                }
            }
        }

        public void UndoOperation()
        {
            if (pathLayerIndex == MapBuilder.PATHLOWERLAYER || pathLayerIndex == MapBuilder.PATHUPPERLAYER)
            {
                SelectedMapPath.IsSelected = false;

                MapPathMethods.GetMapPathList().Add(SelectedMapPath);

                MapLayer pathLayer = MapBuilder.GetMapLayerByIndex(Map, pathLayerIndex);

                pathLayer.MapLayerComponents.Add(SelectedMapPath);
            }
        }
    }
}
