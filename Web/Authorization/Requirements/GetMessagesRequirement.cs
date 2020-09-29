using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Web.Authorization.Requirements
{
    public class GetMessagesRequirement : IAuthorizationRequirement
    {

    }

    public class GetMessageAuthorizationHandler : AuthorizationHandler<GetMessagesRequirement, string>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetMessagesRequirement requirement, string resource)
        {
            if (resource == context.User?.Claims.FirstOrDefault(c => c.Type == AuthConstants.UserNameClaimType)?.Value)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
