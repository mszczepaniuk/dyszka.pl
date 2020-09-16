using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public ApplicationUser CurrentUser { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Offer>()
                .Property(o => o.Tags)
                .HasConversion(p => string.Join(',', p),
                    p => p.Split(',', StringSplitOptions.RemoveEmptyEntries));
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.CreatedBy)
                .WithMany(u => u.Offers)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Offer>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Offer>()
                .Property(o => o.Tags)
                .IsRequired();
            modelBuilder.Entity<Offer>()
                .Property(o => o.Title)
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateEntities();
            return base.SaveChanges();
        }

        private void UpdateEntities()
        {
            var now = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
            {
                var entity = entry.Entity;
                if (entity is ICreatedDate e1)
                {
                    e1.CreatedDate = now;
                }
                if (entity is IOwnable e2)
                {
                    e2.CreatedBy = CurrentUser;
                }
                if (entity is IUpdatedBy e3)
                {
                    e3.UpdatedBy = CurrentUser;
                }
                if (entity is IUpdatedDate e4)
                {
                    e4.UpdatedDate = now;
                }
            }
            foreach (var entry in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                var entity = entry.Entity;
                if (entity is ICreatedDate e1)
                {
                    base.Entry(e1).Property(x => x.CreatedDate).IsModified = false;
                }
                if (entity is IOwnable e2)
                {
                    base.Entry(e2).Reference(x => x.CreatedBy).IsModified = false;
                }
                if (entity is IUpdatedBy e3)
                {
                    e3.UpdatedBy = CurrentUser;
                }
                if (entity is IUpdatedDate e4)
                {
                    e4.UpdatedDate = now;
                }
            }
        }
    }
}
