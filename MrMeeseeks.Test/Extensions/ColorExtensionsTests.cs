using System.Collections.Generic;
using System.Drawing;
using MrMeeseeks.Extensions;
using Xunit;

namespace MrMeeseeks.Test.Extensions
{
    public class ColorExtensionsTests
    {
        public static IEnumerable<object[]> ToLongData => 
            new [] {
                new [] {
                    (object) Color.Red, 16711680
                },
                new [] {
                    (object) Color.Blue, 255
                },
                new [] {
                    (object) Color.Green, 32768
                },
                new [] {
                    (object) Color.Purple, 8388736
                },
            };
        
        [Theory]
        [MemberData(nameof(ToLongData))]
        public void ToLong_GivenColor_ExpectedLong(Color color, long expected)
        {
            // Arrange

            // Act
            var result = color.ToLong();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}