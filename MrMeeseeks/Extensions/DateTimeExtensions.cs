using System;

namespace MrMeeseeks.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime PreviousMonth(this DateTime @this) =>
            new DateTime(
                @this.Month != 1 ? @this.Year : @this.Year - 1,
                @this.Month != 1 ? @this.Month - 1 : 12,
                1);

        public static DateTime NextMonth(this DateTime @this) =>
            new DateTime(
                @this.Month != 12 ? @this.Year : @this.Year + 1,
                @this.Month != 12 ? @this.Month + 1 : 1,
                1);

        public static DateTime OffsetMonthBy(this DateTime @this, int offset) => 
            FromMonthIndex(@this.ToMonthIndex() + offset);

        public static DateTime FromMonthIndex(int index) => 
            new DateTime(
                index / 12 + 1, 
                index % 12 + 1, 
                1);

        public static int ToMonthIndex(this DateTime month) => 
            (month.Year - 1) * 12 + month.Month - 1;

        public static int CountOfMonths => 
            // Count months of the years between the min and max date exclusively
            (DateTime.MaxValue.Year - DateTime.MinValue.Year + 1 - 2) * 12
            // Count the months of year of the min date
            + 12 - DateTime.MinValue.Month + 1 +
            // Count the months of year of the max date
            DateTime.MaxValue.Month;
    }
}
