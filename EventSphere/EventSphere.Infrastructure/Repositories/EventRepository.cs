using EventSphere.Domain.Entities;
using EventSphere.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EventSphere.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventSphereDbContext _context;

        public EventRepository(EventSphereDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.Include(e => e.Organizer).ToListAsync();
              
        }


        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task AddAsync(Event events)
        {
            await _context.Events.AddAsync(events);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event events)
        {
            _context.Events.Update(events);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events != null)
            {
                _context.Events.Remove(events);
                await _context.SaveChangesAsync();
            }
        }
    }
}
