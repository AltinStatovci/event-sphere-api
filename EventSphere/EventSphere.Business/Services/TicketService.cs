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

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        public TicketService(ITicketRepository ticketRepository,
            IGenericRepository<Event> eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
        }

        public async Task<Ticket> CreateAsync(TicketDTO Tid)
        {
            if (Tid == null || Tid.Price <= 0)
            {
                throw new ArgumentException("Price cannot be equal to or less then 0");
            }

            var events = await _eventRepository.GetByIdAsync(Tid.EventID);


            var eventsName = events.EventName;

            var totalTickets = await _ticketRepository.GetTotalTicketsAsync(Tid.EventID);

            if (totalTickets + Tid.TicketAmount > events.MaxAttendance)
            {
                throw new ArgumentException("Ticket amount cannot exceed the maximum attendance of the event.");
            }

            var ticket = new Ticket
            {
                EventID = Tid.EventID,
                EventName = eventsName,
                TicketType = Tid.TicketType,
                Price = Tid.Price,
                TicketAmount = Tid.TicketAmount,
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

        public async Task<IEnumerable<Ticket>> GetTicketByEventIdAsync(int eventId)
        {
            return await _ticketRepository.GetTicketByEventId(eventId);
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, TicketDTO Tid)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            var events = await _eventRepository.GetByIdAsync(Tid.EventID);


            var totalTickets = await _ticketRepository.GetTotalTicketsUpdateAsync(Tid.EventID, id);

            if (totalTickets + Tid.TicketAmount > ticket.Event.MaxAttendance)
            {
                throw new InvalidOperationException("Total tickets for the event cannot exceed MaxAttendance.");
            }

            ticket.EventID = Tid.EventID;
            ticket.TicketType = Tid.TicketType;
            ticket.Price = Tid.Price;
            ticket.TicketAmount = Tid.TicketAmount;
            ticket.BookingReference = Tid.BookingReference;

            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task<int> GetTicketCountAsync()
        {
            return await _ticketRepository.CountAsync();
        }
    }
}
