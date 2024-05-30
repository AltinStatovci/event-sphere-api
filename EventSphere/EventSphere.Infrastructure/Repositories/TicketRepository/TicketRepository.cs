using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories.TicketRepository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly EventSphereDbContext _context;
        public TicketRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ticket>> GetTicketByEvent(int eventId)
        {
            return await _context.Tickets.Where(u => u.EventID == eventId).ToListAsync();
        }

    }
}
