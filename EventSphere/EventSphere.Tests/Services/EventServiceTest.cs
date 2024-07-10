using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventSphere.Tests.Services;

public class EventServiceTest
{
    private readonly IEventServices _eventService;
    private readonly Mock<EventSphereDbContext> _mockDbContext;
    private readonly Mock<IGenericRepository<Event>> _mockEventRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly Mock<IGenericRepository<EventCategory>> _mockEventCategoryRepository;
    private readonly Mock<IGenericRepository<Location>> _mockLocationRepository;

    public EventServiceTest()
    {
        _mockDbContext = new Mock<EventSphereDbContext>();
        _mockEventRepository = new Mock<IGenericRepository<Event>>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockEventCategoryRepository = new Mock<IGenericRepository<EventCategory>>();
        _mockLocationRepository = new Mock<IGenericRepository<Location>>();
        _eventService = new EventServices(_mockDbContext.Object,_mockEventRepository.Object, _mockUserRepository.Object, _mockEventCategoryRepository.Object, _mockLocationRepository.Object);
    }

    [Fact]
    public async Task CreateEventsAsync_OnImageNullOrEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var eventDto = new EventDTO();
        IFormFile image = null;
        
        var exceptioMessage = "Event DTO or image is null or empty.";
        //Act
        var result = await _eventService.CreateEventsAsync(eventDto, image);
        

        //Assert
        
        Assert.Equal(exceptioMessage, result.Message);
    }
    
    
}