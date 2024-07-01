using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using EventSphere.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class PaymentServices : IPaymentService
    {
        private readonly IGenericRepository<Payment> _genericRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Domain.Entities.Event> _eventRepository;
        private readonly StripeSettings _stripeSettings;
        public PaymentServices(IGenericRepository<Payment> genericRepository, IGenericRepository<User> userRepository, IGenericRepository<Domain.Entities.Event> eventRepository, IOptions<StripeSettings> stripeSettings)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<PaymentResponseDto> AddPaymentAsync(PaymentDTO Pid)
        {
            var ticket = await _ticketRepository.GetByIdAsync(Pid.TicketId);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            var options = new ChargeCreateOptions
            {
                Amount = (int)(ticket.Price * Pid.Amount * 100),
                Currency = "usd",
                Description = $"Payment for {Pid.Amount} ticket(s)",
                Source = Pid.StripeToken
            };

            var user = await _userRepository.GetByIdAsync(Pid.UserId);
            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Status == "succeeded")
            {
                var payment = new Payment
                {

                    UserId = Pid.UserId,
                    TicketId = Pid.TicketId,
                    Amount = Pid.Amount,
                    PaymentMethod = Pid.PaymentMethod,
                    PaymentDate = Pid.PaymentDate,
                    PaymentStatus = true, // payment successful
                    TicketName = ticket.TicketType,
                    UserName = user.Name + " " + user.LastName,
                };

                var user = await _userRepository.GetByIdAsync(Pid.UserId);


                var availableTick = await _eventRepository.GetByIdAsync(ticket.EventId);
                if (availableTick != null)
                {
                    if(availableTick.AvailableTickets == 0 || Pid.Amount >= availableTick.AvailableTickets)
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

            payment.TicketId = Pid.TicketId;
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
            return await _genericRepository.GetAsync(p => p.UserId == userId);
        }
        public async Task<IEnumerable<Payment>> GetPaymentsByEventIdAsync(int eventId)
        {
            return await _genericRepository.FindByConditionAsync(p => p.Ticket.EventId == eventId);
        }

    }
}
