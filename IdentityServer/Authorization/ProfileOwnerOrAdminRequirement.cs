using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Authorization
{
    public class ProfileOwnerOrAdminRequirement : IAuthorizationRequirement
    {
        
    }

    public class ProfileOwnerOrAdminAuthorizationHandler : AuthorizationHandler<ProfileOwnerOrAdminRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<CustomIdentityUser> userManager;

        public ProfileOwnerOrAdminAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            UserManager<CustomIdentityUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileOwnerOrAdminRequirement requirement)
        {
            var adminClaim = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value=="admin");
            var username = context.User?.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var usernameFromPath = httpContextAccessor.HttpContext.Request.Path.Value.Split("/").Last();
            var isBanned = userManager.FindByNameAsync(usernameFromPath).GetAwaiter().GetResult()?.IsBanned ?? false;
            if ((adminClaim != null && isBanned) || username == usernameFromPath)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}