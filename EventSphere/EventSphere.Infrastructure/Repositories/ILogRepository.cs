using EventSphere.Domain.Entities;

namespace EventSphere.Infrastructure.Repositories;

public interface ILogRepository :IGenericRepository<Logg>
{
    Task<IEnumerable<Logg>> GetLogsByLevel(string level);
    Task DeleteAllLogsAsync();
}