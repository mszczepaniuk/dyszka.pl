using IdentityServer.Model;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<CustomIdentityUser> userManager;

        public ProfileService(UserManager<CustomIdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.FindByIdAsync(context.Subject.Identity.GetSubjectId());
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                context.IssuedClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            context.IssuedClaims.Add(new Claim("isBanned", user.IsBanned.ToString()));
            context.IssuedClaims.Add(new Claim("userName", user.UserName));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}
