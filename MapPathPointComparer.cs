namespace MapCreator
{
    public class MapPathPointComparer : IEqualityComparer<MapPathPoint>
    {
        public bool Equals(MapPathPoint? p1, MapPathPoint? p2)
        {
            if (p1 == null ||  p2 == null) return false;
            return p1.MapPoint.X == p2.MapPoint.X && p1.MapPoint.Y == p2.MapPoint.Y;
        }

        public int GetHashCode(MapPathPoint obj)
        {
            // don't want points compared by hashcode, so return 0, forcing comarison through Equals method
            return 0;
        }
    }
}
