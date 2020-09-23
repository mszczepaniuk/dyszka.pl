using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Middleware;

namespace Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<UserMiddleware>();
    }
}
