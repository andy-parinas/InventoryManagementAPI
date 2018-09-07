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

        public AppDbContext(){}

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<InventoryStatus> InventoryStatuses { get; set; }



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

            builder.Entity<Inventory>()
                .HasOne(i => i.Status)
                .WithMany(s => s.Inventories);

            builder.Entity<InventoryTransaction>()
                .HasOne(t => t.TransactionType)
                .WithMany(y => y.Transactions);

            builder.Entity<InventoryTransaction>()
                .HasOne(t => t.Inventory)
                .WithMany(i => i.Transactions);

        }


    }
}
