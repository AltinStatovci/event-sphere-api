using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories.TicketRepository
{
    public interface ITicketRepository
    {
        public Task<IEnumerable<Ticket>> GetTicketByEvent(int eventId);
    }
}
