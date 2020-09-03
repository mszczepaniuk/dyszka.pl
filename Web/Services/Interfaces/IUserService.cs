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
        public ApplicationUser GetByUserName(string username);
    }
}
