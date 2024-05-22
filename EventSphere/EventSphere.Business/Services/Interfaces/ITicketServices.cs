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
        Task<Ticket> AddTicketAsync(TicketDTO Tid);
        Task<Ticket> UdpateTicketAsync(int id, TicketDTO Tid);
        Task DeleteTicketAsync(int id);
    }
}