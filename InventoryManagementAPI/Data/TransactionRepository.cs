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
            var transaction = await _dbContext.InventoryTransactions
                                                    .Include(t => t.TransactionType)
                                                    .Include(t => t.Inventory)
                                                    .Where(t => t.IsArchived == false)
                                                    .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public async Task<PageList<InventoryTransaction>> GetInventoryTransactions(TransactionParams transactionParams)
        {
            var transactionQuery = _dbContext.InventoryTransactions.Include(t => t.TransactionType).AsQueryable();

            return await PageList<InventoryTransaction>.CreateAsync(transactionQuery, transactionParams.PageNumber, transactionParams.PageSize);


        }

        public async Task<PageList<InventoryTransaction>> GetTransactionsByInventoryId(int id, 
            TransactionParams transactionParams)
        {
            var transactionQuery = _dbContext.InventoryTransactions
                                                .Include(t => t.TransactionType)
                                                .Where(t => t.IsArchived == false)
                                                .Where(t => t.Inventory.Id == id).AsQueryable();

            return await PageList<InventoryTransaction>.CreateAsync(transactionQuery, transactionParams.PageNumber, 
                transactionParams.PageSize);
        }

        public async Task<ICollection<TransactionType>> GetTransactionTypes()
        {
            var transactionTypes = await _dbContext.TransactionTypes.ToListAsync();


            return transactionTypes;
        }

        public async Task<TransactionType> GetTransactionType(int id)
        {
            var transactionType = await _dbContext.TransactionTypes.SingleOrDefaultAsync(t => t.Id == id);

            return transactionType;
        }

        public async Task<bool> Save()
        {
            
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return true;

                }catch(Exception e)
                {

                    transaction.Rollback();

                    return false;
                }
            }
            


        }

        public Task<bool> AddTransaction(Inventory inventory, InventoryTransaction inventoryTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
