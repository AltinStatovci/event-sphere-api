using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventsByIdAsync(int id);
        Task<Event> CreateEventsAsync(EventDTO eventDto, IFormFile image);
        Task<Event> UpdateEventsAsync(int id, EventDTO eventDto, IFormFile newImage = null);
        Task DeleteEventsAsync(int id);
        Task<int> GetEventCountAsync();
        Task<IEnumerable<Event>> GetEventByCategoryId(int id);

    }
}
