using System;
using System.Collections.Generic;
using MrMeeseeks.Extensions;
using Xunit;

namespace MrMeeseeks.Test.Extensions
{
    public class DateTimeOffsetExtensionsTests
    {
        [Fact]
        public void PreviousMonth_FirstMonth_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var firstMonth = new DateTimeOffset(DateTime.MinValue.Year, DateTime.MinValue.Month, 1, 0, 0, 0, TimeSpan.Zero);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.PreviousMonth());
        }

        public static IEnumerable<object[]> ValidForPreviousMonthOrNextMonth => new[]
        {
            new object[] {new DateTimeOffset(DateTimeOffset.MinValue.Year, 2, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(DateTimeOffset.MinValue.Year, 1, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(68, 12, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 5, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(69, 4, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] { new DateTimeOffset(DateTimeOffset.MaxValue.Year, 12, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(DateTimeOffset.MaxValue.Year, 11, 1, 0, 0, 0, TimeSpan.Zero)},
        };

        [Theory]
        [MemberData(nameof(ValidForPreviousMonthOrNextMonth))]
        public void PreviousMonth_ValidMonth_PreviousMonth(DateTimeOffset month, DateTimeOffset expected)
        {
            // Act
            var previousMonth = month.PreviousMonth();

            // Assert
            Assert.Equal(expected, previousMonth);
        }
        
        [Fact]
        public void NextMonth_LastMonth_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var firstMonth = new DateTimeOffset(DateTimeOffset.MaxValue.Year, DateTimeOffset.MaxValue.Month, 1, 0, 0, 0, TimeSpan.Zero);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.NextMonth());
        }

        [Theory]
        [MemberData(nameof(ValidForPreviousMonthOrNextMonth))]
        public void NextMonth_ValidMonth_PreviousMonth(DateTimeOffset expected, DateTimeOffset month)
        {
            // Act
            var nextMonth = month.NextMonth();

            // Assert
            Assert.Equal(expected, nextMonth);
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-12)]
        [InlineData(-13)]
        public void OffsetMonthBy_FirstMonth_ThrowsArgumentOutOfRange(int offset)
        {
            // Arrange
            var firstMonth = new DateTimeOffset(DateTimeOffset.MinValue.Year, DateTimeOffset.MinValue.Month, 1, 0, 0, 0, TimeSpan.Zero);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.OffsetMonthBy(offset));
        }

        public static IEnumerable<object[]> ValidForOffsetMonthBy => new[]
        {
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), 0, new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), -1, new DateTimeOffset(68, 12, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), 1, new DateTimeOffset(69, 2, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), -2, new DateTimeOffset(68, 11, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), 2, new DateTimeOffset(69, 3, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), -12, new DateTimeOffset(68, 1, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), 12, new DateTimeOffset(70, 1, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), -13, new DateTimeOffset(67, 12, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {new DateTimeOffset(69, 1, 1, 0, 0, 0, TimeSpan.Zero), 13, new DateTimeOffset(70, 2, 1, 0, 0, 0, TimeSpan.Zero)},
        };

        [Theory]
        [MemberData(nameof(ValidForOffsetMonthBy))]
        public void OffsetMonthBy_ValidMonth_PreviousMonth(DateTimeOffset month, int offset, DateTimeOffset expected)
        {
            // Act
            var offsetMonth = month.OffsetMonthBy(offset);

            // Assert
            Assert.Equal(expected, offsetMonth);
        }

        public static IEnumerable<object[]> MonthsAndIndexes => new[]
        {
            new object[] { DateTimeOffset.MinValue, 0 },
            new object[] { new DateTimeOffset(1, 2, 1, 0, 0, 0, TimeSpan.Zero), 1 },
            new object[] { new DateTimeOffset(9999, 11, 1, 0, 0, 0, TimeSpan.Zero), 119986 }, // 9999 * 12 - 2
            new object[] { new DateTimeOffset(DateTimeOffset.MaxValue.Year, DateTimeOffset.MaxValue.Month, 1, 0, 0, 0, TimeSpan.Zero), 119987 } // 9999 * 12 - 1
        };

        [Theory]
        [MemberData(nameof(MonthsAndIndexes))]
        public void FromMonthIndex_ValidIndex_ExpectedMonth(DateTimeOffset expected, int index)
        {
            // Act
            var month = DateTimeOffsetExtensions.FromMonthIndex(index);

            // Assert
            Assert.Equal(expected, month);
        }

        [Theory]
        [MemberData(nameof(MonthsAndIndexes))]
        public void ToMonthIndex_ValidIndex_ExpectedMonth(DateTimeOffset month, int expected)
        {
            // Act
            var index = month.ToMonthIndex();

            // Assert
            Assert.Equal(expected, index);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(12)]
        [InlineData(13)]
        public void OffsetMonthBy_LastMonth_ThrowsArgumentOutOfRange(int offset)
        {
            // Arrange
            var lastMonth = new DateTimeOffset(DateTimeOffset.MaxValue.Year, DateTimeOffset.MaxValue.Month, 1, 0, 0, 0, TimeSpan.Zero);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => lastMonth.OffsetMonthBy(offset));
        }

        [Fact]
        public void CountOfMonths_NoConstraint_9999times12()
        {
            // Arrange
            const int expected = 119988; // 9999 * 12

            // Act


            // Assert
            Assert.Equal(expected, DateTimeOffsetExtensions.CountOfMonths);
        }
    }
}