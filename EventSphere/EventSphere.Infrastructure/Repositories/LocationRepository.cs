using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        protected EventSphereDbContext _context;
        public LocationRepository(EventSphereDbContext context) : base(context)
        {

            _context = context;

        }
        public async Task<IEnumerable<Location>> GetLocationsByCity(string city)
        {
            return await _context.Locations.Where(u => u.City == city).ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByCountry(string country)
        {
            return await _context.Locations.Where(u => u.Country == country).ToListAsync();
        }
    }
}
