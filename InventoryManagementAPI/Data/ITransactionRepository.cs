using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface ITransactionRepository : IBaseRepository
    {
        Task<PageList<InventoryTransaction>> GetInventoryTransactions(TransactionParams transactionParams);

        Task<InventoryTransaction> GetInventoryTransaction(int id);

        Task<PageList<InventoryTransaction>> GetTransactionsByInventoryId(int id, TransactionParams transactionParams);


    }
}
