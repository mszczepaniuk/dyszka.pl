using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Authorization
{
    public class NotBannedRequirement : IAuthorizationRequirement
    {

    }

    public class NotBannedAuthorizationHandler : AuthorizationHandler<NotBannedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NotBannedRequirement requirement)
        {
            var isBannedClaim = context.User?.Claims.FirstOrDefault(c => c.Type == "isBanned");
            if (isBannedClaim != null && isBannedClaim.Value == "False")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
