using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventsByIdAsync(int id);
        Task<Event> CreateEventsAsync(EventDTO eventDto);
        Task UpdateEventsAsync(int id, EventDTO eventDto);
        Task DeleteEventsAsync(int id);
        Task<int> GetEventCountAsync();
    }
}
