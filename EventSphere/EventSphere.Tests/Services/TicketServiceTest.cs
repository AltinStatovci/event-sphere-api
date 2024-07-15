using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventSphere.Tests.Services
{
    public class TicketServiceTest
    {
        private readonly ITicketService _ticketService;
        private readonly Mock<ITicketRepository> _mockTicketRepository;
        private readonly Mock<IGenericRepository<Event>> _mockEventRepository;

        public TicketServiceTest()
        {
            _mockTicketRepository = new Mock<ITicketRepository>();
            _mockEventRepository = new Mock<IGenericRepository<Event>>();
            _ticketService = new TicketService(_mockTicketRepository.Object, _mockEventRepository.Object);
        }
        [Fact]
        public async Task CreateAsync_OnPriceZero_ShouldThrowArgumentException()
        {
            // Arrange
            var ticketDTO = new TicketDTO()
            {
                EventID = 1,
                TicketType = "Standard",
                TicketAmount = 1,
                BookingReference = "test",
                Price = 0
            };

            var eventEntity = new Event
            {
                ID = 1,
                EventName = "test",
                MaxAttendance = 100
            };

            var exceptionMessage = "Price cannot be equal to or less then 0";

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(ticketDTO.EventID)).ReturnsAsync(eventEntity);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _ticketService.CreateAsync(ticketDTO));

            Assert.Equal(exceptionMessage, exception.Message);
        }
        [Fact]
        public async Task CreateAsync_TotalTicketsToMaxAttendance_ShouldThrowArgumentException()
        {
            // Arrange

            var eventEntity = new Event
            {
                ID = 1,
                MaxAttendance = 100,
                EventName = "Test"
            };
            var totalTickets = 90;
            var newTicketAmount = 15;

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventEntity.ID)).ReturnsAsync(eventEntity);

            _mockTicketRepository.Setup(repo => repo.GetTotalTicketsAsync(eventEntity.ID)).ReturnsAsync(totalTickets);

            var ticketDto = new TicketDTO
            {
                EventID = 1,
                TicketType = "Standard",
                Price = 50,
                TicketAmount = newTicketAmount,
                BookingReference = "test"
            };

            var exceptionMessage = "Ticket amount cannot exceed the maximum attendance of the event.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _ticketService.CreateAsync(ticketDto));

            Assert.Equal(exceptionMessage, exception.Message);
        }
        [Fact]
        public async Task CreateAsync_OnSuccess_ShouldCreateTicket()
        {
            // Arrange
            var eventEntity = new Event
            {
                ID = 1,
                EventName = "Test",
                MaxAttendance = 100
            };

            _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventEntity.ID))
                .ReturnsAsync(eventEntity);

            var ticketDto = new TicketDTO
            {
                EventID = 1,
                TicketType = "Standard",
                Price = 50,
                TicketAmount = 10,
                BookingReference = "test"
            };

            Ticket createdTicket = null;
            _mockTicketRepository.Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
                .Callback<Ticket>(ticket => createdTicket = ticket)
                .ReturnsAsync((Ticket ticket) => ticket);

            // Act
            var result = await _ticketService.CreateAsync(ticketDto);

            // Assert
            _mockTicketRepository.Verify(repo => repo.AddAsync(It.IsAny<Ticket>()), Times.Once);

            Assert.NotNull(createdTicket);
            Assert.Equal(ticketDto.EventID, createdTicket.EventID);
            Assert.Equal(eventEntity.EventName, createdTicket.EventName);
            Assert.Equal(ticketDto.TicketType, createdTicket.TicketType);
            Assert.Equal(ticketDto.Price, createdTicket.Price);
            Assert.Equal(ticketDto.TicketAmount, createdTicket.TicketAmount);
            Assert.Equal(ticketDto.BookingReference, createdTicket.BookingReference);
        }
        [Fact]
        public async Task DeleteAsync_OnSuccess_ShouldCallRepositoryDeleteAsync()
        {
            // Arrange
            int ticketId = 1;

            // Act
            await _ticketService.DeleteAsync(ticketId);

            // Assert
            _mockTicketRepository.Verify(repo => repo.DeleteAsync(ticketId), Times.Once);
        }
        [Fact]
        public async Task GetAllTicketsAsync_OnSuccess_ShouldReturnAllTickets()
        {
            // Arrange
            var expectedTickets = new List<Ticket>
            {
                new Ticket { ID = 1, EventID = 1, TicketType = "Standard", Price = 50, TicketAmount = 10, BookingReference = "test" },
                new Ticket { ID = 2, EventID = 1, TicketType = "VIP", Price = 100, TicketAmount = 5, BookingReference = "testi" }
            };

            _mockTicketRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedTickets);

            // Act
            var result = await _ticketService.GetAllTicketsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTickets.Count, result.Count());
            Assert.Equal(expectedTickets, result);
        }
        [Fact]
        public async Task GetTicketByEventIdAsync_OnSuccess_ShouldReturnTicketsByEvent()
        {
            // Arrange
            var eventId = 1;
            var expectedTickets = new List<Ticket>
            {
                new Ticket { ID = 1, EventID = eventId, TicketType = "Standard", Price = 50, TicketAmount = 10, BookingReference = "test" },
                new Ticket { ID = 2, EventID = eventId, TicketType = "VIP", Price = 100, TicketAmount = 5, BookingReference = "test" }
            };

            _mockTicketRepository.Setup(repo => repo.GetTicketByEventId(eventId)).ReturnsAsync(expectedTickets);

            // Act
            var result = await _ticketService.GetTicketByEventIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTickets.Count, result.Count());
            Assert.Equal(expectedTickets, result);
        }
        [Fact]
        public async Task GetTicketByIdAsync_OnSuccess_ShouldReturnTicket()
        {
            // Arrange
            var ticketId = 1;
            var expectedTicket = new Ticket { ID = ticketId, EventID = 1, TicketType = "Standard", Price = 50, TicketAmount = 10, BookingReference = "ABC123" };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync(expectedTicket);

            // Act
            var result = await _ticketService.GetTicketByIdAsync(ticketId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTicket, result);
        }
        [Fact]
        public async Task UpdateAsync_OnSuccess_ShouldReturnUpdatedTicket()
        {
            // Arrange
            var ticketId = 1;
            var existingTicket = new Ticket
            {
                ID = ticketId,
                EventID = 1,
                TicketType = "Standard",
                Price = 50,
                TicketAmount = 10,
                BookingReference = "test",
                Event = new Event { MaxAttendance = 100 }
            };

            var updateTicketDto = new TicketDTO
            {
                EventID = 1,
                TicketType = "VIP",
                Price = 100,
                TicketAmount = 5,
                BookingReference = "test"
            };

            var eventEntity = new Event { ID = 1, MaxAttendance = 100 };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync(existingTicket);
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateTicketDto.EventID)).ReturnsAsync(eventEntity);
            _mockTicketRepository.Setup(repo => repo.GetTotalTicketsUpdateAsync(updateTicketDto.EventID, ticketId)).ReturnsAsync(40);

            // Act
            await _ticketService.UpdateAsync(ticketId, updateTicketDto);

            // Assert
            Assert.NotNull(existingTicket);
            Assert.Equal(updateTicketDto.EventID, existingTicket.EventID);
            Assert.Equal(updateTicketDto.TicketType, existingTicket.TicketType);
            Assert.Equal(updateTicketDto.Price, existingTicket.Price);
            Assert.Equal(updateTicketDto.TicketAmount, existingTicket.TicketAmount);
            Assert.Equal(updateTicketDto.BookingReference, existingTicket.BookingReference);

            _mockTicketRepository.Verify(repo => repo.UpdateAsync(existingTicket), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_OnExceedingMaxAttendance_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var existingTicket = new Ticket
            {
                ID = 1,
                EventID = 1,
                TicketType = "Standard",
                Price = 50,
                TicketAmount = 10,
                BookingReference = "test",
                Event = new Event { MaxAttendance = 100 }
            };

            var updateTicketDto = new TicketDTO
            {
                EventID = 1,
                TicketType = "VIP",
                Price = 100,
                TicketAmount = 95,
                BookingReference = "test"
            };

            var eventEntity = new Event { ID = 1, MaxAttendance = 100 };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(existingTicket.ID)).ReturnsAsync(existingTicket);
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateTicketDto.EventID)).ReturnsAsync(eventEntity);
            _mockTicketRepository.Setup(repo => repo.GetTotalTicketsUpdateAsync(updateTicketDto.EventID, existingTicket.ID)).ReturnsAsync(10);

            var exceptionMessage = "Total tickets for the event cannot exceed MaxAttendance.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _ticketService.UpdateAsync(existingTicket.ID, updateTicketDto));

            Assert.Equal(exceptionMessage, exception.Message);
        }
        [Fact]
        public async Task GetTicketCountAsync_OnSuccess_ShouldReturnTicketCount()
        {
            // Arrange
            var ticketCount = 10;
            _mockTicketRepository.Setup(repo => repo.CountAsync()).ReturnsAsync(ticketCount);

            // Act
            var result = await _ticketService.GetTicketCountAsync();

            // Assert
            Assert.Equal(ticketCount, result);
        }
    }
}
