using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_RemoveMapPathPoint(ref MapPath selectedMapPath, ref MapPathPoint selectedPathPoint) : IMapOperation
    {
        private MapPath? mapPath = selectedMapPath;
        private MapPathPoint? pathPoint = selectedPathPoint;

        private MapPath? storedMapPath = null;

        public void DoOperation()
        {
            if (mapPath != null && pathPoint != null)
            {
                storedMapPath = mapPath;

                MapPathPoint? removePathPoint = MapPathMethods.GetMapPathPointById(mapPath, pathPoint.PointGuid);

                if (removePathPoint != null)
                {
                    mapPath.PathPoints.Remove(removePathPoint);
                }

                SKPath boundarypath = MapPathMethods.GenerateMapPathBoundaryPath(mapPath.PathPoints);
                mapPath.BoundaryPath?.Dispose();
                mapPath.BoundaryPath = new(boundarypath);
                boundarypath.Dispose();
            }
        }

        public void UndoOperation()
        {
            mapPath = storedMapPath;
        }
    }
}
