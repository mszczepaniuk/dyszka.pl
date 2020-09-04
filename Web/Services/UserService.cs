using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        private readonly HttpClient client;
        private readonly string identityUrl;

        public UserService(IBaseRepository<ApplicationUser> repository,
            HttpClient client,
            IConfiguration config) : base(repository)
        {
            this.client = client;
            identityUrl = config.GetSection("URI").GetValue<string>("IdentityServer") + "/api/identity/";
        }

        public ApplicationUser GetByUserName(string username)
        {
            return repository.GetAll().FirstOrDefault(u => u.UserName == username);
        }

        public async Task<IList<ApplicationUser>> GetAdmins()
        {
            var result = await client.PostAsJsonAsync($"{identityUrl}/role/admin", new { });
        }
    }
}
