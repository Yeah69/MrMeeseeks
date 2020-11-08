using System.Drawing;

namespace MrMeeseeks.Extensions
{
    public static class ColorExtensions
    {
        public static long ToLong(this Color color)
        {
            long ret = color.A << 24 + color.R << 16 + color.G << 8 + color.B << 0;
            ret <<= 8;
            ret += color.R;
            ret <<= 8;
            ret += color.G;
            ret <<= 8;
            ret += color.B;
            return ret;
        }
    }
}