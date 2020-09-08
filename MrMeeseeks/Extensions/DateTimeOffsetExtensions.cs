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

        public static DateTimeOffset OffsetMonthBy(this DateTimeOffset @this, int offset) => 
            FromMonthIndex(@this.ToMonthIndex() + offset);

        public static DateTimeOffset FromMonthIndex(int index) => 
            new DateTimeOffset(
                index / 12 + 1, 
                index % 12 + 1, 
                1,
                0,
                0,
                0,
                TimeSpan.Zero);

        public static int ToMonthIndex(this DateTimeOffset month) => 
            (month.Year - 1) * 12 + month.Month - 1;

        public static int CountOfMonths => 
            // Count months of the years between the min and max date exclusively
            (DateTimeOffset.MaxValue.Year - DateTimeOffset.MinValue.Year + 1 - 2) * 12
            // Count the months of year of the min date
            + 12 - DateTimeOffset.MinValue.Month + 1 +
            // Count the months of year of the max date
            DateTime.MaxValue.Month;
    }
}
