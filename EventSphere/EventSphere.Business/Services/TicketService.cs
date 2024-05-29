using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class TicketService: ITicketService
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;
        public TicketService(IGenericRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ticket> CreateAsync(TicketDTO Tid)
        {
            var ticket = new Ticket
            {
                EventID = Tid.EventID,
                TicketType = Tid.TicketType,
                Price = Tid.Price,
                BookingReference = Tid.BookingReference,
            };
            await _ticketRepository.AddAsync(ticket);
            return ticket;
        }

        public async Task DeleteAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, TicketDTO Tid)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            ticket.EventID = Tid.EventID;
            ticket.TicketType = Tid.TicketType;
            ticket.Price = Tid.Price;
            ticket.BookingReference = Tid.BookingReference;

            await _ticketRepository.UpdateAsync(ticket);
        }
    }
}
