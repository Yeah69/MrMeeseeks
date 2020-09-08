using System;
using System.Collections.Generic;
using MrMeeseeks.Extensions;
using Xunit;

namespace MrMeeseeks.Test.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void PreviousMonth_FirstMonth_ThrowsArgumentOutOfRange()
        {
            // Arrange
            var firstMonth = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, 1);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.PreviousMonth());
        }

        public static IEnumerable<object[]> ValidForPreviousMonthOrNextMonth => new[]
        {
            new object[] {new DateTime(DateTime.MinValue.Year, 2, 1), new DateTime(DateTime.MinValue.Year, 1, 1)},
            new object[] {new DateTime(69, 1, 1), new DateTime(68, 12, 1)},
            new object[] {new DateTime(69, 5, 1), new DateTime(69, 4, 1)},
            new object[] { new DateTime(DateTime.MaxValue.Year, 12, 1), new DateTime(DateTime.MaxValue.Year, 11, 1)},
        };

        [Theory]
        [MemberData(nameof(ValidForPreviousMonthOrNextMonth))]
        public void PreviousMonth_ValidMonth_PreviousMonth(DateTime month, DateTime expected)
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
            var firstMonth = new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, 1);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.NextMonth());
        }

        [Theory]
        [MemberData(nameof(ValidForPreviousMonthOrNextMonth))]
        public void NextMonth_ValidMonth_PreviousMonth(DateTime expected, DateTime month)
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
            var firstMonth = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, 1);
            
            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => firstMonth.OffsetMonthBy(offset));
        }

        public static IEnumerable<object[]> ValidForOffsetMonthBy => new[]
        {
            new object[] {new DateTime(69, 1, 1), 0, new DateTime(69, 1, 1)},
            new object[] {new DateTime(69, 1, 1), -1, new DateTime(68, 12, 1)},
            new object[] {new DateTime(69, 1, 1), 1, new DateTime(69, 2, 1)},
            new object[] {new DateTime(69, 1, 1), -2, new DateTime(68, 11, 1)},
            new object[] {new DateTime(69, 1, 1), 2, new DateTime(69, 3, 1)},
            new object[] {new DateTime(69, 1, 1), -12, new DateTime(68, 1, 1)},
            new object[] {new DateTime(69, 1, 1), 12, new DateTime(70, 1, 1)},
            new object[] {new DateTime(69, 1, 1), -13, new DateTime(67, 12, 1)},
            new object[] {new DateTime(69, 1, 1), 13, new DateTime(70, 2, 1)},
        };

        [Theory]
        [MemberData(nameof(ValidForOffsetMonthBy))]
        public void OffsetMonthBy_ValidMonth_PreviousMonth(DateTime month, int offset, DateTime expected)
        {
            // Act
            var offsetMonth = month.OffsetMonthBy(offset);

            // Assert
            Assert.Equal(expected, offsetMonth);
        }

        public static IEnumerable<object[]> MonthsAndIndexes => new[]
        {
            new object[] { DateTime.MinValue, 0 },
            new object[] { new DateTime(1, 2, 1), 1 },
            new object[] { new DateTime(9999, 11, 1), 119986 }, // 9999 * 12 - 2
            new object[] { new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, 1), 119987 } // 9999 * 12 - 1
        };

        [Theory]
        [MemberData(nameof(MonthsAndIndexes))]
        public void FromMonthIndex_ValidIndex_ExpectedMonth(DateTime expected, int index)
        {
            // Act
            var month = DateTimeExtensions.FromMonthIndex(index);

            // Assert
            Assert.Equal(expected, month);
        }

        [Theory]
        [MemberData(nameof(MonthsAndIndexes))]
        public void ToMonthIndex_ValidIndex_ExpectedMonth(DateTime month, int expected)
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
            var lastMonth = new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, 1);
            
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
            Assert.Equal(expected, DateTimeExtensions.CountOfMonths);
        }
    }
}