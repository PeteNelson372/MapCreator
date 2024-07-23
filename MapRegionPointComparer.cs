namespace MapCreator
{
    public class MapRegionPointComparer : IEqualityComparer<MapRegionPoint>
    {
        public bool Equals(MapRegionPoint? p1, MapRegionPoint? p2)
        {
            if (p1 == null ||  p2 == null) return false;
            return p1.RegionPoint.X == p2.RegionPoint.X && p1.RegionPoint.Y == p2.RegionPoint.Y;
        }

        public int GetHashCode(MapRegionPoint obj)
        {
            // don't want points compared by hashcode, so return 0, forcing comarison through Equals method
            return 0;
        }
    }
}
