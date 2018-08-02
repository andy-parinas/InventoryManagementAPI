using InventoryManagementAPI.Data;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InventoryManagementAPITests
{
    
    public class ProductRepositoryTests
    {
       
        [Fact]
        public async Task VerifyGetProducts()
        {

            //Arrange

            var products = new List<Product>
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

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IAsyncEnumerable<Product>>().Setup(m => m.GetEnumerator())
               .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider)
              .Returns(new TestAsyncQueryProvider<Product>(products.Provider));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var productParams = new ProductParams();

            var productRepo = new ProductRepository(mockContext.Object);


            //Act
            var repoResult = await productRepo.GetProducts(productParams);

            //Assert
            Assert.IsType<PageList<Product>>(repoResult);
            Assert.Equal(2, repoResult.Count);
        }

        [Fact]
        public async Task VerifyGetProduct()
        {
            //Arrange

            var products = new List<Product>
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

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IAsyncEnumerable<Product>>().Setup(m => m.GetEnumerator())
               .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider)
              .Returns(new TestAsyncQueryProvider<Product>(products.Provider));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var productParams = new ProductParams();

            var productRepo = new ProductRepository(mockContext.Object);


            //Act
            var repoResult = await productRepo.GetProduct(1);

            //Assert
            Assert.IsType<Product>(repoResult);
            Assert.Equal(1, repoResult.Id);
        }

        [Fact]
        public async Task VerifyGetCategories()
        {
            //Arrange
            var categories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    Id = 1,
                    Name = "Category 1"
                },
                new ProductCategory
                {
                    Id = 2,
                    Name = "Category 2"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ProductCategory>>();
            mockSet.As<IAsyncEnumerable<ProductCategory>>().Setup(m => m.GetEnumerator())
               .Returns(new TestAsyncEnumerator<ProductCategory>(categories.GetEnumerator()));

            mockSet.As<IQueryable<ProductCategory>>().Setup(m => m.Provider)
              .Returns(new TestAsyncQueryProvider<ProductCategory>(categories.Provider));

            mockSet.As<IQueryable<ProductCategory>>().Setup(m => m.Expression).Returns(categories.Expression);
            mockSet.As<IQueryable<ProductCategory>>().Setup(m => m.ElementType).Returns(categories.ElementType);
            mockSet.As<IQueryable<ProductCategory>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.ProductCategories).Returns(mockSet.Object);

            var productRepo = new ProductRepository(mockContext.Object);


            //Act

            var categoriesResult = await productRepo.GetProductCategories();

            //Assert
            Assert.IsAssignableFrom<ICollection<ProductCategory>>(categoriesResult);
            Assert.Equal(2, categoriesResult.Count);
        }


    }
}
