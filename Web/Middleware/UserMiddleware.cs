using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Web.Authorization;
using Web.Services.Interfaces;

namespace Web.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService, ApplicationDbContext dbContext)
        {
            var contextUsername = context.User.Claims.FirstOrDefault(c => c.Type == AuthConstants.UserNameClaimType)?.Value;
            if (contextUsername != null)
            {
                var user = userService.GetByUserName(contextUsername);
                if (user == null)
                {
                    await userService.AddAsync(new ApplicationUser { UserName = contextUsername });
                }

                dbContext.CurrentUser = user;
                userService.CurrentUserToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
                userService.CurrentUser = user ?? userService.GetByUserName(contextUsername);
            }
            await _next(context);
        }
    }
}
