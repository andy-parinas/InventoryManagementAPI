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

        public async Task<Product> GetProductByName(string name)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Name == name);

            return product;
        }

        public async Task<ProductCategory> GetProductCategory(int id)
        {
            var category = await _dbContext.ProductCategories
                                        .Include(c => c.Products)
                                        .SingleOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task<PageList<Product>> GetProducts(ProductParams productParams)
        {
            var products = _dbContext.Products
                                        .Include(p => p.ProductCategory)
                                        .Where(p => p.IsArchived == false)
                                        .AsQueryable();

            if (string.Equals(productParams.Direction, "ASC"))
            {
                switch (productParams.OrderBy.ToLower())
                {
                    case "name":
                        products = products.OrderBy(p => p.Name);
                        break;

                    case "category":
                        products = products.OrderBy(p => p.ProductCategory.Name);
                        break;

                    case "upc":
                        products = products.OrderBy(p => p.Upc);
                        break;

                    case "price":
                        products = products.OrderBy(p => p.Price);
                        break;

                    default:
                        products = products.OrderBy(p => p.Name);
                        break;
                }

            }
            else
            {
                switch (productParams.OrderBy.ToLower())
                {
                    case "name":
                        products = products.OrderByDescending(p => p.Name);
                        break;

                    case "category":
                        products = products.OrderByDescending(p => p.ProductCategory.Name);
                        break;

                    case "upc":
                        products = products.OrderByDescending(p => p.Upc);
                        break;

                    case "price":
                        products = products.OrderByDescending(p => p.Price);
                        break;

                    default:
                        products = products.OrderByDescending(p => p.Name);
                        break;
                }

            }

            if (!string.IsNullOrEmpty(productParams.Name))
                products = products.Where(p => p.Name.Contains(productParams.Name));

            return await PageList<Product>.CreateAsync(products, productParams.PageNumber, productParams.PageSize);

        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<PageList<Inventory>> GetProductInventories(Product product, PageParams pageParams)
        {

            var inventories = _dbContext.Inventories
                                        .Where(i => i.Product == product)
                                        .Where(i => i.IsArchived == false)
                                        .AsQueryable();

            return await PageList<Inventory>.CreateAsync(inventories, pageParams.PageNumber, pageParams.PageSize);

        }


        public async Task<ICollection<ProductCategory>> GetProductCategories()
        {
            var categories = await _dbContext.ProductCategories.ToListAsync();

            return categories;
        }




    }
}
