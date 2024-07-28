using SkiaSharp;

namespace MapCreator
{
    public interface IMapComponent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public void Render(SKCanvas canvas);
    }
}
