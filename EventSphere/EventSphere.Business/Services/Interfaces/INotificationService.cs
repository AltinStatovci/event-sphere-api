    using EventSphere.Domain.Entities;

    namespace EventSphere.Business.Services.Interfaces;

    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, string message);
        Task MarkAsReadAsync(int notificationId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
    }