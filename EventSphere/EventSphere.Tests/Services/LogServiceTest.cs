using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services
{
    public class LogServiceTest
    {
        private readonly ILogService _logService;
        private readonly Mock<ILogRepository> _mockLogRepository;

        public LogServiceTest()
        {
            _mockLogRepository = new Mock<ILogRepository>();
            _logService = new LogService(_mockLogRepository.Object);
        }

        [Fact]
        public async Task GetLogsByLevelAsync_ShouldReturnLogsByLevel()
        {
            // Arrange
            var level = "Error";
            var logs = new List<Logg>
            {
                new Logg
                {
                    Id = 1,
                    Message = "Error message 1",
                    Level = "Error",
                    TimeStamp = DateTime.Now
                },
                new Logg
                {
                    Id = 2,
                    Message = "Error message 2",
                    Level = "Error",
                    TimeStamp = DateTime.Now
                }
            };

            _mockLogRepository.Setup(repo => repo.GetLogsByLevel(level)).ReturnsAsync(logs);

            // Act
            var result = await _logService.GetLogsByLevelAsync(level);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, log => Assert.Equal(level, log.Level));
        }

        [Fact]
        public async Task DeleteLogAsync_ShouldDeleteLog()
        {
            // Arrange
            var logId = 1;

            _mockLogRepository.Setup(repo => repo.DeleteAsync(logId)).Returns(Task.CompletedTask);

            // Act
            await _logService.DeleteLogAsync(logId);

            // Assert
            _mockLogRepository.Verify(repo => repo.DeleteAsync(logId), Times.Once);
        }

        [Fact]
        public async Task DeleteAllLogsAsync_ShouldDeleteAllLogs()
        {
            // Arrange
            _mockLogRepository.Setup(repo => repo.DeleteAllLogsAsync()).Returns(Task.CompletedTask);

            // Act
            await _logService.DeleteAllLogsAsync();

            // Assert
            _mockLogRepository.Verify(repo => repo.DeleteAllLogsAsync(), Times.Once);
        }
    }
}
