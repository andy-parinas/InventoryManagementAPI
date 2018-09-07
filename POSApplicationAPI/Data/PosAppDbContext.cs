using Microsoft.EntityFrameworkCore;
using POSApplicationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Data
{
    public class PosAppDbContext : DbContext
    {
        public PosAppDbContext(DbContextOptions<PosAppDbContext> options): base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders);


            builder.Entity<OrderItem>()
                .HasOne(i => i.Order)
                .WithMany(o => o.OrderItems);


        }


    }
}
