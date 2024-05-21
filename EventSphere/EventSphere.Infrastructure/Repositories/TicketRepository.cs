using EventSphere.Domain.Entities;
using EventSphere.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly EventSphereDbContext _db;
        public TicketRepository(EventSphereDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Ticket Tid)
        {
            await _db.Tickets.AddAsync(Tid);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tick = await _db.Tickets.FindAsync(id);
            if (tick != null)
            {
                _db.Tickets.Remove(tick);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _db.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
           return await _db.Tickets.FindAsync(id);
        }

        public async Task UdpateAsync(Ticket Tid)
        {
            _db.Tickets.Update(Tid);
            await _db.SaveChangesAsync();
        }
    }
}
