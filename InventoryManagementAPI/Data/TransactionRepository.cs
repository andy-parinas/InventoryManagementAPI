using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class TransactionRepository : ITransactionRepository
    {


        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext)
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

        public async Task<InventoryTransaction> GetInventoryTransaction(int id)
        {
            var transaction = await _dbContext.InventoryTransactions.Include(t => t.TransactionType).SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public async Task<PageList<InventoryTransaction>> GetInventoryTransactions(TransactionParams transactionParams)
        {
            var transactionQuery = _dbContext.InventoryTransactions.Include(t => t.TransactionType).AsQueryable();

            return await PageList<InventoryTransaction>.CreateAsync(transactionQuery, transactionParams.PageNumber, transactionParams.PageSize);


        }

        public async Task<PageList<InventoryTransaction>> GetTransactionsByInventoryId(int id, TransactionParams transactionParams)
        {
            var transactionQuery = _dbContext.InventoryTransactions
                                                .Include(t => t.TransactionType)
                                                .Where(t => t.Inventory.Id == id).AsQueryable();

            return await PageList<InventoryTransaction>.CreateAsync(transactionQuery, transactionParams.PageNumber, transactionParams.PageSize);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
