using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync();
        Task<PromoCode> GetPromoCodeByIdAsync(int id);
        Task<PromoCode> CreatePromoCodeAsync(PromoCode promoCode);
        Task UpdatePromoCodeAsync(PromoCode promoCode);
        Task DeletePromoCodeAsync(int id);
        Task<PromoCode> GetByCodeAsync(string code);
        Task<bool> IsPromoCodeValidAsync(string code);
    }
}
