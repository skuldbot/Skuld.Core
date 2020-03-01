using Skuld.Core.Extensions;
using System;
using Xunit;

namespace Skuld.Core.Tests
{
    public class GenericExtensionsTest
    {
        [Fact]
        public void TestAll()
        {
            Assert.Equal(1UL, 0UL.Add(1));
            Assert.Equal(18446744073709551615UL, 18446744073709551615UL.Add(1));

            Assert.Equal(0UL, 1UL.Subtract(1));
            Assert.Equal(0UL, 0UL.Subtract(1));

            Assert.Equal(0UL, new DateTime(1970, 1, 1).ToEpoch());
            Assert.Equal(new DateTime(1970, 1, 1), 0UL.FromEpoch());

            Assert.Equal(-1, DateTime.Now.MonthsBetween(DateTime.Now.AddMonths(1)));
            Assert.Equal(1, DateTime.Now.AddMonths(1).MonthsBetween(DateTime.Now));
        }
    }
}
