
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class Cmd_MapPathEdit(MapPath selectedPath, Point point) : IMapOperation
    {
        private readonly MapPath SelectedPath = selectedPath;
        private Point Point = point;
        private MapPathPoint? StoredPoint = null;

        public void DoOperation()
        {
            SelectedPath.IsSelected = true;
            SelectedPath.ShowPathPoints = true;

            MapPaintMethods.DeselectAllMapComponents(SelectedPath);

            MapPathPoint? mapPathPoint = MapPathMethods.GetSelectedMapPathPoint();
            if (mapPathPoint != null)
            {
                SKPoint mapPoint = Extensions.ToSKPoint(Point);

                if (SelectedPath != null)
                {
                    MapPathPoint? pathPoint = MapPathMethods.GetMapPathPointById(SelectedPath, mapPathPoint.PointGuid);
                    StoredPoint = pathPoint;

                    if (pathPoint != null)
                    {
                        pathPoint.MapPoint = mapPoint;
                    }

                    SKPath path = MapPathMethods.GenerateMapPathBoundaryPath(SelectedPath.PathPoints);
                    SelectedPath.BoundaryPath?.Dispose();
                    SelectedPath.BoundaryPath = new(path);
                    path.Dispose();
                }
            }
        }

        public void UndoOperation()
        {
            if (SelectedPath != null && StoredPoint != null)
            {
                MapPathPoint? pathPoint = MapPathMethods.GetMapPathPointById(SelectedPath, StoredPoint.PointGuid);

                if (pathPoint != null)
                {
                    pathPoint.MapPoint = StoredPoint.MapPoint;
                }

                SKPath path = MapPathMethods.GenerateMapPathBoundaryPath(SelectedPath.PathPoints);
                SelectedPath.BoundaryPath?.Dispose();
                SelectedPath.BoundaryPath = new(path);
                path.Dispose();
            }
        }
    }
}
