using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POSApplicationAPI.Helpers;
using POSApplicationAPI.Models;

namespace POSApplicationAPI.Data
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly PosAppDbContext _dbContext;

        public CustomerRepository(PosAppDbContext dbContext)
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

        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await _dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);

            return customer;
        }

        public async Task<PageList<Order>> GetCustomerOrders(int id, PageParams pageParams)
        {
            //var customer = await _dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);

            var orders = _dbContext.Orders.Where(o => o.Customer.Id == id).AsQueryable();

            return await PageList<Order>.CreateAsync(orders, pageParams.PageNumber, pageParams.PageSize);
            
        }

        public async Task<PageList<Customer>> GetCustomers(CustomerParams customerParams)
        {
            var customers = _dbContext.Customers.AsQueryable();

            return await PageList<Customer>.CreateAsync(customers, customerParams.PageNumber, customerParams.PageSize);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
