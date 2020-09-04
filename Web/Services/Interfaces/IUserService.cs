using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Interfaces
{
    public interface IUserService : IBaseService<ApplicationUser>
    {
        public string CurrentUserToken { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public ApplicationUser GetByUserName(string username);
        public Task<IList<ApplicationUser>> GetAllInRole(string roleName);
        public Task<bool> AddToRole(string username, string roleName);
        public Task<bool> RemoveFromRole(string username, string roleName);
        public Task<bool> BanUser(string username);
        public Task<bool> UnbanUser(string username);
    }
}
