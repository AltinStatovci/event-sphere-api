using EventSphere.Domain.Entities;

namespace EventSphere.Domain.IRepositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task AddAsync(Event events);
        Task UpdateAsync(Event events);
        Task DeleteAsync(int id);
    }
}
