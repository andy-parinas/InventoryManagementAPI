using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface IInventoryTransactionRepository: IBaseRepository
    {
        Task<PageList<InventoryTransaction>> GetTransactions(PageParams pageParams);

        Task<InventoryTransaction> GetTransaction(int id);

        Task<PageList<InventoryTransaction>> GetTransactions(int inventoryId, PageParams pageParams);
    }
}
