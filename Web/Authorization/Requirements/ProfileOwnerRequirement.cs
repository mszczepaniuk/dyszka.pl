using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Web.Services.Interfaces;

namespace Web.Authorization.Requirements
{
    public class ProfileOwnerRequirement : IAuthorizationRequirement
    {

    }

    public class ProfileOwnerAuthorizationHandler : AuthorizationHandler<ProfileOwnerRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public ProfileOwnerAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileOwnerRequirement requirement)
        {
            var username = context.User?.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var contextUserId = userService.GetByUserName(username)?.Id;
            var idFromPath = httpContextAccessor.HttpContext.Request.Path.Value.Split("/").Last();
            if (contextUserId.HasValue 
                && Guid.TryParse(idFromPath, out var guidFromPath)
                && guidFromPath == contextUserId.Value)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
