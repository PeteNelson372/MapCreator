using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Extensions = SkiaSharp.Views.Desktop.Extensions;
using Point = System.Drawing.Point;
using Clipper2Lib;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Color = System.Drawing.Color;

namespace MapCreator
{
    internal class MapDrawingMethods
    {
        const float PI_OVER_180 = (float)Math.PI / 180F;
        const float D180_OVER_PI = (float)((float)180.0F / Math.PI);

        enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows 
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        public static Bitmap CopyGraphicsContent(Graphics source, Rectangle rect)
        {
            Bitmap bmp = new(rect.Width, rect.Height);

            using (Graphics dest = Graphics.FromImage(bmp))
            {
                //dest.Clear(Color.White);
                IntPtr hdcSource = source.GetHdc();
                IntPtr sourceCDC = CreateCompatibleDC(hdcSource);
                IntPtr hdcDest = dest.GetHdc();
                IntPtr destCDC = CreateCompatibleDC(hdcDest);

                BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, sourceCDC, rect.X, rect.Y, TernaryRasterOperations.SRCCOPY);

                source.ReleaseHdc(hdcSource);
                dest.ReleaseHdc(hdcDest);
            }

            return bmp;
        }

        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public static bool CompareBitmaps(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) || (b2 == null)) return false;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }

        static readonly float[][] colorMatrixArray =
        {
            [1, 0, 0, 0, 0],
            [0, 1, 0, 0, 0],
            [0, 0, 1, 0, 0],
            [0, 0, 0, 0, 0],
            [0, 0, 0, 0, 1],
        };

        static ColorMatrix oceanTxMatrix = new ColorMatrix(colorMatrixArray);
        static ColorMatrix landTxMatrix = new ColorMatrix(colorMatrixArray);

        public static Rectangle GetRectangle(Point p1, Point p2)
        {
            int left = Math.Min(p1.X, p2.X);
            int right = Math.Max(p1.X, p2.X);
            int top = Math.Min(p1.Y, p2.Y);
            int bottom = Math.Max(p1.Y, p2.Y);
            int width = right - left;
            int height = bottom - top;
            return new Rectangle(left, top, width, height);
        }

        public static ImageAttributes ImageColorTransformAttributes(Color newColor)
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix TxMatrix = new ColorMatrix(colorMatrixArray)
            {
                Matrix40 = newColor.R / 255.0F,
                Matrix41 = newColor.G / 255.0F,
                Matrix42 = newColor.B / 255.0F,
                Matrix33 = newColor.A / 255.0F  //Matrix33 is alpha channel
            };

            attributes.SetColorMatrix(TxMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);

            return attributes;
        }

        public static SKPoint[] GetPoints(uint quantity, SKPoint p1, SKPoint p2)
        {
            var points = new SKPoint[quantity];

            double distance = DistanceBetween(p1, p2);

            points[0] = p1;
            points[quantity - 1] = p2;

            double tdelta = distance / quantity;

            for (int i = 1; i < quantity; i++)
            {
                //t = dt / d
                //(xt, yt) = (((1 - t) * x0 + t * x1), ((1 - t) * y0 + t * y1))
                double t = (i * tdelta) / distance;
                points[i].X = (int)Math.Ceiling((1 - t) * p1.X + t * p2.X);
                points[i].Y = (int)Math.Ceiling((1 - t) * p1.Y + t * p2.Y);
            }

            return points;
        }

        public static bool IsPointInsidePolygon(Point[] polygon, Point point)
        {
            var path = new GraphicsPath();
            path.AddPolygon(polygon);

            var region = new Region(path);
            return region.IsVisible(point);
        }

        public static Bitmap GetLakeBitmap(int width, int height)
        {
            float seed = (float)Random.Shared.NextDouble();

            SKBitmap b = new SKBitmap(width, height);
            SKCanvas skc = new SKCanvas(b);
            skc.Clear();

            using (SKPaint paint = new SKPaint())
            {
                SKRect tileRect = new SKRect(0, 0, width, height);

                paint.Style = SKPaintStyle.Fill;
                paint.Shader = SKShader.CreatePerlinNoiseTurbulence(0.01f, 0.01f, 2, seed);

                skc.DrawRect(tileRect, paint);
            }

            //Bitmap newB = Extensions.ToBitmap(b);
            Bitmap gsb = MakeGrayscale(Extensions.ToBitmap(b), 0.19F, true);

            //FlattenBitmapColors(ref gsb);
            MakeLakeBitmap(ref gsb);

            return gsb;
        }

        public static Bitmap GetNoisyBitmap(int width, int height)
        {
            float seed = (float)Random.Shared.NextDouble();

            SKBitmap b = new SKBitmap(width, height);
            SKCanvas skc = new SKCanvas(b);
            skc.Clear();

            using (SKPaint paint = new SKPaint())
            {
                SKRect tileRect = new SKRect(0, 0, width, height);

                paint.Style = SKPaintStyle.Fill;
                paint.Shader = SKShader.CreatePerlinNoiseTurbulence(0.04f, 0.04f, 4, seed);

                skc.DrawRect(tileRect, paint);
            }

            Bitmap gsb = MakeGrayscale(Extensions.ToBitmap(b), 0.19F, true);

            // "flatten" the bitmap to b/w
            //Bitmap gsbbw = gsb.Clone(new Rectangle(0, 0, width, height), PixelFormat.Format1bppIndexed);

            return gsb;
        }

        public static Bitmap MakeGrayscale(Bitmap original, float threshold, bool makeMonochrome)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                    [
                        [0.299f, 0.299f, 0.299f, 0, 0],
                        [0.587f, 0.587f, 0.587f, 0, 0],
                        [0.114f, 0.114f, 0.114f, 0, 0],
                        [0,      0,      0,      1, 0],
                        [0,      0,      0,      0, 1]
                    ]);

                //ColorMatrix colorMatrix = new ColorMatrix(
                //    [
                //        [1.5F,  1.5F,   1.5F,   0F, 0F],
                //        [1.5F,  1.5F,   1.5F,   0F, 0F],
                //        [1.5F,  1.5F,   1.5F,   0F, 0F],
                //        [0F,    0F,     0F,     1F, 0F],
                //        [-1F,   -1F,    -1F,    0F, 1F]
                //    ]);

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    if (makeMonochrome)
                    {
                        // set threshold to convert grayscale to monochrome
                        attributes.SetThreshold(threshold);
                    }

                    //set the color matrix attribute so colors from light gray to white are set to transparent
                    attributes.SetColorMatrix(colorMatrix);

                    Color lowerColor = Color.LightGray;
                    Color upperColor = Color.White;

                    attributes.SetColorKey(lowerColor, upperColor, ColorAdjustType.Default);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }


            return newBitmap;
        }

        public static void ChangeBitmapColors(ref Bitmap bitmap, Color oldColor, Color newColor)
        {
            if (bitmap == null)
            {
                return;
            }

            var lockedBitmap = new LockBitmap(bitmap);
            lockedBitmap.LockBits();

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    Color pixelColor = lockedBitmap.GetPixel(x, y);

                    if (pixelColor.ToArgb() == oldColor.ToArgb())
                    {
                        lockedBitmap.SetPixel(x, y, newColor);
                    }
                }
            }

            lockedBitmap.UnlockBits();
        }

        public static void MapCustomColorsToColorableBitmap(ref Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            var lockedBitmap = new LockBitmap(bitmap);
            lockedBitmap.LockBits();

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    Color pixelColor = lockedBitmap.GetPixel(x, y);

                    byte rValue = pixelColor.R;
                    byte gValue = pixelColor.G;
                    byte bValue = pixelColor.B;

                    if (rValue > 64 && gValue == 0 && bValue == 0)
                    {
                        float colorScale = rValue / 255F;
                        SKColor newColor = SymbolMethods.GetCustomColorAtIndex(0);

                        newColor.ToHsl(out float hue, out float saturation, out float brightness);
                        brightness *= colorScale;

                        Color scaledColor = SKColor.FromHsl(hue, saturation, brightness).ToDrawingColor();
                        lockedBitmap.SetPixel(x, y, scaledColor);
                    }
                    else if (gValue > 64 && rValue == 0 && bValue == 0)
                    {
                        float colorScale = gValue / 255F;
                        SKColor newColor = SymbolMethods.GetCustomColorAtIndex(1);

                        newColor.ToHsl(out float hue, out float saturation, out float brightness);
                        brightness *= colorScale;

                        Color scaledColor = SKColor.FromHsl(hue, saturation, brightness).ToDrawingColor();
                        lockedBitmap.SetPixel(x, y, scaledColor);
                    }
                    else if (bValue > 64 && rValue == 3 && gValue == 3)
                    {
                        float colorScale = bValue / 255F;
                        SKColor newColor = SymbolMethods.GetCustomColorAtIndex(2);

                        newColor.ToHsl(out float hue, out float saturation, out float brightness);
                        brightness *= colorScale;

                        Color scaledColor = SKColor.FromHsl(hue, saturation, brightness).ToDrawingColor();
                        lockedBitmap.SetPixel(x, y, scaledColor);
                    }
                }
            }

            lockedBitmap.UnlockBits();
        }

        public static List<Color> GetBitmapColors(Bitmap bitmap)
        {
            // return a list of all of the colors used in the bitmap
            List<Color> colors = [];

            if (bitmap != null)
            {
                var lockedBitmap = new LockBitmap(bitmap);
                lockedBitmap.LockBits();

                for (int y = 0; y < lockedBitmap.Height; y++)
                {
                    for (int x = 0; x < lockedBitmap.Width; x++)
                    {
                        Color pixelColor = lockedBitmap.GetPixel(x, y);

                        if (!colors.Contains(pixelColor))
                        {
                            colors.Add(pixelColor);
                        }
                    }
                }

                lockedBitmap.UnlockBits();
            }

            return colors;
        }

        public static bool IsGrayScaleImage(Bitmap bitmap)
        {
            bool IsGrayScaleImage = true;

            if (bitmap != null)
            {
                var lockedBitmap = new LockBitmap(bitmap);
                lockedBitmap.LockBits();

                for (int y = 0; y < lockedBitmap.Height; y++)
                {
                    for (int x = 0; x < lockedBitmap.Width; x++)
                    {
                        Color pixelColor = lockedBitmap.GetPixel(x, y);

                        if (pixelColor.R != pixelColor.G || pixelColor.G != pixelColor.B || pixelColor.R != pixelColor.B)
                        {
                            IsGrayScaleImage = false;
                            break;
                        }
                    }
                }

                lockedBitmap.UnlockBits();
            }

            return IsGrayScaleImage;
        }

        public static bool IsPaintableImage(Bitmap bitmap)
        {
            bool isPaintableImage = true;

            if (bitmap != null)
            {
                var lockedBitmap = new LockBitmap(bitmap);
                lockedBitmap.LockBits();

                for (int y = 0; y < lockedBitmap.Height; y++)
                {
                    for (int x = 0; x < lockedBitmap.Width; x++)
                    {
                        Color pixelColor = lockedBitmap.GetPixel(x, y);

                        byte rValue = pixelColor.R;
                        byte gValue = pixelColor.G;
                        byte bValue = pixelColor.B;

                        if (rValue > 64 && (gValue != 0 || bValue != 0))
                        {
                            continue;
                        }
                        else if (gValue > 64 && rValue == 0 && bValue == 0)
                        {
                            continue;
                        }
                        else if (bValue > 64 && rValue == 3 && gValue == 3)
                        {
                            continue;
                        }
                        else
                        {
                            isPaintableImage = false;
                            break;
                        }
                    }
                }

                lockedBitmap.UnlockBits();
            }

            return isPaintableImage;
        }

        public static void FlattenBitmapColors(ref Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            var lockedBitmap = new LockBitmap(bitmap);
            lockedBitmap.LockBits();

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    if (lockedBitmap.GetPixel(x, y) == Color.Transparent
                        || lockedBitmap.GetPixel(x, y) == Color.White)
                    {
                        lockedBitmap.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        lockedBitmap.SetPixel(x, y, Color.White);
                    }
                }
            }

            lockedBitmap.UnlockBits();
        }

        public static void MakeLakeBitmap(ref Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return;
            }

            var lockedBitmap = new LockBitmap(bitmap);
            lockedBitmap.LockBits();

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    Color c = lockedBitmap.GetPixel(x, y);

                    if (c.R < 128 && c.G < 128 && c.B < 128)
                    {
                        lockedBitmap.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        lockedBitmap.SetPixel(x, y, Color.CadetBlue);
                    }
                }
            }

            lockedBitmap.UnlockBits();
        }

        public static bool ColorIsNear(Color color, Color checkColor, int tolerance)
        {
            int rValue = color.R;
            int gValue = color.G;
            int bValue = color.B;

            int minR = checkColor.R - tolerance;
            int maxR = checkColor.R + tolerance;

            int minG = checkColor.G - tolerance;
            int maxG = checkColor.G + tolerance;

            int minB = checkColor.B - tolerance;
            int maxB = checkColor.B + tolerance;

            if (rValue < minR || rValue > maxR)
            {
                return false;
            }

            if (gValue < minG || gValue > maxG)
            {
                return false;
            }

            if (bValue < minB || bValue > maxB)
            {
                return false;
            }

            return true;

        }

        public static Color RGBtoRGBA(Color rgbColor)
        {
            float r = rgbColor.R;
            float g = rgbColor.G;
            float b = rgbColor.B;

            float minValue = Math.Min(Math.Min(r, g), b);
            float scaledValue = (255.0F - minValue) / 255.0F;

            byte A = (byte) scaledValue;
            byte R = (byte)((r - minValue) / scaledValue);
            byte G = (byte)((g - minValue) / scaledValue);
            byte B = (byte)((b - minValue) / scaledValue);

            Color argbColor = Color.FromArgb(A, R, G, B);

            return argbColor;
        }

        public static List<SKPoint> GenerateRandomPolygonPoints(PointF center, float averageRadius, float irregularity, float spikiness, int numVertices, int seed)
        {
            Random random = new(seed);

            // implemented from python code at: https://stackoverflow.com/questions/8997099/algorithm-to-generate-random-2d-polygon
            irregularity *= (float)(2.0F * Math.PI / numVertices);
            spikiness *= averageRadius;

            List<float> angleSteps = RandomAngleSteps(random, numVertices, irregularity);

            List<SKPoint> points = [];

            float angle = RandomNextFloat(random, 0F, (float)(2.0F * Math.PI));

            for (int i = 0; i < numVertices; i++)
            {
                float radius = Clip(RandomGaussianNextFloat(random, averageRadius, spikiness), 0, 2.0F * averageRadius);

                SKPoint point = new(center.X + radius * (float)Math.Cos(angle), center.Y + radius * (float)Math.Sin(angle));

                points.Add(point);

                angle += angleSteps[i];
            }

            return points;
        }

        public static List<float> RandomAngleSteps(Random random, int steps, float irregularity)
        {
            List<float> angles = [];
            float lower = (float)(2.0F * Math.PI / steps) - irregularity;
            float upper = (float)(2.0F * Math.PI / steps) + irregularity;

            float cumulativeSum = 0;

            for (int i = 0; i < steps; i++)
            {
                float angle = RandomUniformNextFloat(random, lower, upper);
                angles.Add(angle);
                cumulativeSum += angle;
            }

            // normalize the steps so that point 0 and point n+1 are the same
            cumulativeSum /= (float)(2.0F * Math.PI);

            for (int i = 0; i < steps; i++)
            {
                angles[i] /= cumulativeSum;
            }

            return angles;
        }

        public static float RandomNextFloat(Random random, float minValue, float maxValue)
        {
            double sample = random.NextDouble();
            return (float)((float)(maxValue * sample) + (minValue * (1.0D - sample)));
        }

        public static float RandomGaussianNextFloat(Random random, float minValue, float maxValue)
        {
            float sample = RandomGaussian(random);
            return (maxValue * sample) + (minValue * (1.0F - sample));
        }

        public static float RandomGaussian(Random random, double mu = 0, double sigma = 1)
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return (float)rand_normal;
        }

        public static float RandomUniformNextFloat(Random random, float minValue, float maxValue)
        {
            return RandomUniform(random);
        }

        public static float RandomUniform(Random random, float a = 0.0F, float b = 1.0F)
        {
            return (float)(a + (b - a) * random.NextDouble());
        }

        public static float Clip(float value, float lower, float upper)
        {
            return Math.Min(upper, Math.Max(value, lower));
        }

        public static int ClosestNumber(int n, int m)
        {
            // find the quotient
            int q = n / m;

            // 1st possible closest number
            int n1 = m * q;

            // 2nd possible closest number
            int n2 = (n * m) > 0 ? (m * (q + 1)) : (m * (q - 1));

            // if true, then n1 is the required closest number
            if (Math.Abs(n - n1) < Math.Abs(n - n2))
                return n1;

            // else n2 is the required closest number
            return n2;
        }

        public static SKPoint PointOnCircle(float radius, float angleInDegrees, SKPoint origin)
        {
            float angleInRadians = angleInDegrees * PI_OVER_180;

            // Convert from degrees to radians via multiplication by PI/180        
            float x = (float)(radius * Math.Cos(angleInRadians)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInRadians)) + origin.Y;

            return new SKPoint(x, y);
        }

        public static SKPoint PointOnCircleRadians(float radius, float angleInRadians, SKPoint origin)
        {
            float x = (float)(radius * Math.Cos(angleInRadians)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInRadians)) + origin.Y;

            return new SKPoint(x, y);
        }

        public static bool PointInCircle(float radius, SKPoint origin, SKPoint pointToTest)
        {
            float square_dist = SKPoint.DistanceSquared(origin, pointToTest);
            return square_dist < (radius * radius);
        }

        public static float CalculateLineAngle(SKPoint start, SKPoint end)
        {
            float xDiff = end.X - start.X;
            float yDiff = end.Y - start.Y;

            return (float)((float)Math.Atan2(yDiff, xDiff) * D180_OVER_PI);
        }

        public static float CalculateLineAngleRadians(SKPoint start, SKPoint end)
        {
            float xDiff = end.X - start.X;
            float yDiff = end.Y - start.Y;

            return (float)Math.Atan2(yDiff, xDiff);
        }

        public static float DistanceBetween(SKPoint from, SKPoint to)
        {
            return SKPoint.Distance(from, to);
        }

        public static SKPath GenerateRandomLakePath(SKPoint location, float radius)
        {
            int minBubbles = 4;
            int maxBubbles = 8;

            int minBubbleRadius = 4;
            int maxBubbleRadius = Math.Max(minBubbleRadius, (int)radius / 2);

            SKPath path = new()
            {
                FillType = SKPathFillType.Winding,
            };

            path.AddCircle(location.X, location.Y, radius);

            int numBubbles = Random.Shared.Next(minBubbles, maxBubbles);

            for (int i = 0; i < numBubbles; i++)
            {
                int bubbleRadius = Random.Shared.Next(minBubbleRadius, maxBubbleRadius);

                float bubbleAngle = Random.Shared.Next(0, 359);
                SKPoint bubbleLocation = PointOnCircle(radius, bubbleAngle, location);

                path.AddCircle(bubbleLocation.X, bubbleLocation.Y, bubbleRadius);
            }

            return path;
        }

        internal static float signedMod(float a, float n)
        {
            return (float)(a - Math.Floor(a / n) * n);
        }

        internal static float GetAngleDifference(float angle1, float angle2)
        {
            //float a1 = (float)((angle1 % 360.0F) * (Math.PI / 180.0F));
            //float a2 = (float)((angle2 % 360.0F) * (Math.PI / 180.0F));

            // angles are in degrees and returned difference is in degrees
            //float diffRadians = (float)Math.Atan2(Math.Sin(a1 - a2), Math.Cos(a1 - a2));

            //float diffDegrees = (float)(diffRadians * (180.0F / Math.PI));

            float a = angle1 - angle2;
            float diffDegrees = signedMod(a + 180, 360) - 180;

            return diffDegrees;
        }

        internal static SKPoint GetPointAtDistanceOnAngle(SKPoint p, float distance, float angle)
        {
            // angle is in degrees, convert it to radians
            float a = (float)(angle * (Math.PI / 180));
            float x = (float)(p.X + distance * Math.Cos(a));
            float y = (float)(p.Y + distance * Math.Sin(a));

            return new SKPoint(x, y);
        }

        private const double SELECTION_FUZZINESS = 3;

        internal static bool LineContainsPoint(SKPoint pointToCheck, SKPoint lineStartPoint, SKPoint lineEndPoint)
        {
            SKPoint leftPoint;
            SKPoint rightPoint;

            // Normalize start/end to left right to make the offset calc simpler.
            if (lineStartPoint.X <= lineEndPoint.X)
            {
                leftPoint = lineStartPoint;
                rightPoint = lineEndPoint;
            }
            else
            {
                leftPoint = lineEndPoint;
                rightPoint = lineStartPoint;
            }

            // If point is out of bounds, no need to do further checks.                  
            if (pointToCheck.X + SELECTION_FUZZINESS < leftPoint.X || rightPoint.X < pointToCheck.X - SELECTION_FUZZINESS)
            {
                return false;
            }
            else if (pointToCheck.Y + SELECTION_FUZZINESS < Math.Min(leftPoint.Y, rightPoint.Y) || Math.Max(leftPoint.Y, rightPoint.Y) < pointToCheck.Y - SELECTION_FUZZINESS)
            {
                return false;
            }

            double deltaX = rightPoint.X - leftPoint.X;
            double deltaY = rightPoint.Y - leftPoint.Y;

            // If the line is straight, the earlier boundary check is enough to determine that the point is on the line.
            // Also prevents division by zero exceptions.
            if (deltaX == 0 || deltaY == 0)
            {
                return true;
            }

            double slope = deltaY / deltaX;
            double offset = leftPoint.Y - leftPoint.X * slope;
            double calculatedY = pointToCheck.X * slope + offset;

            // Check calculated Y matches the points Y coord with some easing.
            bool lineContains = pointToCheck.Y - SELECTION_FUZZINESS <= calculatedY && calculatedY <= pointToCheck.Y + SELECTION_FUZZINESS;

            return lineContains;
        }

        internal static List<SKPoint> GetParallelSKPoints(List<SKPoint> points, float distance, ParallelEnum location)
        {
            PathD clipperPath = [];

            foreach (SKPoint point in points)
            {
                clipperPath.Add(new PointD(point.X, point.Y));
            }

            PathsD clipperPaths = [];
            PathsD inflatedPaths = [];

            clipperPaths.Add(clipperPath);

            float d = (location == ParallelEnum.Below) ? -distance : distance;

            // offset polyline
            inflatedPaths = Clipper.InflatePaths(clipperPaths, d, JoinType.Square, EndType.Polygon);

            if (inflatedPaths.Count > 0)
            {
                PathD inflatedPathD = inflatedPaths.First();

                List<SKPoint> inflatedPath = [];

                foreach (PointD p in inflatedPathD)
                {
                    inflatedPath.Add(new SKPoint((float)p.x, (float)p.y));
                }

                return inflatedPath;
            }
            else
            {
                return points;
            }
        }

        internal static List<MapRegionPoint> GetParallelRegionPoints(List<MapRegionPoint> points, float distance, ParallelEnum location)
        {
            PathD clipperPath = [];

            foreach (MapRegionPoint point in points)
            {
                clipperPath.Add(new PointD(point.RegionPoint.X, point.RegionPoint.Y));
            }

            PathsD clipperPaths = [];
            PathsD inflatedPaths = [];

            clipperPaths.Add(clipperPath);

            float d = (location == ParallelEnum.Below) ? -distance : distance;

            // offset polyline
            inflatedPaths = Clipper.InflatePaths(clipperPaths, d, JoinType.Square, EndType.Polygon);

            if (inflatedPaths.Count > 0)
            {
                PathD inflatedPathD = inflatedPaths.First();

                List<MapRegionPoint> inflatedPath = [];

                foreach (PointD p in inflatedPathD)
                {
                    inflatedPath.Add(new MapRegionPoint(new SKPoint((float)p.x, (float)p.y)));
                }

                return inflatedPath;
            }
            else
            {
                return points;
            }
        }

        internal static List<MapPathPoint> GetParallelPathPoints(List<MapPathPoint> points, float distance, ParallelEnum location)
        {
            List<MapPathPoint> parallelPoints = [];

            for (int i = 0; i < points.Count - 1; i += 2)
            {
                float lineAngle = CalculateLineAngle(points[i].MapPoint, points[i + 1].MapPoint);

                float angle = (location == ParallelEnum.Below) ? 90 : -90;

                SKPoint p1 = PointOnCircle(distance, lineAngle + angle, points[i].MapPoint);
                SKPoint p2 = PointOnCircle(distance, lineAngle + angle, points[i + 1].MapPoint);

                parallelPoints.Add(new MapPathPoint(p1));
                parallelPoints.Add(new MapPathPoint(p2));
            }

            return parallelPoints;
        }

        internal static List<MapRiverPoint> GetParallelRiverPoints(List<MapRiverPoint> points, float distance, ParallelEnum location, bool fromStartingPoint)
        {
            List<MapRiverPoint> parallelPoints = [];

            float calcDistance = distance;

            for (int i = 0; i < points.Count - 1; i += 2)
            {
                float lineAngle = CalculateLineAngle(points[i].RiverPoint, points[i + 1].RiverPoint);

                float angle = (location == ParallelEnum.Below) ? 90 : -90;

                if (fromStartingPoint)
                {
                    calcDistance = distance * ((float)i / (float)points.Count);
                }

                SKPoint p1 = PointOnCircle(calcDistance, lineAngle + angle, points[i].RiverPoint);
                SKPoint p2 = PointOnCircle(calcDistance, lineAngle + angle, points[i + 1].RiverPoint);

                parallelPoints.Add(new MapRiverPoint(p1));
                parallelPoints.Add(new MapRiverPoint(p2));
            }

            return parallelPoints;
        }

        internal static SKBitmap RotateBitmap(SKBitmap bmp, float angle, bool flipX)
        {
            Bitmap bitmap = Extensions.ToBitmap(bmp);
            double radianAngle = angle / 180.0 * Math.PI;
            double cosA = Math.Abs(Math.Cos(radianAngle));
            double sinA = Math.Abs(Math.Sin(radianAngle));

            int newWidth = (int)Math.Ceiling(cosA * bitmap.Width + sinA * bitmap.Height);
            int newHeight = (int)Math.Ceiling(cosA * bitmap.Height + sinA * bitmap.Width);

            Bitmap rotatedBitmap = new(newWidth, newHeight);
            rotatedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using Graphics g = Graphics.FromImage(rotatedBitmap);
            g.TranslateTransform(rotatedBitmap.Width / 2, rotatedBitmap.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-bitmap.Width / 2, -bitmap.Height / 2);
            g.DrawImage(bitmap, new Point(0, 0));

            if (flipX)
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            return Extensions.ToSKBitmap(bitmap);
        }

        internal static bool BitmapIsTransparent(SKBitmap b)
        {
            bool isTransparent = true;

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            foreach (SKColor c in b.Pixels)
            {
                if (c.Alpha != 0)
                {
                    isTransparent = false;
                    break;
                }
                else if (c.Red != 0 || c.Green != 0 || c.Blue != 0)
                {
                    isTransparent = false;
                    break;
                }
            }

            return isTransparent;
        }

        internal static SKBitmap ScaleBitmap(SKBitmap bitmap, float scale)
        {
            int bitmapWidth = (int)Math.Round(bitmap.Width * scale);
            int bitmapHeight = (int)Math.Round(bitmap.Height * scale);

            SKBitmap resizedSKBitmap = new(bitmapWidth, bitmapHeight);
            bitmap.ScalePixels(resizedSKBitmap, SKFilterQuality.High);

            return resizedSKBitmap;
        }

        internal static SKBitmap[] SliceNinePatchBitmap(SKBitmap resizedBitmap, SKRectI center)
        {
            SKBitmap[] slicedBitmaps = new SKBitmap[9];

            slicedBitmaps[0] = new SKBitmap(center.Left, center.Top);
            using SKCanvas canvas_A = new(slicedBitmaps[0]);
            SKRect src_A = new(0, 0, center.Left, center.Top);
            SKRect dst_A = new(0, 0, slicedBitmaps[0].Width, slicedBitmaps[0].Height);
            canvas_A.DrawBitmap(resizedBitmap, src_A, dst_A);

            slicedBitmaps[1] = new SKBitmap(center.Right - center.Left, center.Top);
            using SKCanvas canvas_B = new(slicedBitmaps[1]);
            SKRect src_B = new(center.Left, 0, center.Right, center.Top);
            SKRect dst_B = new(0, 0, slicedBitmaps[1].Width, slicedBitmaps[1].Height);
            canvas_B.DrawBitmap(resizedBitmap, src_B, dst_B);

            slicedBitmaps[2] = new SKBitmap(resizedBitmap.Width - center.Right, center.Top);
            using SKCanvas canvas_C = new(slicedBitmaps[2]);
            SKRect src_C = new(center.Right, 0, resizedBitmap.Width, center.Top);
            SKRect dst_C = new(0, 0, slicedBitmaps[2].Width, slicedBitmaps[2].Height);
            canvas_C.DrawBitmap(resizedBitmap, src_C, dst_C);

            slicedBitmaps[3] = new SKBitmap(center.Left, center.Height);
            using SKCanvas canvas_D = new(slicedBitmaps[3]);
            SKRect src_D = new(0, center.Top, center.Left, center.Bottom);
            SKRect dst_D = new(0, 0, slicedBitmaps[3].Width, slicedBitmaps[3].Height);
            canvas_D.DrawBitmap(resizedBitmap, src_D, dst_D);

            slicedBitmaps[4] = new SKBitmap(center.Width, center.Height);
            using SKCanvas canvas_E = new(slicedBitmaps[4]);
            SKRect src_E = new(center.Left, center.Top, center.Right, center.Bottom);
            SKRect dst_E = new(0, 0, slicedBitmaps[4].Width, slicedBitmaps[4].Height);
            canvas_E.DrawBitmap(resizedBitmap, src_E, dst_E);

            slicedBitmaps[5] = new SKBitmap(resizedBitmap.Width - center.Right, center.Height);
            using SKCanvas canvas_F = new(slicedBitmaps[5]);
            SKRect src_F = new(center.Right, center.Top, resizedBitmap.Width, center.Bottom);
            SKRect dst_F = new(0, 0, slicedBitmaps[5].Width, slicedBitmaps[5].Height);
            canvas_F.DrawBitmap(resizedBitmap, src_F, dst_F);

            slicedBitmaps[6] = new SKBitmap(center.Left, resizedBitmap.Height - center.Bottom);
            using SKCanvas canvas_G = new(slicedBitmaps[6]);
            SKRect src_G = new(0, center.Bottom, center.Left, resizedBitmap.Height);
            SKRect dst_G = new(0, 0, slicedBitmaps[6].Width, slicedBitmaps[6].Height);
            canvas_G.DrawBitmap(resizedBitmap, src_G, dst_G);

            slicedBitmaps[7] = new SKBitmap(center.Width, resizedBitmap.Height - center.Bottom);
            using SKCanvas canvas_H = new(slicedBitmaps[7]);
            SKRect src_H = new(center.Left, center.Bottom, center.Right, resizedBitmap.Height);
            SKRect dst_H = new(0, 0, slicedBitmaps[7].Width, slicedBitmaps[7].Height);
            canvas_H.DrawBitmap(resizedBitmap, src_H, dst_H);

            slicedBitmaps[8] = new SKBitmap(resizedBitmap.Width - center.Right, resizedBitmap.Height - center.Bottom);
            using SKCanvas canvas_I = new(slicedBitmaps[8]);
            SKRect src_I = new(center.Right, center.Bottom, resizedBitmap.Width, resizedBitmap.Height);
            SKRect dst_I = new(0, 0, slicedBitmaps[8].Width, slicedBitmaps[8].Height);
            canvas_I.DrawBitmap(resizedBitmap, src_I, dst_I);

            return slicedBitmaps;
        }

        internal static List<SKPoint> GetPointsInCircle(SKPoint cursorPoint, int radius, int stepSize)
        {
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Argument must be positive.");
            }

            List<SKPoint> pointsInCircle = [];

            int minX = (int)Math.Max(0, cursorPoint.X - radius);
            int maxX = (int)(cursorPoint.X + radius);
            int minY = (int)Math.Max(0, cursorPoint.Y - radius);
            int maxY = (int)(cursorPoint.Y + radius);

            for (int i = minX; i <= maxX; i += stepSize)
            {
                for (int j = minY; j <= maxY; j += stepSize)
                {
                    SKPoint p = new(i, j);
                    if (PointInCircle(radius, cursorPoint, p))
                    {
                        pointsInCircle.Add(p);
                    }
                }
            }

            return pointsInCircle;
        }

        internal static float CalculatePolygonArea(List<SKPoint> polygonPoints)
        {
            if (polygonPoints.Count < 3)
            {
                return 0;
            }

            // use shoelace algorithm to calculate area of the polygon
            float area = 0;

            int j = polygonPoints.Count - 1;
            for (int i = 0; i < polygonPoints.Count; i++)
            {
                area += (polygonPoints[j].X + polygonPoints[i].X) * (polygonPoints[j].Y - polygonPoints[i].Y);
                j = i;  // j is previous vertex to i
            }

            area = Math.Abs(area / 2.0F);

            return area;
        }

        internal static SKPath GetInnerOrOuterPath(List<SKPoint> pathPoints, float distance, ParallelEnum location)
        {
            int numPoints = pathPoints.Count;

            SKPoint[] newPoints = new SKPoint[numPoints];
            SKPath newPath = new();

            for (int i = 0; i < numPoints - 1; i += 2)
            {
                float lineAngleRadians = CalculateLineAngleRadians(pathPoints[i], pathPoints[i + 1]);

                float angle = (location == ParallelEnum.Below) ? 1.570796F : -1.570796F; //(90, -90 degrees in radians)

                SKPoint p1 = PointOnCircleRadians(distance, lineAngleRadians + angle, pathPoints[i]);
                SKPoint p2 = PointOnCircleRadians(distance, lineAngleRadians + angle, pathPoints[i + 1]);

                newPoints[i] = p1;

                if (i > 0)
                {
                    newPoints[i].X = (newPoints[i - 1].X + newPoints[i].X) / 2.0F;
                    newPoints[i].Y = (newPoints[i - 1].Y + newPoints[i].Y) / 2.0F;
                }

                newPoints[i + 1] = p2;
            }

            if (numPoints > 0)
            {
                newPath.MoveTo(newPoints[0]);

                for (int i = 1; i < numPoints; i++)
                {
                    if (newPoints[i] != SKPoint.Empty)
                    {
                        newPath.LineTo(newPoints[i]);
                    }
                }

                newPath.Close();
            }

            return newPath;
        }


        internal static SKPath FlattenPath(SKPath path)
        {
            SKPath flattenedPath = new(path.FlattenAndClone());

            return flattenedPath;
        }

        internal static SKPath GetContourPath(SKPath path, int width, int height, out List<SKPoint> contourPoints)
        {
            // create a black bitmap from the path with background pixels set to Color.Empty
            using SKBitmap contourBitmap = new(width, height);

            using SKCanvas canvas = new(contourBitmap);

            canvas.Clear();

            using SKPaint paint = new();
            paint.Style = SKPaintStyle.Fill;
            paint.IsAntialias = false;
            paint.Color = SKColors.Black;
            paint.StrokeWidth = 1;

            canvas.DrawPath(path, paint);

            // make sure the bitmap has a 2-pixel wide margin of empty pixels
            // so that the contour points can be found
            using SKPath marginPath = new SKPath();
            marginPath.MoveTo(1, 1);
            marginPath.LineTo(width - 1, 1);
            marginPath.LineTo(width - 1, height - 1);
            marginPath.LineTo(1, height - 1);
            marginPath.Close();

            using SKPaint marginpaint = new();
            marginpaint.Style = SKPaintStyle.Stroke;
            marginpaint.IsAntialias = false;
            marginpaint.Color = SKColors.White;
            marginpaint.StrokeWidth = 2;

            canvas.DrawPath(marginPath, marginpaint);

            //Bitmap b = Extensions.ToBitmap(contourBitmap);
            //b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\contour.bmp");

            contourPoints = GetBitmapContourPoints(Extensions.ToBitmap(contourBitmap));

            SKPath contourPath = new();

            if (contourPoints.Count > 1)
            {
                // the Moore-Neighbor algorithm sets the first (0th) pixel in the list of contour points to
                // an empty pixel, so remove it before constructing the path from the contour points
                contourPoints.RemoveAt(0);

                if (contourPoints.Count > 0)
                {
                    contourPath.MoveTo(contourPoints[0]);

                    for (int i = 1; i < contourPoints.Count; i++)
                    {
                        contourPath.LineTo(contourPoints[i]);
                    }

                    contourPath.Close();
                }
            }


            return contourPath;
        }

        internal static List<SKPoint> GetBitmapContourPoints(Bitmap bitmap)
        {
            List<SKPoint> contourPoints = [];

            if (bitmap != null)
            {
                try
                {
                    var lockedBitmap = new LockBitmap(bitmap);
                    lockedBitmap.LockBits();

                    contourPoints = MooreNeighborTraceContour(lockedBitmap);

                    lockedBitmap.UnlockBits();
                }
                catch { }

            }

            return contourPoints;
        }

        private static SKPoint? FindStartingPoint(LockBitmap lockedBitmap, int width, int height)
        {
            // Iterate over the pixels in the image to find a starting point on the boundary of the object.
            // this implementation works from left-to-right, bottom-to-top
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = height - 2; j > 0; j--)
                {
                    // If the current pixel is on the boundary of the object, return it.
                    if (lockedBitmap.GetPixel(i, j).ToArgb() == Color.Black.ToArgb()
                        && (lockedBitmap.GetPixel(i - 1, j).ToArgb() == Color.Empty.ToArgb()
                        || lockedBitmap.GetPixel(i + 1, j).ToArgb() == Color.Empty.ToArgb()
                        || lockedBitmap.GetPixel(i, j - 1).ToArgb() == Color.Empty.ToArgb()
                        || lockedBitmap.GetPixel(i, j + 1).ToArgb() == Color.Empty.ToArgb()
                        || lockedBitmap.GetPixel(i - 1, j).ToArgb() == Color.White.ToArgb()
                        || lockedBitmap.GetPixel(i + 1, j).ToArgb() == Color.White.ToArgb()
                        || lockedBitmap.GetPixel(i, j - 1).ToArgb() == Color.White.ToArgb()
                        || lockedBitmap.GetPixel(i, j + 1).ToArgb() == Color.White.ToArgb()))
                    {
                        return new SKPoint(i, j);
                    }
                }
            }
            // If no starting point is found, return null.
            return null;
        }

        private static List<SKPoint> MooreNeighborTraceContour(LockBitmap image)
        {
            // see https://www.imageprocessingplace.com/downloads_V3/root_downloads/tutorials/contour_tracing_Abeer_George_Ghuneim/moore.html
            // and https://en.wikipedia.org/wiki/Moore_neighborhood
            // for a description of how this algorithm works

            List<SKPoint> contour = [];  // B
            contour.Add(SKPoint.Empty);

            SKPoint? sp = FindStartingPoint(image, image.Width, image.Height);

            if (sp == null)
            {
                return contour;
            }

            SKPoint startPoint = (SKPoint)sp;  // s

            contour.Add(startPoint);

            SKPoint contourPoint = new(startPoint.X, startPoint.Y);  // p

            SKPoint backPoint = new(startPoint.X, startPoint.Y + 1);  // b

            SKPoint checkPoint = new(backPoint.X - 1, backPoint.Y);  // c

            // the indexes of the Moore neighborhood cells around the point being checked
            // 1 2 3
            // 0 X 4
            // 7 6 5

            // since the first point in the Moore neighborhood around the startPoint to be checked
            // is below and to the left of the starting point, the checkStartIndex begins at cell 7

            int checkStartIndex = 7;

            // foundIndex is updated by the CheckMooreNeighborhood method;
            // when the CheckMooreNeighborhood method finds a black contour (boundary) pixel
            // the foundIndex is updated to point to the cell *previously* checked (the empty pixel checked before the black pixel);
            // that cell becomes the first one checked when looking for the next black contour pixel
            int foundIndex;

            while (!CheckExit(contour, contourPoint))
            {
                if (image.GetPixel((int)checkPoint.X, (int)checkPoint.Y).ToArgb() == Color.Black.ToArgb())
                {
                    contour.Add(checkPoint);
                    
                    backPoint.X = contourPoint.X;
                    backPoint.Y = contourPoint.Y;

                    contourPoint.X = checkPoint.X;
                    contourPoint.Y = checkPoint.Y;

                    SKPoint newCheckPoint = CheckMooreNeighborhood(image, checkPoint, checkStartIndex, out foundIndex);

                    if (newCheckPoint != SKPoint.Empty)
                    {
                        checkPoint.X = newCheckPoint.X;
                        checkPoint.Y = newCheckPoint.Y;

                        // update the checkStartIndex to the value of the foundIndex
                        checkStartIndex = foundIndex;
                    }
                    else
                    {
                        // error condition
                        break;
                    }
                }
                else
                {
                    backPoint.X = contourPoint.X;
                    backPoint.Y = contourPoint.Y;

                    SKPoint newCheckPoint = CheckMooreNeighborhood(image, checkPoint, checkStartIndex, out foundIndex);

                    if (newCheckPoint != SKPoint.Empty)
                    {
                        checkPoint.X = newCheckPoint.X;
                        checkPoint.Y = newCheckPoint.Y;

                        // update the checkStartIndex to the value of the foundIndex
                        checkStartIndex = foundIndex;
                    }
                    else
                    {
                        // error condition
                        break;
                    }
                }
            }

            return contour;
        }

        private static bool CheckExit(List<SKPoint> contour, SKPoint contourPoint)
        {
            // the CheckExit method looks at the list of contour points
            // if the startPoint appears in the list of contour points at least twice and
            // if the point before the startPoint in the list is the same, then the entire contour
            // has been found
            // this stopping method is described at
            // https://www.imageprocessingplace.com/downloads_V3/root_downloads/tutorials/contour_tracing_Abeer_George_Ghuneim/ray.html
            // as a method for stopping the Radial Sweep contour tracing algorithm; it has been adapted here
            // for use with the Moore-Neighbor algorithm

            bool boundaryComplete = false;
            List<int> foundIndexes = [];

            if (contour.Contains(contourPoint) && contour.Count > 2)
            {
                for (int i = 0; i < contour.Count; i++)
                {
                    if (contour[i].X == contourPoint.X && contour[i].Y == contourPoint.Y)
                    {
                        foundIndexes.Add(i);
                    }
                }

                if (foundIndexes.Count == 2)
                {
                    if (contour[foundIndexes[0]].X == contour[foundIndexes[1]].X && contour[foundIndexes[0]].Y == contour[foundIndexes[1]].Y)
                    {
                        if (contour[foundIndexes[0] - 1].X == contour[foundIndexes[1] - 1].X && contour[foundIndexes[0] - 1].Y == contour[foundIndexes[1] - 1].Y)
                        {
                            boundaryComplete = true;
                        }
                    }
                }    
            }

            return boundaryComplete;
        }

        private static readonly Point[] mooreNeighborhoodCheckTable =
        [
            new Point(-1, 0),
            new Point(-1, -1),
            new Point(0, -1),
            new Point(1, -1),
            new Point(1, 0),
            new Point(1,1),
            new Point(0, 1),
            new Point(-1, 1)
        ];

        private static SKPoint CheckMooreNeighborhood(LockBitmap image, SKPoint p, int startIndex, out int foundIndex)
        {
            foundIndex = 0;
            // move around the given point clockwise from the starting index, looking for a black pixel
            for (int i = 0; i < mooreNeighborhoodCheckTable.Length; i++)
            {
                // make sure that as the Moore neighborhood is checked, the index into the mooreNeighborhoodCheckTable is in range (0 - 7)
                int tableIndex = (i + startIndex + mooreNeighborhoodCheckTable.Length) % mooreNeighborhoodCheckTable.Length;

                int px = (int)(p.X + mooreNeighborhoodCheckTable[tableIndex].X);
                int py = (int)(p.Y + mooreNeighborhoodCheckTable[tableIndex].Y);

                if (px < 0 || px >= image.Width || py < 0 || py >= image.Height)
                {
                    break;
                }

                if (image.GetPixel(px, py).ToArgb() == Color.Black.ToArgb())
                {
                    foundIndex = tableIndex - 1;
                    return new SKPoint(px, py);
                }
            }

            foundIndex = -1;
            return SKPoint.Empty;
        }
    }
}
