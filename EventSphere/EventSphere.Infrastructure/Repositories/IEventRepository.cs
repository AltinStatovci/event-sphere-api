using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventByCategoryId(int eventCategoryId);
        Task<IEnumerable<Event>> GetEventByOrganizerId(int organizerId);
        Task<IEnumerable<Event>> GetEventsByCity(string city);
        Task<IEnumerable<Event>> GetEventsByCountry(string country);
        Task<string> GetOrganizerEmail(int id);
    }
}
