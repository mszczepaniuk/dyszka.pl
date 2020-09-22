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
    public class PaymentsControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/payments/";

        public PaymentsControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_FirstPage_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url + "1");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task MarkAsDone_GoodId_MarksAsDone()
        {
            var payment = fixture.Context.Payments.AsNoTracking().FirstOrDefault();
            await fixture.Client.PutAsJsonAsync(url + payment.Id, new object());
            Assert.True(fixture.Context.Payments.AsNoTracking().FirstOrDefault().Done);
        }
    }
}
