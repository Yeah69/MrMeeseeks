using System;

namespace MrMeeseeks.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset PreviousMonth(this DateTimeOffset @this)
        {
            return new DateTimeOffset(
                @this.Month != 1 ? @this.Year : @this.Year - 1,
                @this.Month != 1 ? @this.Month - 1 : 12,
                1,
                0,
                0,
                0,
                TimeSpan.Zero);
        }

        public static DateTimeOffset NextMonth(this DateTimeOffset @this)
        {
            return new DateTimeOffset(
                @this.Month != 12 ? @this.Year : @this.Year + 1,
                @this.Month != 12 ? @this.Month + 1 : 1,
                1,
                0,
                0,
                0,
                TimeSpan.Zero);
        }

        public static DateTimeOffset OffsetMonthBy(this DateTimeOffset @this, int offset)
        {
            if (offset == 0)
                return new DateTimeOffset(
                    @this.Year,
                    @this.Month,
                    1,
                    0,
                    0,
                    0,
                    TimeSpan.Zero);
            if (offset > 0)
            {
                var current = new DateTimeOffset(
                    @this.Year + offset / 12, 
                    @this.Month, 
                    1,
                    0,
                    0,
                    0,
                    TimeSpan.Zero);
                var limit = offset % 12;
                for (int i = 1; i <= limit; i++)
                    current = current.NextMonth();
                return current;
            }
            else
            {
                offset *= -1;

                var current = new DateTimeOffset(
                    @this.Year - offset / 12, 
                    @this.Month, 
                    1,
                    0,
                    0,
                    0,
                    TimeSpan.Zero);
                var limit = offset % 12;
                for (int i = 1; i <= limit; i++)
                    current = current.PreviousMonth();
                return current;
            }
        }
    }
}
