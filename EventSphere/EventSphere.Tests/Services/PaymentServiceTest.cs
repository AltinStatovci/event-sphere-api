using EventSphere.Business.Hubs;
using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Stripe;
using Xunit;
using Event = EventSphere.Domain.Entities.Event;

namespace EventSphere.Tests.Services
{
    public class PaymentServiceTest
    {
        private readonly IPaymentService _paymentServices;
        private readonly Mock<IGenericRepository<User>> _mockUserRepository;
        private readonly Mock<IGenericRepository<Payment>> _mockPaymentRepository;
        private readonly Mock<IGenericRepository<Event>> _mockEventRepository;
        private readonly Mock<IGenericRepository<Ticket>> _mockTicketRepository;
        private readonly Mock<IGenericRepository<PromoCode>> _mockPromoCodeRepository;
        private readonly Mock<ChargeService> _mockChargeService;
        private readonly Mock<IHubContext<TicketHub>> _mockTicketHub;
        
        public PaymentServiceTest()
        {
            _mockUserRepository = new Mock<IGenericRepository<User>>();
            _mockPaymentRepository = new Mock<IGenericRepository<Payment>>();
            _mockEventRepository = new Mock<IGenericRepository<Event>>();
            _mockTicketRepository = new Mock<IGenericRepository<Ticket>>();
            _mockPromoCodeRepository = new Mock<IGenericRepository<PromoCode>>();
            _mockChargeService = new Mock<ChargeService>();
            _mockTicketHub = new Mock<IHubContext<TicketHub>>();
            _paymentServices = new PaymentService(_mockPaymentRepository.Object, _mockUserRepository.Object, _mockEventRepository.Object,_mockTicketRepository.Object,_mockPromoCodeRepository.Object,_mockChargeService.Object, _mockTicketHub.Object);
        }
        
        [Fact]
        public async Task AddPaymentAsync_ShouldAddPayment()
        {
            // Arrange
            var paymentDTO = new PaymentDTO()
            {
                UserID = 1,
                TicketID = 1,
                Amount = 100,
                PaymentMethod = "Credit Card",
                StripeToken = "tok_test",
                PaymentDate = DateTime.Now
            };

            var ticket = new Ticket() { ID = 1, EventID = 1, Price = 100, TicketType = "Test Ticket" };
            var user = new User() { ID = 1, Name = "John", LastName = "Doe" };
            var eventEntity = new Event() { ID = 1, AvailableTickets = 200 };

            var charge = new Charge() { Status = "succeeded" };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(paymentDTO.TicketID)).ReturnsAsync(ticket);
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(paymentDTO.UserID)).ReturnsAsync(user);
            _mockEventRepository.Setup(repo => repo.GetByIdAsync(ticket.EventID)).ReturnsAsync(eventEntity);
            _mockChargeService
                .Setup(service => service.CreateAsync(It.IsAny<ChargeCreateOptions>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(charge);

            // Act
            var result = await _paymentServices.AddPaymentAsync(paymentDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paymentDTO.UserID, result.Payment.UserID);
            Assert.Equal(paymentDTO.TicketID, result.Payment.TicketID);
            Assert.Equal(user.Name + " " + user.LastName, result.Payment.UserName);
            Assert.Equal(ticket.TicketType, result.Payment.TicketName);
            Assert.Equal(paymentDTO.Amount, result.Payment.Amount);
            Assert.Equal(paymentDTO.PaymentMethod, result.Payment.PaymentMethod);
            Assert.True(result.Payment.PaymentStatus);

            _mockPaymentRepository.Verify(repo => repo.AddAsync(It.IsAny<Payment>()), Times.Once);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(eventEntity), Times.Once);
        }
        [Fact]
        public async Task ValidatePromoCodeAsync_OnValidCode_ShouldReturnDiscount()
        {
            // Arrange
            var promoCode = new PromoCode
            {
                Code = "DISCOUNT10",
                DiscountPercentage = 10,
                ExpiryDate = DateTime.Now.AddDays(1)
            };
            _mockPromoCodeRepository.Setup(repo => repo.GetByCodeAsync("DISCOUNT10")).ReturnsAsync(promoCode);

            // Act
            var result = await _paymentServices.ValidatePromoCodeAsync("DISCOUNT10");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result);
        }
        [Fact]
        public async Task ValidatePromoCodeAsync_OnInvalidCode_ShouldReturnNull()
        {
            // Arrange
            _mockPromoCodeRepository.Setup(repo => repo.GetByCodeAsync("INVALID")).ReturnsAsync((PromoCode)null);

            // Act
            var result = await _paymentServices.ValidatePromoCodeAsync("INVALID");

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task AddPaymentAsync_OnInvalidPromoCode_ShouldThrowException()
        {
            // Arrange
            var ticket = new Ticket { ID = 1, Price = 50, EventID = 1, TicketType = "Standard" };
            var paymentDto = new PaymentDTO
            {
                UserID = 1,
                TicketID = 1,
                Amount = 2,
                PaymentMethod = "CreditCard",
                PaymentDate = DateTime.Now,
                PromoCode = "INVALID"
            };

            _mockTicketRepository.Setup(repo => repo.GetByIdAsync(paymentDto.TicketID)).ReturnsAsync(ticket);
            _mockPromoCodeRepository.Setup(repo => repo.GetByCodeAsync(paymentDto.PromoCode)).ReturnsAsync((PromoCode)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _paymentServices.AddPaymentAsync(paymentDto));
            Assert.Equal("Invalid or expired promo code", exception.Message);
        }
        [Fact]
        public async Task DeletePaymentAsync_OnSuccess_ShouldDelete()
        {
            // Arrange
            int paymentId = 1;

            // Act
            await _paymentServices.DeletePaymentAsync(paymentId);

            // Assert
            _mockPaymentRepository.Verify(repo => repo.DeleteAsync(paymentId), Times.Once);
        }
        [Fact]
        public async Task GetAllPaymentsAsync_OnSuccess_ShouldReturnAllPayments()
        {
            // Arrange
            var expectedPayments = new List<Payment>
            {
                new Payment { ID = 1, UserID = 1, TicketID = 1, Amount = 100 },
                new Payment { ID = 2, UserID = 2, TicketID = 2, Amount = 200 }
            };

            _mockPaymentRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedPayments);

            // Act
            var result = await _paymentServices.GetAllPaymentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPayments.Count, result.Count());
            Assert.Equal(expectedPayments, result);
        }
        [Fact]
        public async Task GetPaymentByIdAsync_OnSuccess_ShouldReturnPaymentById()
        {
            // Arrange
            var expectedPayment = new Payment { ID = 1, UserID = 1, TicketID = 1, Amount = 100 };

            _mockPaymentRepository.Setup(repo => repo.GetByIdAsync(expectedPayment.ID)).ReturnsAsync(expectedPayment);

            // Act
            var result = await _paymentServices.GetPaymentByIdAsync(expectedPayment.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPayment, result);
        }
        [Fact]
        public async Task UpdatePaymentAsync_OnSuccess_ShouldReturnUpdatedPayment()
        {
            // Arrange
            var existingPayment = new Payment
            {
                ID = 1,
                UserID = 1,
                TicketID = 1,
                Amount = 100,
                PaymentMethod = "CreditCard",
                PaymentDate = DateTime.Now,
                PaymentStatus = true
            };

            var updatePaymentDto = new PaymentDTO
            {
                TicketID = 2,
                Amount = 200,
                PaymentMethod = "PayPal",
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentStatus = false
            };

            _mockPaymentRepository.Setup(repo => repo.GetByIdAsync(existingPayment.ID)).ReturnsAsync(existingPayment);

            // Act
            await _paymentServices.UpdatePaymentAsync(existingPayment.ID, updatePaymentDto);

            // Assert
            Assert.NotNull(existingPayment);
            Assert.Equal(updatePaymentDto.TicketID, existingPayment.TicketID);
            Assert.Equal(updatePaymentDto.Amount, existingPayment.Amount);
            Assert.Equal(updatePaymentDto.PaymentMethod, existingPayment.PaymentMethod);
            Assert.Equal(updatePaymentDto.PaymentDate, existingPayment.PaymentDate);
            Assert.Equal(updatePaymentDto.PaymentStatus, existingPayment.PaymentStatus);
            _mockPaymentRepository.Verify(repo => repo.UpdateAsync(existingPayment), Times.Once);
        }
        [Fact]
        public async Task GetPaymentCountAsync_OnSuccess_ShouldReturnPaymentCount()
        {
            // Arrange
            var paymentCount = 10;
            _mockPaymentRepository.Setup(repo => repo.CountAsync()).ReturnsAsync(paymentCount);

            // Act
            var result = await _paymentServices.GetPaymentCountAsync();

            // Assert
            Assert.Equal(paymentCount, result);
        }

        [Fact]
        public async Task GetPaymentsByUserIdAsync_OnSuccess_ShouldReturnPaymentsByUser()
        {
            // Arrange
            var userId = 1;
            var expectedPayments = new List<Payment>
            {
                new Payment { ID = 1, UserID = userId, TicketID = 1, Amount = 100 },
                new Payment { ID = 2, UserID = userId, TicketID = 2, Amount = 200 }
            };

            _mockPaymentRepository.Setup(repo => repo.GetAsync(p => p.UserID == userId)).ReturnsAsync(expectedPayments);

            // Act
            var result = await _paymentServices.GetPaymentsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPayments.Count, result.Count());
            Assert.Equal(expectedPayments, result);
        }

        [Fact]
        public async Task GetPaymentsByEventIdAsync_OnSuccess_ShouldReturnPaymentsByEvent()
        {
            // Arrange
            var eventId = 1;
            var expectedPayments = new List<Payment>
            {
                new Payment { ID = 1, UserID = 1, TicketID = 1, Amount = 100 },
                new Payment { ID = 2, UserID = 2, TicketID = 2, Amount = 200 }
            };

            _mockPaymentRepository.Setup(repo => repo.FindByConditionAsync(p => p.Ticket.EventID == eventId)).ReturnsAsync(expectedPayments);

            // Act
            var result = await _paymentServices.GetPaymentsByEventIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPayments.Count, result.Count());
            Assert.Equal(expectedPayments, result);
        }
    }
}
