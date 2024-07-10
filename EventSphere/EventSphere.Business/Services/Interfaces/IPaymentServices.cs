using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IPaymentServices
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<PaymentResponseDto> AddPaymentAsync(PaymentDTO Pid);
        Task UpdatePaymentAsync(int id, PaymentDTO Pid);
        Task DeletePaymentAsync(int id);
        Task<int> GetPaymentCountAsync();
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetPaymentsByEventIdAsync(int eventId);
        Task<int?> ValidatePromoCodeAsync(string code);

    }
}
