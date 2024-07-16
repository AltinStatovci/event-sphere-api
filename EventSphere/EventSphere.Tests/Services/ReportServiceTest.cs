using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services
{
    public class ReportServiceTest
    {
        private readonly IReportService _reportService;
        private readonly Mock<IGenericRepository<Report>> _mockReportRepository;

        public ReportServiceTest()
        {
            _mockReportRepository = new Mock<IGenericRepository<Report>>();
            _reportService = new ReportService(_mockReportRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateReport()
        {
            // Arrange
            var reportDTO = new ReportDTO
            {
                userId = 1,
                userName = "Eliza",
                userlastName = "Bleta",
                userEmail = "eliza.bleta@gmail.com",
                reportName = "Test Report",
                reportDesc = "This is a test report.",
                reportAnsw = "Waiting for Respond"
            };

            var report = new Report
            {
                userId = reportDTO.userId,
                userName = reportDTO.userName,
                userlastName = reportDTO.userlastName,
                userEmail = reportDTO.userEmail,
                reportName = reportDTO.reportName,
                reportDesc = reportDTO.reportDesc,
                reportAnsw = reportDTO.reportAnsw
            };

            _mockReportRepository.Setup(repo => repo.AddAsync(It.IsAny<Report>())).ReturnsAsync(report);

            // Act
            var result = await _reportService.CreateAsync(reportDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportDTO.userId, result.userId);
            Assert.Equal(reportDTO.userName, result.userName);
            Assert.Equal(reportDTO.userlastName, result.userlastName);
            Assert.Equal(reportDTO.userEmail, result.userEmail);
            Assert.Equal(reportDTO.reportName, result.reportName);
            Assert.Equal(reportDTO.reportDesc, result.reportDesc);
            Assert.Equal(reportDTO.reportAnsw, result.reportAnsw);

            _mockReportRepository.Verify(repo => repo.AddAsync(It.IsAny<Report>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateReport()
        {
            // Arrange
            var reportDTO = new ReportDTO
            {
                userId = 1,
                userName = "Eliza",
                userlastName = "Bleta",
                userEmail = "eliza.bleta@gmail.com",
                reportName = "Updated Report",
                reportDesc = "This is an updated report.",
                reportAnsw = "Responded"
            };

            var report = new Report
            {
                reportId = 1,
                userId = 1,
                userName = "Eliza",
                userlastName = "Bleta",
                userEmail = "eliza.bleta@gmail.com",
                reportName = "Test Report",
                reportDesc = "This is a test report.",
                reportAnsw = "Waiting for Respond"
            };

            _mockReportRepository.Setup(repo => repo.GetByIdAsync(report.reportId)).ReturnsAsync(report);
            _mockReportRepository.Setup(repo => repo.UpdateAsync(report)).Returns(Task.CompletedTask);

            // Act
            await _reportService.UpdateAsync(report.reportId, reportDTO);

            // Assert
            Assert.Equal(reportDTO.userId, report.userId);
            Assert.Equal(reportDTO.userName, report.userName);
            Assert.Equal(reportDTO.userlastName, report.userlastName);
            Assert.Equal(reportDTO.userEmail, report.userEmail);
            Assert.Equal(reportDTO.reportName, report.reportName);
            Assert.Equal(reportDTO.reportDesc, report.reportDesc);
            Assert.Equal(reportDTO.reportAnsw, report.reportAnsw);

            _mockReportRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Report>()), Times.Once);
        }

        [Fact]
        public async Task GetReportByIdAsync_ShouldReturnReport()
        {
            // Arrange

            var report = new Report
            {
                reportId = 1,
                userId = 1,
                userName = "Eliza",
                userlastName = "Bleta",
                userEmail = "eliza.bleta@gmail.com",
                reportName = "Test Report",
                reportDesc = "This is a test report.",
                reportAnsw = "Waiting for Respond"
            };

            _mockReportRepository.Setup(repo => repo.GetByIdAsync(report.reportId)).ReturnsAsync(report);

            // Act
            var result = await _reportService.GetReportByIdAsync(report.reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(report.reportId, result.reportId);
            Assert.Equal(report.userId, result.userId);
            Assert.Equal(report.userName, result.userName);
            Assert.Equal(report.userlastName, result.userlastName);
            Assert.Equal(report.userEmail, result.userEmail);
            Assert.Equal(report.reportName, result.reportName);
            Assert.Equal(report.reportDesc, result.reportDesc);
            Assert.Equal(report.reportAnsw, result.reportAnsw);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteReport()
        {
            // Arrange
            var reportId = 1;

            _mockReportRepository.Setup(repo => repo.DeleteAsync(reportId)).Returns(Task.CompletedTask);

            // Act
            await _reportService.DeleteAsync(reportId);

            // Assert
            _mockReportRepository.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        [Fact]
        public async Task GetAllReportsAsync_ShouldReturnAllReports()
        {
            // Arrange
            var reports = new List<Report>
            {
                new Report
                {
                    reportId = 1,
                    userId = 1,
                    userName = "Eliza",
                    userlastName = "Bleta",
                    userEmail = "eliza.bleta@gmail.com",
                    reportName = "Test Report 1",
                    reportDesc = "This is a test report 1.",
                    reportAnsw = "Waiting for Respond"
                },
                new Report
                {
                    reportId = 2,
                    userId = 2,
                    userName = "Jane",
                    userlastName = "Smith",
                    userEmail = "jane.smith@gmail.com",
                    reportName = "Test Report 2",
                    reportDesc = "This is a test report 2.",
                    reportAnsw = "Responded"
                }
            };

            _mockReportRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(reports);

            // Act
            var result = await _reportService.GetAllReportsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.reportId == 1);
            Assert.Contains(result, r => r.reportId == 2);
        }
    }
}