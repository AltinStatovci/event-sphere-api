using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface ITicketServices
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<Ticket> CreateAsync(TicketDTO Tid);
        Task UpdateAsync(int id, TicketDTO Tid);
        Task DeleteAsync(int id);

        Task<int> GetTicketCountAsync();

        Task<IEnumerable<Ticket>> GetTicketByEventId(int eventId);


    }
}
