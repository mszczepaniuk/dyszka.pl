using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IUserService : IBaseService<ApplicationUser>
    {
        public string CurrentUserToken { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public ApplicationUser GetByUserName(string username);
        public Task<string> GetUserIdentityData(string username);
        public Task<IList<ApplicationUser>> GetAllInRole(string roleName);
        public Task<bool> AddToRole(string username, string roleName);
        public Task<bool> RemoveFromRole(string username, string roleName);
        public Task<bool> BanUser(string username);
        public Task<bool> UnbanUser(string username);
        public BillingData GetUserBillingData(string username);
        public Task CreateOrUpdateUserBillingData(string username, BillingData billingData);
    }
}
