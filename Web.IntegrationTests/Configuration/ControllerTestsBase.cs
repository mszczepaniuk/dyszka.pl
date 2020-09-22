using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Web.IntegrationTests.Configuration
{
    public class ControllerTestsBase : IDisposable
    {
        public HttpClient Client;
        public TestWebApplicationFactory Factory;
        public IMapper Mapper;
        public ApplicationDbContext Context;

        public ControllerTestsBase()
        {
            Factory = new TestWebApplicationFactory();
            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var services = Factory.Services;
            Context = services.GetRequiredService<ApplicationDbContext>();
            SeedData().GetAwaiter().GetResult();
            Mapper = services.GetRequiredService<IMapper>();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
            Context.Dispose();
        }

        private async Task SeedData()
        {
            var user = new ApplicationUser
            {
                UserName = "administrator"
            };
            await Context.ApplicationUsers.AddAsync(user);
            await Context.SaveChangesAsync();
            Context.CurrentUser = Context.ApplicationUsers.Where(u => u.UserName == "administrator").FirstOrDefault();
            var offer = new Offer
            {
                IsBlocked = false,
                IsHidden = false,
                Description = "test",
                ShortDescription = "test",
                Image = "test",
                Price = 15,
                Tags = new List<string> { "tag", "tag2" }
            };
            await Context.Offers.AddAsync(offer);
            var offer2 = new Offer
            {
                IsBlocked = false,
                IsHidden = false,
                Description = "test",
                ShortDescription = "test",
                Image = "test",
                Price = 15,
                Tags = new List<string> { "tag4", "tag5" }
            };
            await Context.Offers.AddAsync(offer2);

            var comments = new List<Comment>
            {
                new Comment
                {
                    IsPositive = false,
                    Offer = offer,
                    Text = "abc"
                },
                new Comment
                {
                    IsPositive = false,
                    Offer = offer,
                    Text = "abc"
                }
            };

            var order = new Order
            {
                Done = false,
                Offer = offer
            };
            await Context.Orders.AddAsync(order);

            var payment = new Payment
            {
                Done = false,
                Order = order,
                Value = 25
            };
            await Context.Payments.AddAsync(payment);

            await Context.Comments.AddRangeAsync(comments);
            await Context.SaveChangesAsync();
        }
    }

    [CollectionDefinition("ControllersCollection")]
    public class ControllersCollection : ICollectionFixture<ControllerTestsBase>
    {

    }
}
