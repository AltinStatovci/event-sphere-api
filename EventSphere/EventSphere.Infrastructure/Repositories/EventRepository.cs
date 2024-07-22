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

        public async Task<IEnumerable<Event>> GetEventsByDate(DateTime date)
        {
            return await _context.Events.Where(e => e.ScheduleDate <= date && e.IsApproved == true && e.EndDate >= date).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateTime(DateTime date)
        {
            return await _context.Events.Where(e => e.ScheduleDate >= date && e.EndDate >= date).ToListAsync();
        }

        public async Task<string> GetOrganizerEmail(int id)
        {
            var email = await _context.Events
                .Where(e => e.ID == id)
                .Select(e => e.Organizer.Email)
                .FirstOrDefaultAsync();

            return email;
        }
        public async Task<IEnumerable<Event>> GetEventsNearbyAsync(double latitude, double longitude, double radiusInKm)
        {
            var events = await _context.Events
                .Include(e => e.Location)
                .ToListAsync();

            var nearbyEvents = events.Where(e =>
                CalculateDistance(latitude, longitude, e.Location.Latitude, e.Location.Longitude) < radiusInKm
            ).ToList();

            return nearbyEvents;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;
            return Math.Sqrt(dLat * dLat + dLon * dLon) * 111; // Approximation: 1 degree ~ 111 km
        }

    }
}
