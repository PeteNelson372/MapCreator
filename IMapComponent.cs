using SkiaSharp;

namespace MapCreator
{
    public interface IMapComponent
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public void Render(SKCanvas canvas);
    }
}
