using DelaunatorSharp;
using SkiaSharp;

namespace MapCreator
{
    public class GeneratedMapData
    {
        public float SeaLevel { get; set; } = 0.5F;
        public int MapWidth { get; set; } = 0;
        public int MapHeight { get; set; } = 0;
        public int GridSize {  get; set; } = 0;
        public List<IPoint> MapPoints { get; set; } = [];
        public Delaunator? MapDelaunator { get; set; }
        public float NoiseScale { get; set; } = 0.1F;
        public float InterpolationWeight { get; set; } = 0.5F;
        public string DistanceFunction { get; set; } = string.Empty;
        public List<Tuple<int, float, VoronoiCell>> CellsWithHeight { get; set; } = [];
        public Bitmap? OriginalBitmap { get; set; }
        public Bitmap? ScaledBitmap { get; set; }
        public Bitmap? RotatedScaledBitmap { get; set; }
        public SKPath? LandformContourPath { get; set; }
    }
}
