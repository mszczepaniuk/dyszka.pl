using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class OfferPromotionsControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/offer-promotions/";

        public OfferPromotionsControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetAvailableTags_NoPromotedTags_ReturnsAll()
        {
            var offer = fixture.Context.Offers.AsNoTracking().LastOrDefault();
            var testUrl = url + offer.Id + "?tags=" + offer.Tags[0];
            for (int i = 1; i < offer.Tags.Count; i++)
            {
                testUrl += $"&tags={offer.Tags[i]}";
            }
            var response = await fixture.Client.GetAsync(testUrl);
            Assert.True(offer.Tags.Count == JsonSerializer.Deserialize<Dictionary<string, decimal>>(
                await response.Content.ReadAsStringAsync()).Keys.Count);
        }

        [Fact]
        public async Task PromoteOffer_AvailableTag_ReturnsOk()
        {
            var offer = fixture.Context.Offers.AsNoTracking().FirstOrDefault();
            var response = await fixture.Client.PostAsJsonAsync($"{url}{offer.Id}/{offer.Tags[0]}", new object());
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
