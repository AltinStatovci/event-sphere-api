using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services.Interfaces;

public interface ILogService
{
    Task<IEnumerable<Logg>> GetLogsByLevelAsync(string level);
    Task DeleteLogAsync(int id);
    Task DeleteAllLogsAsync();

}