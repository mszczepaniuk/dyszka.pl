using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var contextUsername = context.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            if (contextUsername != null)
            {
                var user = userService.GetByUserName(contextUsername);
                if (user == null)
                {
                    await userService.AddAsync(new ApplicationUser { UserName = contextUsername });
                }
            }
            await _next(context);
        }
    }
}
