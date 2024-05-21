using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Domain.IRepositories;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class TicketServices : ITicketServices
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketServices(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<Ticket> AddTicketAsync(TicketDTO Tid)
        {
            var ticket = new Ticket
            {
                ID = Tid.ID,
                EventID = Tid.EventID,
                UserID = Tid.UserID,
                TicketType = Tid.TicketType,
                Price = Tid.Price,
                DatePurchased = Tid.DatePurchased

            };
            await _ticketRepository.AddAsync(ticket);
            return ticket;
            
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<Ticket> UdpateTicketAsync(int id, TicketDTO Tid)
        {
            var tick = await _ticketRepository.GetByIdAsync(id);
            tick.ID = Tid.ID;
            tick.EventID = Tid.EventID;
            tick.UserID = Tid.UserID;
            tick.TicketType = Tid.TicketType;
            tick.Price = Tid.Price;
            tick.DatePurchased = Tid.DatePurchased;
            await _ticketRepository.UdpateAsync(tick);
            return tick;
        }
    }
}
