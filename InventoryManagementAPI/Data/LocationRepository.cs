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
            var location = await _dbContext.Locations.SingleOrDefaultAsync(l => l.Id == id);

            return location;
        }

        public async Task<ICollection<Location>> GetLocations(LocationParams locationParams)
        {
            var locations = _dbContext.Locations.Include(l => l.LocationType).AsQueryable();

            if (!string.IsNullOrEmpty(locationParams.Name))
                locations = locations.Where(l => l.Name == locationParams.Name);

            if (!string.IsNullOrEmpty(locationParams.LocationType))
                locations = locations.Where(l => l.LocationType.Name == locationParams.LocationType);

            return await locations.ToListAsync();

        }

        public async Task<LocationType> GetLocationType(int id)
        {
            var locationType = await _dbContext.LocationTypes.SingleOrDefaultAsync(t => t.Id == id);

            return locationType;
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
