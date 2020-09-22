using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using Microsoft.EntityFrameworkCore;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class OffersControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/offers/";

        public OffersControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_NoParams_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_WrongId_ReturnsNotFound()
        {
            var response = await fixture.Client.GetAsync(url + Guid.NewGuid());
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task HideOffer_GoodId_HidesOffer()
        {
            var offer = fixture.Context.Offers.AsNoTracking().FirstOrDefault();
            await fixture.Client.PutAsJsonAsync(url + offer.Id + "/hide", new object());
            Assert.True(fixture.Context.Offers.AsNoTracking().FirstOrDefault().IsHidden);
        }

        [Fact]
        public async Task ShowOffer_GoodId_ShowsOffer()
        {
            var offer = fixture.Context.Offers.AsNoTracking().FirstOrDefault();
            await fixture.Client.PutAsJsonAsync(url + offer.Id + "/show", new object());
            Assert.True(!fixture.Context.Offers.AsNoTracking().FirstOrDefault().IsHidden);
        }

        [Fact]
        public async Task AddOffer_MissingFields_BadRequest()
        {
            var response = await fixture.Client.PostAsJsonAsync(url, new OfferBm
            {
                Title = "Abc",
                Description = "Abc",
                ShortDescription = "Abc"
            });
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}
