using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
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
        private readonly IGenericRepository<Event> _eventRepository;
        public PaymentServices(IGenericRepository<Payment> genericRepository, IGenericRepository<User> userRepository, IGenericRepository<Event> eventRepository)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public async Task<PaymentResponseDto> AddPaymentAsync(PaymentDTO Pid)
        {
            var payment = new Payment
            {
                UserID = Pid.UserID,
                TicketID = Pid.TicketID,
                Amount = Pid.Amount,
                PaymentMethod = Pid.PaymentMethod,
                PaymentDate = Pid.PaymentDate,
                PaymentStatus = Pid.PaymentStatus,
            };
            var user = await _userRepository.GetByIdAsync(Pid.UserID);
            var response = new PaymentResponseDto
            {
                Payment = payment,
                User = user,
            };
            await _genericRepository.AddAsync(payment);
            return response;
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
