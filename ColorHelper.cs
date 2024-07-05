namespace MapCreator
{
    internal class ColorHelper
    {
        public static Color ColorFromHSL(float h, float s, float l)
        {
            float r = 0;
            float g = 0;
            float b = 0;

            if (s == 0)
            {
                r = g = b = l; // achromatic
            }
            else
            {
                float q = (l < 0.5F) ? l * (1 + s) : l + s - (l * s);
                float p = 2.0F * l - q;

                r = HueToRgb(p, q, h + (1.0F / 3.0F));
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - (1.0F / 3.0F));
            }

            int newR = (int)Math.Round(r * 255.0F);
            int newG = (int)Math.Round(g * 255.0F);
            int newB = (int)Math.Round(b * 255.0F);

            Color rgbColor = Color.FromArgb(255, newR, newG, newB);
            return rgbColor;
        }

        public static float HueToRgb(float p, float q, float t)
        {
            if (t < 0.0F) t += 1.0F;
            if (t > 1.0F) t -= 1.0F;
            if (t < (1.0F / 6.0F)) return p + (q - p) * 6.0F * t;
            if (t < (1.0F / 2.0F)) return q;
            if (t < (2.0F / 3.0F)) return p + (q - p) * ((2.0F / 3.0F) - t) * 6.0F;
            return p;
        }


        // return HSL from RGB (not RGBA)
        // H is from 0.0 to 359.0
        // S is from 0.0 to 1.0
        // L is from 0.0 to 1.0
        // Algorithm derived from codereview.stackexchange.com/questions/248583/c-color-implementation-with-conversions-from-rgb-to-hsl-and-cmyk-and-vice-versa
        public static ValueTuple<float, float, float> RGBColortoHSL(Color rgbColor)
        {
            ValueTuple<float, float, float> HSL = new ValueTuple<float, float, float>(0, 0, 0);

            var r = rgbColor.R;
            var g = rgbColor.G;
            var b = rgbColor.B;

            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);

            var c = max - min;

            if (c != 0)
            {
                if (max == r)
                {
                    var segment = (g - b) / c;
                    var shift = 0 / 60;
                    if (segment < 0)
                        shift = 360 / 60;
                    HSL.Item1 = (segment + shift) * 60;
                }
                else if (max == g)
                {
                    var segment = (b - r) / c;
                    var shift = 120 / 60;
                    HSL.Item1 = (segment + shift) * 60;
                }
                else if (max == b)
                {
                    var segment = (r - g) / c;
                    var shift = 240 / 60;
                    HSL.Item1 = (segment + shift) * 60;
                }
            }

            var l = (max + min) / 2; // Lightness

            if (min != max)
            {
                if (l < 0.5)
                {
                    HSL.Item2 = (max - min) / (max + min);
                }
                else
                {
                    HSL.Item2 = (max - min) / (2 - max - min);
                }

            }

            HSL.Item3 = (max + min) / 2;

            return HSL;
        }
    }
}
