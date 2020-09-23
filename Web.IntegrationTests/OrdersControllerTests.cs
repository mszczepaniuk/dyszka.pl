using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class OrdersControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/orders/";

        public OrdersControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_FirstPage_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url + "user-orders/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task MarkAsDone_GoodId_MarksAsDone()
        {
            var order = fixture.Context.Orders.AsNoTracking().FirstOrDefault();
            await fixture.Client.PostAsJsonAsync(url + order.Id + "/done", new object());
            Assert.True(fixture.Context.Orders.AsNoTracking().FirstOrDefault().Done);
        }
    }
}
