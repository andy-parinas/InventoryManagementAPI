using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Inventory> Inventories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products);

            builder.Entity<Location>()
                .HasOne(l => l.LocationType)
                .WithMany(t => t.Locations);

            builder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Inventories);

            builder.Entity<Inventory>()
                .HasOne(i => i.Location)
                .WithMany(l => l.Inventories);

        }


    }
}
