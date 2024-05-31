using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Repositories.EventRepository;

namespace EventSphere.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IEventRepository _eventRepositori;

        public EventService(IGenericRepository<Event> eventRepository, IEventRepository eventRepositori)
        {
            _eventRepository = eventRepository;
            _eventRepositori = eventRepositori;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event> GetEventsByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event> CreateEventsAsync(EventDTO eventDto)
        {
            var events = new Event
            {
                EventName = eventDto.EventName,
                Description = eventDto.Description,
                Location = eventDto.Location,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                CategoryID = eventDto.CategoryID,
                OrganizerID = eventDto.OrganizerID,
                PhotoData = eventDto.PhotoData,
                MaxAttendance = eventDto.MaxAttendance,
                AvailableTickets = eventDto.AvailableTickets,
                DateCreated = eventDto.DateCreated
            };
            await _eventRepository.AddAsync(events);
            return events;
        }

        public async Task UpdateEventsAsync(int id, EventDTO eventDto)
        {
            var eventById = await _eventRepository.GetByIdAsync(id);
            if (eventById == null) return;

            eventById.EventName = eventDto.EventName;
            eventById.Description = eventDto.Description;
            eventById.Location = eventDto.Location;
            eventById.StartDate = eventDto.StartDate;
            eventById.EndDate = eventDto.EndDate;
            eventById.CategoryID = eventDto.CategoryID;
            eventById.OrganizerID = eventDto.OrganizerID;
            eventById.PhotoData = eventDto.PhotoData;
            eventById.MaxAttendance = eventDto.MaxAttendance;
            eventById.AvailableTickets = eventDto.AvailableTickets;
            eventById.DateCreated = eventDto.DateCreated;

            await _eventRepository.UpdateAsync(eventById);
        }

        public async Task DeleteEventsAsync(int id)
        {
            await _eventRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Event>> GetEventByCategoryId(int id)
        {
            return await _eventRepositori.GetEventByCategoryId(id);
        }
    }
}
