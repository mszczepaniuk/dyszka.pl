using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services.Interfaces
{
    public interface IDbInitializer
    {
        public Task SeedAsync();
        public Task MigrateAsync();
    }
}
