﻿namespace BlazorShop.Data
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Interfaces;
    using Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrdersProducts { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<City> Cities { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        private void ApplyAuditInfoRules()
        {
            var entries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added ||
                     e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            var entries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IDeletableEntity &&
                    e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.UtcNow;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}