using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        protected readonly EventSphereDbContext _context;
        public EventRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetEventByCategoryId(int eventCategoryId)
        {
            return await _context.Events.Where(u => u.CategoryID == eventCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventByOrganizerId(int organizerId)
        {
            return await _context.Events.Where(u => u.OrganizerID == organizerId).ToListAsync();

        }

        public async Task<IEnumerable<Event>> GetEventsByCity(string city)
        {
            return await _context.Events.Include(e => e.Location).Where(e => e.Location.City == city).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByCountry(string country)
        {
            return await _context.Events.Include(e => e.Location).Where(e => e.Location.Country == country).ToListAsync();
        }

        public async Task<string> GetOrganizerEmail(int id)
        {
            var email = await _context.Events
                .Where(e => e.ID == id)
                .Select(e => e.Organizer.Email)
                .FirstOrDefaultAsync();

            return email;
        }
    }
}
