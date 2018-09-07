using POSApplicationAPI.Helpers;
using POSApplicationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Data
{
    public interface ICustomerRepository : IBaseRepository
    {
        Task<PageList<Customer>> GetCustomers(CustomerParams customerParams);

        Task<Customer> GetCustomer(int id);

        Task<PageList<Order>> GetCustomerOrders(int id, PageParams pageParams);


    }
}
