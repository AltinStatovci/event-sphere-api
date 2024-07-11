using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
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
        
        public PaymentServiceTest()
        {
            _mockUserRepository = new Mock<IGenericRepository<User>>();
            _mockPaymentRepository = new Mock<IGenericRepository<Payment>>();
            _mockEventRepository = new Mock<IGenericRepository<Event>>();
            _mockTicketRepository = new Mock<IGenericRepository<Ticket>>();
            _mockPromoCodeRepository = new Mock<IGenericRepository<PromoCode>>();
            _mockChargeService = new Mock<ChargeService>();
            _paymentServices = new PaymentService(_mockPaymentRepository.Object, _mockUserRepository.Object, _mockEventRepository.Object,_mockTicketRepository.Object,_mockPromoCodeRepository.Object,_mockChargeService.Object);
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
    }
}
