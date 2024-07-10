using EventSphere.Business.Services;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Moq;
using Stripe;
using Xunit;
using Event = EventSphere.Domain.Entities.Event;

namespace EventSphere.Tests.Services;

public class PaymentServiceTest
{
    private readonly IPaymentServices _paymentServices;
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
        _paymentServices = new PaymentServices(_mockPaymentRepository.Object, _mockUserRepository.Object, _mockEventRepository.Object,_mockTicketRepository.Object,_mockPromoCodeRepository.Object,_mockChargeService.Object);
        
    }
    
    [Fact]
    public async Task AddPaymentAsync_ShouldAddPayment()
    {
        // Arrange
        var payment = new PaymentDTO()
        {
            ID = 1,
            UserID = 1,
            TicketID = 1,
            Amount = 100,
            PaymentMethod = "Credit Card",
            StripeToken = Guid.NewGuid().ToString(),
            PaymentStatus = true,
            PaymentDate = DateTime.Now
        };
        
        _mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync( new Ticket(){ ID = 1, EventID = 1, Price = 100 , TicketType = "test"});
        _mockUserRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User() { ID = 1, Name = "test" , LastName = "Last Name"});
        
        var result = _paymentServices.AddPaymentAsync(payment);
        
    }
}