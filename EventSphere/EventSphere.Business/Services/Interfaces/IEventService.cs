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
        Task<IEnumerable<Event>> GetEventByCategoryIdAsync(int id);
        Task<IEnumerable<Event>> GetEventByOrganizerIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsByCityAsync(string city);
        Task<IEnumerable<Event>> GetEventsByCountryAsync(string country);
        Task<IEnumerable<Event>> GetEventsByNameAsync(string name);
        Task<Event> UpdateEventStatus(int id);

        Task<IEnumerable<Event>> GetEventsByDate(DateTime date);
        Task<IEnumerable<Event>> GetEventsByDateTime(DateTime date);

        Task<string> GetOrganizerEmailAsync(int id);
        Task UpdateMessage(int id, string message);

    }
}
