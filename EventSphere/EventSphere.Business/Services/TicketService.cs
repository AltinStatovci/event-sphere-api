using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class TicketServiceBase
    {
        protected readonly EventSphereDbContext _context;

        public TicketServiceBase(EventSphereDbContext context)
        {
            _context = context;
        }
    }
    public class TicketService : TicketServiceBase, ITicketService
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        public TicketService(EventSphereDbContext context,
            IGenericRepository<Ticket> ticketRepository,
            IGenericRepository<Event> eventRepository) : base(context)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
        }

        public async Task<Ticket> CreateAsync(TicketDTO Tid)
        {
            var events = await _eventRepository.GetByIdAsync(Tid.EventID);
            var eventsName = events.EventName;
            
            var ticket = new Ticket
            {
                EventID = Tid.EventID,
                EventName = eventsName,
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

        public async Task<IEnumerable<Ticket>> GetTicketByEventId(int eventId)
        {
            return await _context.Tickets.Where(u => u.EventID == eventId).ToListAsync();
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

        public async Task<int> GetTicketCountAsync()
        {
            return await _ticketRepository.CountAsync();
        }



    }
}
