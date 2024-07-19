using EventSphere.API.Hubs;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services;

public class NotificationServiceTest
{
       private readonly INotificationService _notificationService;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<IHubContext<NotificationHub>> _mockHubContext;

        public NotificationServiceTest()
        {
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockHubContext = new Mock<IHubContext<NotificationHub>>();

            _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockHubContext.Object);
        }

        

        [Fact]
        public async Task MarkAsReadAsync_OnSuccess_ShouldUpdateNotification()
        {
            // Arrange
            int notificationId = 1;
            var notification = new Notification { Id = notificationId, IsRead = false };

            _mockNotificationRepository.Setup(repo => repo.GetNotificationById(notificationId)).ReturnsAsync(notification);
            _mockNotificationRepository.Setup(repo => repo.UpdateNotification(notification)).Returns(Task.CompletedTask);

            // Act
            await _notificationService.MarkAsReadAsync(notificationId);

            // Assert
            Assert.True(notification.IsRead);
            _mockNotificationRepository.Verify(repo => repo.UpdateNotification(notification), Times.Once);
        }

        [Fact]
        public async Task MarkAsReadAsync_OnNotificationNotFound_ShouldThrowException()
        {
            // Arrange
            var notificationId = 1;
            var exceptionMsg = "Notification not found";

            _mockNotificationRepository.Setup(repo => repo.GetNotificationById(notificationId))!.ReturnsAsync((Notification)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _notificationService.MarkAsReadAsync(notificationId));
            Assert.Equal(exceptionMsg, exception.Message);
        }

        [Fact]
        public async Task GetUnreadNotificationsAsync_OnSuccess_ShouldReturnUnreadNotifications()
        {
            // Arrange
            int userId = 1;
            var expectedNotifications = new List<Notification>
            {
                new Notification { UserId = userId, IsRead = false },
                new Notification { UserId = userId, IsRead = false }
            };

            _mockNotificationRepository.Setup(repo => repo.GetUnReadNotificationsByUserId(userId)).ReturnsAsync(expectedNotifications);

            // Act
            var result = await _notificationService.GetUnreadNotificationsAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNotifications, result);
        }

        [Fact]
        public async Task MarkAllAsReadAsync_OnSuccess_ShouldMarkAllNotificationsAsRead()
        {
            // Arrange
            int userId = 1;

            _mockNotificationRepository.Setup(repo => repo.MarkAllAsReadAsync(userId)).Returns(Task.CompletedTask);

            // Act
            await _notificationService.MarkAllAsReadAsync(userId);

            // Assert
            _mockNotificationRepository.Verify(repo => repo.MarkAllAsReadAsync(userId), Times.Once);
        }
    }
