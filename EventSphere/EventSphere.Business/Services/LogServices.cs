using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;

namespace EventSphere.Business.Services;

public class LogServices :ILogServices
{
    private readonly ILogRepository _logRepository;

    public LogServices(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<IEnumerable<Logg>> GetLogsByLevelAsync(string level)
    {
        return await _logRepository.GetLogsByLevel(level); 
    }

    public async Task DeleteLogAsync(int id)
    {
        await _logRepository.DeleteAsync(id);
    }

    public async Task DeleteAllLogsAsync()
    {
        await _logRepository.DeleteAllLogsAsync();
    }
}