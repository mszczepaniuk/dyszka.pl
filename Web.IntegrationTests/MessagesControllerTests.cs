using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class MessagesControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/messages/";

        public MessagesControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetBeetwenUsers_TwoSameUsers_ReturnsBadRequest()
        {
            var response = await fixture.Client.GetAsync(url + "1" + "/administrator" + "/administrator");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetLastMessages_AnotherUser_ReturnsForbidden()
        {
            var response = await fixture.Client.GetAsync(url  + "1" + "/wojtek");
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetLastMessages_LoggedInUser_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url + "1" + "/administrator");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddMessage_MissingParams_ReturnsBadRequest()
        {
            var response = await fixture.Client.PostAsJsonAsync(url, new MessageBm { Text = "abc" });
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}
