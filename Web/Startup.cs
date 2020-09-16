using System.Net.Http;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web.Extensions;
using Web.Services.Interfaces;
using Web.Services;
using ApplicationCore.Repositories;
using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Web.Authorization;
using Web.Authorization.Requirements;

namespace Web
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
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration.GetSection("URI").GetValue<string>("IdentityServer");
                    options.ApiName = "web";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.OnlyAdminPolicy, policy => policy.RequireClaim(ClaimTypes.Role, AuthConstants.AdminRoleName));
                options.AddPolicy(AuthConstants.ModeratorOrAdminPolicy, policy => policy.RequireClaim(ClaimTypes.Role, AuthConstants.AdminRoleName, AuthConstants.ModeratorRoleName));
                options.AddPolicy(AuthConstants.OnlyModeratorPolicy, policy => policy.RequireClaim(ClaimTypes.Role, AuthConstants.ModeratorRoleName));
                options.AddPolicy(AuthConstants.NotBannedPolicy, policy => policy.RequireClaim(AuthConstants.IsBannedClaimType, "False"));
                options.AddPolicy(AuthConstants.ProfileOwnerPolicy, policy => policy.AddRequirements(new ProfileOwnerRequirement()));
                options.AddPolicy(AuthConstants.UserRemovalPolicy, policy => policy.AddRequirements(new UserRemovalRequirement()));
                options.AddPolicy(AuthConstants.IsOwnerPolicy, policy => policy.AddRequirements(new IsOwnerRequirement()));
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpClient>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorizationHandler, ProfileOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, UserRemovalAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, IsOwnerAuthorizationHandler>();

            services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddTransient(typeof(IExtendedBaseService<,>), typeof(BaseService<,>));
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddTransient<IAuditLogService, AuditLogService>();
            services.AddTransient<IOfferService, OfferService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseUserMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
