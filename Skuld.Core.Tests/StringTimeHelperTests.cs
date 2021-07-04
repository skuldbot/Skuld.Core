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
            var expected = new TimeSpan(1, 1, 1, 1, 1);
            var res = StringTimeHelper.TryParse("1d 1h 1m 1s 1ms", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);

            res = StringTimeHelper.TryParse("1D 1H 1M 1S 1MS", out r);
            result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);

            res = StringTimeHelper.TryParse("1 day 1 hour 1 minute 1 second 1 millisecond", out r);
            result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);

            res = StringTimeHelper.TryParse("1 Day 1 Hour 1 Minute 1 Second 1 MilliSecond", out r);
            result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);

            expected = new TimeSpan(2, 2, 2, 2, 2);
            res = StringTimeHelper.TryParse("2 days 2 hours 2 minutes 2 seconds 2 milliseconds", out r);
            result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);

            res = StringTimeHelper.TryParse("2 Days 2 Hours 2 Minutes 2 Seconds 2 MilliSeconds", out r);
            result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
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
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
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
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
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
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
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
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
        }
        [Fact]
        public void TestMilliSecond()
        {
            var expected = new TimeSpan(0, 0, 0, 0, 1);
            var res = StringTimeHelper.TryParse("1ms", out TimeSpan? r);
            var result = r.Value;

            Assert.True(res);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.Hours, result.Hours);
            Assert.Equal(expected.Minutes, result.Minutes);
            Assert.Equal(expected.Seconds, result.Seconds);
            Assert.Equal(expected.Milliseconds, result.Milliseconds);
        }
    }
}
