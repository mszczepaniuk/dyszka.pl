using System.Net;
using System.Threading.Tasks;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class AuditLogsControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/audit-logs/";

        public AuditLogsControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_PageSmallerThanOne_ReturnsBadRequest()
        {
            var response = await fixture.Client.GetAsync(url + "-1");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_PageBiggerThanOne_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url + "1");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
