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
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
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

        public async Task<IEnumerable<Location>> GetLocationsByCityAsync(string city)
        {
            return await _locationRepository.GetLocationsByCity(city);
        }

        public async Task<IEnumerable<Location>> GetLocationsByCountryAsync(string country)
        {
            return await _locationRepository.GetLocationsByCountry(country);
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
