using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Remove(entity);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _dbContext.Products
                            .Include(p => p.ProductCategory)
                            .SingleOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<ICollection<ProductCategory>> GetProductCategories()
        {
            var categories = await _dbContext.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetProductCategory(int id)
        {
            var category = await _dbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<PageList<Product>> GetProducts(ProductParams productParams)
        {
            var products = _dbContext.Products.Include(p => p.ProductCategory).AsQueryable();

            return await PageList<Product>.CreateAsync(products, productParams.PageNumber, productParams.PageSize);

        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
