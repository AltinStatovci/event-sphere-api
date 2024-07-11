using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketByEventId(int eventId);
        Task<int> GetTotalTicketsAsync(int eventId);
        Task<int> GetTotalTicketsUpdateAsync(int eventId, int excludeTicketId);
    }
}
