using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Model
{
    public class CustomIdentityUser : IdentityUser
    {
        public bool IsBanned { get; set; }
    }
}
