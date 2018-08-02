using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagementAPITests
{
    public class MyFixture : IDisposable
    {
        public IQueryable Products { get; set; }

        public MyFixture()
        {
            this.Products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Upc= "1234567890",
                    Name = "Sample Product 1",
                    Descriptions = "This is a sample Product",
                    Price = 100.00,
                    ProductCategory = new ProductCategory
                    {
                        Id = 1,
                        Name ="Category 1"
                    },
                    Inventories = new List<Inventory>{ }
                },
                new Product
                {
                    Id = 2,
                    Upc= "1234567890",
                    Name = "Sample Product 2",
                    Descriptions = "This is a sample Product",
                    Price = 100.00,
                    ProductCategory = new ProductCategory
                    {
                        Id = 1,
                        Name ="Category 1"
                    },
                    Inventories = new List<Inventory>{ }
                }

            }.AsQueryable();

        }

        public void Dispose()
        {

            this.Products = null;
        }
    }
}
