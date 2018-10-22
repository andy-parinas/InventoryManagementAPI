using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Data
{
    public interface ILocationRepository : IBaseRepository
    {
        Task<Location> GetLocation(int id);

        Task<Location> GetLocationByName(string name);

        Task<PageList<Location>> GetLocations(LocationParams locationParams);

        Task<LocationType> GetLocationType(int id);

        Task<ICollection<LocationType>> GetLocationTypes();

    }
}
