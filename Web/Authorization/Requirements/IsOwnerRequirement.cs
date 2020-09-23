using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Web.Authorization.Requirements
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {

    }

    public class IsOwnerAuthorizationHandler : AuthorizationHandler<IsOwnerRequirement, IOwnable>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement, IOwnable resource)
        {
            if (resource == null)
            {
                throw new ElementNotFoundException("Element not found");
            }
            var username = context.User?.Claims.FirstOrDefault(c => c.Type == AuthConstants.UserNameClaimType)?.Value;
            if (username == resource.CreatedBy.UserName)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
