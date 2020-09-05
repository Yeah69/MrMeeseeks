using System;

namespace MrMeeseeks.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime PreviousMonth(this DateTime @this)
        {
            return new DateTime(
                @this.Month != 1 ? @this.Year : @this.Year - 1,
                @this.Month != 1 ? @this.Month - 1 : 12,
                1);
        }

        public static DateTime NextMonth(this DateTime @this)
        {
            return new DateTime(
                @this.Month != 12 ? @this.Year : @this.Year + 1,
                @this.Month != 12 ? @this.Month + 1 : 1,
                1);
        }

        public static DateTime OffsetMonthBy(this DateTime @this, int offset)
        {
            if (offset == 0)
                return new DateTime(
                    @this.Year,
                    @this.Month,
                    1);
            if (offset > 0)
            {
                var current = new DateTime(@this.Year + offset / 12, @this.Month, 1);
                var limit = offset % 12;
                for (int i = 1; i <= limit; i++)
                    current = current.NextMonth();
                return current;
            }
            else
            {
                offset *= -1;

                var current = new DateTime(@this.Year - offset / 12, @this.Month, 1);
                var limit = offset % 12;
                for (int i = 1; i <= limit; i++)
                    current = current.PreviousMonth();
                return current;
            }
        }
        
        public static int CountOfMonths() => 
            // Count months of the years between the min and max date exclusively
            (DateTime.MaxValue.Year - DateTime.MinValue.Year - 2) * 12
            // Count the months of year of the min date
            + 12 - DateTime.MinValue.Month - 1 +
            // Count the months of year of the max date
            DateTime.MaxValue.Month;
    }
}
