using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        protected readonly EventSphereDbContext _context;
        public TicketRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ticket>> GetTicketByEventId(int eventId)
        {
            return await _context.Tickets.Where(u => u.EventID == eventId).ToListAsync();
        }

        public async Task<int> GetTotalTicketsAsync(int eventId)
        {
            return await _context.Tickets
               .Where(t => t.EventID == eventId)
               .SumAsync(t => t.TicketAmount);
        }

        public async Task<int> GetTotalTicketsUpdateAsync(int eventId, int excludeTicketId)
        {
            return await _context.Tickets
                .Where(t => t.EventID == eventId && t.ID != excludeTicketId)
                .SumAsync(t => t.TicketAmount);
        }
    }
}
