using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_EraseLandform : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private List<Tuple<Guid, SKPath>> StoredPaths = [];

        public Cmd_EraseLandform(MapCreatorMap map)
        {
            Map = map;
            foreach (MapLandformType2 lft2 in LandformType2Methods.LANDFORM_LIST)
            {
                Tuple<Guid, SKPath> t = new(lft2.LandformGuid, new(lft2.LandformPath));
                StoredPaths.Add(t);
            }
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            LandformType2Methods.LAND_LAYER_ERASER_PATH.AddCircle(x, y, brushRadius);
        }

        public void DoOperation()
        {
            LandformType2Methods.EraseLandForm(Map);

            foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
            {
                lf.LandformContourPath.Dispose();
                LandformType2Methods.CreateType2LandformPaths(Map, lf);
            }

            LandformType2Methods.LAND_LAYER_ERASER_PATH.Reset();
        }

        public void UndoOperation()
        {
            if (StoredPaths.Count > 0)
            {
                foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
                {
                    foreach(Tuple<Guid, SKPath> t in StoredPaths)
                    {
                        if (t.Item1.ToString() == lf.LandformGuid.ToString())
                        {
                            lf.LandformPath.Dispose();
                            lf.LandformPath = new(t.Item2);
                            LandformType2Methods.CreateType2LandformPaths(Map, lf);
                        }
                    }
                }
            }

        }
    }
}
