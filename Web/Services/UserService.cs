using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityModel;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        public string CurrentUserToken { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        private readonly HttpClient client;
        private readonly string identityUrl;
        private readonly string baseUrl;

        public UserService(IBaseRepository<ApplicationUser> repository,
            HttpClient client,
            IConfiguration config) : base(repository)
        {
            this.client = client;
            baseUrl = config.GetSection("URI").GetValue<string>("IdentityServer");
            identityUrl = baseUrl + "/api/identity/";
        }

        public ApplicationUser GetByUserName(string username)
        {
            return repository.GetAll().FirstOrDefault(u => u.UserName == username);
        }

        public async Task<string> GetUserIdentityData(string username)
        {
            var response = await SendRequestWithToken(HttpMethod.Get, new Uri($"{identityUrl}{username}"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }

        // TODO: Logi
        public async Task<IList<ApplicationUser>> GetAllInRole(string roleName)
        {
            var response = await SendRequestWithToken(HttpMethod.Get, new Uri($"{identityUrl}role/{roleName}"));
            if (!response.IsSuccessStatusCode)
            {
                return new List<ApplicationUser>();
            }
            var names = JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
            var users = repository.GetAll().Where(u => names.Contains(u.UserName)).ToList();
            return users;
        }

        public async Task<bool> AddToRole(string username, string roleName)
        {
            return (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}claim/{username}/{roleName}")))
                .IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromRole(string username, string roleName)
        {
            return (await SendRequestWithToken(HttpMethod.Delete, new Uri($"{identityUrl}claim/{username}/{roleName}")))
                .IsSuccessStatusCode;
        }

        public async Task<bool> BanUser(string username)
        {
            return (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}ban/{username}")))
                .IsSuccessStatusCode;
        }

        public async Task<bool> UnbanUser(string username)
        {
            return (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}unban/{username}")))
                .IsSuccessStatusCode;
        }

        public override async Task<bool> RemoveAsync(Guid id)
        {
            var appUser = repository.GetById(id);
            if (appUser == null)
            {
                return false;
            }
            var response = await SendRequestWithToken(HttpMethod.Delete, new Uri($"{identityUrl}{appUser.UserName}"));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var result = await repository.RemoveAsync(id);
            if (!result)
            {
                //TODO: Background service deleting unused user data.
            }

            return true;
        }

        private async Task<HttpResponseMessage> SendRequestWithToken(HttpMethod method, Uri uri)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUserToken);
            var response = await client.SendAsync(request);
            request.Dispose();
            return response;
        }
    }
}
