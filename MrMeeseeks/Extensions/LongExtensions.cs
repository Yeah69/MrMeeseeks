using System.Drawing;

namespace MrMeeseeks.Extensions
{
    public static class LongExtensions
    {
        public static Color ToColor(this long number) =>
            Color.FromArgb(
                (byte) (number >> 24 & 0xff),
                (byte) (number >> 16 & 0xff),
                (byte) (number >> 8 & 0xff),
                (byte) (number & 0xff));
    }
}