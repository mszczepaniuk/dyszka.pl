﻿using IdentityServer.Data;
using IdentityServer.Model;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly CustomIdentityDbContext dbContext;
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly string[] roles = { "moderator", "admin" };

        public DatabaseInitializer(
            CustomIdentityDbContext dbContext,
            UserManager<CustomIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public Task MigrateAsync()
        {
            return dbContext.Database.MigrateAsync();
        }

        public async Task SeedAsync()
        {
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByNameAsync("administrator") == null)
            {
                await userManager.CreateAsync(new CustomIdentityUser { UserName = "administrator" }, "wojtek123");
                var admin = await userManager.FindByNameAsync("administrator");
                foreach (var role in roles)
                {
                    await userManager.AddToRoleAsync(admin, role);
                }
            }

            if (await userManager.FindByNameAsync("moderator") == null)
            {
                await userManager.CreateAsync(new CustomIdentityUser { UserName = "moderator" }, "wojtek123");
                var admin = await userManager.FindByNameAsync("moderator");
                await userManager.AddToRoleAsync(admin, "moderator");
            }
        }
    }
}
