using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface ILocationServices
    {
        Task<Location> AddLocation(Location location);
        Task UpdateLocation(Location location);
        Task DeleteLocation(int id);
        Task<IEnumerable<Location>> GetAllLocations();
        Task<Location> GetLocationById(int id);
        Task<IEnumerable<Location>> GetLocationsByCity(string city);
        Task<IEnumerable<Location>> GetLocationsByCountry(string country);

    }
}
