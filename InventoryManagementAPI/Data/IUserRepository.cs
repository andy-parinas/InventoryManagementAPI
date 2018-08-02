using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);

        Task<bool> Save();
    }
}
