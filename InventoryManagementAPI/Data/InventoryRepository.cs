using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class InventoryRepository : IInventoryRepository
    {

        private readonly AppDbContext _dbContext;

        public InventoryRepository(AppDbContext dbContext)
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

        public async Task<PageList<Inventory>> GetInventories(InventoryParams inventoryParams)
        {

            var inventoriesQuery = _dbContext.Inventories
                                        .Include(i => i.Product)
                                        .Include(i => i.Location)
                                        .Include(i => i.Status)
                                        .Where(i => i.IsArchived == false)
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(inventoryParams.Sku))
                inventoriesQuery = inventoriesQuery.Where(i => i.Sku.Contains(inventoryParams.Sku));

            if (!string.IsNullOrEmpty(inventoryParams.Product))
                inventoriesQuery = inventoriesQuery.Where(i => i.Product.Name.Contains(inventoryParams.Product));


            return await PageList<Inventory>.CreateAsync(inventoriesQuery, inventoryParams.PageNumber, inventoryParams.PageSize);

        }

        public async Task<Inventory> GetInventory(int id)
        {
            var inventory = await _dbContext.Inventories
                                            .Include(i => i.Product)
                                            .Include(i => i.Location)
                                            .Include(i => i.Status)
                                            .SingleOrDefaultAsync(i => i.Id == id);

            return inventory;
        }

        public async Task<Inventory> GetInventoryBySku(string sku)
        {
            var inventory = await _dbContext.Inventories.SingleOrDefaultAsync(i => i.Sku == sku);

            return inventory;
        }

        public async Task<ICollection<InventoryStatus>> GetInventoryStatuses()
        {
            var inventoryStatuses = await _dbContext.InventoryStatuses.ToListAsync();

            return inventoryStatuses;
        }

        public async Task<InventoryStatus> GetInventoryStatusByName(string name)
        {
            var status = await _dbContext.InventoryStatuses.SingleOrDefaultAsync(s => s.Status == name);

            return status;
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

     
    }
}
