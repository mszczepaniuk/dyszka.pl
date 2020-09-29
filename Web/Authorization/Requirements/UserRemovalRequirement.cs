using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Web.Authorization.Requirements
{
    public class UserRemovalRequirement : IAuthorizationRequirement
    {

    }

    public class UserRemovalAuthorizationHandler : AuthorizationHandler<UserRemovalRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public UserRemovalAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRemovalRequirement requirement)
        {
            var adminClaim = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "admin");
            var username = context.User?.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var idFromPath = Guid.Parse(httpContextAccessor.HttpContext.Request.Path.Value.Split("/").Last());
            if (adminClaim != null || idFromPath == userService.GetByUserName(username).Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
