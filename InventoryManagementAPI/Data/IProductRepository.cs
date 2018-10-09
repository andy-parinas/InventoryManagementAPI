using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface IProductRepository : IBaseRepository
    {
        Task<PageList<Product>> GetProducts(ProductParams productParams);

        Task<Product> GetProduct(int id);

        Task<Product> GetProductByName(string name);

        Task<ICollection<ProductCategory>> GetProductCategories();

        Task<ProductCategory> GetProductCategory(int id);

        Task<PageList<Inventory>> GetProductInventories(Product product, PageParams pageParams);
    }
}
