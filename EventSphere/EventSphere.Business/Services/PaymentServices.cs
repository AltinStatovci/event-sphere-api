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
        public PaymentServices(IGenericRepository<Payment> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<Payment> AddPaymentAsync(PaymentDTO Pid)
        {
            var payment = new Payment
            {
                ID = Pid.ID,
                UserID = Pid.UserID,
                EventID = Pid.EventID,
                TicketID = Pid.TicketID,
                Amount = Pid.Amount,
                PaymentMethod = Pid.PaymentMethod,
                PaymentStatus = Pid.PaymentStatus,
                PaymentDate = Pid.PaymentDate,
            };
            await _genericRepository.AddAsync(payment);
            return payment;

        }

        public async Task DeletePaymentAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task<Payment> UdpatePaymentAsync(int id, PaymentDTO Pid)
        {
            var payment = await _genericRepository.GetByIdAsync(id);
            payment.ID = Pid.ID;
            payment.UserID = Pid.UserID;
            payment.EventID = Pid.EventID;
            payment.TicketID = Pid.TicketID;
            payment.Amount = Pid.Amount;
            payment.PaymentMethod = Pid.PaymentMethod;
            payment.PaymentStatus = Pid.PaymentStatus;
            payment.PaymentDate = Pid.PaymentDate;

            
            await _genericRepository.UpdateAsync(payment);
            return payment;
        }

    }
}
