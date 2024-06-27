using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class LocationServiceBase
    {
        protected readonly EventSphereDbContext _context;

        public LocationServiceBase(EventSphereDbContext context)
        {
            _context = context;
        }
    }
    public class LocationServices : LocationServiceBase, ILocationServices
    {
        private readonly IGenericRepository<Location> _locationRepository;
        public LocationServices(EventSphereDbContext context, IGenericRepository<Location> locationRepository) : base(context) 
        { 
           _locationRepository = locationRepository;
        }
        public async Task<Location> AddLocation(Location location)
        {
           return await _locationRepository.AddAsync(location);
        }

        public async Task DeleteLocation(int id)
        {
           await _locationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByCity(string city)
        {
            return await _context.Locations.Where(u => u.City == city).ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByCountry(string country)
        {
            return await _context.Locations.Where(u => u.Country == country).ToListAsync();
        }

        public async Task<Location> GetLocationById(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task UpdateLocation(Location location)
        {
            await _locationRepository.UpdateAsync(location);
        }
    }
}
