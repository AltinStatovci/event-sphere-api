using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    
    private readonly EventSphereDbContext _context;

    public NotificationRepository(EventSphereDbContext context)
    {
        _context = context;
    }


    public async Task AddNotification(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNotification(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<Notification> GetNotificationById(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task<IEnumerable<Notification>> GetUnReadNotificationsByUserId(int userId)
    {
        return await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
    }
}