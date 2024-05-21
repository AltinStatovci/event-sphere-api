using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Domain.IRepositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllAsync();
        Task<Ticket>GetByIdAsync(int id);
        Task AddAsync(Ticket Tid);
        Task UdpateAsync(Ticket Tid);
        Task DeleteAsync(int id);
    }
}
