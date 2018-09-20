using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface IInventoryRepository : IBaseRepository
    {

        Task<PageList<Inventory>> GetInventories(InventoryParams inventoryParams);

        Task<Inventory> GetInventory(int id);

        Task<Inventory> GetInventoryBySku(string sku);

        Task<ICollection<InventoryStatus>> GetInventoryStatuses();

        Task<InventoryStatus> GetInventoryStatusByName(string name);
    }
}
