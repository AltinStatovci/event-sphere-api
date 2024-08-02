using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using EventSphere.Business.Validator;
using Microsoft.AspNetCore.SignalR.Client;
using EventSphere.Business.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EventSphere.Tests.Services;

public class EventServiceTest
{
    private readonly IEventService _eventService;
   
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IGenericRepository<User>> _mockUserRepository;
    private readonly Mock<IGenericRepository<EventCategory>> _mockEventCategoryRepository;
    private readonly Mock<IGenericRepository<Location>> _mockLocationRepository;


    public EventServiceTest()
    {
      
        _mockEventRepository = new Mock<IEventRepository>();
        _mockUserRepository = new Mock<IGenericRepository<User>>();
        _mockEventCategoryRepository = new Mock<IGenericRepository<EventCategory>>();
        _mockLocationRepository = new Mock<IGenericRepository<Location>>();
        _eventService = new EventService( _mockEventRepository.Object, _mockUserRepository.Object, _mockEventCategoryRepository.Object, _mockLocationRepository.Object);
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetEventsByName_OnNameEmpty_ShouldGetAllEvents(string name)
    {
        //Arrange
        var allEvents = new List<Event>
        {
            new Event {EventName = "Event1"},
            new Event {EventName = "Event2"}
        };
        _mockEventRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allEvents);

        //Act
        var result = await _eventService.GetEventsByNameAsync(name);

        //Assert
        Assert.Equal(allEvents, result);
        _mockEventRepository.Verify(repo => repo.GetAllAsync(), Times.Once());
        _mockEventRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>()), Times.Never);

    }
    [Theory]
    [InlineData("Event1")]
    [InlineData("Event2")]
    public async Task GetEventsByName_OnSuccess_ShouldReturnFilteredEvents(string name)
    {
        //Arrange
        var allEvents = new List<Event>
        {
            new Event
            {
                EventName = "Event1",
            
            },
            new Event {EventName = "Event2"}
        };
        _mockEventRepository
        .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>()))
        .ReturnsAsync(allEvents.Where(e => e.EventName.Contains(name)).ToList());

        //Act
        var result = await _eventService.GetEventsByNameAsync(name);
        var filteredEvents = allEvents.Where(e => e.EventName.Contains(name)).ToList();

        //Assert
        Assert.Equal(filteredEvents, result);
        _mockEventRepository.Verify( repo => repo.GetAsync(It.IsAny<Expression<Func<Event, bool>>>()),Times.Once());
        _mockEventRepository.VerifyNoOtherCalls();

    }
    [Fact]
    public async Task GetAllEventsAsync_OnSuccess_ShouldReturnAllEvents()
    {
        //Assert
        var allEvents = new List<Event>
        {
            new Event { EventName = "Event1"},
            new Event { EventName = "Event2"},
        };
        _mockEventRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allEvents);

        //Arrange
        var result = await _eventService.GetAllEventsAsync();

        //Assert
        Assert.Equal(allEvents, result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetEventsByIdAsync_OnSuccess_ShouldReturnEventById(int id)
    {
        //Arrange
        var allEvents = new List<Event>
    {
        new Event { ID = 1, EventName = "Event1" },
        new Event { ID = 2, EventName = "Event2" },
    };
        var eventById = allEvents.FirstOrDefault(e => e.ID == id);
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(eventById);

        //Act
        var result = await _eventService.GetEventsByIdAsync(id);

        //Assert
        Assert.Equal(eventById, result);
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _mockEventRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateEventsAsync_OnImageNullOrEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        var eventDto = new EventDTO();
        IFormFile image = null;
    
        var exceptionMessage = "Event DTO or image is null or empty.";

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _eventService.CreateEventsAsync(eventDto, image));
    
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task CreateEventsAsync_OnResizeAndConvertToBase64Async_ThrowsException()
    {
        //Arrange
        var eventDto = new EventDTO();
        var mockImage = new Mock<IFormFile>();
        mockImage.Setup(i => i.Length).Returns(1);
        var exceptionMessage = "Error occurred while creating the event.";

        //Act 
        var exception = await Assert.ThrowsAsync<Exception>(() => _eventService.CreateEventsAsync(eventDto, mockImage.Object));

        //Assert 
        Assert.Equal(exceptionMessage, exception.Message);
        Assert.IsType<UnknownImageFormatException>(exception.InnerException?.InnerException);
    }
    [Fact]
    public async Task CreateEventsAsync_OnSuccess_CreatesEventAndReturnsEvent()
    {
        // Arrange
        var eventDto = new EventDTO
        {
            EventName = "Test Event",
            Description = "Test Description",
            Address = "Test Address",
            LocationId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CategoryID = 1,
            OrganizerID = 1,
            MaxAttendance = 100,
            AvailableTickets = 100,
            IsApproved = false,
            ScheduleDate = DateTime.Now
        };

        var image = new Image<Rgba32>(100, 100);
        image.Mutate(x => x.BackgroundColor(Color.White));
        var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpegEncoder());
        memoryStream.Position = 0;

        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        formFileMock.Setup(f => f.FileName).Returns("test.jpg");
        formFileMock.Setup(f => f.Length).Returns(memoryStream.Length);
        formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<System.Threading.CancellationToken>())).Callback<Stream, System.Threading.CancellationToken>((s, _) =>
        {
            memoryStream.Position = 0;
            memoryStream.CopyTo(s);
        }).Returns(Task.CompletedTask);
        string expectedBase64String = await _eventService.ResizeAndConvertToBase64Async(formFileMock.Object);

        var user = new User { ID = 1, Name = "Test Organizer" };
        var category = new EventCategory { ID = 1, CategoryName = "Test Category" };
        var location = new Location { Id = 1, City = "Test Location" };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(eventDto.OrganizerID)).ReturnsAsync(user);
        _mockEventCategoryRepository.Setup(repo => repo.GetByIdAsync(eventDto.CategoryID)).ReturnsAsync(category);
        _mockLocationRepository.Setup(repo => repo.GetByIdAsync(eventDto.LocationId)).ReturnsAsync(location);
        _mockEventRepository.Setup(repo => repo.AddAsync(It.IsAny<Event>())).ReturnsAsync((Event evt) => evt);


        // Act
        var result = await _eventService.CreateEventsAsync(eventDto, formFileMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventDto.EventName, result.EventName);
        Assert.Equal(eventDto.Description, result.Description);
        Assert.Equal(eventDto.Address, result.Address);
        Assert.Equal(location, result.Location); 
        Assert.Equal(eventDto.StartDate, result.StartDate);
        Assert.Equal(eventDto.EndDate, result.EndDate);
        Assert.Equal(eventDto.CategoryID, result.CategoryID);
        Assert.Equal(eventDto.OrganizerID, result.OrganizerID);
        Assert.Equal(eventDto.MaxAttendance, result.MaxAttendance);
        Assert.Equal(eventDto.AvailableTickets, result.AvailableTickets);
        Assert.Equal(eventDto.IsApproved, result.IsApproved);
        Assert.Equal(eventDto.ScheduleDate, result.ScheduleDate);
        Assert.Equal(expectedBase64String, result.PhotoData);

        _mockEventRepository.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public async Task ResizeAndConvertToBase64Async_OnImageNullOrEmpty_ShouldThrowArgumentException()
    {
        //Arrange
        IFormFile image = null;

        var exceptionMessage = "Image file is null or empty.";

        //Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _eventService.ResizeAndConvertToBase64Async(image));
        _mockEventRepository.VerifyNoOtherCalls();
    }
    [Fact]
    public async Task ResizeAndConvertToBase64Async_OnInvalidImageFile_ThrowsException()
    {
        // Arrange
        var mockImage = new Mock<IFormFile>();
        var content = "invalid image content"; 
        var fileName = "invalid.jpg";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        mockImage.Setup(_ => _.OpenReadStream()).Returns(ms);
        mockImage.Setup(_ => _.FileName).Returns(fileName);
        mockImage.Setup(_ => _.Length).Returns(ms.Length);

        var exceptionMessage = "Error occurred while resizing and converting the image to Base64.";

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _eventService.ResizeAndConvertToBase64Async(mockImage.Object));

        // Assert
        Assert.Equal(exceptionMessage, exception.Message);
    }
    [Fact]
    public async Task UpdateEventsAsync_OnInvalidEventId_ShouldThrowArgumentException()
    {
        //Arrange
        int eventId = 0;
        EventDTO eventDto = new EventDTO
        {
            EventName = "Sample Event",
            Description = "Sample Description",
            LocationId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CategoryID = 1,
            OrganizerID = 1,
            MaxAttendance = 100,
            AvailableTickets = 50,
            DateCreated = DateTime.Now
        };
        IFormFile newImage = null;

        var exceptionMessage = $"Event with ID {eventId} not found.";

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        //Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _eventService.UpdateEventsAsync(eventId, eventDto, newImage));

        //Assert
        Assert.Equal(exceptionMessage , result.Message);

    }
    [Fact]
    public async Task UpdateEventsAsync_ValidEvent_UpdatesEventAndProcessesNewImage()
    {
        // Arrange
        int eventId = 1;
        var existingEvent = new Event
        {
            ID = eventId,
            EventName = "Old Event",
            Description = "Old Description",
            LocationId = 1,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now,
            CategoryID = 2,
            OrganizerID = 3,
            MaxAttendance = 200,
            AvailableTickets = 100,
            DateCreated = DateTime.Now.AddDays(-10),
            PhotoData = "OldBase64Image"
        };

        var eventDto = new EventDTO
        {
            EventName = "Updated Event",
            Description = "Updated Description",
            LocationId = 2,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(2),
            CategoryID = 3,
            OrganizerID = 4,
            MaxAttendance = 250,
            AvailableTickets = 150,
            DateCreated = DateTime.Now
        };
        var image = new Image<Rgba32>(100, 100);
        image.Mutate(x => x.BackgroundColor(Color.White));
        var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpegEncoder());
        memoryStream.Position = 0;
        

        var newImageMock = new Mock<IFormFile>();
        var newImageStream = memoryStream;
        newImageMock.Setup(img => img.OpenReadStream()).Returns(newImageStream);
        newImageMock.Setup(img => img.FileName).Returns("newImage.jpg");
        newImageMock.Setup(img => img.Length).Returns(newImageStream.Length);
        newImageMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<System.Threading.CancellationToken>())).Callback<Stream, System.Threading.CancellationToken>((s, _) =>
        {
            memoryStream.Position = 0;
            memoryStream.CopyTo(s);
        }).Returns(Task.CompletedTask);

        _mockEventRepository
            .Setup(repo => repo.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockEventRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
            .Returns(Task.CompletedTask);

        var newBase64Image = await _eventService.ResizeAndConvertToBase64Async(newImageMock.Object);

        // Act
        var result = await _eventService.UpdateEventsAsync(eventId, eventDto, newImageMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Event", result.EventName);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal(2, result.LocationId);
        Assert.Equal(eventDto.StartDate, result.StartDate);
        Assert.Equal(eventDto.EndDate, result.EndDate);
        Assert.Equal(3, result.CategoryID);
        Assert.Equal(4, result.OrganizerID);
        Assert.Equal(250, result.MaxAttendance);
        Assert.Equal(150, result.AvailableTickets);
        Assert.Equal(eventDto.DateCreated, result.DateCreated);
        Assert.Equal(newBase64Image, result.PhotoData);

        _mockEventRepository.Verify(repo => repo.UpdateAsync(result), Times.Once); 
    }
    [Fact]
    public async Task UpdateEventsAsync_OnFailedUpdate_ShouldThrowException()
    {
        // Arrange
        int eventId = 1;
        var eventDto = new EventDTO
        {
            EventName = "Test Event",
            Description = "Test Description",
            LocationId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CategoryID = 1,
            OrganizerID = 1,
            MaxAttendance = 100,
            AvailableTickets = 50,
            DateCreated = DateTime.Now
        };
        IFormFile newImage = null;

        var existingEvent = new Event
        {
            ID = eventId,
            EventName = "Old Event",
            PhotoData = "OldPhotoData"
        };
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId))
           .ReturnsAsync(existingEvent);

        _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
            .ThrowsAsync(new Exception("Database update failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _eventService.UpdateEventsAsync(eventId, eventDto, newImage));

        Assert.Equal("Error occurred while updating the event.", exception.Message);
    }
    [Fact]
    public async Task DeleteEventsAsync_OnSuccess_ShouldCallRepositoryDeleteAsync()
    {
        //Arrange
        int eventId = 1;

        //Act
        await _eventService.DeleteEventsAsync(eventId);

        //Assert
        _mockEventRepository.Verify(repo => repo.DeleteAsync(eventId), Times.Once);
    }

    [Fact]
    public async Task GetEventCountAsync_OnSuccess_ShouldCountEvents()
    {
        //Act
        await _eventService.GetEventCountAsync();

        //Assert
        _mockEventRepository.Verify(repo => repo.CountAsync(), Times.Once);
    }

    [Fact]
    public async Task GetEventByCategoryIdAsync_OnSuccess_ShouldReturnEventsOfCategories()
    {
        //Arrange
        int categoryId = 1;
        //Act
        await _eventService.GetEventByCategoryIdAsync(categoryId);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventByCategoryId(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetEventByOrganizerIdAsync_OnSuccess_ShouldReturnEventsOfOrganizer()
    {
        //Arrange
        int organizerId = 1;
        //Act
        await _eventService.GetEventByOrganizerIdAsync(organizerId);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventByOrganizerId(organizerId), Times.Once);
    }
    [Fact]
    public async Task GetEventsByCityAsync_OnSuccess_ShouldReturnEventsBasedOnCity()
    {
        //Arrange
        string city = "city 1"; 
        //Act
        await _eventService.GetEventsByCityAsync(city);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventsByCity(city), Times.Once);
    }
    [Fact]
    public async Task GetEventsByCountryAsync_OnSuccess_ShouldReturnEventsBasedOnCountry()
    {
        //Arrange
        string country = "country 1";
        //Act
        await _eventService.GetEventsByCountryAsync(country);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventsByCountry(country), Times.Once);
    }
    [Fact]
    public async Task UpdateEventStatus_OnSuccess_ShouldUpdateStatusOfEvent()
    {
        //Arrange
        int eventId = 1;
        var existingEvent = new Event { ID = eventId, IsApproved = false, Message = "Pending" };
        var updatedEvent = new Event { ID = eventId, IsApproved = true, Message = "Approved" };

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        _mockEventRepository.Setup(repo => repo.UpdateAsync(existingEvent)).Returns(Task.CompletedTask);


        //Act
        var result = await _eventService.UpdateEventStatus(eventId);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.UpdateAsync(It.Is<Event>(e => e.ID == eventId && e.IsApproved == true && e.Message == "Approved")), Times.Once);
        Assert.True(result.IsApproved);
        Assert.Equal("Approved", result.Message);
    }
    [Fact]
    public async Task UpdateEventStatus_OnError_ShouldThrowException()
    {
        //Arrange
        int eventId = 1;
        var existingEvent = new Event { ID = eventId, IsApproved = false, Message = "Pending" };
        var exceptionMessage = "Error while updating event";

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        _mockEventRepository.Setup(repo => repo.UpdateAsync(existingEvent)).ThrowsAsync(new Exception(exceptionMessage));


        //Act 
        var exception = await Assert.ThrowsAsync<Exception>(() => _eventService.UpdateEventStatus(eventId));

        //Assert
        Assert.Equal("Error occurred while approving the event.", exception.Message);
        Assert.Equal(exceptionMessage, exception.InnerException.Message);
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.UpdateAsync(existingEvent), Times.Once);
    }


    [Fact]
    public async Task GetEventsByDateAsync_OnSuccess_ShouldReturnEventsBasedOnDate()
    {
        //Arrange
        DateTime date = new DateTime(2023, 7, 15);

        //Act
        await _eventService.GetEventsByDateAsync(date);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventsByDate(date), Times.Once);
    }
    [Fact]
    public async Task GetEventsByDateTimeAsync_OnSuccess_ShouldReturnEventsBasedOnDateTime()
    {
        //Arrange
        DateTime date = new DateTime(2023, 7, 15);

        //Act
        await _eventService.GetEventsByDateTimeAsync(date);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetEventsByDateTime(date), Times.Once);
    }
    [Fact]
    public async Task GetOrganizerEmailAsync_OnSuccess_ShouldReturnEmailOfOrganizer()
    {
        //Arrange
        int organizerId = 1;

        //Act
        await _eventService.GetOrganizerEmailAsync(organizerId);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetOrganizerEmail(organizerId), Times.Once);
    }
    [Fact]
    public async Task UpdateMessage_OnSuccess_ShouldUpdateMessage()
    {
        //Arrange
        var eventId = 1;
        var existingEvent = new Event { ID = 1, Message = "Event1" };
        var updatedMessage = "Event22";
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

        //Act
        await _eventService.UpdateMessage(eventId, updatedMessage);

        //Assert
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.UpdateAsync(It.Is<Event>(e => e.ID == eventId && e.Message == updatedMessage)), Times.Once);
        Assert.Equal(updatedMessage, existingEvent.Message);
    }
}