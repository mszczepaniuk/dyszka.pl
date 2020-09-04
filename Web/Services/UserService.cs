using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        public UserService(IBaseRepository<ApplicationUser> repository) : base(repository)
        {

        }

        public ApplicationUser GetByUserName(string username)
        {
            return repository.GetAll().FirstOrDefault(u => u.UserName == username);
        }
    }
}
