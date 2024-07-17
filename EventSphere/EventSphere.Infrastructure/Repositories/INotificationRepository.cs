using EventSphere.Domain.Entities;

namespace EventSphere.Infrastructure.Repositories;

public interface INotificationRepository
{
 Task AddNotification(Notification notification);
 Task UpdateNotification(Notification notification);
 Task<Notification> GetNotificationById(int id);
 Task<IEnumerable<Notification>> GetUnReadNotificationsByUserId(int userId);
 
}