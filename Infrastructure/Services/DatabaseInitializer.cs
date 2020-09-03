using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext dbContext;

        public DatabaseInitializer(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task MigrateAsync()
        {
            return dbContext.Database.MigrateAsync();
        }
    }
}
