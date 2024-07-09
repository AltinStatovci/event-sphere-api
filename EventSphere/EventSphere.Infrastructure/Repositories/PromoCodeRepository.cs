using EventSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Infrastructure.Repositories
{
    public class PromoCodeRepository : GenericRepository<PromoCode>, IPromoCodeRepository
    {
        private readonly EventSphereDbContext _context;

        public PromoCodeRepository(EventSphereDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PromoCode> GetByCodeAsync(string code)
        {
            return await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<bool> IsPromoCodeValidAsync(string code)
        {
            var promoCode = await GetByCodeAsync(code);
            return promoCode != null && promoCode.IsValid && !promoCode.IsExpired();
        }
    }
}
