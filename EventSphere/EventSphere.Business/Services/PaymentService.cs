using EventSphere.Business.Hubs;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Settings;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _genericRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Domain.Entities.Event> _eventRepository;
        private readonly IGenericRepository<PromoCode> _promoCodeRepository;
      
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly ChargeService _chargeService;
        private readonly IHubContext<TicketHub> _hubContext;

        public PaymentService(IGenericRepository<Payment> genericRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Domain.Entities.Event> eventRepository,
         
            IGenericRepository<Ticket> ticketRepository,
            IGenericRepository<PromoCode> promoCodeRepository,
            ChargeService chargeService,
            IHubContext<TicketHub> hubContext
            )
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
           
            _ticketRepository = ticketRepository;
            _promoCodeRepository = promoCodeRepository;
            _chargeService = chargeService;
            _hubContext = hubContext;
        }

        public async Task<int?> ValidatePromoCodeAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetByCodeAsync(code);
            if (promoCode != null && promoCode.ExpiryDate >= DateTime.Now)
            {
                return (int?)promoCode.DiscountPercentage;
            }
            return null;
        }

        public async Task<PaymentResponseDto> AddPaymentAsync(PaymentDTO Pid)
        {
            var ticket = await _ticketRepository.GetByIdAsync(Pid.TicketID);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            double discount = 0;
            if (!string.IsNullOrEmpty(Pid.PromoCode))
            {
                var promoCode = await _promoCodeRepository.GetByCodeAsync(Pid.PromoCode);
                if (promoCode != null && promoCode.IsValid && !promoCode.IsExpired())
                {
                    discount = promoCode.DiscountPercentage;
                }
                else
                {
                    throw new Exception("Invalid or expired promo code");
                }
            }

            var totalAmount = ticket.Price * Pid.Amount;
            var discountAmount = totalAmount * (discount / 100);
            var finalAmount = totalAmount - discountAmount;

            var options = new ChargeCreateOptions
            {
                Amount = (int)(finalAmount * 100), // Stripe expects amount in cents
                Currency = "usd",
                Description = $"Payment for {Pid.Amount} ticket(s)",
                Source = Pid.StripeToken
            };

            var user = await _userRepository.GetByIdAsync(Pid.UserID);
            
            Charge charge = await _chargeService.CreateAsync(options);

            if (charge.Status == "succeeded")
            {
                var payment = new Payment
                {
                    UserID = Pid.UserID,
                    TicketID = Pid.TicketID,
                    TicketName = ticket.TicketType,
                    UserName = user.Name + " " + user.LastName,
                    Amount = Pid.Amount,
                    PaymentMethod = Pid.PaymentMethod,
                    PaymentDate = Pid.PaymentDate,
                    PaymentStatus = true,
                };

                var availableTick = await _eventRepository.GetByIdAsync(ticket.EventID);
                if (availableTick != null)
                {
                    if (availableTick.AvailableTickets == 0 || Pid.Amount >= availableTick.AvailableTickets)
                    {
                        throw new Exception("There are not enough tickets available");
                    }
                    else
                    {
                        availableTick.AvailableTickets -= Pid.Amount;
                        await _eventRepository.UpdateAsync(availableTick);
                    }
                }

                var response = new PaymentResponseDto
                {
                    Payment = payment,
                    User = user,
                    Ticket = ticket,
                };

                await _genericRepository.AddAsync(payment);
                await _hubContext.Clients.All.SendAsync("ReceiveTicketCountUpdate", ticket.EventID, availableTick.AvailableTickets);
                return response;
            }
            else
            {
                throw new Exception("Payment failed");
            }
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            var payment = await _genericRepository.GetAllAsync();
            return payment;
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            var payment = await _genericRepository.GetByIdAsync(id);
            return payment;
        }

        public async Task UpdatePaymentAsync(int id, PaymentDTO Pid)
        {
            var payment = await _genericRepository.GetByIdAsync(id);

            payment.TicketID = Pid.TicketID;
            payment.Amount = Pid.Amount;
            payment.PaymentMethod = Pid.PaymentMethod;
            payment.PaymentDate = Pid.PaymentDate;
            payment.PaymentStatus = Pid.PaymentStatus;

            await _genericRepository.UpdateAsync(payment);
        }

        public async Task<int> GetPaymentCountAsync()
        {
            return await _genericRepository.CountAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _genericRepository.GetAsync(p => p.UserID == userId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByEventIdAsync(int eventId)
        {
            return await _genericRepository.FindByConditionAsync(p => p.Ticket.EventID == eventId);
        }
    }
}
