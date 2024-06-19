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
        private readonly IGenericRepository<Ticket> _ticketRepository;
        public PaymentServices(IGenericRepository<Payment> genericRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Domain.Entities.Event> eventRepository,
            IOptions<StripeSettings> stripeSettings,
            IGenericRepository<Ticket> ticketRepository)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _stripeSettings = stripeSettings.Value;
            _ticketRepository = ticketRepository;
        }

        public async Task<PaymentResponseDto> AddPaymentAsync(PaymentDTO Pid)
        {
            var options = new ChargeCreateOptions
            {
                Amount = Pid.Amount * 100, // amount in cents
                Currency = "usd",
                Description = "Payment for ticket",
                Source = Pid.StripeToken
            };

            var user = await _userRepository.GetByIdAsync(Pid.UserID);
            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Status == "succeeded")
            {
                var ticket = await _ticketRepository.GetByIdAsync(Pid.TicketID);
                var ticketName = ticket.TicketType;
                var payment = new Payment
                {
                    UserID = Pid.UserID,
                    TicketID = Pid.TicketID,
                    TicketName = ticketName,
                    UserName = user.Name + " " + user.LastName,
                    Amount = Pid.Amount,
                    PaymentMethod = Pid.PaymentMethod,
                    PaymentDate = Pid.PaymentDate,
                    PaymentStatus = true, 
                };
                var response = new PaymentResponseDto
                {
                    Payment = payment,
                    User = user,
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
    }
}
