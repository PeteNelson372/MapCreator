using DelaunatorSharp;
using SkiaSharp;

namespace MapCreator
{
    internal class GeneratedMapData
    {
        public float SeaLevel { get; set; } = 0.5F;
        public int MapWidth { get; set; } = 0;
        public int MapHeight { get; set; } = 0;
        public int GridSize {  get; set; } = 0;
        public List<IPoint> MapPoints { get; set; } = [];
        public Delaunator? MapDelaunator { get; set; }
        public List<Tuple<int, float, VoronoiCell>> CellsWithHeight { get; set; } = [];
        public Bitmap? OriginalBitmap { get; set; }
        public Bitmap? ScaledBitmap { get; set; }
        public Bitmap? RotatedScaledBitmap { get; set; }
        public SKPath? LandformContourPath { get; set; }
    }
}
