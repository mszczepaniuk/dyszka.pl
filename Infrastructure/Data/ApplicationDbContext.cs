using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
