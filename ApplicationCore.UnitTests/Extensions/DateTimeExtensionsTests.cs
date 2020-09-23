using System;
using Xunit;
using ApplicationCore.Extensions;

namespace ApplicationCore.UnitTests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void IsEarlier_True()
        {
            var date2019 = new DateTime(2019,10,10);
            var date2020 = new DateTime(2020,10,10);
            Assert.True(date2019.IsEarlier(date2020));
        }

        [Fact]
        public void IsLater_True()
        {
            var date2019 = new DateTime(2019, 10, 10);
            var date2020 = new DateTime(2020, 10, 10);
            Assert.True(date2020.IsLater(date2019));
        }
    }
}
