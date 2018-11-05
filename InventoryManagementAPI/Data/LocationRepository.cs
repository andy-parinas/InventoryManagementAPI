using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Data
{
    public class LocationRepository : ILocationRepository
    {

        private readonly AppDbContext _dbContext;

        public LocationRepository(AppDbContext dbContext)
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

        public async Task<Location> GetLocation(int id)
        {
            var location = await _dbContext.Locations
                                        .Include(l => l.Inventories)
                                        .SingleOrDefaultAsync(l => l.Id == id);

            return location;
        }

        public async Task<Location> GetLocationByName(string name)
        {
            var location = await _dbContext.Locations
                                        .Include(l => l.Inventories)
                                        .SingleOrDefaultAsync(l => l.Name == name);

            return location;
        }

        public async Task<PageList<Location>> GetLocations(LocationParams locationParams)
        {
            var locations = _dbContext.Locations
                                    .Include(l => l.LocationType)
                                    .Where(l => l.IsArchived == false)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(locationParams.Name))
                locations = locations.Where(l => l.Name.Contains(locationParams.Name));

            if (!string.IsNullOrEmpty(locationParams.LocationType))
                locations = locations.Where(l => l.LocationType.Name == locationParams.LocationType);

            if(string.Equals(locationParams.Direction, "ASC"))
            {

                switch (locationParams.OrderBy.ToLower())
                {
                    case "name":
                        locations = locations.OrderBy(l => l.Name);
                        break;

                    case "address":
                        locations = locations.OrderBy(l => l.Address);
                        break;

                    case "locationtype":
                        locations = locations.OrderBy(l => l.LocationType.Name);
                        break;

                    default:
                        locations = locations.OrderBy(l => l.Name);
                        break;
                }

            }else
            {

                switch (locationParams.OrderBy.ToLower())
                {
                    case "name":
                        locations = locations.OrderByDescending(l => l.Name);
                        break;

                    case "address":
                        locations = locations.OrderByDescending(l => l.Address);
                        break;

                    case "locationtype":
                        locations = locations.OrderByDescending(l => l.LocationType.Name);
                        break;

                    default:
                        locations = locations.OrderByDescending(l => l.Name);
                        break;

                }
               

            }




            //return await locations.ToListAsync();
            return await PageList<Location>.CreateAsync(locations,
                locationParams.PageNumber, locationParams.PageSize);

        }

        public async Task<LocationType> GetLocationType(int id)
        {
            var locationType = await _dbContext.LocationTypes.Include(t => t.Locations).SingleOrDefaultAsync(t => t.Id == id);

            return locationType;
        }

        public async Task<ICollection<LocationType>> GetLocationTypes()
        {
            var locationTypes = await _dbContext.LocationTypes.ToListAsync();

            return locationTypes;
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
