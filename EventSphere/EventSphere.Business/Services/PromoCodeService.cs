using EventSphere.Business.Services.Interfaces;
using EventSphere.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSphere.Domain.Entities;

namespace EventSphere.Business.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;

        public PromoCodeService(IPromoCodeRepository promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }

        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync()
        {
            return await _promoCodeRepository.GetAllAsync();
        }

        public async Task<PromoCode> GetPromoCodeByIdAsync(int id)
        {
            return await _promoCodeRepository.GetByIdAsync(id);
        }

        public async Task<PromoCode> CreatePromoCodeAsync(PromoCode promoCode)
        {
            await _promoCodeRepository.AddAsync(promoCode);
            return promoCode;
        }

        public async Task UpdatePromoCodeAsync(PromoCode promoCode)
        {
            await _promoCodeRepository.UpdateAsync(promoCode);
        }

        public async Task DeletePromoCodeAsync(int id)
        {
            await _promoCodeRepository.DeleteAsync(id);
        }

        public async Task<PromoCode> GetByCodeAsync(string code)
        {
            return await _promoCodeRepository.GetByCodeAsync(code);
        }

        public async Task<bool> IsPromoCodeValidAsync(string code)
        {
            return await _promoCodeRepository.IsPromoCodeValidAsync(code);
        }
    }
}
