using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Infrastructure.Repositories;

public class LogRepository : GenericRepository<Logg> , ILogRepository

{
    private readonly EventSphereDbContext _context;
    
    public LogRepository(EventSphereDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Logg>> GetLogsByLevel(string level)
    {
        return await _context.Logs.Where(l => l.Level == level).ToListAsync();
    }

    public  Task DeleteAllLogsAsync()
    {
        _context.Logs.RemoveRange(_context.Logs);
        return  _context.SaveChangesAsync();
    }
}