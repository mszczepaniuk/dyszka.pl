using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Authorization;
using IdentityServer.Data;
using IdentityServer.Model;
using IdentityServer.Services;
using IdentityServer.Services.Interfaces;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, NotBannedAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ProfileOwnerOrAdminAuthorizationHandler>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddDbContextPool<CustomIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<CustomIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddInMemoryApiResources(IdentityConfig.GetApiResources())
                .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<CustomIdentityUser>()
                .AddProfileService<ProfileService>();
            services.AddControllers();
            services.AddLocalApiAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName,
                    policy =>
                    {
                        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ClaimTypes.Role, "admin");
                        policy.Requirements.Add(new NotBannedRequirement());
                    });
                options.AddPolicy("ProfileOwnerOrAdmin",
                    policy =>
                    {
                        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new ProfileOwnerOrAdminRequirement());
                    });
            });
            services.AddCors(options => options.AddPolicy("WebPolicy", builder =>
            {
                builder.WithOrigins(configuration.GetSection("URI").GetValue<string>("Web"))
                    .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TYMCZASOWO DO TESTÓW
            app.UseMiddleware<RequestDiagnosticsMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("WebPolicy");
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    //TYMCZASOWO
    public class RequestDiagnosticsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestDiagnosticsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //try
            //{
            //    var reader = new StreamReader(context.Request.Body);
            //    //reader.BaseStream.Seek(0, SeekOrigin.Begin);
            //    var rawMessage = await reader.ReadToEndAsync();
            //}
            //catch (Exception e)
            //{

            //}

            await _next(context);
        }
    }
}
