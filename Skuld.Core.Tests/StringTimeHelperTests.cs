using Skuld.Core.Helpers;
using System;
using Xunit;

namespace Skuld.Core.Tests
{
    public class StringTimeHelperTests
    {
        [Fact]
        public void TestAll()
        {
            var expected = new TimeSpan(1, 1, 1, 1);
            var res = StringTimeHelper.TryParse("1d 1h 1m 1s", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
        }
        [Fact]
        public void TestDay()
        {
            var expected = new TimeSpan(1, 0, 0, 0);
            var res = StringTimeHelper.TryParse("1d", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
        }
        [Fact]
        public void TestHour()
        {
            var expected = new TimeSpan(0, 1, 0, 0);
            var res = StringTimeHelper.TryParse("1h", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
        }
        [Fact]
        public void TestMinute()
        {
            var expected = new TimeSpan(0, 0, 1, 0);
            var res = StringTimeHelper.TryParse("1m", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
        }
        [Fact]
        public void TestSecond()
        {
            var expected = new TimeSpan(0, 0, 0, 1);
            var res = StringTimeHelper.TryParse("1s", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
        }
    }
}
